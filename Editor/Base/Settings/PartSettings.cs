using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace XT.Base {

internal class PartSettings : IHasSettings {

internal Setting[] settings;
readonly Dictionary<string, Setting> dict = new Dictionary<string, Setting>();

public PartSettings() {
	dict = SettingParser.Parse(GetType(), this);
	settings = dict.Values.ToArray();
}

public Setting[] GetSettings() {
	return settings;
}

public Setting GetSetting(string name) {
	dict.TryGetValue(name, out Setting setting);
	return setting;
}

public virtual void Check() { }

public virtual void Load() { }

public virtual void Save() { }

public virtual void Reset() { }

[SerializeField]
public bool foldout = true;

public virtual void OnGUI(Setting setting) {
	if (PaneGUI.BeginPart(setting.label, this)) {
		PaneGUI.DrawSettings(settings);
	}
	PaneGUI.EndPart();
}

public virtual void OnChange() { }

}

}
