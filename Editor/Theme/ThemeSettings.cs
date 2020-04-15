using System;
using XT.Base;

namespace XT.Enhancer {

[Serializable]
internal class ThemeSettings : PartSettings {

[Setting("Enable Dark Theme")]
public bool darkEnabled = false;

public override void OnChange() {
	if (darkEnabled != ThemeManager.darkEnabled) {
		ThemeManager.SetTheme();
	}
}

}

}
