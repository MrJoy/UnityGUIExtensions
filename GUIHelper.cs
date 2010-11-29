using UnityEngine;

public static class GUIHelper {
  public static GUILayoutOption ExpandWidth = GUILayout.ExpandWidth(true),
                                NoExpandWidth = GUILayout.ExpandWidth(false),
                                ExpandHeight = GUILayout.ExpandHeight(true),
                                NoExpandHeight = GUILayout.ExpandHeight(false);

  public static GUILayoutOption Width(float w) { return GUILayout.Width(w); }

  public static GUIStyle NoStyle = GUIStyle.none;
  public static GUIContent NoContent = GUIContent.none;
}