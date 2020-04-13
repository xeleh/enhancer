using System;
using System.Reflection;

namespace XT.Base {

internal class WindowLayoutR : EditorType {

WindowLayoutR() { }

static Type type = GetType("WindowLayout");
static PropertyInfo layoutsPreferencesPathProperty = 
	type?.GetNonPublicProperty("layoutsPreferencesPath");
static PropertyInfo layoutsDefaultModePreferencesPathProperty = 
	type?.GetNonPublicProperty("layoutsDefaultModePreferencesPath");
static MethodInfo GetCurrentLayoutPathMethod = type?.GetNonPublicMethod("GetCurrentLayoutPath");
static MethodInfo SaveWindowLayoutMethod = type?.GetMethod("SaveWindowLayout");
static MethodInfo FindMainViewMethod = type?.GetNonPublicMethod("FindMainView");

public static string layoutsModePreferencesPath =
	(string)layoutsPreferencesPathProperty?.GetValue(null);

public static string layoutsDefaultModePreferencesPath =
	(string)layoutsDefaultModePreferencesPathProperty?.GetValue(null);

public static string GetCurrentLayoutPath() {
	return (string)GetCurrentLayoutPathMethod?.Invoke(null, null );
}

public static void SaveWindowLayout(string layoutPath) {
	SaveWindowLayoutMethod?.Invoke(null, new object[] { layoutPath } );
}

public static object FindMainView() {
	return FindMainViewMethod?.Invoke(null, null);
}

}

}
