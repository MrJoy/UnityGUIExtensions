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

public static class GUIStyleExtensions {
  public static GUIStyle NoBackgroundImages(this GUIStyle style) {
    style.normal.background = null;
    style.active.background = null;
    style.hover.background = null;
    style.focused.background = null;
    style.onNormal.background = null;
    style.onActive.background = null;
    style.onHover.background = null;
    style.onFocused.background = null;
    return style;
  }

  public static GUIStyle BaseTextColor(this GUIStyle style, Color c) {
    style.normal.textColor =
      style.active.textColor =
      style.hover.textColor =
      style.focused.textColor =
      style.onNormal.textColor =
      style.onActive.textColor =
      style.onHover.textColor =
      style.onFocused.textColor =
      c;
    return style;
  }

  public static GUIStyle ResetBoxModel(this GUIStyle style) {
    style.border = new RectOffset();
    style.margin = new RectOffset();
    style.padding = new RectOffset();
    style.overflow = new RectOffset();
    style.contentOffset = Vector2.zero;

    return style;
  }

  public static GUIStyle Padding(this GUIStyle style, int left, int right, int top, int bottom) {
    style.padding = new RectOffset(left, right, top, bottom);

    return style;
  }

  public static GUIStyle Margin(this GUIStyle style, int left, int right, int top, int bottom) {
    style.margin = new RectOffset(left, right, top, bottom);

    return style;
  }

  public static GUIStyle Border(this GUIStyle style, int left, int right, int top, int bottom) {
    style.border = new RectOffset(left, right, top, bottom);

    return style;
  }

  public static GUIStyle Named(this GUIStyle style, string name) {
    style.name = name;

    return style;
  }

  public static GUIStyle ClipText(this GUIStyle style) {
    style.clipping = TextClipping.Clip;

    return style;
  }

  public static GUIStyle Size(this GUIStyle style, int width, int height, bool stretchWidth, bool stretchHeight) {
    style.fixedWidth = width;
    style.fixedHeight = height;
    style.stretchWidth = stretchWidth;
    style.stretchHeight = stretchHeight;

    return style;
  }
}
