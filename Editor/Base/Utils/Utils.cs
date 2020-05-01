using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace XT.Base {

internal static partial class Utils {

public static Dictionary<string, T> FindObjects<T>() where T : UObject {
	T[] objects = Resources.FindObjectsOfTypeAll<T>();
	var dict = new Dictionary<string, T>();
	foreach (T obj in objects) {
		dict[obj.name] = obj;
	}
	return dict;
}

public static T FindObject<T>() where T : UObject {
	T[] objects = Resources.FindObjectsOfTypeAll<T>();
	return objects.Length > 0 ? objects[0] : null;
}

public static UObject FindObject(Type type) {
	UObject[] objects = Resources.FindObjectsOfTypeAll(type);
	return objects.Length > 0 ? objects[0] : null;
}

}

}
