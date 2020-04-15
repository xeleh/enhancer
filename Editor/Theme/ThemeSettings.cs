using System;
using UnityEditor;
using XT.Base;

namespace XT.Enhancer {

[Serializable]
internal class ThemeSettings : PartSettings {

[Setting("Enable Dark Theme")]
public bool darkEnabled = false;

public override void Reset() {
	OnChange();
}

public override void OnChange() {
	if (darkEnabled != ThemeManager.darkEnabled) {
		EditorApplication.delayCall += () => { ThemeManager.SetTheme(); };
	}
}

}

}
