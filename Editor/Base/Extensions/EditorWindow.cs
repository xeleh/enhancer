using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XT.Base {

internal static partial class Extensions {

class EditorWindowR : EditorType {

static Type type = typeof(EditorWindow);
#if !UNITY_2020_1_OR_NEWER
public static PropertyInfo dockedProperty = type.GetNonPublicProperty("docked");
#endif
public static FieldInfo parentField = type.GetNonPublicField("m_Parent");

}

public static object GetParent(this EditorWindow window) {
	return EditorWindowR.parentField?.GetValue(window);
}

public static bool IsDocked(this EditorWindow window) {
	#if !UNITY_2020_1_OR_NEWER
	return (bool)EditorWindowR.dockedProperty?.GetValue(window);
	#else
	return window.docked;
	#endif
}

public static Rect GetPosition(this EditorWindow window) {
	object dockArea = window.GetParent();
	return (Rect)ViewR.Wrap(dockArea).position;
}

public static Rect GetWindowPosition(this EditorWindow window) {
	object dockArea = window.GetParent();
	return (Rect)ViewR.Wrap(dockArea).windowPosition;
}

public static float GetWidth(this EditorWindow window) {
	object dockArea = window.GetParent();
	Rect p = (Rect)ViewR.Wrap(dockArea).windowPosition;
	return p.width;
}

public static void SetWidth(this EditorWindow window, float width) {
	object dockArea = window.GetParent();
	Rect p = (Rect)ViewR.Wrap(dockArea).position;
	p.width = width;
	ViewR.Wrap(dockArea).position = p;
	object dockAreaParent = ViewR.Wrap(dockArea).parent;
	ViewR.Wrap(dockAreaParent).Reflow();
}

public static bool IsFloating(this EditorWindow window) {
	object parent = window.GetParent();
	if (parent == null) {
		return true;
	}
	parent = ViewR.Wrap(parent).parent;
	if (parent == null) {
		return true;
	}
	parent = ViewR.Wrap(parent).parent;
	return parent == null;
}

}

}
