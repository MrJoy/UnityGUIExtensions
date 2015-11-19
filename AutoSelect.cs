using UnityEngine;

// Extensions in the spirit/style of GUI.*
public static class GUIAutoSelect {
  public static string TextArea(string name, Rect pos, string text) {
    CoreAutoSelect.Pre(name);
    string tmp = GUI.TextArea(pos, text);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, Rect pos, string text, int maxLength) {
    CoreAutoSelect.Pre(name);
    string tmp = GUI.TextArea(pos, text, maxLength);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, Rect pos, string text, GUIStyle style) {
    CoreAutoSelect.Pre(name);
    string tmp = GUI.TextArea(pos, text, style);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    CoreAutoSelect.Pre(name);
    string tmp = GUI.TextArea(pos, text, maxLength, style);
    CoreAutoSelect.Post(name);
    return tmp;
  }
}

// Extensions in the spirit/style of GUILayout.*
public static class GUILayoutAutoSelect {
  public static string TextArea(string name, string text, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = GUILayout.TextArea(text, options);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = GUILayout.TextArea(text, maxLength, options);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = GUILayout.TextArea(text, style, options);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = GUILayout.TextArea(text, maxLength, style, options);
    CoreAutoSelect.Post(name);
    return tmp;
  }
}

// Helper/support stuff.
public static class CoreAutoSelect {
  // Internal gubbins for auto-select controls.
  public static string lastFocusedControl = null;

  public static void Pre(string name) {
    // Each widget needs a unique name so we can differentiate them.
    GUI.SetNextControlName(name);
  }

  public static void Post(string name) {
    // And now, the magic:
    // Check to see if keyboard focus has changed on us...
    string focusedControl = GUI.GetNameOfFocusedControl();
    if(lastFocusedControl != focusedControl) {
      // It has!  Now, check to see if the focused control is this text area...
      if(focusedControl == name) {
        // It is!  Now, get the editor state (spooky voodo!), and tweak it.
        TextEditor t = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
        t.SelectAll();

        // Update this here or state gets mangled when there's multiple
        // AutoSelectTextArea objects and you switch between them.
        lastFocusedControl = focusedControl;
      } else if(focusedControl == "") {
        // Update this here or switching back and forth between normal TextArea
        // and AutoSelectTextArea widgets will have problems.
        lastFocusedControl = focusedControl;
      }
    }
  }
}
