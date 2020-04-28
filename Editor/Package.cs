using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using XT.Base;

namespace XT.Enhancer {

internal class Package {

static PackageInfo info = PackageInfo.FindForAssembly(typeof(Package).Assembly);
public static string name = info.name;
public static string displayName = info.displayName;
public static string path = $"Packages/{name}";

[InitializeOnLoadMethod]
static void OnLoad() {
	LoadSettings();
	ThemeManager.OnLoad();
	Sidebar.OnLoad();
}

public static Settings settings { get; private set; }

static void LoadSettings() {
	string settingsPath = $"{Host.settingsPath}/Packages/{name}/Settings.asset"; 
	settings = ProjectSettings.GetInstance<Settings>(settingsPath);
}

public static void SaveSettings() {
	settings.Save();
}

static ProjectSettingsPane settingsPane;

[SettingsProvider]
static SettingsProvider GetSettingsProvider() {
	settingsPane = settingsPane ?? new ProjectSettingsPane(displayName, settings);
	return settingsPane;
}

}

}
