using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HierarchySearcher : MonoBehaviour
{
    public static EditorWindow window;

    public const int width = 400;
    public const int height = 700;

    [MenuItem("HierarchySearcher/Searcher")]
    public static void HierarchySearcher_Show()
    {
        window = EditorWindow.GetWindow<HierarchySearcherWindow>();
        Rect WindowRect = HierarchySearcher.window.position;
        WindowRect.width = width;
        WindowRect.height = height;
        HierarchySearcher.window.position = WindowRect;
        HierarchySearcher.window.minSize = new Vector2(width, height);
        HierarchySearcher.window.maxSize = new Vector2(width, height);
    }
}
