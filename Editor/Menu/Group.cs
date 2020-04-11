using UnityEditor;
using UnityEngine;

namespace XT.Enhancer {

static partial class MenuAdditions {

static bool done;

[MenuItem("GameObject/Group %g", true, 0)]
static bool ValidateGroup() {
	done = false;
	return Selection.transforms.Length > 0;
}

[MenuItem("GameObject/Group %g", false, 0)]
static void Group() {
	// prevent multiple execution when invoked via context menu
	if (done) {	return;	}
	// get the last top-level transform in selection
	Transform top = Selection.transforms[0];
	foreach (Transform t in Selection.transforms) {
		if (t != top && (t.parent == null || !t.IsChildOf(top.parent))) {
			if (t.parent != top.parent || t.GetSiblingIndex() > top.GetSiblingIndex()) {
				top = t;
			}
		}
	}
	int siblingIndex = top.GetSiblingIndex();
	// create the group root gameobject
	const string newGroupName = "Group";
	GameObject groupRoot = new GameObject(newGroupName);
	const string undoText = "Group";
	Undo.RegisterCreatedObjectUndo(groupRoot, undoText);
	Selection.transforms[0].GetSiblingIndex();
	Undo.SetTransformParent(groupRoot.transform, top.parent, undoText);
	// calculate the group root new position (average point)
	Vector3 averagePoint = Vector3.zero;
	foreach (Transform t in Selection.transforms) {
		averagePoint += t.position;
	}
	averagePoint /= Selection.transforms.Length;
	groupRoot.transform.position = averagePoint;
	// re-parent transforms in selection
	foreach (Transform t in Selection.transforms) {
		Undo.SetTransformParent(t, groupRoot.transform, undoText);
	}
	groupRoot.transform.SetSiblingIndex(siblingIndex);
	Selection.activeGameObject = groupRoot;
	done = true;
}

}

}
