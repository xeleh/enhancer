using System;
using System.Reflection;

namespace XT.Base {

internal class EditorResourcesR : EditorType {

EditorResourcesR() { }

static Type type = GetType("Experimental.EditorResources");

internal class Constants : EditorType {

Constants() { }

static Type type = GetType("Experimental.EditorResources+Constants");
static FieldInfo isDarkThemeField = type?.GetField("isDarkTheme");

public static bool isDarkTheme {
	set => isDarkThemeField?.SetValue(null, value);
	get => (bool)isDarkThemeField?.GetValue(null);
}

}

}

}
