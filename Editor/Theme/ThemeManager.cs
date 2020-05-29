using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UObject = UnityEngine.Object;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using XT.Base;

namespace XT.Enhancer {

internal static class ThemeManager {

static Version v2019_3_0 = new Version(2019, 3, 0);
static Version v2019_3_14 = new Version(2019, 3, 14);

public static bool supported = Host.version >= v2019_3_0 && Host.version <= v2019_3_14;

static ThemeSettings settings => Package.settings.theme;

public static bool darkEnabled;

enum State {
	Startup,
	Switching,
	Done
}

static string stateKey => $"{Package.name}.ThemeManager.state";

static State state {
	get => (State)SessionState.GetInt(stateKey, (int)State.Startup);
	set => SessionState.SetInt(stateKey, (int)value);
}

static string incompleteKey => $"{Package.name}.ThemeManager.incomplete";

static bool incomplete {
	get => EditorPrefs.GetBool(incompleteKey, false);
	set => EditorPrefs.SetBool(incompleteKey, value);
}

static string darkThemeKey = "darkTheme";

public static void OnLoad() {
	if (!supported) { return; }
	darkEnabled = settings.darkEnabled;
	EditorPrefs.SetBool(darkThemeKey, settings.darkEnabled);
	SetDarkModeInternalFlag();
	switch (state) {
	case State.Startup:
		if (settings.autoEnable && settings.darkEnabled && !incomplete) {
			EditorApplication.delayCall += async () => {
				// give editor enough time to complete initial load
				await Task.Delay(500);
				SetTheme();
			};
		} else {
			state = State.Done;
		}
		break;
	case State.Switching:
		ProgressWindow.Update();
		EditorApplication.delayCall += () => {
			ReplaceIcons();
			ProgressWindow.Update();
			ReplaceSkin();
			ProgressWindow.Hide();
			state = State.Done;
			incomplete = false;
		};
		break;
	}
}

static Version v2019_3_8 = new Version(2019, 3, 8);

static void SetDarkModeInternalFlag() {
	if (Host.version >= v2019_3_8) {
		EditorResourcesR.Constants.isDarkTheme = settings.darkEnabled;
	}
}

// --------------------------------------------------------------------------------------------

public static void SetTheme() {
	if (!supported) { return; }
	string themeName = settings.darkEnabled ? "Dark" : "Light";
	ProgressWindow.Init($"Setting {themeName} Theme", "Hold on...", 3);
	incomplete = true;
	state = State.Switching;
	PreLoad();
	ReplaceSheets();
	ProgressWindow.Update();
	SetDarkModeInternalFlag();
	InternalEditorUtility.SwitchSkinAndRepaintAllViews();
	EditorUtility.RequestScriptReload();
}

static void PreLoad() {
	// force some rebel toolbars to embrace the dark side ;)
	FlashWindow("UIElements.Debugger.UIElementsDebugger");
	// ensure known light stylesheets are loaded
	EditorGUIUtility.Load("StyleSheets/PackageManager/Light.uss");
	EditorGUIUtility.Load("StyleSheets/SettingsWindowLight.uss");
	AssetDatabase.LoadAssetAtPath<StyleSheet>(
		"Packages/com.unity.timeline/Editor/StyleSheets/Extensions/light.uss");
}

// cause copies of the original sheets are stored in memory, we need a dictionary
static Dictionary<string, StyleSheet> sheetDict;

static void ReplaceSheets() {
	// replace stylesheets
	sheetDict = Utils.FindObjects<StyleSheet>();
	foreach (StyleSheet sheet in sheetDict.Values) {
		// skip if sheet is a copy
		if (sheet.name.EndsWith(".copy")) {
			continue;
		}
		// skip if sheet is main light sheet (only on v2019.3.8 or newer)
		if (sheet.name == "light" && Host.version >= v2019_3_8) {
			continue;
		}
		// if sheet is a light stylesheet, replace it
		if (sheet.name.Contains("light") || sheet.name.Contains("Light")) {
			ReplaceSheet(sheet);
		}
	}
}

static void ReplaceSheet(StyleSheet sheet) {
	// is there a dark version of the stylesheet?
	StyleSheet dark = GetDarkSheet(sheet);
	if (dark == null) {
		return;
	}
	// get the sheet copy if exists
	string copyName = sheet.name + ".copy";
	sheetDict.TryGetValue(copyName, out StyleSheet copy);
	// make a copy if necessary and replace with dark version
	if (settings.darkEnabled) {
		if (copy == null) {
			copy = ScriptableObject.CreateInstance<StyleSheet>();
			copy.name = copyName;
			CopySerialized(sheet, copy);
		}
		CopySerialized(dark, sheet);
	} else {
		// or restore the light skin copy
		CopySerialized(copy, sheet);
	}
}

static StyleSheet GetDarkSheet(StyleSheet sheet) {
	// cause EditorGUIUtility.Load() is case insensitive, we need to solve this case manually
	if (sheet.name == "light") {
		return EditorGUIUtility.Load("dark") as StyleSheet;
	}
	if (sheet.name == "Light") {
		return EditorGUIUtility.Load("StyleSheets/PackageManager/Dark.uss") as StyleSheet;
	}
	// is it a built-in stylesheet?
	string path = AssetDatabase.GetAssetPath(sheet);
	if (path == "Library/unity editor resources") {
		string darkName = sheet.name.Replace("Light", "Dark").Replace("light", "dark");
		return EditorGUIUtility.Load(darkName) as StyleSheet;
	}
	// definitely not a built-in stylesheet, so load it using path in database
	path = path.Replace("Light", "Dark").Replace("light", "dark");
	return AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
}

// --------------------------------------------------------------------------------------------

static void ReplaceSkin() {
	// load editor legacy skins
	GUISkin light = EditorGUIUtility.Load("LightSkin") as GUISkin;
	GUISkin dark = EditorGUIUtility.Load("DarkSkin") as GUISkin;
	if (light == null || dark == null) {
		return;
	}
	// get the skin copy if exists
	var skinDict = Utils.FindObjects<GUISkin>();
	const string copyName = "CopySkin";
	skinDict.TryGetValue(copyName, out GUISkin copy);
	// make a copy if necessary and replace with dark version
	if (settings.darkEnabled) {
		if (copy == null) {
			copy = ScriptableObject.CreateInstance<GUISkin>();
			copy.name = copyName;
			CopySerialized(light, copy);
		}
		CopySerialized(dark, light);
	} else {
		// or restore the light skin copy
		CopySerialized(copy, light);
	}
}

// --------------------------------------------------------------------------------------------

// cause copies of the original icons are stored in memory, we can't rely on Load()
static Dictionary<string, Texture2D> iconDict; 

static void ReplaceIcons() {
	// create a dictionary of icons for later restoration of the light theme
	iconDict = Utils.FindObjects<Texture2D>();
	// replace all light icons with themed versions
	AssetBundle bundle = EditorGUIUtilityR.GetEditorAssetBundle();
	string[] paths = bundle.GetAllAssetNames();
	foreach (string path in paths) {
		// skip if asset does not correspond to a dark version
		string name = Path.GetFileName(path);
		if (!name.StartsWith("d_")) {
			continue;
		}
		string dir = Path.GetDirectoryName(path);
		string lightPath = $"{dir}/{name.Substring(2)}";
		// load the corresponding light version
		Texture2D light = bundle.LoadAsset<Texture2D>(lightPath);
		if (light != null) {
			string darkPath = path;
			// dirty fix for a weird case...
			if (darkPath.Contains("d_horizontallayout")) {
				darkPath = darkPath.Replace("/unityengine/ui", "");
			}
			Texture2D dark = bundle.LoadAsset<Texture2D>(darkPath);
			ReplaceIcon(light, dark);
		}
	}
}

static void ReplaceIcon(Texture2D light, Texture2D dark) {
	// get the icon copy if exists
	Texture2D copy = null;
	string copyName = light.name + ".copy";
	iconDict?.TryGetValue(copyName, out copy);
	// make a copy if necessary and replace with dark version
	if (settings.darkEnabled) {
		if (copy == null) {
			copy = light.ReadableClone();
			copy.name = copyName;
			copy.hideFlags = HideFlags.HideAndDontSave;
		}
		CopyIcon(dark, light);
	} else {
		// or restore the light copy
		CopyIcon(copy, light);
	}
}

static void CopyIcon(Texture2D source, Texture2D dest) {
	if (source != null) {
		IntPtr p = source.GetNativeTexturePtr();
		dest.UpdateExternalTexture(p);
	}
}

// --------------------------------------------------------------------------------------------

public static void FlashWindow(string typeName) {
	Type type = EditorType.GetType(typeName);
	if (type == null) {
		return;
	}
	EditorWindow window = Host.GetWindow(type);
	if (window == null) {
		window = EditorWindow.GetWindow(type, false, "", false);
		window.Close();
	}
}

static void CopySerialized(UObject source, UObject dest) {
	if (source == null) {
		return;
	}
	string oldName = dest.name;
	EditorUtility.CopySerialized(source, dest);
	dest.name = oldName;
	dest.hideFlags = HideFlags.HideAndDontSave;
}

}

}

