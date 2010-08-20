using UnityEngine;

// Extensions in the spirit/style of GUILayout.*
public class GUILayoutExt : GUICommon {
  public static string AutoSelectTextArea(string name, string text, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, options); });
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, maxLength, options); });
  }

  public static string AutoSelectTextArea(string name, string text, GUIStyle style, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, style, options); });
  }

  public static string AutoSelectTextArea(string name, string text, int maxLength, GUIStyle style, params GUILayoutOption[] options) {
    return AutoSelect(name, delegate() { return GUILayout.TextArea(text, maxLength, style, options); });
  }

  // Don't allow instantiation of this class...
  protected GUILayoutExt() {}
}

// Extensions in the spirit/style of GUI.*
public class GUIExt : GUICommon {
  public static string AutoSelectTextArea(string name, Rect pos, string text) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text); });
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text, maxLength); });
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, GUIStyle style) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text, style); });
  }
  public static string AutoSelectTextArea(string name, Rect pos, string text, int maxLength, GUIStyle style) {
    return AutoSelect(name, delegate() { return GUI.TextArea(pos, text, maxLength, style); });
  }

  // Don't allow instantiation of this class...
  protected GUIExt() {}
}

// Functionality shared by both GUIExt and GUILayoutExt.
public class GUICommon {
  protected GUICommon() {}
  // Using delegates to DRY up the various parametric polymorphisms that Unity provides.
  protected delegate string Block();


  // Internal gubbins for auto-select controls.
  protected static int lastKeyboardControl = -1;
  protected static string AutoSelect(string name, Block b) {
    // Each widget needs a unique name so we can differentiate them.
    GUI.SetNextControlName(name);

    // Use the callback to actually draw the widget / manage the flow of data...
    string tmp = b();

    // And now, the magic:
    // Check to see if keyboard focus has changed on us...
    int kbdCtrlId = GUIUtility.keyboardControl;
    if(kbdCtrlId != lastKeyboardControl) {
      // It has!  Now, check to see if the focused control is this text area...
      string focusedControl = GUI.GetNameOfFocusedControl();
      if(focusedControl == name) {
        // It is!  Now, get the editor state (spooky voodo!), and tweak it.
        TextEditor t = GUIUtility.GetStateObject(typeof(TextEditor), kbdCtrlId) as TextEditor;
        t.SelectAll();
        // Update this here or state gets mangled when there's multiple 
        // AutoSelectTextArea.
        lastKeyboardControl = kbdCtrlId;
      }
    }
    return tmp;
  }

  public static int HexToInt(char c) {
    int digit = 0;
    if ( c >= '0' && c <= '9' ) {
      digit = c - '0';
    } else if ( c >= 'A' && c <= 'F' ) {
      digit = c - 'A' + 10;
    } else if ( c >= 'a' && c <= 'f' ) {
      digit = c - 'a' + 10;
    }

    return digit;
  }
  
  private static char[] SPECIAL_CHARS = new char[] { '#', '\n' };
  
  // Recognized color string sequences: 
  // #aabbccdd - Change current color to the one specified by the hex string 
  //             red green blue alpha 
  // #!       - Revert back to the original color that was used before this function call 
  // #n       - normal font 
  // #x       - bold font 
  // #i       - italic font 
  private static GUIStyle fontStyle;
  private static GUILayoutOption[] LINE_OPTIONS_V = new GUILayoutOption[] { GUILayout.ExpandHeight(false), GUILayout.Height(0), GUILayout.MinHeight(0) }, 
                                   LINE_OPTIONS_H = new GUILayoutOption[] { GUILayout.ExpandWidth(false), GUILayout.Width(0), GUILayout.MinWidth(0) };
  private static GUIContent DUMMY_SPACE_WITH_MARKER = new GUIContent(" ."), DUMMY_MARKER = new GUIContent(".");
  public static void FancyLabel( Rect rect, string text, GUIStyle testStyle,
                    Font normalFont, Font boldFont, Font italicFont, 
                    TextAlignment alignment ) { 
     int    i1 = 0, i2 = 0; 
     bool    done = false; 
     Color   originalColor = GUI.contentColor; 
     Color    textColor = originalColor; 

     Font   newFont = null; 

     // TODO: This is broken if we're called with different styles.
     fontStyle = fontStyle ?? new GUIStyle(testStyle);
     fontStyle.font = normalFont ?? GUI.skin.font; 

     // NOTE: Lowering this padding reduces the line spacing 
     // May need to adjust per font 
     fontStyle.padding.bottom = -5; 
     // TODO: Use spacing to simulate the left/right pad that we're disabling 
     // TODO: here.  We need to disable it to get segments to line up right, but
     // TODO: we're disregarding the intent of the style.
//     fontStyle.padding.right = fontStyle.padding.left = 0;
     fontStyle.margin.right = fontStyle.margin.left = 0;
     fontStyle.stretchWidth = false;
     fontStyle.wordWrap = false;
     fontStyle.clipping = TextClipping.Overflow;

     GUILayout.BeginArea( rect );
     
     LINE_OPTIONS_V[1].value = rect.height;
     LINE_OPTIONS_V[2].value = rect.height;

     LINE_OPTIONS_H[1].value = rect.width;
     LINE_OPTIONS_H[2].value = rect.width;

     GUILayout.BeginVertical(LINE_OPTIONS_V);
     BeginLine(alignment);

     while ( !done ) { 
       fontStyle.normal.textColor = textColor; 
       if ( newFont != null ) { 
          fontStyle.font = newFont; 
          newFont = null; 
       }

        int skipChars = 0; 

        i1 = text.IndexOfAny(SPECIAL_CHARS, i2); 

        // We're at the end, set the index to the end of the 
        // string and signal an end 
        char specialChar = '\0';
        if ( i1 == -1 ) { 
           i1 = text.Length - 1; 
           done = true; 
        }  else {
          specialChar = text[i1];
          char command = '\0';
          int remainder = text.Length - i1;
          if(remainder > 0) command = text[i1 + 1];
          i1--;
          // If the next character is # then we have a ## sequence 
          // We want to point one of the # so advance the index by 
          // one to include the first # 
           if ( specialChar == '#' ) { 
              if ( remainder >= 2 && command == '#' ) { 
                 skipChars = 2; 
                 i1++;
              } 
              // Revert to original color sequence 
              else if ( remainder >= 2 && command == '!' ) { 
                 textColor = originalColor;
                 skipChars = 3; 
              } 
              // Set normal font 
              else if ( remainder >= 2 && command == 'n' ) { 
                newFont = normalFont ?? GUI.skin.font; 
                 skipChars = 3; 
              } 
              // Set bold font 
              else if ( remainder >= 2 && command == 'x' ) { 
                newFont = boldFont ?? GUI.skin.font; 
                 skipChars = 3; 
              } 
              // Set italic font 
              else if ( remainder >= 2 && command == 'i' ) { 
                newFont = italicFont ?? GUI.skin.font; 
                 skipChars = 3; 
              } 
              //  New color sequence 
              else if ( remainder >= 10 ) { 
                 char r1 = text[i1 + 2], r2 = text[i1 + 3]; 
                 char g1 = text[i1 + 4], g2 = text[i1 + 5]; 
                 char b1 = text[i1 + 6], b2 = text[i1 + 7]; 
                 char a1 = text[i1 + 8], a2 = text[i1 + 9]; 

                 float r = (HexToInt(r1) * 16 + HexToInt(r2)) / 255.0f; 
                 float g = (HexToInt(g1) * 16 + HexToInt(g2)) / 255.0f; 
                 float b = (HexToInt(b1) * 16 + HexToInt(b2)) / 255.0f; 
                 float a = (HexToInt(a1) * 16 + HexToInt(a2)) / 255.0f; 

                 textColor = new Color( r, g, b, a ); 
                 skipChars = 11;
              } else { 
                skipChars = 2;
                 Debug.Log("Invalid # escape sequence"); 
//                 return; 
              } 
           } else if ( specialChar == '\n' ) { 
              skipChars = 2; 
           } else { 
             skipChars = 2;
              Debug.Log("Invalid escape sequence"); 
//              return; 
           } 
        } 

        string textPiece = text.Substring( i2, i1 - i2 + 1 );
        GUILayout.Label( textPiece, fontStyle ); 

        // Unity seems to cut off the trailing spaces in the label, he have 
        // to add them manually here 
        // Figure out how many trailing spaces there are 
        int idx = textPiece.Length - 1;
        while(idx >= 0 && textPiece[idx] == ' ') idx--;
        int spaces = textPiece.Length - (idx + 1);

        // NOTE: Add the proper amount of gap for trailing spaces. 
        // the length of space is a questimate here, 
        // may need to be adjusted for different fonts 
        Vector2 s1 = fontStyle.CalcSize(DUMMY_SPACE_WITH_MARKER);
        Vector2 s2 = fontStyle.CalcSize(DUMMY_MARKER);
        float spaceSize = s1.x - s2.x;
        GUILayout.Space( spaces * spaceSize );

        if ( specialChar == '\n' ) { 
           // Create a new line by ending the horizontal layout 
           if ( /*alignment == TextAlignment.Left ||*/ alignment == TextAlignment.Center) { 
              GUILayout.FlexibleSpace(); 
           } 
           GUILayout.EndHorizontal(); 
           BeginLine(alignment);
        } 

        // Store the last index 
        i2 = i1 + skipChars; 
     } 
     if ( /*alignment == TextAlignment.Left ||*/ alignment == TextAlignment.Center) { 
        GUILayout.FlexibleSpace(); 
     } 
     GUILayout.EndHorizontal(); 
//     GUILayout.FlexibleSpace(); 
     GUILayout.EndVertical(); 
     GUILayout.EndArea();       
  }
  
  private static void BeginLine(TextAlignment alignment) {
    GUILayout.BeginHorizontal(LINE_OPTIONS_H);
    if ( alignment == TextAlignment.Right || alignment == TextAlignment.Center)
       GUILayout.FlexibleSpace(); 
  }
}
