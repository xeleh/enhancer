using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace XT.Enhancer {

internal class Package {

public static PackageInfo info = PackageInfo.FindForAssembly(typeof(Package).Assembly);

static Package() {
}

}

}
