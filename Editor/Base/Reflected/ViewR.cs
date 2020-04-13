using System;
using System.Reflection;
using UnityEngine;

namespace XT.Base {

internal class ViewR : EditorType {

ViewR() { }

static ViewR instance = new ViewR();

object view;

public static ViewR Wrap(object view) {
	instance.view = view;
	return view != null ? instance : null;
}

static Type type = GetType("View");
static PropertyInfo screenPositionProperty = type?.GetProperty("screenPosition");
static PropertyInfo windowPositionProperty = type?.GetProperty("windowPosition");
static PropertyInfo positionProperty = type?.GetProperty("position");
static FieldInfo parentField = type?.GetNonPublicField("m_Parent");
static MethodInfo reflowMethod = type?.GetNonPublicMethod("Reflow");
static PropertyInfo allChildrenProperty = type?.GetProperty("allChildren");

public Rect screenPosition {
	get => (Rect)screenPositionProperty?.GetValue(view);
}

public Rect windowPosition {
	get => (Rect)windowPositionProperty?.GetValue(view);
}

public Rect position {
	get => (Rect)positionProperty?.GetValue(view);
	set => positionProperty?.SetValue(view, value);
}

public object parent {
	get => parentField?.GetValue(view);
}

public void Reflow() {
	reflowMethod?.Invoke(view, null);
	Host.OnReflow();
}

public object[] allChildren {
	get => (object[])allChildrenProperty?.GetValue(view, null);
}

}

}
