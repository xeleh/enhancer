using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using UObject = UnityEngine.Object;

namespace XT.Base {

internal class ProjectSettings : ScriptableObject, IHasSettings {

internal Setting[] settings;
string assetPath;
Dictionary<string, Setting> dict = new Dictionary<string, Setting>();

internal static T GetInstance<T>(string assetPath) where T : ProjectSettings {
	UObject[] objects = InternalEditorUtility.LoadSerializedFileAndForget(assetPath);
	bool create = objects == null || objects.Length == 0 || objects[0] == null;
	if (create) {
		objects = new UObject[] { ScriptableObject.CreateInstance<T>() };
		Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
		InternalEditorUtility.SaveToSerializedFileAndForget(objects, assetPath, true);
		AssetDatabase.Refresh();
		if (objects == null || objects.Length == 0) {
			return null;
		}
	}
	T instance = objects[0] as T;
	if (instance != null) {
		instance.assetPath = assetPath;
		instance.Setup();
	}
	return instance;
}

void Setup() {
	hideFlags = HideFlags.HideAndDontSave; 
	dict = SettingParser.Parse(GetType(), this);
	settings = dict.Values.ToArray();
}

Setting[] IHasSettings.GetSettings() {
	return settings;
}

Setting IHasSettings.GetSetting(string name) {
	dict.TryGetValue(name, out Setting setting);
	return setting;
}

void IHasSettings.Check() {
	if (!File.Exists(assetPath)) {
		this.Save();
	}
}

void IHasSettings.Load() {
}

void IHasSettings.Save() {
	UObject[] objects = new UObject[] { this };
	Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
	InternalEditorUtility.SaveToSerializedFileAndForget(objects, assetPath, true);
}

void IHasSettings.Reset() {
	ProjectSettings instance = ScriptableObject.CreateInstance(GetType()) as ProjectSettings;
	EditorUtility.CopySerialized(instance, this);
	this.Save();
}

}

// ----------------------------------------------------------------------------------------------

[CustomEditor(typeof(ProjectSettings), true)]
class SettingsEditor : Editor {
	
// prevent from being visible on Inspector
public override void OnInspectorGUI() {
}	
	
}

}
