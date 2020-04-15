using System;
using System.Reflection;
using UnityEngine;

namespace XT.Base {

internal class EditorGUIUtilityR : EditorType {

EditorGUIUtilityR() {}

static Type type = typeof(UnityEditor.EditorGUIUtility);
static MethodInfo GetEditorAssetBundleMethod = type.GetNonPublicMethod("GetEditorAssetBundle");
	
public static AssetBundle GetEditorAssetBundle() {
	return (AssetBundle)GetEditorAssetBundleMethod?.Invoke(null, null);
}

}

}
