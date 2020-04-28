using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XT.Base;

namespace XT.Enhancer {

[Serializable]
internal class SidebarSettings : PartSettings {

[Setting("Enabled")]
public bool enabled = false;

[Setting("Docking Side", toggle: true)]
public Sidebar.DockingSide dockingSide = Sidebar.DockingSide.Right;

[Setting("Background Tint")]
public Color backColor = Color.white;

[Setting("Vertical Padding"), Range(0, 32)]
public int verticalPadding = 8;

[Setting("Items")]
public List<SidebarItem> items = new List<SidebarItem>();

[Setting("Play Mode Layout")]
public string playModeLayout;

[Setting("Save Layout On Change")]
public bool saveLayoutOnChange = false;

Setting enabledSetting;
Setting dockingSideSetting;
Setting backColorSetting;
Setting verticalGapSetting;
Setting itemsSetting;
Setting playModeLayoutSetting;
Setting saveLayoutSetting;

public override void OnGUI(Setting setting) {
	Prepare();
	DrawSettings(setting.label);
}

void Prepare() {
	enabledSetting = enabledSetting ?? GetSetting(nameof(enabled));
	dockingSideSetting = dockingSideSetting ?? GetSetting(nameof(dockingSide));
	backColorSetting = backColorSetting ?? GetSetting(nameof(backColor));
	verticalGapSetting = verticalGapSetting ?? GetSetting(nameof(verticalPadding));
	itemsSetting = itemsSetting ?? GetSetting(nameof(items));
	playModeLayoutSetting = playModeLayoutSetting ?? GetSetting(nameof(playModeLayout));
	saveLayoutSetting = saveLayoutSetting ?? GetSetting(nameof(saveLayoutOnChange));
	layouts = layouts ?? GetLayouts("<Default>");
	playModeLayouts = playModeLayouts ?? GetLayouts("<Current Layout>");
}

void DrawSettings(string label) {
	bool guiEnabled = GUI.enabled;
	GUI.enabled = !Application.isPlaying;
	if (PaneGUI.BeginPart(label, this)) {
		PaneGUI.SettingField(enabledSetting);
		PaneGUI.SettingField(dockingSideSetting);
		PaneGUI.SettingField(backColorSetting);
		PaneGUI.SettingField(verticalGapSetting);
		PaneGUI.Space(8);
		PaneGUI.SettingList(itemsSetting, DrawItem, iconSize + 12);
		PaneGUI.SettingField(saveLayoutSetting);
		int i = playModeLayout.IsNullOrEmpty() ? 
			0 : ArrayUtility.IndexOf<string>(playModeLayouts, playModeLayout);
		i = PaneGUI.SettingField(playModeLayoutSetting, i < 0 ? 0 : i, playModeLayouts);
		playModeLayout = i == 0 ? "" : playModeLayout;
	}
	PaneGUI.EndPart();
	GUI.enabled = guiEnabled;
}

const float iconSize = 32;
GUIContent iconLabel = new GUIContent("", "Click to select icon");

void DrawItem(Rect rect, int index, bool isActive, bool isFocused) {
	SidebarItem item = items[index];
	// icon field
	Rect iconRect = rect;
	iconRect.y += 3;
	iconRect.width = iconSize + 6;
	iconRect.height = iconSize + 6;
	Color savedColor = GUI.backgroundColor;
	GUI.backgroundColor = backColor;
	GUI.Label(iconRect, iconLabel, EditorResources.dockHeaderStyle);
	GUI.backgroundColor = savedColor;
	Event e = Event.current;
	if (e.type == EventType.ExecuteCommand && e.commandName == "ObjectSelectorUpdated") {
		int id = EditorGUIUtility.GetObjectPickerControlID();
		if (id == index) {
			item.icon = EditorGUIUtility.GetObjectPickerObject() as Texture2D;
			GUI.changed = true;
		}
	}
	iconRect.x += 3;
	iconRect.y += 3;
	switch (item.function) {
	case SidebarItem.Function.SetLayout:
	case SidebarItem.Function.ExecuteMenuItem:
		Sidebar.PrepareGUI();
		bool clicked = Sidebar.DrawIcon(iconRect, item.icon);
		if (clicked) {
			PaneGUI.FocusListElementAt(index);
			EditorGUIUtility.ShowObjectPicker<Texture2D>(item.icon, false, "", index);
		}
		break;
	case SidebarItem.Function.Separator:
		Sidebar.PrepareGUI();
		Sidebar.DrawSeparator(iconRect, item.icon);
		break;
	}
	// command field
	rect.x += iconSize + 10;
	rect.y += 3;
	rect.width -= iconSize + 10;
	rect.height = EditorGUIUtility.singleLineHeight;
	item.function = (SidebarItem.Function)EditorGUI.EnumPopup(rect, item.function);
	// command data field
	rect.y += EditorGUIUtility.singleLineHeight + 2;
	switch (item.function) {
	case SidebarItem.Function.SetLayout:
		int i = item.layoutName.IsNullOrEmpty() ? 
			0 : ArrayUtility.IndexOf<string>(layouts, item.layoutName);
		i = EditorGUI.Popup(rect, "", i < 0 ? 0 : i, layouts);
		item.layoutName = i == 0 ? "" : layouts[i];
		break;
	case SidebarItem.Function.ExecuteMenuItem:
		item.menuPath = EditorGUI.TextField(rect, item.menuPath);
		break;
	case SidebarItem.Function.Separator:
		item.icon = Resources.separatorIcon;
		break;
	}
}

public override void OnChange() {
	Sidebar sidebar = Host.GetWindow<Sidebar>();
	if (sidebar != null) {
		if (!enabled) {
			Sidebar.Hide();
			return;
		}
		if (dockingSide != Sidebar.dockingSide) {
			Sidebar.Hide();
			Sidebar.Toggle();
			return;
		}
		sidebar.Repaint();
	} else
	if (enabled) {
		Sidebar.Toggle();
	}
}

static string[] layouts;
static string[] playModeLayouts;

static string[] GetLayouts(string defaultOption) {
	string[] layouts = { defaultOption, "/-" };
	ArrayUtility.AddRange(ref layouts, LayoutManager.GetLayouts());
	return layouts;
}

}

}
