using UnityEngine;
using UnityEditor;

// Extensions in the spirit/style of GUILayout.*
public static class EditorGUILayoutToolbar {
  public const int BUTTON_SPACE = 6;

  public static void Space() { GUILayout.Space(BUTTON_SPACE); }
  public static void FlexibleSpace() { GUILayout.Label(GUIHelper.NoContent, GUIHelper.NoStyle, GUIHelper.ExpandWidth); }
  public static void Begin() { GUILayout.BeginHorizontal(EditorStyles.toolbar, GUIHelper.ExpandWidth); }
  public static void End() { GUILayout.EndHorizontal(); }
}
