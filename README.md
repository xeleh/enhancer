# Editor Enhancer
A collection of extensions to enhance the Unity Editor.

## Sidebar

The Sidebar is a configurable window that will keep docked on the left or the right side of the main editor window automatically.

This window can be populated with a series of clickable icons (items) each one having an associated one of the following functions:

* **Set Layout**: Load the layout associated with the item as if you would have chosen it from the Layout menu of the top right corner |
* **Execute Menu Item**: Perform the menu action associated with the item. You specify a menu action by indicating its path. As an example, you would specify `File/Save` to save the current scene.
* **Separator**: No action, just a visual separator.

### Settings

You can fully configure the items and the rest of options of the Sidebar in the corresponding "Editor Enhancer" pane in the Project Settings window. This is a quick explanation of the less obvious options:

* **Save Layout On Change**: When enabled your current window layout will be saved before changing to another layout by clicking another Sidebar 'Set Layout' item. This is a nice convenience because "as is" Unity does not save a modified layout automatically but requires you to explicitly use the Save Layout option in the Layout menu.|
* **Play Mode Layout**: You can specify here the layout you want to use during Play mode and the Sidebar will manage the change automatically. Try it.


## Menu Additions

Just some simple but convenient menu scripts that I use frequently in my projects:

| Menu Item | Key | Purpose |
|---|---|---|
| Assets/Duplicate | Ctrl+G ⌘+G |Duplicate the selected asset. I know we can do this already with the Edit/Duplicate option, but this is more convenient because the Duplicate option will be now available in the asset context menu as well. |
| GameObject/Group | Ctrl+D ⌘+D |Creates a new GameObject "Group" containing the current selection of GameObjects. The group will take the place of the last top-level GameObject in the selection. |


