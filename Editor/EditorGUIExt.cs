using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

// Extensions in the spirit/style of EditorGUILayout.*
public class EditorGUILayoutExt : GUILayoutExt {
  public static new string AutoSelectTextArea(string name, string text, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = EditorGUILayout.TextArea(text, options);
    AutoSelectPost(name);
    return tmp;
  }

  public static new string AutoSelectTextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, options), maxLength);
    AutoSelectPost(name);
    return tmp;
  }

  public static new string AutoSelectTextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = EditorGUILayout.TextArea(text, style, options);
    AutoSelectPost(name);
    return tmp;
  }

  public static new string AutoSelectTextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, style, options), maxLength);
    AutoSelectPost(name);
    return tmp;
  }

  // Don't allow instantiation of this class...
  protected EditorGUILayoutExt() {}
}

// Extensions in the spirit/style of EditorGUI.*
public class EditorGUIExt : GUIExt {
  public static new string AutoSelectTextArea(string name, Rect pos, string text) {
    AutoSelectPre(name);
    string tmp = EditorGUI.TextArea(pos, text);
    AutoSelectPost(name);
    return tmp;
  }

  public static new string AutoSelectTextArea(string name, Rect pos, string text, int maxLength) {
    AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text), maxLength);
    AutoSelectPost(name);
    return tmp;
  }

  public static new string AutoSelectTextArea(string name, Rect pos, string text, GUIStyle style) {
    AutoSelectPre(name);
    string tmp = EditorGUI.TextArea(pos, text, style);
    AutoSelectPost(name);
    return tmp;
  }

  public static new string AutoSelectTextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    AutoSelectPre(name);
    string tmp = EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text, style), maxLength);
    AutoSelectPost(name);
    return tmp;
  }

  // Don't allow instantiation of this class...
  protected EditorGUIExt() {}
}




public class EditorGUICommon : GUICommon {
  public static string ClampLength(string str, int maxLength) {
    if(!String.IsNullOrEmpty(str) && str.Length > maxLength) {
      str = str.Substring(0, maxLength);
    }
    return str;
  }
}