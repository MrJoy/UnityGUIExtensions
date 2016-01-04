using UnityEngine;

public static class GUIHelper {
  // Don't create new instances every time we need an option -- just pre-create the permutations:
  public static GUILayoutOption ExpandWidth     = GUILayout.ExpandWidth(true),
                                NoExpandWidth   = GUILayout.ExpandWidth(false),
                                ExpandHeight    = GUILayout.ExpandHeight(true),
                                NoExpandHeight  = GUILayout.ExpandHeight(false);

  // Provided for consistency of interface, but not actually a savings/win:
  public static GUILayoutOption Width(float w) { return GUILayout.Width(w); }

  // Again, don't create instances when we don't need to:
  public static GUIStyle NoStyle = GUIStyle.none;
  public static GUIContent NoContent = GUIContent.none;
}

public static class GUIStyleExtensions {
  public static GUIStyle NoBackgroundImages(this GUIStyle style) {
    style.normal.background =
      style.active.background =
      style.hover.background =
      style.focused.background =
      style.onNormal.background =
      style.onActive.background =
      style.onHover.background =
      style.onFocused.background =
      null;
    return style;
  }

  public static GUIStyle BaseTextColor(this GUIStyle style, Color c) {
    // *INDENT-OFF*
    style.normal.textColor =
      style.active.textColor =
      style.hover.textColor =
      style.focused.textColor =
      style.onNormal.textColor =
      style.onActive.textColor =
      style.onHover.textColor =
      style.onFocused.textColor =
        c;
    // *INDENT-ON*
    return style;
  }

  public static GUIStyle ResetBoxModel(this GUIStyle style) {
    style.border        = new RectOffset();
    style.margin        = new RectOffset();
    style.padding       = new RectOffset();
    style.overflow      = new RectOffset();
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
    style.fixedWidth    = width;
    style.fixedHeight   = height;
    style.stretchWidth  = stretchWidth;
    style.stretchHeight = stretchHeight;

    return style;
  }
}
