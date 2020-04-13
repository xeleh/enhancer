using System;
using System.Reflection;

namespace XT.Base {

[AttributeUsage(AttributeTargets.Field)]
internal class Setting : Attribute {

public object obj;
public FieldInfo field;
public string name;
public Type type;
public string label = null;
public object defaultValue;
public bool toggle;

public Setting(string label = null, bool toggle = false) {
	this.label = label;
	this.toggle = toggle;
}

}

}