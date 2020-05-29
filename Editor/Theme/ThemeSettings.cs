using System;
using UnityEditor;
using UnityEngine;
using XT.Base;

namespace XT.Enhancer {

[Serializable]
internal class ThemeSettings : PartSettings {

[Setting("Enable Dark Theme")]
public bool darkEnabled = false;

[Setting("Auto Enable On Startup")]
public bool autoEnable = false;

public override void OnGUI(Setting setting) {
	if (!ThemeManager.supported) { return; }
	bool guiEnabled = GUI.enabled;
	GUI.enabled = !Application.isPlaying;
	base.OnGUI(setting);
	GUI.enabled = guiEnabled;
}

public override void OnChange() {
	if (darkEnabled != ThemeManager.darkEnabled) {
		EditorApplication.delayCall += () => { ThemeManager.SetTheme(); };
	}
}

}

}
