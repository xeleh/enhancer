using UnityEditor;
using UnityEngine;

namespace XT.Base {

internal class PreferencesPane : Pane {

protected internal PreferencesPane(string label, Preferences preferences) : 
base(label, "Preferences/" + label, SettingsScope.User, preferences) { }

protected override void OnFooterGUI() {
	PaneGUI.Space(5);
	if (GUILayout.Button("Use Defaults", GUILayout.Width(120))) {
		GUI.FocusControl("");
		Reset();
		GUI.changed = true;
	}
}

}

}