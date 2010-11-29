using UnityEngine;
using UnityEditor;

public class VerticalPaneState {
  public int id = 0;
  public bool isDraggingSplitter = false,
              isPaneHeightChanged = false;
  public float topPaneHeight = -1, initialTopPaneHeight = -1,
               lastAvailableHeight = -1, availableHeight = 0,
               minPaneHeightTop = 75, minPaneHeightBottom = 75;

  private float _splitterHeight = 5;
  public float splitterHeight {
    get { return _splitterHeight; }
    set {
      if(value != _splitterHeight) {
        _splitterHeight = value;
        _SplitterHeight = null;
      }
    }
  }

  private GUILayoutOption _SplitterHeight = null;
  public GUILayoutOption SplitterHeight {
    get {
      if(_SplitterHeight == null)
        _SplitterHeight = GUILayout.Height(_splitterHeight);
      return _SplitterHeight;
    }
  }

  /*
  * Unity can, apparently, recycle state objects.  In that event we want to
  * wipe the slate clean and just start over to avoid wackiness.
  */
  protected virtual void Reset(int newId) {
    id = newId;
    isDraggingSplitter = false;
    isPaneHeightChanged = false;
    topPaneHeight = -1;
    initialTopPaneHeight = -1;
    lastAvailableHeight = -1;
    availableHeight = 0;
    minPaneHeightTop = 75;
    minPaneHeightBottom = 75;
  }

  /*
  * Some aspects of our state are really just static configuration that
  * shouldn't be modified by the control, so we blindly set them if we have a
  * prototype from which to do so.
  */
  protected virtual void InitFromPrototype(int newId, VerticalPaneState prototype) {
    id = newId;
    initialTopPaneHeight = prototype.initialTopPaneHeight;
    minPaneHeightTop = prototype.minPaneHeightTop;
    minPaneHeightBottom = prototype.minPaneHeightBottom;
  }

  /*
  * This method takes care of guarding against state object recycling, and
  * ensures we pick up what we need, when we need to, from the prototype state
  * object.
  */
  public void ResolveStateToCurrentContext(int currentId, VerticalPaneState prototype) {
    if(id != currentId) {
      Reset(currentId);
    } else if(prototype != null) {
      InitFromPrototype(currentId, prototype);
    }
  }
}

public static class EditorGUILayoutVerticalPanes {
  // TODO: This makes it impossible to nest pane sets!
  private static VerticalPaneState vState;

  public static void Begin() {
    Begin(null);
  }

  public static void Begin(VerticalPaneState prototype) {
    int id = GUIUtility.GetControlID(FocusType.Passive);
    vState = (VerticalPaneState)GUIUtility.GetStateObject(typeof(VerticalPaneState), id);
    vState.ResolveStateToCurrentContext(id, prototype);

    Rect totalArea = EditorGUILayout.BeginVertical();
      vState.availableHeight = totalArea.height - vState.splitterHeight;
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
  }

  public static void Splitter() {
    GUILayout.EndVertical();

    float availableHeightForOnePanel = vState.availableHeight - (vState.splitterHeight + vState.minPaneHeightBottom);
    Rect splitterArea = GUILayoutUtility.GetRect(GUIHelper.NoContent, GUI.skin.box, vState.SplitterHeight, GUIHelper.ExpandWidth);
    if(splitterArea.Contains(Event.current.mousePosition) || vState.isDraggingSplitter) {
      switch(Event.current.type) {
        case EventType.MouseDown:
          vState.isDraggingSplitter = true;
          break;
        case EventType.MouseDrag:
          if(vState.isDraggingSplitter) {
            vState.topPaneHeight += Event.current.delta.y;
            vState.isPaneHeightChanged = true;
          }
          break;
        case EventType.MouseUp:
          vState.isDraggingSplitter = false;
          break;
      }
    }
    if(vState.isPaneHeightChanged) {
      if(vState.topPaneHeight < vState.minPaneHeightTop) vState.topPaneHeight = vState.minPaneHeightTop;
      if(vState.topPaneHeight >= availableHeightForOnePanel) vState.topPaneHeight = availableHeightForOnePanel;
      if(EditorWindow.focusedWindow != null) EditorWindow.focusedWindow.Repaint();
    }
    // TODO: Better styling here...
    GUI.Label(splitterArea, vSplitterContent, GUI.skin.box);
    //EditorGUIUtility.AddCursorRect(splitterArea, MouseCursor.ResizeVertical);
  }
  private static GUIContent vSplitterContent = new GUIContent("--");

  public static void End() {
    EditorGUILayout.EndVertical();
  }
}