using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using static UnityEditorInternal.ReorderableList;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace XT.Base {

internal static class PaneGUI  {

public static void Space(float pixels) {
	GUILayout.Space(pixels);
}

static bool firstElement = true;

public static void Header(string text) {
	Space(4);
	if (firstElement) {
		Space(-5);
		firstElement = false;
	}
	EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
	Space(2);
}

public static void DrawSettings(Setting[] settings) {
	foreach (Setting setting in settings) {
		SettingField(setting);
	}
}

public static void SettingField(Setting setting) {
	if (setting == null) {
		return;
	}
	HeaderAttribute headerAttr = setting.field.GetCustomAttribute<HeaderAttribute>();
	if (headerAttr != null) {
		Header(headerAttr.header);
	}
	bool delayed = setting.field.GetCustomAttribute<DelayedAttribute>() != null;
	if (setting.type == typeof(string)) {
		string stringValue = (string)setting.field.GetValue(setting.obj);
		stringValue = !delayed ?
			EditorGUILayout.TextField(setting.label, stringValue) :
			EditorGUILayout.DelayedTextField(setting.label, stringValue);
		setting.field.SetValue(setting.obj, stringValue);
	} else
	if (setting.type == typeof(int)) {
		int intValue = (int)setting.field.GetValue(setting.obj);
		RangeAttribute rangeAttr = setting.field.GetCustomAttribute<RangeAttribute>();
		if (rangeAttr != null) {
			intValue = EditorGUILayout.IntSlider(setting.label, intValue,
				(int)rangeAttr.min, (int)rangeAttr.max);
		} else {
			intValue = !delayed ?
				EditorGUILayout.IntField(setting.label, intValue) :
				EditorGUILayout.DelayedIntField(setting.label, intValue);
			MinAttribute minAttr = setting.field.GetCustomAttribute<MinAttribute>();
			if (minAttr != null) {
				intValue = intValue < (int)minAttr.min ? (int)minAttr.min : intValue;
			}
		}
		setting.field.SetValue(setting.obj, intValue);
	} else
	if (setting.type == typeof(float)) {
		float floatValue = (float)setting.field.GetValue(setting.obj);
		RangeAttribute rangeAttr = setting.field.GetCustomAttribute<RangeAttribute>();
		if (rangeAttr != null) {
			floatValue = EditorGUILayout.Slider(setting.label, floatValue, rangeAttr.min, 
			  rangeAttr.max);
		} else {
			floatValue = !delayed ?
				EditorGUILayout.FloatField(setting.label, floatValue) :
				EditorGUILayout.DelayedFloatField(setting.label, floatValue);
			MinAttribute minAttr = setting.field.GetCustomAttribute<MinAttribute>();
			if (minAttr != null) {
				floatValue = floatValue < minAttr.min ? minAttr.min : floatValue;
			}
		}
		setting.field.SetValue(setting.obj, floatValue);
	} else
	if (setting.type == typeof(bool)) {
		bool boolValue = (bool)setting.field.GetValue(setting.obj);
		boolValue = EditorGUILayout.Toggle(setting.label, boolValue);
		setting.field.SetValue(setting.obj, boolValue);
	} else
	if (setting.type == typeof(Color)) {
		Color colorValue = (Color)setting.field.GetValue(setting.obj);
		colorValue = EditorGUILayout.ColorField(setting.label, colorValue);
		setting.field.SetValue(setting.obj, colorValue);
	} else
	if (setting.type.IsEnum) {
		Enum enumValue = (Enum)setting.field.GetValue(setting.obj);
		int intValue = Convert.ToInt32(enumValue);
		string[] enumOptions = EnumCache.GetOptions(setting.type);
		Array enumValues = EnumCache.GetValues(setting.type);
		if (!setting.toggle) {
			intValue = EditorGUILayout.IntPopup(setting.label, intValue, enumOptions, 
			(int[])enumValues);
			setting.field.SetValue(setting.obj, intValue);
		} else {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(setting.label);
			int i = ArrayUtility.IndexOf<string>(enumOptions, enumValue.ToString());
			i = GUILayout.Toolbar(i, enumOptions);
			EditorGUILayout.EndHorizontal();
			setting.field.SetValue(setting.obj, i);
		}
	} else
	if (setting.type.IsGenericList()) {
		SettingList(setting);
	} else
	if (typeof(PartSettings).IsAssignableFrom(setting.type)) {
		PartSettings partSettings = (PartSettings)setting.field.GetValue(setting.obj);
		EditorGUI.BeginChangeCheck();
		partSettings.OnGUI(setting);
		if (EditorGUI.EndChangeCheck()) {
			partSettings.OnChange();
		}
	}
	firstElement = false;
}

public static int SettingField(Setting setting, int selectedIndex, string[] options) {
	if (setting == null) {
		return 0;
	}
	int newIndex = EditorGUILayout.Popup(setting.label, selectedIndex, options);
	setting.field.SetValue(setting.obj, options[newIndex]);
	return newIndex;
}

public static void SettingField(Setting setting, Func<string, string> callback) {
	if (setting == null) {
		return;
	}
	TextAreaAttribute textAreaAttr = setting.field.GetCustomAttribute<TextAreaAttribute>();
	if (setting.type == typeof(string)) {
		string stringValue = (string)setting.field.GetValue(setting.obj);
		EditorGUILayout.BeginHorizontal();
		if (textAreaAttr != null) {
			stringValue = EditorGUILayout.TextArea(setting.label, stringValue);
		} else {
			stringValue = EditorGUILayout.TextField(setting.label, stringValue);
		}
		if (GUILayout.Button("...", Styles.buttonStyle)) {
			stringValue = callback?.Invoke(stringValue);
		}
		EditorGUILayout.EndHorizontal();	
		setting.field.SetValue(setting.obj, stringValue);
	}
	firstElement = false;
}

public static bool BeginPart(string title, PartSettings settings) {
	EditorGUILayout.BeginVertical(Styles.groupStyle);
	EditorGUIUtility.labelWidth = 200;
	settings.foldout = EditorGUILayout.Foldout(settings.foldout, title, Styles.groupTitle);
	EditorGUILayout.BeginVertical(settings.foldout ?
		Styles.groupInnerStyle : EditorStyles.inspectorFullWidthMargins);
	return settings.foldout;
}

public static void EndPart() {
	Space(2);
	EditorGUILayout.EndVertical();
	EditorGUILayout.EndVertical();
}

public static void HelpBox(string message, MessageType type = MessageType.Info) {
	EditorStyles.helpBox.richText = true;
	EditorGUILayout.HelpBox(message, type);
	EditorStyles.helpBox.richText = false;
}

// ---------------------------------------------------------------------------------------------
// reorderable list

static Setting setting;
static ReorderableList rlist;

public static void SettingList(Setting setting, ElementCallbackDelegate OnDrawElement = null, 
	float elementHeight = 0) {
	// setup the list
	PaneGUI.setting = setting;
	rlist = rlist ?? new ReorderableList(null, typeof(IList));
	rlist.list = (IList)setting.field.GetValue(setting.obj);
	rlist.headerHeight = 2;
	rlist.elementHeight = elementHeight > 0 ? elementHeight : 
		EditorGUIUtility.singleLineHeight + 4;
	rlist.footerHeight = EditorGUIUtility.singleLineHeight + 3;
	rlist.drawElementCallback = OnDrawElement ?? PaneGUI.OnDrawElement;
	rlist.onAddCallback = OnAddElement;
	rlist.onRemoveCallback = OnRemoveElement;
	rlist.onReorderCallback = OnReorder;
	rlist.drawNoneElementCallback = OnEmptyList;
	rlist.showDefaultBackground = true;
	// draw the reorderable list
	EditorGUILayout.BeginHorizontal();
	EditorGUILayout.PrefixLabel(setting.label);
	GUILayout.BeginVertical(Styles.listStyle);
	GUILayout.Space(1);
	rlist.DoLayoutList();
	GUILayout.EndVertical();
	EditorGUILayout.EndHorizontal();
}

static void OnEmptyList(Rect rect) {
	EditorGUI.LabelField(rect, "No items", EditorStyles.centeredGreyMiniLabel);
}

static void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
	rect.y += 2;
	rect.width += 2;
	rect.height = EditorGUIUtility.singleLineHeight;
	if (setting.type == typeof(string)) {
		rlist.list[index] = EditorGUI.TextField(rect, (string)rlist.list[index]);
	} else
	if (setting.type == typeof(UObject)) {
		rlist.list[index] = EditorGUI.ObjectField(rect, rlist.list[index] as UObject, 
		  setting.type, false);
	} else
	if (setting.type.IsEnum) {
		int intValue = Convert.ToInt32(rlist.list[index]);
		string[] enumOptions = EnumCache.GetOptions(setting.type);
		Array enumValues = EnumCache.GetValues(setting.type);
		intValue = EditorGUI.IntPopup(rect, "", intValue, enumOptions, (int[])enumValues);
		rlist.list[index] = intValue;
	}
}

static void OnAddElement(ReorderableList rlist) {
	if (setting.type.IsGenericList()) {
		Type valueType = setting.type.GetGenericArguments()[0];
		if (valueType.IsEnum) {
			rlist.list.Add(setting.defaultValue);
		} else {
			rlist.list.Add(Activator.CreateInstance(valueType));
		}
	} else {
		rlist.list.Add(setting.defaultValue);
	}
	rlist.index = rlist.list.Count - 1;
	GUI.changed = true;
}

static void OnRemoveElement(ReorderableList rlist) {
	// keep the list focused
	int index = rlist.index;
	if (index == rlist.count - 1) {
		if (rlist.count > 0) {
			rlist.index--;
		}
	}
	rlist.list.RemoveAt(index);
	GUI.changed = true;
}

static void OnReorder(ReorderableList rlist) {
	GUI.changed = true;
}

public static void FocusListElementAt(int index) {
	rlist.index = index;
}

}

}