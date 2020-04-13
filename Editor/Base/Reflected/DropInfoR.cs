using System;
using System.Reflection;

namespace XT.Base {

internal class DropInfoR : EditorType {

DropInfoR() { }

static DropInfoR instance = new DropInfoR();

object dropInfo;

public static DropInfoR Wrap(object dropInfo) {
	instance.dropInfo = dropInfo;
	return instance;
}

static Type type = GetType("DropInfo");
static FieldInfo dropAreaField = type?.GetField("dropArea");

public object dropArea {
	get => dropAreaField.GetValue(dropInfo);
}

}

}
