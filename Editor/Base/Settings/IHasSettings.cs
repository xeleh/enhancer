
namespace XT.Base {
	
internal interface IHasSettings {
	
Setting[] GetSettings();

Setting GetSetting(string name);

void Check();

void Load();

void Save();

void Reset();

}

// ---------------------------------------------------------------------------------------------

internal static partial class Extensions {
	
internal static void Save(this IHasSettings settings) {
	settings.Save();
}

}

}

