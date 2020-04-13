using System;
using System.Collections.Generic;
using System.Reflection;
using BF = System.Reflection.BindingFlags;

namespace XT.Base {

internal static partial class Extensions {

public static FieldInfo GetNonPublicField(this Type type, string name) {
	return type.GetField(name, BF.Instance | BF.Static | BF.NonPublic);
}

public static PropertyInfo GetNonPublicProperty(this Type type, string name) {
	return type.GetProperty(name, BF.Instance | BF.Static | BF.NonPublic);
}

public static MethodInfo GetNonPublicMethod(this Type type, string name) {
	return type.GetMethod(name, BF.Instance | BF.Static | BF.NonPublic);
}

public static MethodInfo GetNonPublicMethod(this Type type, string name, Type[] types) {
	return type.GetMethod(name, BF.Instance | BF.Static | BF.NonPublic, null, types, null);
}

public static bool IsGenericList(this Type type) {
	return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
}

public static FieldInfo[] GetAllFields(this Type type) {
	return type.GetFields(BF.Instance | BF.Static | BF.Public| BF.NonPublic);
}


}

}
