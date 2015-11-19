using UnityEngine;
using UnityEditor;

// Extensions in the spirit/style of EditorGUI.*
public static class EditorGUIAutoSelect {
  public static string TextArea(string name, Rect pos, string text) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUI.TextArea(pos, text);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, Rect pos, string text, int maxLength) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text), maxLength);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, Rect pos, string text, GUIStyle style) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUI.TextArea(pos, text, style);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text, style), maxLength);
    CoreAutoSelect.Post(name);
    return tmp;
  }
}

// Extensions in the spirit/style of EditorGUILayout.*
public static class EditorGUILayoutAutoSelect {
  public static string TextArea(string name, string text, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUILayout.TextArea(text, options);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, options), maxLength);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUILayout.TextArea(text, style, options);
    CoreAutoSelect.Post(name);
    return tmp;
  }

  public static string TextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    CoreAutoSelect.Pre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, style, options), maxLength);
    CoreAutoSelect.Post(name);
    return tmp;
  }
}
