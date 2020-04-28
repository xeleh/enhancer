using System;
using UnityEngine;

namespace XT.Enhancer {

[Serializable]
internal class SidebarItem {

public enum Function {
	SetLayout,
	ExecuteMenuItem,
	Separator
}

public Texture2D icon;
public Function function;
public string layoutName = "";
public string menuPath = "";

}

}
