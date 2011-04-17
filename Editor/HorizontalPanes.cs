using UnityEngine;
using UnityEditor;

public class HorizontalPaneState {
  public int id = 0;
  public bool isDraggingSplitter = false,
              isPaneWidthChanged = false;
  public float leftPaneWidth = -1, initialLeftPaneWidth = -1,
               lastAvailableWidth = -1, availableWidth = 0,
               minPaneWidthLeft = 75, minPaneWidthRight = 75;

  private float _splitterWidth = 7;
  public float splitterWidth {
    get { return _splitterWidth; }
    set {
      if(value != _splitterWidth) {
        _splitterWidth = value;
        _SplitterWidth = null;
      }
    }
  }

  private GUILayoutOption _SplitterWidth = null;
  public GUILayoutOption SplitterWidth {
    get {
      if(_SplitterWidth == null)
        _SplitterWidth = GUILayout.Width(_splitterWidth);
      return _SplitterWidth;
    }
  }

  /*
  * Unity can, apparently, recycle state objects.  In that event we want to
  * wipe the slate clean and just start over to avoid wackiness.
  */
  protected virtual void Reset(int newId) {
    id = newId;
    isDraggingSplitter = false;
    isPaneWidthChanged = false;
    leftPaneWidth = -1;
    initialLeftPaneWidth = -1;
    lastAvailableWidth = -1;
    availableWidth = 0;
    minPaneWidthLeft = 75;
    minPaneWidthRight = 75;
  }

  /*
  * Some aspects of our state are really just static configuration that
  * shouldn't be modified by the control, so we blindly set them if we have a
  * prototype from which to do so.
  */
  protected virtual void InitFromPrototype(int newId, HorizontalPaneState prototype) {
    id = newId;
    initialLeftPaneWidth = prototype.initialLeftPaneWidth;
    minPaneWidthLeft = prototype.minPaneWidthLeft;
    minPaneWidthRight = prototype.minPaneWidthRight;
  }

  /*
  * This method takes care of guarding against state object recycling, and
  * ensures we pick up what we need, when we need to, from the prototype state
  * object.
  */
  public void ResolveStateToCurrentContext(int currentId, HorizontalPaneState prototype) {
    if(id != currentId) {
      Reset(currentId);
    } else if(prototype != null) {
      InitFromPrototype(currentId, prototype);
    }
  }
}

public static class EditorGUILayoutHorizontalPanes {
  // TODO: This makes it impossible to nest pane sets!
  private static HorizontalPaneState hState;

  public static void Begin() {
    Begin(null);
  }

  public static void Begin(HorizontalPaneState prototype) {
    int id = GUIUtility.GetControlID(FocusType.Passive);
    hState = (HorizontalPaneState)GUIUtility.GetStateObject(typeof(HorizontalPaneState), id);
    hState.ResolveStateToCurrentContext(id, prototype);

    Rect totalArea = EditorGUILayout.BeginHorizontal();
      hState.availableWidth = totalArea.width - hState.splitterWidth;
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
  }

  public static void Splitter() {
    GUILayout.EndHorizontal();

    float availableWidthForOnePanel = hState.availableWidth - (hState.splitterWidth + hState.minPaneWidthRight);
    Rect splitterArea = GUILayoutUtility.GetRect(GUIHelper.NoContent, HorizontalPaneStyles.Splitter, hState.SplitterWidth, GUIHelper.ExpandHeight);
    if(splitterArea.Contains(Event.current.mousePosition) || hState.isDraggingSplitter) {
      switch(Event.current.type) {
        case EventType.MouseDown:
          hState.isDraggingSplitter = true;
          break;
        case EventType.MouseDrag:
          if(hState.isDraggingSplitter) {
            hState.leftPaneWidth += Event.current.delta.x;
            hState.isPaneWidthChanged = true;
          }
          break;
        case EventType.MouseUp:
          hState.isDraggingSplitter = false;
          break;
      }
    }
    if(hState.isPaneWidthChanged) {
      if(hState.leftPaneWidth < hState.minPaneWidthLeft) hState.leftPaneWidth = hState.minPaneWidthLeft;
      if(hState.leftPaneWidth >= availableWidthForOnePanel) hState.leftPaneWidth = availableWidthForOnePanel;
      if(EditorWindow.focusedWindow != null) EditorWindow.focusedWindow.Repaint();
    }
    GUI.Label(splitterArea, GUIHelper.NoContent, HorizontalPaneStyles.Splitter);
    //EditorGUIUtility.AddCursorRect(splitterArea, MouseCursor.ResizeHorizontal);
  }

  public static void End() {
    EditorGUILayout.EndHorizontal();
  }
}

public static class HorizontalPaneStyles {
  private static Texture2D SplitterImage;
  static HorizontalPaneStyles() {
    // TODO: Change the image color based on chosen editor skin.
    SplitterImage = new Texture2D(7, 1, TextureFormat.ARGB32, false);
    SplitterImage.hideFlags = HideFlags.HideAndDontSave;
    Color _ = Color.clear, X = Color.black;
    SplitterImage.SetPixels(new Color[] {
      _,_,_,X,_,_,_,
    });
    SplitterImage.Apply();
    SplitterImage.anisoLevel = 0;
    SplitterImage.filterMode = FilterMode.Point;
    SplitterImage.wrapMode = TextureWrapMode.Clamp;
  }

  private static GUIStyle _Splitter = null;
  public static GUIStyle Splitter {
    get {
      if(_Splitter == null) {
        _Splitter = new GUIStyle() {
          normal = new GUIStyleState() {
            background = SplitterImage
          },
          imagePosition = ImagePosition.ImageOnly,
          padding = new RectOffset(0,0,0,0),
          margin = new RectOffset(0,0,0,0),
          overflow = new RectOffset(0,0,0,0),
          contentOffset = Vector2.zero,
          wordWrap = false,
          clipping = TextClipping.Clip,
          alignment = TextAnchor.MiddleCenter,

          //border = new RectOffset(l,r,t,b),
          border = new RectOffset(2,2,1,1),
          fixedWidth = 7,
          fixedHeight = 0,
          stretchWidth = true,
          stretchHeight = false,
        };
      }
      return _Splitter;
    }
  }
}
