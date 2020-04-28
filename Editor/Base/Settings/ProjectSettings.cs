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
	// not happy with this cleanup, there must be a better way
	T[] zombies = Resources.FindObjectsOfTypeAll<T>();
	foreach (var zombie in zombies) {
		zombie.DestroyImmediate();
	}
	// load the .asset file
	UObject[] objects = InternalEditorUtility.LoadSerializedFileAndForget(assetPath);
	bool notLoaded = objects == null || objects.Length == 0 || objects[0] == null;
	if (notLoaded) {
		// ok, then create it
		objects = SaveAsset<T>(ScriptableObject.CreateInstance<T>(), assetPath);
		if (objects == null || objects.Length == 0) {
			return null;
		}
	}
	// setup the instance
	T instance = objects[0] as T;
	if (instance != null) {
		instance.assetPath = assetPath;
		instance.hideFlags = HideFlags.HideAndDontSave; 
		instance.dict = SettingParser.Parse(typeof(T), instance);
		instance.settings = instance.dict.Values.ToArray();
	}
	return instance;
}

static UObject[] SaveAsset<T>(T instance, string assetPath) where T : ProjectSettings {
	UObject[] objects = new UObject[] { instance };
	Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
	InternalEditorUtility.SaveToSerializedFileAndForget(objects, assetPath, true);
	return objects;
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
	SaveAsset(this, assetPath);
}

void IHasSettings.Reset() {
	ProjectSettings instance = ScriptableObject.CreateInstance(GetType()) as ProjectSettings;
	EditorUtility.CopySerialized(instance, this);
	instance.DestroyImmediate();
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
