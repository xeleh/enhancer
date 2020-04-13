using System.Collections.Generic;
using System.IO;
using UnityEditor;
using XT.Base;

namespace XT.Enhancer {

public static class LayoutManager {

public static string currentLayout {
	get {
		string layoutName = ToolbarR.m_LastLoadedLayoutName;
		return layoutName.IsNullOrEmpty() ? "" : layoutName;
	}
}

public static string[] GetLayouts() {
	string defaultPath = WindowLayoutR.layoutsDefaultModePreferencesPath;
	string[] paths = Directory.GetFiles(defaultPath, "*.wlt");
	List<string> layouts = new List<string>();
	for (int i = 0; i < paths.Length; i++) {
		string name = Path.GetFileNameWithoutExtension(paths[i]);
		if (!name.IsNullOrEmpty()) {
			layouts.Add(name);
		}
	}
	return layouts.ToArray();
}

public static void LoadLayout(string layoutName) {
	EditorUtility.LoadWindowLayout(GetLayoutPath(layoutName));
}

public static void SaveLayout(string layoutName) {
	WindowLayoutR.SaveWindowLayout(GetLayoutPath(layoutName));
}

static string GetLayoutPath(string layoutName) {
	return layoutName.IsNullOrEmpty() ? 
		WindowLayoutR.GetCurrentLayoutPath() :
		$"{WindowLayoutR.layoutsDefaultModePreferencesPath}/{layoutName}.wlt";
}

}

}
