using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XT.Base {

internal class SplitViewR : EditorType {

SplitViewR() { }

static SplitViewR instance = new SplitViewR();

object splitView;

public static SplitViewR Wrap(object splitView) {
	instance.splitView = splitView;
	return instance;
}

public static readonly Type type = GetType("SplitView");
static MethodInfo DragOverRootViewMethod = type?.GetMethod("DragOverRootView");
static MethodInfo PerformDropMethod = type?.GetMethod("PerformDrop");
static FieldInfo verticalField = type?.GetField("vertical");
static MethodInfo PlaceViewMethod = type?.GetNonPublicMethod("PlaceView");

public object DragOverRootView(Vector2 screenPoint) {
	return DragOverRootViewMethod?.Invoke(splitView, new object[] { screenPoint });
}

public void PerformDrop(EditorWindow window, object dropInfo, Vector2 screenPoint) {
	PerformDropMethod?.Invoke(splitView, new object[] { window, dropInfo, screenPoint });
}

public bool vertical => (bool)verticalField?.GetValue(splitView);

public float PlaceView(int i, float pos, float size) {
	return (float)PlaceViewMethod?.Invoke(splitView, new object[] { i, pos, size });
}

}

}
