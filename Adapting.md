## Adapting "rebel" cases

After enabling the dark theme you could find a few windows (like Timeline) and 3rd party plugins still showing some non darken parts. The reason for this is the use of the `EditorGUIUtility.isProSkin` property in those codes to setup their styles conditionally, a pretty common practice in editor scripting.

The only proven solution we have at the moment for this is to modify those codes so that they also check whether our dark theme is activated or not. To accomplish this we only need to find all the references to the above mentioned property and append the additional check `EditorPrefs.GetBool("darkTheme")` to them.

Example:

``` if (EditorGUIUtility.isProSkin) { ... } ```

Becomes:

``` if (EditorGUIUtility.isProSkin || EditorPrefs.GetBool("darkTheme")) { ... } ```

Of course, this is just a simple example. We are not considering performance or best coding practices here, but only showing that we are using EditorPrefs to enable a simple "communication" without having to create any hard dependencies between the code of those plugins and the Editor Enhancer package.

A word of caution, though: in case that you update any of the adapted plugins you will have to repeat the process or just deal with merging the changes. That sounds bad, but it is not a big deal in the practice. Just as an example, adapting the Timeline package to properly work with our dark theme is just a matter of modifying a total of 9 lines of code. This is the result:

![](https://user-images.githubusercontent.com/148356/80822781-ba6c9b80-8bdb-11ea-9673-792a1358d37f.png)

