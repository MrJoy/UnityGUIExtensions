# UnityGUIExtensions

This package provides you with a set of classes providing extended GUI widgets
for Unity -- both for runtime usage and for editor usage --

Widgets (including where they can be used from):

* AutoSelect TextArea (_Runtime & Editor_) - A variant of the TextArea that
  auto-selects all content when the widget gets focus, and which provides
  length-clamping for the Editor variant.

* Toolbar (_Editor_) - Some helpers for drawing toolbars in the same manner Unity
  itself does.

* Vertical Split-Panes (_Editor_) - Vertically stacked split-panes with draggable
  resizing.

* Horizontal Split-Panes (_Editor_) - Horizontally stacked split-panes with
  draggable resizing.


## Requirements

* Unity 3.1 or 3.0.  This will not work on Unity 2.x, or Unity/iPhone 1.x.


## Install

Grab the unitypackage and install it into your project:

    http://github.com/MrJoy/UnityGUIExtensions.Examples/downloads

OR, if you are using Git for your Unity project, you can add this as a sub-module:

    mkdir -p Assets/Editor/
    git submodule add git://github.com/MrJoy/UnityGUIExtensions.git Assets/Editor/UnityGUIExtensions
    git submodule init
    git submodule update


## Source

UnityGUIExtensions' Git repo is available on GitHub, which can be browsed at:

    http://github.com/MrJoy/UnityGUIExtensions

and cloned with:

    git clone git://github.com/MrJoy/UnityGUIExtensions.git


An example Unity project (which refers to this project as a sub-module) is
available here:

    http://github.com/MrJoy/UnityGUIExtensions.Examples

and can be cloned with:

    git clone git://github.com/MrJoy/UnityGUIExtensions.Examples.git
    git submodule init
    git submodule update


## Usage

### AutoSelect TextArea

Instead of:

    myString = GUILayout.TextArea(myString);

You just do:

    myString = GUILayoutAutoSelect.TextArea("uniqueWidgetName", myString);

Variants exist corresponding to all the method signatures of GUI.TextArea,
GUILayout.TextArea, EditorGUI.TextArea, and EditorGUILayout.TextArea.
Additionally, the EditorGUIExt/EditorGUILayoutExt versions have an additional
set of signatures that include the maxLength attribute.

### Vertical Split-Panes

### Toolbars


## Copyright

Copyright (c) 2010 MrJoy, Inc. See LICENSE for details.


## Contributing

If you'd like to contribute to UnityGUIExtensions, I ask that you fork
MrJoy/UnityGUIExtensions on GitHub, and push up a topic branch for each feature
you add or bug you fix.  Then create a pull request and explain what your code
does. This allows us to discuss and merge each change separately.
