using System;
using System.Reflection;

namespace XT.Base {

internal class ContainerWindowR : EditorType {

ContainerWindowR() {}

static ContainerWindowR instance = new ContainerWindowR();

object containerWindow;

public static ContainerWindowR Wrap(object containerWindow) {
	instance.containerWindow = containerWindow;
	return instance;
}

static Type type = GetType("ContainerWindow");
static PropertyInfo rootSplitViewProperty = type?.GetProperty("rootSplitView");

public object rootSplitView {
	get => rootSplitViewProperty?.GetValue(containerWindow);
}

}

}
