using UnityEngine;
using UnityEditor;

namespace XT.Base {

internal static class ProgressWindow {

static string prefix = $"{typeof(ProgressWindow).FullName}";
static string titleKey = $"{prefix}.title";
static string messageKey = $"{prefix}.message";
static string totalKey = $"{prefix}.total";
static string currentKey = $"{prefix}.current";

public static void Init(string title, string message, int total) {
	SessionState.SetString(titleKey, title);
	SessionState.SetString(messageKey, message);
	SessionState.SetInt(totalKey, total);
	SessionState.SetInt(currentKey, 0);
	EditorUtility.DisplayProgressBar(title, message, 0);
}

public static void Expect(int steps) {
	int total = SessionState.GetInt(totalKey, 0);
	SessionState.SetInt(totalKey, total + steps);
}

public static void Update(int increment = 1) {
	string title = 	SessionState.GetString(titleKey, "");
	string message = SessionState.GetString(messageKey, "");
	int current = SessionState.GetInt(currentKey, 0) + increment;
	int total = SessionState.GetInt(totalKey, 0);
	float progress = Mathf.InverseLerp(0, total, current);
	EditorUtility.DisplayProgressBar(title, message, progress);
	SessionState.SetInt(currentKey, current);
}

public static void Update(string message, int increment = 0) {
	SessionState.SetString(messageKey, message);
	Update(increment);
}

public static void Hide() {
	EditorUtility.ClearProgressBar();
}

}

}
