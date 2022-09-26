// An Editor script to move anchors and resize rect boundaries at the same time
// Limitation: it cannot stick anchors to a rect if it is rotated, since anchors are always axis-aligned

// Credits
// Source: https://answers.unity.com/questions/1100603/how-to-make-anchor-snap-to-self-rect-transform-in.html
// Phedg1: original script
// stephane.lallee: combined component and editor tool into one script
// hsandt:
// - added toggle to enable sticking anchors to rect only when wanted
// - added button to immediately stick anchors
// - fixed Undo stick anchors
// - log on callback (un)registration

// Further:
// See achimmihca's MoveCornersToAnchors https://gist.github.com/achimmihca/a8d92347f2fe88050fae5f381eff9a6d
// and AnchorsToCornersMenuItems https://gist.github.com/achimmihca/4f053a81983c91bdf661214e1b88f65b
// for the menu item variant (useful to bind shortcuts) and the reverse operation

using UnityEngine;
using UnityEditor;

public class AnchorToolsEditor : EditorWindow
{
    /// When true make the anchors match the rect boundaries after a rect resize
    private bool stickAnchorsToRect = false;

    static AnchorToolsEditor()
    {
        Debug.Log("[AnchorToolsEditor] (static) Registering for anchors update On Scene GUI");
        SceneView.duringSceneGui += OnScene;
    }

    [MenuItem("Tools/Anchor Tools")]
    static void Init()
    {
        AnchorToolsEditor editorScreenshot = GetWindow<AnchorToolsEditor>(title: "Anchor Tools");

        if (EditorPrefs.HasKey("AnchorToolsEditor.screenshotFolderPath"))
            editorScreenshot.stickAnchorsToRect = EditorPrefs.GetBool("AnchorToolsEditor.stickAnchorsToRect");
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        stickAnchorsToRect = EditorGUILayout.Toggle("Stick Anchors to Rect", stickAnchorsToRect);

        if (EditorGUI.EndChangeCheck()) {
            EditorPrefs.SetBool("AnchorToolsEditor.stickAnchorsToRect", stickAnchorsToRect);
        }

        if (GUILayout.Button("Stick Anchors to Rect")) UpdateAnchors();
    }

    private static void OnScene(SceneView sceneView)
    {
        // detect mouse up button as a resize event; this is not accurate as other actions may be used,
        // and we may modify the rect by inputting values with the keyboard, but works for quick usage
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            // Get Anchor Tools Editor window to check if we should stick anchors to rect
            // ! This will focus the Anchor Tools Editor, so do this at the deepest level possible
            // Currently it will do this every time we release the left mouse button on the scene (without even checking
            // if we actually modified a rectangle transform), which may be unwanted (currently it doesn't bother much)
            // Consider storing stickAnchorsToRect in User Preferences instead to access it without depending on Window
            AnchorToolsEditor editorScreenshot = GetWindow<AnchorToolsEditor>(title: "Anchor Tools");
            if (editorScreenshot.stickAnchorsToRect)
            {
                UpdateAnchors();
            }
        }
    }

    public void OnDestroy()
    {
        Debug.Log("[AnchorToolsEditor] Unregistering for anchors update On Scene GUI");
        SceneView.duringSceneGui -= OnScene;
    }

    private static Rect anchorRect;
    private static Rect anchorRectOld;
    private static RectTransform currentRectTransform;
    private static RectTransform parentRectTransform;
    private static Vector2 pivotOld;
    private static Vector2 offsetMinOld;
    private static Vector2 offsetMaxOld;

    private static void UpdateAnchors()
    {
        TryToGetRectTransform();
        if (currentRectTransform != null && parentRectTransform != null && ShouldStick())
        {
            Stick();
        }
    }

    private static bool ShouldStick()
    {
        return (
            currentRectTransform.offsetMin != offsetMinOld ||
            currentRectTransform.offsetMax != offsetMaxOld ||
            currentRectTransform.pivot != pivotOld ||
            anchorRect != anchorRectOld
            );
    }

    private static void Stick()
    {
        CalculateCurrentWH();
        CalculateCurrentXY();

        CalculateCurrentXY();
        pivotOld = currentRectTransform.pivot;

        AnchorsToCorners();
        anchorRectOld = anchorRect;
    }

    private static void TryToGetRectTransform()
    {
        if (Selection.activeGameObject != null)
        {
            currentRectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
            if (currentRectTransform != null && currentRectTransform.parent != null)
            {
                parentRectTransform = currentRectTransform.parent.GetComponent<RectTransform>();
            }
            else
            {
                parentRectTransform = null;
            }
        }
        else
        {
            currentRectTransform = null;
            parentRectTransform = null;
        }
    }

    private static void CalculateCurrentXY()
    {
        float pivotX = anchorRect.width * currentRectTransform.pivot.x;
        float pivotY = anchorRect.height * (1 - currentRectTransform.pivot.y);
        Vector2 newXY = new Vector2(currentRectTransform.anchorMin.x * parentRectTransform.rect.width + currentRectTransform.offsetMin.x + pivotX,
                                  -(1 - currentRectTransform.anchorMax.y) * parentRectTransform.rect.height + currentRectTransform.offsetMax.y - pivotY + parentRectTransform.rect.height);
        anchorRect.x = newXY.x;
        anchorRect.y = newXY.y;
        anchorRectOld = anchorRect;
    }

    private static void CalculateCurrentWH()
    {
        anchorRect.width = currentRectTransform.rect.width;
        anchorRect.height = currentRectTransform.rect.height;
        anchorRectOld = anchorRect;
    }

    private static void AnchorsToCorners()
    {
        Undo.RecordObject(currentRectTransform, "Stick Anchors");

        float pivotX = anchorRect.width * currentRectTransform.pivot.x;
        float pivotY = anchorRect.height * (1 - currentRectTransform.pivot.y);
        currentRectTransform.anchorMin = new Vector2(0f, 1f);
        currentRectTransform.anchorMax = new Vector2(0f, 1f);
        currentRectTransform.offsetMin = new Vector2(anchorRect.x / currentRectTransform.localScale.x, anchorRect.y / currentRectTransform.localScale.y - anchorRect.height);
        currentRectTransform.offsetMax = new Vector2(anchorRect.x / currentRectTransform.localScale.x + anchorRect.width, anchorRect.y / currentRectTransform.localScale.y);
        currentRectTransform.anchorMin = new Vector2(currentRectTransform.anchorMin.x + (currentRectTransform.offsetMin.x - pivotX) / parentRectTransform.rect.width * currentRectTransform.localScale.x,
                                                 currentRectTransform.anchorMin.y - 1f + (currentRectTransform.offsetMin.y + pivotY) / parentRectTransform.rect.height * currentRectTransform.localScale.y);
        currentRectTransform.anchorMax = new Vector2(currentRectTransform.anchorMax.x + (currentRectTransform.offsetMax.x - pivotX) / parentRectTransform.rect.width * currentRectTransform.localScale.x,
                                                 currentRectTransform.anchorMax.y - 1f + (currentRectTransform.offsetMax.y + pivotY) / parentRectTransform.rect.height * currentRectTransform.localScale.y);
        currentRectTransform.offsetMin = new Vector2((0 - currentRectTransform.pivot.x) * anchorRect.width * (1 - currentRectTransform.localScale.x), (0 - currentRectTransform.pivot.y) * anchorRect.height * (1 - currentRectTransform.localScale.y));
        currentRectTransform.offsetMax = new Vector2((1 - currentRectTransform.pivot.x) * anchorRect.width * (1 - currentRectTransform.localScale.x), (1 - currentRectTransform.pivot.y) * anchorRect.height * (1 - currentRectTransform.localScale.y));

        offsetMinOld = currentRectTransform.offsetMin;
        offsetMaxOld = currentRectTransform.offsetMax;
    }
}
