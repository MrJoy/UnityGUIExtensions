UnityGUIExtensions
==================

This package provides you with a set of classes in the spirit of GUI, GUILayout,
EditorGUI, and EditorGUILayout but with additional functionality that I've found
helpful.  Right now, that's not a lot, but I present this to you anyway.

Widgets:

* AutoSelectTextArea - A variant of the TextArea that auto-selects all content 
when the widget gets focus, and which provides length-clamping for the Editor
variant.

## Requirements #############################################################

* Unity 2.6.0.  This is unlikely to work with Unity/iPhone, without at least 
considerable modification.


## Install ##################################################################

Grab the unitypackage and install it into your project, OR add this project as
a submodule if you're using git.

You can grab the unitypackage here:

    http://github.com/MrJoy/UnityGUIExtensions.Examples/downloads


## Source ###################################################################

UnityGUIExtensions' Git repo is available on GitHub, which can be browsed at:

    http://github.com/MrJoy/UnityGUIExtensions

and cloned with:

    git clone git://github.com/MrJoy/UnityGUIExtensions.git


An example project is available here:

    http://github.com/MrJoy/UnityGUIExtensions.Examples

and can be cloned with:

    git clone git://github.com/MrJoy/UnityGUIExtensions.Examples.git


### Contributing

If you'd like to contribute to UnityGUIExtensions, I ask that you fork 
MrJoy/UnityGUIExtensions on GitHub, and push up a topic branch for each feature 
you add or bug you fix.  Then create an issue and link to the topic branch and 
explain what the code does. This allows us to discuss and merge each change 
separately.


## Usage ####################################################################

Instead of:

    myString = GUILayout.TextArea(myString);

You just do:

    myString = GUILayoutExt.AutoSelectTextArea("uniqueWidgetName", myString);

Variants exist corresponding to all the method signatures of GUI.TextArea, 
GUILayout.TextArea, EditorGUI.TextArea, and EditorGUILayout.TextArea.  
Additionally, the EditorGUIExt/EditorGUILayoutExt versions have an additional 
set of signatures that include the maxLength attribute.


Copyright
---------

Copyright (c) 2010 MrJoy, Inc. See LICENSE for details.
