using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using XT.Base;

namespace XT.Enhancer {

[InitializeOnLoad]
internal class Package {

static PackageInfo info = PackageInfo.FindForAssembly(typeof(Package).Assembly);
public static string name = info.name;
public static string displayName = info.displayName;
public static string path = $"Packages/{name}";

static Package() {
	InitSettingsProvider();
	ThemeManager.OnLoad();
	Sidebar.OnLoad();
}

// --------------------------------------------------------------------------------------------

static ProjectSettingsPane settingsPane;

static void InitSettingsProvider() {
	settingsPane = settingsPane ?? new ProjectSettingsPane(displayName, settings);
}

[SettingsProvider]
static SettingsProvider GetSettingsProvider() {
	return settingsPane;
}

static Settings settings_;
public static Settings settings {
	get {
		settings_ = settings_ ?? LoadSettings();
		return settings_;
	}
}

static Settings LoadSettings() {
	string settingsPath = $"{Host.settingsPath}/Packages/{name}/Settings.asset"; 
	return ProjectSettings.GetInstance<Settings>(settingsPath);
}

public static void SaveSettings() {
	settings.Save();
}

}

}
