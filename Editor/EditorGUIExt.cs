using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

// Extensions in the spirit/style of EditorGUILayout.*
public static class EditorGUILayoutExt {
  public static string AutoSelectTextArea(string name, string text, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUILayout.TextArea(text, options);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, options), maxLength);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUILayout.TextArea(text, style, options);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, style, options), maxLength);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }
}


// Extensions in the spirit/style of EditorGUI.*
public static class EditorGUIExt {
  public static string AutoSelectTextArea(string name, Rect pos, string text) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUI.TextArea(pos, text);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text), maxLength);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, Rect pos, string text, GUIStyle style) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUI.TextArea(pos, text, style);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }

  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    GUIAutoSelect.AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text, style), maxLength);
    GUIAutoSelect.AutoSelectPost(name);
    return tmp;
  }
}


// Helper/support stuff.
internal static class EditorGUICommon {
  internal static string ClampLength(string str, int maxLength) {
    if(!String.IsNullOrEmpty(str) && str.Length > maxLength)
      str = str.Substring(0, maxLength);
    return str;
  }
}
