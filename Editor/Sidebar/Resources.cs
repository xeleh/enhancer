using UnityEngine;

namespace XT.Enhancer {

internal static partial class Resources {

static GUIStyle iconStyle_;
public static GUIStyle iconStyle {
	get {
		iconStyle_ = iconStyle_ ?? new GUIStyle("iconButton");
		iconStyle_.fixedWidth = 32;
		iconStyle_.fixedHeight = 32;
		return iconStyle_;
	}
}

static GUIStyle separatorStyle_;
public static GUIStyle separatorStyle {
	get {
		separatorStyle_ = separatorStyle_ ?? new GUIStyle("label");
		separatorStyle_.fixedWidth = 32;
		separatorStyle_.fixedHeight = 32;
		return separatorStyle_;
	}
}

static Texture2D homeIcon_;
public static Texture2D homeIcon {
	get {
		homeIcon_ = homeIcon_ ?? 
			Load<Texture2D>("Icons/home@2x.png") ?? Texture2D.blackTexture;
		return homeIcon_;
	}
}

static Texture2D separatorIcon_;
public static Texture2D separatorIcon {
	get {
		separatorIcon_ = separatorIcon_ ?? 
			Load<Texture2D>("Icons/separator@2x.png") ?? Texture2D.blackTexture;
		return separatorIcon_;
	}
}

static Texture2D settingsIcon_;
public static Texture2D settingsIcon {
	get {
		settingsIcon_ = settingsIcon_ ?? 
			Load<Texture2D>("Icons/settings@2x.png") ?? Texture2D.blackTexture;
		return settingsIcon_;
	}
}

}

}
