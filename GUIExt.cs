using UnityEngine;

// Extensions in the spirit/style of GUILayout.*
public class GUILayoutExt : GUICommon {
  public static string AutoSelectTextArea(string name, string text, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, options);
    AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, maxLength, options);
    AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, style, options);
    AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, maxLength, style, options);
    AutoSelectPost(name);
    return tmp;
  }

  // Don't allow instantiation of this class...
  protected GUILayoutExt() {}
}

// Extensions in the spirit/style of GUI.*
public class GUIExt : GUICommon {
  public static string AutoSelectTextArea(string name, Rect pos, string text) {
    AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text);
    AutoSelectPost(name);
    return tmp;
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength) {
    AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text, maxLength);
    AutoSelectPost(name);
    return tmp;
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, GUIStyle style) {
    AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text, style);
    AutoSelectPost(name);
    return tmp;
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text, maxLength, style);
    AutoSelectPost(name);
    return tmp;
  }

  // Don't allow instantiation of this class...
  protected GUIExt() {}
}

// Functionality shared by both GUIExt and GUILayoutExt.
public class GUICommon {
  protected GUICommon() {}

  // Internal gubbins for auto-select controls.
  protected static int lastKeyboardControl = -1;
  protected static void AutoSelectPre(string name) {
    // Each widget needs a unique name so we can differentiate them.
    GUI.SetNextControlName(name);
  }

  protected static void AutoSelectPost(string name) {
    // And now, the magic:
    // Check to see if keyboard focus has changed on us...
    int kbdCtrlId = GUIUtility.keyboardControl;
    if(kbdCtrlId != lastKeyboardControl) {
      // It has!  Now, check to see if the focused control is this text area...
      string focusedControl = GUI.GetNameOfFocusedControl();
      if(focusedControl == name) {
        // It is!  Now, get the editor state (spooky voodo!), and tweak it.
        TextEditor t = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), kbdCtrlId);
        t.SelectAll();
        // Update this here or state gets mangled when there's multiple 
        // AutoSelectTextArea.
        lastKeyboardControl = kbdCtrlId;
      }
    }
  }
}
