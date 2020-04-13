using UnityEngine;

namespace XT.Base {

internal static partial class Extensions {

public static Color ToColor(this int rgba) {
	byte r = (byte)((rgba & 0xFF000000) >> 24);
	byte g = (byte)((rgba & 0x00FF0000) >> 16);
	byte b = (byte)((rgba & 0x0000FF00) >> 8);
	byte a = (byte)(rgba & 0x000000FF);
	return new Color32(r, g, b, a);
}

public static int ToRGBA(this Color color) {
	int r = (int)(color.r * 255);
	int g = (int)(color.g * 255);
	int b = (int)(color.b * 255);
	int a = (int)(color.a * 255);
	return r << 24 | g << 16 | b << 8 | a;
}

}

}
