using UnityEngine;
using UnityEditor;

public class VerticalPaneState {
  public const int SPLITTER_HEIGHT = 9;

  public int id = 0;
  public bool isDraggingSplitter  = false,
              isPaneHeightChanged = false;
  public float topPaneHeight        = -1,
               initialTopPaneHeight = -1,
               lastAvailableHeight  = -1,
               availableHeight      = 0,
               minPaneHeightTop     = 75,
               minPaneHeightBottom  = 75;

  /*
  * Unity can, apparently, recycle state objects.  In that event we want to
  * wipe the slate clean and just start over to avoid wackiness.
  */
  protected virtual void Reset(int newId) {
    id                    = newId;
    isDraggingSplitter    = false;
    isPaneHeightChanged   = false;
    topPaneHeight         = -1;
    initialTopPaneHeight  = -1;
    lastAvailableHeight   = -1;
    availableHeight       = 0;
    minPaneHeightTop      = 75;
    minPaneHeightBottom   = 75;
  }

  /*
  * Some aspects of our state are really just static configuration that
  * shouldn't be modified by the control, so we blindly set them if we have a
  * prototype from which to do so.
  */
  protected virtual void InitFromPrototype(int newId, VerticalPaneState prototype) {
    id                    = newId;
    initialTopPaneHeight  = prototype.initialTopPaneHeight;
    minPaneHeightTop      = prototype.minPaneHeightTop;
    minPaneHeightBottom   = prototype.minPaneHeightBottom;
  }

  /*
  * This method takes care of guarding against state object recycling, and
  * ensures we pick up what we need, when we need to, from the prototype state
  * object.
  */
  public void ResolveStateToCurrentContext(int currentId, VerticalPaneState prototype) {
    if(id != currentId)
      Reset(currentId);
    else if(prototype != null)
      InitFromPrototype(currentId, prototype);
  }
}

public static class EditorGUILayoutVerticalPanes {
  // TODO: This makes it impossible to nest pane sets!
  private static VerticalPaneState vState;

  public static void Begin() { Begin(null); }

  public static void Begin(VerticalPaneState prototype) {
    int id = GUIUtility.GetControlID(FocusType.Passive);
    vState = (VerticalPaneState)GUIUtility.GetStateObject(typeof(VerticalPaneState), id);
    vState.ResolveStateToCurrentContext(id, prototype);

    // *INDENT-OFF*
    Rect totalArea = EditorGUILayout.BeginVertical();
      vState.availableHeight = totalArea.height - VerticalPaneState.SPLITTER_HEIGHT;
      vState.isPaneHeightChanged = false;
      if(totalArea.height > 0) {
        if(vState.topPaneHeight < 0) {
          if(vState.initialTopPaneHeight < 0)
            vState.topPaneHeight = vState.availableHeight * 0.5f;
          else
            vState.topPaneHeight = vState.initialTopPaneHeight;
          vState.isPaneHeightChanged = true;
        }
        if(vState.lastAvailableHeight < 0)
          vState.lastAvailableHeight = vState.availableHeight;
        if(vState.lastAvailableHeight != vState.availableHeight) {
          vState.topPaneHeight = vState.availableHeight * (vState.topPaneHeight / vState.lastAvailableHeight);
          vState.isPaneHeightChanged = true;
        }
        vState.lastAvailableHeight = vState.availableHeight;
      }

      GUILayout.BeginVertical(GUILayout.Height(vState.topPaneHeight));
    // *INDENT-ON*
  }

  public static void Splitter() {
    GUILayout.EndVertical();

    float availableHeightForOnePanel = vState.availableHeight - (1 + vState.minPaneHeightBottom);
    Rect drawableSplitterArea = GUILayoutUtility.GetRect(GUIHelper.NoContent, VerticalPaneStyles.Splitter, GUILayout.Height(1f), GUIHelper.ExpandWidth);
    Rect splitterArea = new Rect(drawableSplitterArea.xMin, drawableSplitterArea.yMin - (int)(VerticalPaneState.SPLITTER_HEIGHT * 0.5f), drawableSplitterArea.width, VerticalPaneState.SPLITTER_HEIGHT);
    switch(Event.current.type) {
    case EventType.MouseDown:
      if(splitterArea.Contains(Event.current.mousePosition)) {
        vState.isDraggingSplitter = true;
        GUIUtility.hotControl = vState.id;
        Event.current.Use();
      }
      break;
    case EventType.MouseDrag:
      if(vState.isDraggingSplitter && vState.id == GUIUtility.hotControl) {
        vState.topPaneHeight += Event.current.delta.y;
        vState.topPaneHeight = Mathf.Round(vState.topPaneHeight);
        vState.isPaneHeightChanged = true;
        Event.current.Use();
      }
      break;
    case EventType.MouseUp:
      vState.isDraggingSplitter = false;
      if(vState.id == GUIUtility.hotControl) {
        GUIUtility.hotControl = 0;
        Event.current.Use();
      }
      break;
    }

    if(vState.isPaneHeightChanged) {
      if(vState.topPaneHeight < vState.minPaneHeightTop)
        vState.topPaneHeight = vState.minPaneHeightTop;
      if(vState.topPaneHeight >= availableHeightForOnePanel)
        vState.topPaneHeight = availableHeightForOnePanel;
      if(EditorWindow.focusedWindow != null)
        EditorWindow.focusedWindow.Repaint();
    }
    GUI.Label(drawableSplitterArea, GUIHelper.NoContent, VerticalPaneStyles.Splitter);
    EditorGUIUtility.AddCursorRect(splitterArea, MouseCursor.ResizeVertical);
  }

  public static void End() { EditorGUILayout.EndVertical(); }
}

public static class VerticalPaneStyles {
  private static Texture2D SplitterImage;

  static VerticalPaneStyles() {
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
          alignment     = TextAnchor.MiddleCenter
        }
          .Named("VSplitter")
          .Size(0, 1, true, false)
          .ResetBoxModel()
          .Margin(0, 0, 3, 3)
          .ClipText();
        // *INDENT-ON*
      }
      return _Splitter;
    }
  }
}
