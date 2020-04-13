using UnityEditor;

namespace XT.Enhancer {

internal static partial class Resources {

public static T Load<T>(string path) where T : UnityEngine.Object {
	return AssetDatabase.LoadAssetAtPath<T>($"{Package.path}/Editor/Resources/{path}");
}

}

}
