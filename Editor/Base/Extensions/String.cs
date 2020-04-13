using System;

namespace XT.Base {

internal static partial class Extensions {

public static bool IsNullOrEmpty(this string s) {
	return String.IsNullOrEmpty(s);
}

public static bool ContainsIgnoreCase(this string s, string text) {
	return s.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
}

public static bool EqualsIgnoreCase(this string s, string text) {
	return s.Equals(text, StringComparison.CurrentCultureIgnoreCase);
}

}

}
