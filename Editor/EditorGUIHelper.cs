using UnityEngine;
using UnityEditor;

public static class EditorGUIStyleExtensions {
  private static bool wasProSkin = EditorGUIUtility.isProSkin;

  public static void InvalidateOnSkinChange(GUIStyle[] styles) {
    if(EditorGUIUtility.isProSkin != wasProSkin) {
      wasProSkin = EditorGUIUtility.isProSkin;
      for(int i = 0; i < styles.Length; i++)
        styles[i] = null;
    }
  }

  public static GUIStyle BaseTextColor(this GUIStyle style, Color normalSkin, Color proSkin) {
    // *INDENT-OFF*
    style.normal.textColor =
      style.active.textColor =
      style.hover.textColor =
      style.focused.textColor =
      style.onNormal.textColor =
      style.onActive.textColor =
      style.onHover.textColor =
      style.onFocused.textColor =
        EditorGUIUtility.isProSkin ? proSkin : normalSkin;
    // *INDENT-ON*
    return style;
  }
}
