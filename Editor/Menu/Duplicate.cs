using UnityEditor;

namespace XT.Enhancer {

static partial class MenuAdditions {

[MenuItem("Assets/Duplicate", true)]
static bool ValidateDuplicate() {
	return Selection.assetGUIDs.Length > 0;
}

[MenuItem("Assets/Duplicate", false, 19)]
static void Duplicate() {
	EditorApplication.ExecuteMenuItem("Edit/Duplicate");
}

}

}
