using System;
using System.Reflection;
using UnityEngine;

namespace XT.Base {

internal class DockAreaR : EditorType {

DockAreaR() { }

static DockAreaR instance = new DockAreaR();

object dockArea;

public static DockAreaR Wrap(object dockArea) {
	instance.dockArea = dockArea;
	return instance;
}

public static readonly Type type = GetType("DockArea");
static FieldInfo dragSourceField = type?.GetNonPublicField("s_OriginalDragSource");
static PropertyInfo windowProperty = type?.GetProperty("window");
static MethodInfo SetActualViewPositionMethod = type?.GetMethod("SetActualViewPosition");

public static object dragSource {
	get => dragSourceField.GetValue(null);
	set => dragSourceField.SetValue(null, value);
}

public object window {
	get => windowProperty?.GetValue(dockArea);
}

public void SetActualViewPosition(Rect rect) {
	SetActualViewPositionMethod?.Invoke(dockArea, new object[] { rect } );
}

}

}
