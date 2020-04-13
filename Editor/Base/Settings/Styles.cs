using UnityEditor;
using UnityEngine;

namespace XT.Base {

internal static class Styles {

static GUIStyle _pane;
internal static GUIStyle paneStyle {
	get {
		_pane = _pane ?? new GUIStyle(EditorStyles.inspectorFullWidthMargins);
		_pane.padding = new RectOffset(10, 2, 10, 0);
		return _pane;
	}
}

static GUIStyle _group;
internal static GUIStyle groupStyle {
	get {
		_group = _group ?? new GUIStyle(EditorStyles.helpBox);
		_group.padding = new RectOffset(6, 6, 6, 4);
		_group.margin = new RectOffset(3, 7, 2, 2);
		return _group;
	}
}

static GUIStyle _groupTitle;
internal static GUIStyle groupTitle {
	get {
		_groupTitle = _groupTitle ?? new GUIStyle(EditorStyles.foldout);
		_groupTitle.fontStyle = FontStyle.Bold;
		_groupTitle.padding = new RectOffset(16, 6, 4, 4);
		_groupTitle.normal.textColor = EditorResources.textColor;
		_groupTitle.onNormal.textColor = EditorResources.textColor;
		return _groupTitle;
	}
}

static GUIStyle _groupInner;
internal static GUIStyle groupInnerStyle {
	get {
		_groupInner = _groupInner ?? new GUIStyle(EditorStyles.inspectorFullWidthMargins);
		_groupInner.padding = new RectOffset(0, 0, 4, 4);
		return _groupInner;
	}
}

static GUIStyle _subtext;
internal static GUIStyle subtextStyle {
	get {
		_subtext =	_subtext ?? new GUIStyle(EditorStyles.label);
		_subtext.alignment = TextAnchor.MiddleRight;
		return _subtext;
	}
}

static GUIStyle _button;
internal static GUIStyle buttonStyle {
	get {
		_button = _button ?? new GUIStyle(EditorStyles.miniButton);
		_button.alignment = TextAnchor.MiddleCenter;
		_button.margin = new RectOffset(0, 4, 2, 0);
		_button.padding = new RectOffset(2, 0, 0, 0);
		_button.fixedWidth = 20;
		_button.fixedHeight = 18;
		return _button;
	}
}

static GUIStyle _list;
internal static GUIStyle listStyle {
	get {
		_list =	_list ?? new GUIStyle(EditorStyles.inspectorFullWidthMargins);
		_list.padding = new RectOffset(3, 3, 0, 0);
		return _list;
	}
}

static GUIStyle _iconButton;
public static GUIStyle iconButton {
	get {
		_iconButton = _iconButton ?? new GUIStyle("IconButton");
		_iconButton.margin = new RectOffset(0, 3, 6, 0);
		return _iconButton;
	}
}


}

}
