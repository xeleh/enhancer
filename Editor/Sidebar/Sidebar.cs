using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XT.Base;

namespace XT.Enhancer {

public class Sidebar : EditorWindow, IHasCustomMenu {

// ----------------------------------------------------------------------------------------------
// window stuff

static Sidebar FindInstance() {
	return Host.GetWindow<Sidebar>();
}

[MenuItem("Window/Sidebar %&b", false, 9999)]
public static void Toggle() {
	Sidebar instance = FindInstance();
	if (instance != null) {
		instance.Close();
		Reflow();
	} else {
		Create();
	}
}

public static Sidebar Reveal() {
	return FindInstance() ?? Create();
}

public static void Hide() {
	Sidebar instance = FindInstance();
	if (instance != null) {
		instance.Close();
		Reflow();
	}
}

const float SIDEBAR_WIDTH = 38;

static Sidebar Create() {
	Rect rect = new Rect(0, 0, SIDEBAR_WIDTH, 64);
	Sidebar instance = (Sidebar)EditorWindow.GetWindowWithRect(typeof(Sidebar), rect, false, "");
	Vector2 minSize = instance.minSize;
	minSize.x = SIDEBAR_WIDTH;
	minSize.y = 0;
	instance.minSize = minSize;
	Vector2 maxSize = instance.maxSize;
	maxSize.x = SIDEBAR_WIDTH;
	instance.maxSize = maxSize;
	instance.Dock();
	return instance;
}

public enum DockingSide {
	Left,
	Right
}

static string dockingSideKey => $"{Package.name}.Sidebar.dockingSideKey";

internal static DockingSide dockingSide {
	get => (DockingSide)SessionState.GetInt(dockingSideKey, (int)DockingSide.Left);
	set => SessionState.SetInt(dockingSideKey, (int)value);
}

static SidebarSettings settings => Package.settings.sidebar;

void Dock() {
	dockingSide = settings.dockingSide;
	bool success = false;
	switch (dockingSide) {
	case DockingSide.Left:
		success = Host.DockWindowLeft(this);
		break;
	case DockingSide.Right:
		success = Host.DockWindowRight(this);
		break;
	}
	if (success) {
		Resize();
	} else {
		Close();
	}
}

static void Reflow() {
	float mainWidth = Host.position.width;
	object[] views = ViewR.Wrap(Host.mainView).allChildren;
	// if a full width window exists, skip the reflow
	for (int i = 0; i < views.Length; i++) {
		if (views[i].GetType() == DockAreaR.type) {
			Rect p = ViewR.Wrap(views[i]).position;
			if (p.width == mainWidth) {
				return;
			}
		}
	}
	for (int i = 0; i < views.Length; i++) {
		// view triage
		object view = views[i];
		if (view.GetType() == SplitViewR.type) {
			if (!SplitViewR.Wrap(views[i]).vertical) {
				continue;
			}
		} else
		if (view.GetType() != DockAreaR.type) {
			continue;
		}
		// resize or reposition the view
		Rect wp = ViewR.Wrap(view).windowPosition;
		Rect p = ViewR.Wrap(view).position;
		switch (settings.dockingSide) {
		case DockingSide.Left:
			if (wp.x == 0) {
				if (wp.width < mainWidth) {
					p.width -= SIDEBAR_WIDTH;
				}
			} else
			if (wp.x + wp.width == mainWidth) {
				p.x -= SIDEBAR_WIDTH;
			}
			break;
		case DockingSide.Right:
			if (wp.x + wp.width == mainWidth) {
				p.width -= SIDEBAR_WIDTH + 2;
			} else
			if (wp.x == 0) {
				p.x -= SIDEBAR_WIDTH + 2;
			}
			break;
		}
		ViewR.Wrap(view).position = p;
	}
	ViewR.Wrap(Host.mainView).Reflow();
}

void Resize() {
	this.SetWidth(SIDEBAR_WIDTH);
}

bool resizeRequired;

void Update() {
	if (!resizeRequired) {
		return;
	}
	// if the sidebar was resized after docking another window, close it
	if (position.width > SIDEBAR_WIDTH * 2) {
		Close();
	} else {
		// force fixed width
		Resize();
		resizeRequired = false;
	}
}

// ----------------------------------------------------------------------------------------------
// GUI

void OnGUI() {
	if (this == null) {
		return;
	}
	PrepareGUI();
	DrawBackground();
	DrawIcons();
	if (position.width > SIDEBAR_WIDTH && this.IsDocked()) {
		resizeRequired = true;
	}
}

const float iconSize = 32;
const float minMargin = 3;
static bool darkTheme;
static Color itemColor;
static Color activeItemColor;
static Color playModeTint;

internal static void PrepareGUI() {
	darkTheme = Host.darkEnabled || ThemeManager.darkEnabled;
	itemColor = darkTheme ? new Color(1, 1, 1, 0.32f) : new Color(0, 0, 0, 0.18f);
	activeItemColor = darkTheme ? new Color(1, 1, 1, 0.75f) : new Color(0, 0, 0, 0.6f);
}

void DrawBackground() {
	Rect r = position;
	r.position = new Vector2(0, -2);
	r.height += 2;
	Color backColor = GUI.color;
	GUI.color = settings.backColor;
	if (EditorApplication.isPlayingOrWillChangePlaymode) {
		if (playModeTint.a == 0) {
			playModeTint = Host.GetPlayModeTintColor();
		}
		GUI.color *= playModeTint;
	}
	GUI.Label(r, "", EditorResources.dockHeaderStyle);
	GUI.color = backColor;
}

void DrawIcons() {
	// calculate the top icon rect with a minor x correction for dark theme
	float x = minMargin;
	if (darkTheme) {
		x += settings.dockingSide == DockingSide.Right ? -1 : 1;
	}
	float y = Mathf.Max(minMargin, settings.verticalPadding);
	Rect iconRect = new Rect(x, y, iconSize, iconSize);
	// draw the items
	List<SidebarItem> items = settings.items;
	foreach (SidebarItem item in items) {
		switch (item.function) {
		case SidebarItem.Function.SetLayout:
			bool active = item.layoutName == LayoutManager.currentLayout;
			if (DrawIcon(iconRect, item.icon, active) && !active) {
				SetLayout(item.layoutName);
			}
			break;
		case SidebarItem.Function.ExecuteMenuItem:
			if (DrawIcon(iconRect, item.icon)) {
				EditorApplication.ExecuteMenuItem(item.menuPath);
			}
			break;
		case SidebarItem.Function.Separator:
			iconRect.y -= minMargin * 2;
			DrawSeparator(iconRect, item.icon);
			iconRect.y -= minMargin * 2;
			break;
		}
		iconRect.y += settings.verticalPadding + iconSize;
	}
	DrawConfigIcon(iconRect);
}

internal static bool DrawIcon(Rect rect, Texture2D icon, bool active = false) {
	Color backColor = GUI.backgroundColor;
	Color c = settings.backColor;
	c.a = darkTheme ? 0.6f : 0.2f;
	GUI.backgroundColor = c;
	Color contentColor = GUI.contentColor;
	GUI.contentColor = active ? activeItemColor : itemColor;
	bool clicked = GUI.Button(rect, icon, Resources.iconStyle);
	GUI.contentColor = contentColor;
	GUI.backgroundColor = backColor;
	return clicked;
}

internal static void DrawSeparator(Rect rect, Texture2D icon) {
	Color contentColor = GUI.contentColor;
	GUI.contentColor = itemColor * (darkTheme ? 1.0f : 0.8f);
	GUI.Label(rect, icon, Resources.separatorStyle);
	GUI.contentColor = contentColor;
}

void DrawConfigIcon(Rect rect) {
	float bottomMargin = Mathf.Max(minMargin, settings.verticalPadding);
	rect.y = position.height - iconSize - bottomMargin;
	if (DrawIcon(rect, Resources.settingsIcon)) {
		ShowSettings();
	}
}

void ShowSettings() {
	SettingsService.OpenProjectSettings($"Project/{Package.displayName}");
}

// prevent the tab horizontal scroll arrow from appearing in the dock area
void ShowButton(Rect rect) { }

// ----------------------------------------------------------------------------------------------
// activation logic

public static void OnLoad() {
	EditorApplication.delayCall += () => {
		// we need to hide the sidebar on load because the default layout is saved on quit
		bool enabled = settings.enabled;
		Hide();
		if (enabled) {
			Create();
		}
	};
}

void OnEnable() {
	settings.enabled = true;
	Package.SaveSettings();
}

void OnDisable() {
	settings.enabled = false;
	Package.SaveSettings();
}

// ----------------------------------------------------------------------------------------------
// layout change logic

static void SetLayout(string layoutName, bool showSidebar = true) {
	// hide on the current layout to prevent the sidebar from being saved
	Hide();
	// if required save the current layout before setting the new one
	if (settings.saveLayoutOnChange && !Application.isPlaying) {
		LayoutManager.SaveLayout(LayoutManager.currentLayout);
	}
	LayoutManager.LoadLayout(layoutName);
	// hiding the sidebar on the new layout prevents a scroll arrow from appearing on tab
	Hide();
	// and show the sidebar again if required
	if (showSidebar) {
		Reveal();
	}
}

// ----------------------------------------------------------------------------------------------
// play mode layout logic

[InitializeOnLoadMethod]
static void InitPlayModeController() {
	EditorApplication.playModeStateChanged += OnPlayModeStateChange;
}

static string prevLayoutKey => $"{Package.name}.Sidebar.prevLayoutKey";

internal static string prevLayout {
	get => SessionState.GetString(prevLayoutKey, null);
	set => SessionState.SetString(prevLayoutKey, value);
}

static void OnPlayModeStateChange(PlayModeStateChange state) {
	// if <Current Layout> is set as the play mode layout, do nothing
	if (settings.playModeLayout.IsNullOrEmpty()) {
		return;
	}
	switch(state) {
	case PlayModeStateChange.ExitingEditMode:
		prevLayout = LayoutManager.currentLayout;
		if (settings.enabled) {
			SetLayout(settings.playModeLayout, false);
		}
		break;
	case PlayModeStateChange.EnteredEditMode:
		if (prevLayout != LayoutManager.currentLayout) {
			SetLayout(prevLayout);
		}
		break;
	}
}

// ----------------------------------------------------------------------------------------------
// window context menu

static GUIContent configureLabel = new GUIContent("Settings");

void IHasCustomMenu.AddItemsToMenu(GenericMenu menu) {
	menu.AddItem(configureLabel, false, ShowSettings);
}

}

}
