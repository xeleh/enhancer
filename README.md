# Editor Enhancer
[![openupm](https://img.shields.io/npm/v/com.xeleh.enhancer?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.xeleh.enhancer/)

The Editor Enhancer package is a collection of utilities aimed to enhance the Unity Editor. See the [roadmap](#roadmap) section for more information on what is coming next.

### Requirements

Unity 2019.3.0 or newer. Starting in Unity 2019.3.15 the Dark Theme feature is disabled.

### Installation

***Via Git URL***

Open the Package Manager window and follow the [instructions to install a package from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html). This is the URL you need to enter:

```
https://github.com/xeleh/enhancer.git
```

***Via OpenUPM***

The package is available on the [openupm registry](https://openupm.com/packages/com.xeleh.enhancer/). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli#openupm-cli).

```
openupm add com.xeleh.enhancer
```

## Dark Theme

This is an option in Project Settings > Editor Enhancer to replace the personal Editor theme with the "Pro Editor UI theme" which is only available in [Unity Plus and Pro plans](https://store.unity.com/#plans-business). Unity defines this theme as a "Beautiful, easy on the eyes, dark UI environment". So yes, having this is pretty good.

![](https://xeleh.com/media/dark-theme.gif)

Until the release of Unity 2019.3.15 the feature was working with all Unity 2019.3.x versions and also with the first Unity 2020.1 betas. This was supposed to be a legal hack implying no binary patching and based on a technique that even got [some kind of an official approval](https://forum.unity.com/threads/editor-skinning-thread.711059/page-2#post-5620048), but short after this package was made public Unity decided to make some changes in the code to specifically [prevent this package from bypassing their entitlement](https://issuetracker.unity3d.com/issues/enhancer-package-bypasses-dark-theme-entitlement).

### Troubleshooting

After enabling the dark theme you could find a few windows (like Timeline) and 3rd party plugins still showing non darken parts. These cases are special and require [adapting the code for detecting our dark theme](Adapting.md).

Please make sure to leave the 'Auto Enable On Startup' unchecked until you verify that the dark theme change works for your particular Unity configuration as expected.


## Sidebar

The Sidebar is a configurable utility window that will keep docked on the left or the right side of the main editor window automatically. Its purpose is to host a series of clickable icons (items) each one of them having an associated one of the following functions:

* **Set Layout**: Load the layout associated with the item as if you would have chosen it from the Layout menu of the top right corner.
* **Execute Menu Item**: Perform the menu action associated with the item. You specify a menu action by indicating its menu path. As an example, you would specify `File/Save` to save the current scene with one click.
* **Separator**: No action, just a visual separator.

### Settings

You can fully configure the items and the rest of options of the Sidebar in the Project Settings > Editor Enhancer pane. This is a quick explanation of the less obvious options:

* **Save Layout On Change**: When enabled your current window layout will be saved before changing to another layout by clicking another Sidebar 'Set Layout' item. This is a nice convenience because "as is" Unity does not save a modified layout automatically but requires you to explicitly use the Save Layout option in the Layout menu.
* **Play Mode Layout**: You can specify here the layout you want to use during Play mode and the Sidebar will manage the change automatically. Try it.


## Menu Additions

Just some simple but convenient menu scripts that I use frequently in my projects:

| Menu Item | Key | Purpose |
|---|---|---|
| Assets/Duplicate | Ctrl+D ⌘+D |Duplicate the selected asset. I know we can do this already with the Edit/Duplicate option, but this is more convenient because the Duplicate option will be now available in the asset context menu as well. |
| GameObject/Group | Ctrl+G ⌘+G |Creates a new GameObject "Group" containing the current selection of GameObjects. The group will take the place of the last top-level GameObject in the selection. |

## Roadmap

* Double Click manager.
* Enter/Exit play mode actions.
* Overlay toolbars?
* Dockable Build Window?
* Customizable Toolbar?
* Notes?
* Notification Centre?

## Acknowledgements

**Dark Theme**: Thanks to TheZombieKiller, Peter77, Grimreaper358, Kamyker and everyone who contributed with code and ideas to the [Editor skinning thread](https://forum.unity.com/threads/editor-skinning-thread.711059/) in the Unity forums. You gave the hints and inspired me to complete the work.

