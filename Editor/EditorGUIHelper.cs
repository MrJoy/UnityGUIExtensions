using UnityEngine;
using UnityEditor;

public static class EditorGUIStyleExtensions {
  public static bool IsProSkin {
    get {
      GUISkin tmp = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
      Color textColor = tmp.label.normal.textColor;
      return textColor != Color.black;
    }
  }

  public static GUIStyle BaseTextColor(this GUIStyle style, Color normalSkin, Color proSkin) {
    style.normal.textColor =
      style.active.textColor =
      style.hover.textColor =
      style.focused.textColor =
      style.onNormal.textColor =
      style.onActive.textColor =
      style.onHover.textColor =
      style.onFocused.textColor =
        IsProSkin ? proSkin : normalSkin;
    return style;
  }
}