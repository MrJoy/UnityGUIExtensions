using UnityEngine;

// Extensions in the spirit/style of GUILayout.*
public static class GUILayoutExt {
  public static string AutoSelectTextArea(string name, string text, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, options);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, maxLength, options);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, style, options);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUILayout.TextArea(text, maxLength, style, options);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }
}

// Extensions in the spirit/style of GUI.*
public static class GUIExt {
  public static string AutoSelectTextArea(string name, Rect pos, string text) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text, maxLength);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, GUIStyle style) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text, style);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = GUI.TextArea(pos, text, maxLength, style);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }
}

// Helper/support stuff.
public static class GUIAutoSelect {
  // Internal gubbins for auto-select controls.
  private static int lastKeyboardControl = -1;
  public static void AutoSelectPre(string name) {
    // Each widget needs a unique name so we can differentiate them.
    GUI.SetNextControlName(name);
  }

  public static void AutoSelectPost(string name) {
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
