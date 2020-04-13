using System;
using System.Reflection;

namespace XT.Base {

internal class ToolbarR : EditorType {

ToolbarR() { }

static Type type = GetType("Toolbar");
static FieldInfo getField = type?.GetField("get");
static FieldInfo m_LastLoadedLayoutNameField = 
	type?.GetNonPublicField("m_LastLoadedLayoutName");
	
public static string m_LastLoadedLayoutName {
	get {
		object toolbar = getField?.GetValue(null);
		return (string)m_LastLoadedLayoutNameField?.GetValue(toolbar);
	}
}

}

}
