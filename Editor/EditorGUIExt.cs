using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

// Extensions in the spirit/style of EditorGUILayout.*
public class EditorGUILayoutExt : GUILayoutExt {
  public static new string AutoSelectTextArea(string name, string text, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return EditorGUILayout.TextArea(text, options); });
  }

  public static new string AutoSelectTextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, options), maxLength); });
  }

  public static new string AutoSelectTextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return EditorGUILayout.TextArea(text, style, options); });
  }

  public static new string AutoSelectTextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return EditorGUICommon.ClampLength(EditorGUILayout.TextArea(text, style, options), maxLength); });
  }

  // Don't allow instantiation of this class...
  protected EditorGUILayoutExt() {}
}

// Extensions in the spirit/style of EditorGUI.*
public class EditorGUIExt : GUIExt {
  public static new string AutoSelectTextArea(string name, Rect pos, string text) {
    return AutoSelect(name, delegate() { return EditorGUI.TextArea(pos, text); });
  }
  public static new string AutoSelectTextArea(string name, Rect pos, string text, int maxLength) {
    return AutoSelect(name, delegate() { return EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text), maxLength); });
  }
  public static new string AutoSelectTextArea(string name, Rect pos, string text, GUIStyle style) {
    return AutoSelect(name, delegate() { return EditorGUI.TextArea(pos, text, style); });
  }
  public static new string AutoSelectTextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    return AutoSelect(name, delegate() { return EditorGUICommon.ClampLength(EditorGUI.TextArea(pos, text, style), maxLength); });
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