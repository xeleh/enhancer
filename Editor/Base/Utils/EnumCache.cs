using System;
using System.Collections.Generic;
using UnityEditor;

namespace XT.Base {

internal static class EnumCache {

static Dictionary<Type, Array> enumValuesMap = new Dictionary<Type, Array>();

static Dictionary<Type, string[]> enumOptionsMap = new Dictionary<Type, string[]>();

internal static void Set(Type type, string[] options = null) {
	enumValuesMap[type] = Enum.GetValues(type);
	if (options == null) {
		options = NicefyNames(Enum.GetNames(type));
	}
	enumOptionsMap[type] = options;
}

internal static void Set<T>(string[] options = null) where T : Enum {
	Set(typeof(T), options);
}

static string[] NicefyNames(string[] names) {
	for (int i = 0; i < names.Length; i++) {
		names[i] = ObjectNames.NicifyVariableName(names[i]);
	}
	return names;
}

internal static Array GetValues(Type type) {
	if (!enumValuesMap.ContainsKey(type)) {
		Set(type);
	}
	return enumValuesMap[type];
}

internal static Array GetValues<T>() where T : Enum {
	return GetValues(typeof(T));
}

internal static string[] GetOptions(Type type) {
	if (!enumOptionsMap.ContainsKey(type)) {
		Set(type);
	}
	return enumOptionsMap[type];
}

internal static string[] GetOptions<T>() where T : Enum {
	return GetOptions(typeof(T));
}

}

}