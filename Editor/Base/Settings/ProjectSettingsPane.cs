using UnityEditor;
using UnityEngine;

namespace XT.Base {

internal class ProjectSettingsPane : Pane {

protected internal ProjectSettingsPane(string label, IHasSettings settings) : 
base(label, "Project/" + label, SettingsScope.Project, settings) { }

public override void OnTitleBarGUI() {
	// help button
	if (GUILayout.Button(EditorResources.helpIcon, Styles.iconButton)) {
		OnHelp();
	}
	// settings button
	Rect settingsButtonRect = GUILayoutUtility.GetLastRect();
	if (GUILayout.Button(EditorResources.gearIcon, Styles.iconButton)) {
		settingsButtonRect.x += 19;
		OnSettings(settingsButtonRect);
	}
}

protected virtual void OnHelp() {
	EditorUtility.DisplayDialog(label, "No help available.", "Ok"); 
}

protected virtual void OnSettings(Rect rect) {
	GenericMenu menu = new GenericMenu();
	GUIContent resetLabel = new GUIContent("Reset");
	menu.AddItem(resetLabel, false, () => Reset());
	menu.DropDown(rect);
}

protected override void OnReset() {
	foreach (Setting setting in GetSettings()) {
		if (typeof(PartSettings).IsAssignableFrom(setting.type)) {
			PartSettings partSettings = (PartSettings)setting.field.GetValue(setting.obj);
			partSettings.Reset();
			partSettings.OnChange();
		}
	}
}

}

}