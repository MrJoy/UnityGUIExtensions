using System;

// Helper/support stuff.
internal static class EditorGUICommon {
  internal static string ClampLength(string str, int maxLength) {
    if(!String.IsNullOrEmpty(str) && str.Length > maxLength)
      str = str.Substring(0, maxLength);
    return str;
  }
}
