using UnityEngine;
using UnityEditor;

namespace XT.Base {

internal static class EditorResources {

public static Color textColor => EditorStyles.label.normal.textColor;

static Texture2D _gearIcon;
public static Texture2D gearIcon => 
	_gearIcon ?? (_gearIcon = EditorGUIUtility.FindTexture("_Popup"));

static Texture2D _helpIcon;
public static Texture2D helpIcon => 
	_helpIcon ?? (_helpIcon = EditorGUIUtility.FindTexture("_Help"));

static GUIStyle _dockHeaderStyle;
public static GUIStyle dockHeaderStyle =>
	_dockHeaderStyle ?? (_dockHeaderStyle = new GUIStyle("dockHeader"));

static GUIStyle _whiteBackground;
public static GUIStyle whiteBackground => 
	_whiteBackground ?? (_whiteBackground = new GUIStyle("TabWindowBackground"));

}

}