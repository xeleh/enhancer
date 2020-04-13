using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XT.Base {

internal sealed class Preferences : IHasSettings {

readonly string prefix;
readonly Setting[] settings;
readonly Dictionary<string, Setting> dict = new Dictionary<string, Setting>();

internal static Preferences CreateInstance<T>(string prefix) {
	return new Preferences(prefix, typeof(T));
}

Preferences(string prefix, Type type) {
	this.prefix = prefix;
	dict = SettingParser.Parse(type, this);
	settings = dict.Values.ToArray();
	foreach (Setting setting in settings) {
		InitSetting(setting);
	}
}

void InitSetting(Setting setting) {
	string key = prefix + setting.name;
	if (EditorPrefs.HasKey(key)) {
		return;
	}
	if (setting.type == typeof(string)) {
		EditorPrefs.SetString(key, (string)setting.defaultValue);
	} else
	if (setting.type == typeof(int)) {
		EditorPrefs.SetInt(key, (int)setting.defaultValue);
	} else
	if (setting.type == typeof(float)) {
		EditorPrefs.SetFloat(key, (float)setting.defaultValue);
	} else
	if (setting.type == typeof(bool)) {
		int intValue = (bool)setting.defaultValue ? 1 : 0;
		EditorPrefs.SetInt(key, intValue);
	} else
	if (setting.type == typeof(Color)) {
		int intValue = ((Color)setting.defaultValue).ToRGBA();
		EditorPrefs.SetInt(key, intValue);
	} else
	if (setting.type.IsEnum) {
		Enum enumValue = (Enum)setting.field.GetValue(null);
		int intValue = Convert.ToInt32(enumValue);
		EditorPrefs.SetInt(key, intValue);
	}
}

public Setting[] GetSettings() {
	return settings;
}

public Setting GetSetting(string name) {
	dict.TryGetValue(name, out Setting setting);
	return setting;
}

public void Check() {
}

public void Load() {
	for (int i = 0; i < settings.Length; i++) {
		if (settings[i] != null) {
			LoadSetting(settings[i]);
		}
	}
}

void LoadSetting(Setting setting) {
	string key = prefix + setting.name;
	if (setting.type == typeof(string)) {
		setting.field.SetValue(null, EditorPrefs.GetString(key, (string)setting.defaultValue));
	} else
	if (setting.type == typeof(int)) {
		setting.field.SetValue(null, EditorPrefs.GetInt(key, (int)setting.defaultValue));
	} else
	if (setting.type == typeof(float)) {
		float defaultValue = Convert.ToSingle(setting.defaultValue);
		setting.field.SetValue(null, EditorPrefs.GetFloat(key, defaultValue));
	} else
	if (setting.type == typeof(bool)) {
		int defaultValue = (bool)setting.defaultValue ? 1 : 0;
		bool boolValue = EditorPrefs.GetInt(key, defaultValue) == 1;
		setting.field.SetValue(null, boolValue);
	} else
	if (setting.type == typeof(Color)) {
		int defaultValue = ((Color)setting.defaultValue).ToRGBA();
		defaultValue = EditorPrefs.GetInt(key, defaultValue);
		Color colorValue = defaultValue.ToColor();
		setting.field.SetValue(null, colorValue);
	} else
	if (setting.type.IsEnum) {
		int defaultValue = Convert.ToInt32(setting.defaultValue);
		int intValue = EditorPrefs.GetInt(key, defaultValue);
		Array enumValues = Enum.GetValues(setting.type);
		Enum enumValue = (Enum)enumValues.GetValue(intValue);
		setting.field.SetValue(null, enumValue);
	}
}

public void Save() {
	for (int i = 0; i < settings.Length; i++) {
		if (settings[i] != null) {
			SaveSetting(settings[i]);
		}
	}
}

void SaveSetting(Setting setting) {
	string key = prefix + setting.name;
	if (setting.type == typeof(string)) {
		string stringValue = (string)setting.field.GetValue(null);
		EditorPrefs.SetString(key, stringValue);
	} else
	if (setting.type == typeof(int)) {
		int intValue = (int)setting.field.GetValue(null);
		EditorPrefs.SetInt(key, intValue);
	} else
	if (setting.type == typeof(float)) {
		float floatValue = (float)setting.field.GetValue(null);
		EditorPrefs.SetFloat(key, floatValue);
	} else
	if (setting.type == typeof(bool)) {
		int intValue = (bool)setting.field.GetValue(null) ? 1 : 0;
		EditorPrefs.SetInt(key, intValue);
	} else
	if (setting.type == typeof(Color)) {
		Color colorValue = (Color)setting.field.GetValue(null);
		int intValue = colorValue.ToRGBA();
		EditorPrefs.SetInt(key, intValue);
	} else
	if (setting.type.IsEnum) {
		Enum enumValue = (Enum)setting.field.GetValue(null);
		int intValue = Convert.ToInt32(enumValue);
		EditorPrefs.SetInt(key, intValue);
	}
}

public void Reset() {
	for (int i = 0; i < settings.Length; i++) {
		if (settings[i] != null) {
			ResetSetting(settings[i]);
		}
	}
}

void ResetSetting(Setting setting) {
	string key = prefix + setting.name;
	if (setting.type == typeof(string)) {
		EditorPrefs.SetString(key, (string)setting.defaultValue);
	} else
	if (setting.type == typeof(int)) {
		EditorPrefs.SetInt(key, (int)setting.defaultValue);
	} else
	if (setting.type == typeof(float)) {
		float defaultValue = Convert.ToSingle(setting.defaultValue);
		EditorPrefs.SetFloat(key, defaultValue);
	} else
	if (setting.type == typeof(bool)) {
		int defaultValue = (bool)setting.defaultValue ? 1 : 0;
		EditorPrefs.SetInt(key, defaultValue);
	} else
	if (setting.type == typeof(Color)) {
		int defaultValue = ((Color)setting.defaultValue).ToRGBA();
		EditorPrefs.SetInt(key, defaultValue);
	} else
	if (setting.type.IsEnum) {
		int defaultValue = Convert.ToInt32(setting.defaultValue);
		EditorPrefs.SetInt(key, defaultValue);
	}
	setting.field.SetValue(null, setting.defaultValue);
}

}

}