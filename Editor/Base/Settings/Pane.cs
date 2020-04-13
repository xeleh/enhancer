using System;
using UnityEditor;
using UnityEngine;

namespace XT.Base {

internal class Pane : SettingsProvider {

public event Action OnChange;

IHasSettings settingsContainer;

protected Pane(string label, string path, SettingsScope scope, IHasSettings settingsContainer) : 
base(path, scope) {
	this.label = label;
	this.settingsContainer = settingsContainer;
	settingsContainer?.Load();
}

public Setting GetSetting(string name) {
	return settingsContainer?.GetSetting(name);
}

public Setting[] GetSettings() {
	return settingsContainer?.GetSettings();
}

public override void OnGUI(string searchContext) {
	settingsContainer?.Check();
	EditorGUILayout.BeginVertical(Styles.paneStyle);
	EditorGUIUtility.labelWidth = 250;
	PaneGUI.DrawSettings(settingsContainer?.GetSettings());
	OnFooterGUI();
	EditorGUILayout.EndVertical();
	if (GUI.changed) {
		settingsContainer?.Save();
		OnChange?.Invoke();
	}
}

// the default footer GUI is not coherent with the rest of providers, so disable it
public override sealed void OnFooterBarGUI() { }

protected virtual void OnFooterGUI() { }

protected void Reset() {
	settingsContainer?.Reset();
	OnReset();
	Repaint();
	OnChange?.Invoke();
}

protected virtual void OnReset() { }

}

}