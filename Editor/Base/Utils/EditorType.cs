using System;
using System.Reflection;
using UnityEditor;

namespace XT.Base {

internal class EditorType {

protected static Assembly assembly = Assembly.GetAssembly(typeof(Editor));

public static Type GetType(string typeName) {
	return assembly.GetType($"UnityEditor.{typeName}");
}

}

}
