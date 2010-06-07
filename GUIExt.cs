using UnityEngine;

// Extensions in the spirit/style of GUILayout.*
public class GUILayoutExt : GUICommon {
  public static string AutoSelectTextArea(string name, string text, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, options); });
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, maxLength, options); });
  }

  public static string AutoSelectTextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, style, options); });
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, maxLength, style, options); });
  }

  // Don't allow instantiation of this class...
  protected GUILayoutExt() {}
}

// Extensions in the spirit/style of GUI.*
public class GUIExt : GUICommon {
  public static string AutoSelectTextArea(string name, Rect pos, string text) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text); });
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text, maxLength); });
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, GUIStyle style) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text, style); });
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text, maxLength, style); });
  }

  // Don't allow instantiation of this class...
  protected GUIExt() {}
}

// Functionality shared by both GUIExt and GUILayoutExt.
public class GUICommon {
  protected GUICommon() {}
  // Using delegates to DRY up the various parametric polymorphisms that Unity provides.
  protected delegate string Block();


  // Internal gubbins for auto-select controls.
  protected static int lastKeyboardControl = -1;
  protected static string AutoSelect(string name, Block b) {
    // Each widget needs a unique name so we can differentiate them.
    GUI.SetNextControlName(name);

    // Use the callback to actually draw the widget / manage the flow of data...
    string tmp = b();

    // And now, the magic:
    // Check to see if keyboard focus has changed on us...
    int kbdCtrlId = GUIUtility.keyboardControl;
    if(kbdCtrlId != lastKeyboardControl) {
      // It has!  Now, check to see if the focused control is this text area...
      string focusedControl = GUI.GetNameOfFocusedControl();
      if(focusedControl == name) {
        // It is!  Now, get the editor state (spooky voodo!), and tweak it.
        TextEditor t = GUIUtility.GetStateObject(typeof(TextEditor), kbdCtrlId) as TextEditor;
        t.SelectAll();
        // Update this here or state gets mangled when there's multiple 
        // AutoSelectTextArea.
        lastKeyboardControl = kbdCtrlId;
      }
    }
    return tmp;
  }
}
