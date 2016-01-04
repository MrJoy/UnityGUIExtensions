using UnityEngine;
using UnityEditor;

public class HorizontalPaneState {
  public const int SPLITTER_WIDTH = 9;
  public int id = 0;
  public bool isDraggingSplitter = false,
              isPaneWidthChanged = false;
  public float leftPaneWidth        = -1,
               initialLeftPaneWidth = -1,
               lastAvailableWidth   = -1,
               availableWidth       = 0,
               minPaneWidthLeft     = 75,
               minPaneWidthRight    = 75;

  /*
  * Unity can, apparently, recycle state objects.  In that event we want to
  * wipe the slate clean and just start over to avoid wackiness.
  */
  protected virtual void Reset(int newId) {
    id                    = newId;
    isDraggingSplitter    = false;
    isPaneWidthChanged    = false;
    leftPaneWidth         = -1;
    initialLeftPaneWidth  = -1;
    lastAvailableWidth    = -1;
    availableWidth        = 0;
    minPaneWidthLeft      = 75;
    minPaneWidthRight     = 75;
  }

  /*
  * Some aspects of our state are really just static configuration that
  * shouldn't be modified by the control, so we blindly set them if we have a
  * prototype from which to do so.
  */
  protected virtual void InitFromPrototype(int newId, HorizontalPaneState prototype) {
    id                    = newId;
    initialLeftPaneWidth  = prototype.initialLeftPaneWidth;
    minPaneWidthLeft      = prototype.minPaneWidthLeft;
    minPaneWidthRight     = prototype.minPaneWidthRight;
  }

  /*
  * This method takes care of guarding against state object recycling, and
  * ensures we pick up what we need, when we need to, from the prototype state
  * object.
  */
  public void ResolveStateToCurrentContext(int currentId, HorizontalPaneState prototype) {
    if(id != currentId)
      Reset(currentId);
    else if(prototype != null)
      InitFromPrototype(currentId, prototype);
  }
}

public static class EditorGUILayoutHorizontalPanes {
  // TODO: This makes it impossible to nest pane sets!
  private static HorizontalPaneState hState;

  public static void Begin() { Begin(null); }

  public static void Begin(HorizontalPaneState prototype) {
    int id = GUIUtility.GetControlID(FocusType.Passive);
    hState = (HorizontalPaneState)GUIUtility.GetStateObject(typeof(HorizontalPaneState), id);
    hState.ResolveStateToCurrentContext(id, prototype);

    // *INDENT-OFF*
    Rect totalArea = EditorGUILayout.BeginHorizontal();
      hState.availableWidth = totalArea.width - HorizontalPaneState.SPLITTER_WIDTH;
      hState.isPaneWidthChanged = false;
      if(totalArea.width > 0) {
        if(hState.leftPaneWidth < 0) {
          if(hState.initialLeftPaneWidth < 0)
            hState.leftPaneWidth = hState.availableWidth * 0.5f;
          else
            hState.leftPaneWidth = hState.initialLeftPaneWidth;
          hState.isPaneWidthChanged = true;
        }
        if(hState.lastAvailableWidth < 0)
          hState.lastAvailableWidth = hState.availableWidth;
        if(hState.lastAvailableWidth != hState.availableWidth) {
          hState.leftPaneWidth = hState.availableWidth * (hState.leftPaneWidth / hState.lastAvailableWidth);
          hState.isPaneWidthChanged = true;
        }
        hState.lastAvailableWidth = hState.availableWidth;
      }

      GUILayout.BeginHorizontal(GUILayout.Width(hState.leftPaneWidth));
    // *INDENT-ON*
  }

  public static void Splitter() {
    GUILayout.EndHorizontal();

    float availableWidthForOnePanel = hState.availableWidth - (1 + hState.minPaneWidthRight);
    Rect drawableSplitterArea = GUILayoutUtility.GetRect(GUIHelper.NoContent, HorizontalPaneStyles.Splitter, GUILayout.Width(1f), GUIHelper.ExpandHeight);
    Rect splitterArea = new Rect(drawableSplitterArea.xMin - (int)(HorizontalPaneState.SPLITTER_WIDTH * 0.5f), drawableSplitterArea.yMin, HorizontalPaneState.SPLITTER_WIDTH, drawableSplitterArea.height);
    switch(Event.current.type) {
    case EventType.MouseDown:
      if(splitterArea.Contains(Event.current.mousePosition)) {
        hState.isDraggingSplitter = true;
        GUIUtility.hotControl     = hState.id;
        Event.current.Use();
      }
      break;
    case EventType.MouseDrag:
      if(hState.isDraggingSplitter && hState.id == GUIUtility.hotControl) {
        hState.leftPaneWidth     += Event.current.delta.x;
        hState.leftPaneWidth      = Mathf.Round(hState.leftPaneWidth);
        hState.isPaneWidthChanged = true;
        Event.current.Use();
      }
      break;
    case EventType.MouseUp:
      hState.isDraggingSplitter = false;
      if(hState.id == GUIUtility.hotControl) {
        GUIUtility.hotControl = 0;
        Event.current.Use();
      }
      break;
    }

    if(hState.isPaneWidthChanged) {
      if(hState.leftPaneWidth < hState.minPaneWidthLeft)
        hState.leftPaneWidth = hState.minPaneWidthLeft;
      if(hState.leftPaneWidth >= availableWidthForOnePanel)
        hState.leftPaneWidth = availableWidthForOnePanel;
      if(EditorWindow.focusedWindow != null)
        EditorWindow.focusedWindow.Repaint();
    }
    GUI.Label(drawableSplitterArea, GUIHelper.NoContent, HorizontalPaneStyles.Splitter);
    EditorGUIUtility.AddCursorRect(splitterArea, MouseCursor.ResizeHorizontal);
  }

  public static void End() { EditorGUILayout.EndHorizontal(); }
}

public static class HorizontalPaneStyles {
  private static Texture2D SplitterImage;

  static HorizontalPaneStyles() {
    // TODO: Change the image color based on chosen editor skin.
    SplitterImage = new Texture2D(1, 1, TextureFormat.ARGB32, false) {
      hideFlags   = HideFlags.HideAndDontSave,
      anisoLevel  = 0,
      filterMode  = FilterMode.Point,
      wrapMode    = TextureWrapMode.Clamp
    };
    SplitterImage.SetPixels(new Color[] { Color.gray });
    SplitterImage.Apply();
  }

  private static GUIStyle _Splitter = null;

  public static GUIStyle Splitter {
    get {
      if(_Splitter == null) {
        // *INDENT-OFF*
        _Splitter = new GUIStyle() {
          normal        = new GUIStyleState() { background = SplitterImage },
          imagePosition = ImagePosition.ImageOnly,
          wordWrap      = false,
          alignment     = TextAnchor.MiddleCenter,
        }
          .Named("HSplitter")
          .Size(1, 0, false, true)
          .ResetBoxModel()
          .Margin(3, 3, 0, 0)
          .ClipText();
        // *INDENT-ON*
      }
      return _Splitter;
    }
  }
}
