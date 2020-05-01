using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace XT.Base {

internal static class Host {

public static string projectPath = Directory.GetCurrentDirectory();
public static string settingsPath = projectPath + "/ProjectSettings";
public static string assetsPath = Application.dataPath;
public static bool darkEnabled => EditorGUIUtility.isProSkin;
public static Version version = GetCurrentVersion();

public static object mainView => WindowLayoutR.FindMainView();
public static EditorWindow[] windows => Resources.FindObjectsOfTypeAll<EditorWindow>();
public static Rect position => ViewR.Wrap(WindowLayoutR.FindMainView()).screenPosition;

public static event Action reflowEvent;

static Version GetCurrentVersion() {
	string s = Application.unityVersion;
	s = Regex.Replace(s, "[Aa|Bb|Ff|Pp][0-9]+", "");
	string[] parts = s.Split("."[0]);
	int major = Convert.ToInt32(parts[0]);
	int minor = Convert.ToInt32(parts[1]);
	int fix = Convert.ToInt32(parts[2]);
	return new Version(major, minor, fix);
}

public static Color GetPlayModeTintColor() {
	string s = EditorPrefs.GetString("Playmode tint", "");
	s = s.Replace("Playmode tint;", "").Replace(",", ".");
	string[] comp = s.Split(';');
	IFormatProvider format = CultureInfo.InvariantCulture.NumberFormat;
	float r = float.Parse(comp[0], NumberStyles.Float, format);
	float g = float.Parse(comp[1], NumberStyles.Float, format);
	float b = float.Parse(comp[2], NumberStyles.Float, format);
	float a = float.Parse(comp[3], NumberStyles.Float, format);
	return new Color(r, g, b, a);
}

public static EditorWindow GetWindow(Type type) {
	return Utils.FindObject(type) as EditorWindow;
}

public static T GetWindow<T>() where T : EditorWindow {
	return Utils.FindObject<T>();
}

public static void OnReflow() {
	reflowEvent?.Invoke();
}

public static bool DockWindowLeft(EditorWindow window) {
	EditorWindow mostLeft = GetMostLeftWindow(window);
	if (mostLeft == null) {
		return false;
	}
	object parent = mostLeft.GetParent();
	if (parent == null) {
		return false;
	}
	// force the after-dock reflow to respect the current window widths
	EditorWindow[] windows = Host.windows;
	Vector2[] minSizes = new Vector2[windows.Length];
	for (int i = 0; i < minSizes.Length; i++) {
		minSizes[i] = windows[i].minSize;
		Vector2 size = windows[i].minSize;
		size.x = windows[i].position.size.x;
		windows[i].minSize = size;
	}
	// dock the window
	Rect rect = ViewR.Wrap(parent).screenPosition;
	DockWindow(window, mostLeft, rect.position);
	// restore min sizes
	for (int i = 0; i < windows.Length; i++) {
		windows[i].minSize = minSizes[i];
	}
	return true;
}

public static EditorWindow GetMostLeftWindow(EditorWindow except = null) {
	foreach (var window in windows) {
		if (window != except && window != null && !window.IsFloating()) {
			Rect p = window.GetWindowPosition();
			if (p.x == 0) {
				return window;
			}
		}
	}
	return null;
}

public static bool DockWindowRight(EditorWindow window) {
	EditorWindow mostRight = GetMostRightWindow(window);
	if (mostRight == null) {
		return false;
	}
	object parent = mostRight.GetParent();
	if (parent == null) {
		return false;
	}
	// force the after-dock reflow to respect the current window widths
	EditorWindow[] windows = Host.windows;
	Vector2[] minSizes = new Vector2[windows.Length];
	for (int i = 0; i < minSizes.Length; i++) {
		EditorWindow w = windows[i];
		minSizes[i] = w.minSize;
		w.minSize = w.position.size;
	}
	// dock the window
	Rect rect = ViewR.Wrap(parent).screenPosition;
	Vector2 screenPoint = new Vector2(rect.x + rect.width, rect.y);
	DockWindow(window, mostRight, screenPoint);
	// restore min sizes
	for (int i = 0; i < windows.Length; i++) {
		windows[i].minSize = minSizes[i];
	}
	return true;
}

static EditorWindow GetMostRightWindow(EditorWindow except) {
	float mainWidth = position.width;
	foreach (var window in windows) {
		if (window != except && window != null && !window.IsFloating()) {
			Rect p = window.GetWindowPosition();
			if (p.x + p.width == mainWidth) {
				return window;
			}
		}
	}
	return null;
}

static void DockWindow(EditorWindow window, EditorWindow anchor, Vector2 screenPoint) {
	object dockArea = anchor.GetParent();
	object containerWindow = DockAreaR.Wrap(dockArea).window;
	object rootSplitView = ContainerWindowR.Wrap(containerWindow).rootSplitView;
	DockAreaR.dragSource = window.GetParent();
	object dropInfo = SplitViewR.Wrap(rootSplitView).DragOverRootView(screenPoint);
	if (dropInfo == null || DropInfoR.Wrap(dropInfo).dropArea == null) {
		return;
	}
	SplitViewR.Wrap(rootSplitView).PerformDrop(window, dropInfo, screenPoint);
	OnReflow();
}

}

}
