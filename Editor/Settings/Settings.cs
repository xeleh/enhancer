using XT.Base;

namespace XT.Enhancer {

internal partial class Settings : ProjectSettings {

[Setting("Theme")]
public ThemeSettings theme = new ThemeSettings();

[Setting("Sidebar")]
public SidebarSettings sidebar = new SidebarSettings();

}

}
