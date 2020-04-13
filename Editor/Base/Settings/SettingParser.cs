using System;
using System.Collections.Generic;
using System.Reflection;

namespace XT.Base {

public static class SettingParser {

internal static Dictionary<string, Setting> Parse<T>(object target = null) {
	return Parse(typeof(T), target);
}

internal static Dictionary<string, Setting> Parse(Type type, object target = null) {
	FieldInfo[] fields = type.GetAllFields();
	var dict = new Dictionary<string, Setting>();
	foreach (FieldInfo field in fields) {
		// skip if field has no a [Setting] attribute
		Setting setting = field.GetCustomAttribute<Setting>();
		if (setting == null) {
			continue;
		}
		// fill setting data
		setting.obj = target;
		setting.field = field;
		setting.name = field.Name;
		setting.type = field.FieldType;
		setting.label = setting.label ?? setting.name;
		setting.defaultValue = field.GetValue(target);
		dict[setting.name] = setting;
	}
	return dict; 
}

}

}
