using UObject = UnityEngine.Object;

namespace XT.Base {

internal static partial class Extensions {

public static void DestroyImmediate(this UObject obj) {
	if (obj != null) {
		UObject.DestroyImmediate(obj);
	}
}

}

}
