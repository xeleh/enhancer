using UnityEngine;

namespace XT.Base {

internal static partial class Extensions {

public static Texture2D ReadableClone(this Texture2D source) {
	RenderTexture render = RenderTexture.GetTemporary(source.width, source.height, 0,
		RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
	Graphics.Blit(source, render);
	RenderTexture previous = RenderTexture.active;
	RenderTexture.active = render;
	Texture2D readable = new Texture2D(source.width, source.height);
	readable.ReadPixels(new Rect(0, 0, render.width, render.height), 0, 0);
	readable.Apply();
	RenderTexture.active = previous;
	RenderTexture.ReleaseTemporary(render);
	return readable;
}

}

}
