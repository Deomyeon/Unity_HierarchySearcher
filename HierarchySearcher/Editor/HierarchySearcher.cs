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
        window.titleContent = new GUIContent("Hierarchy Searcher");
        Rect WindowRect = window.position;
        WindowRect.x = Screen.width;
        WindowRect.width = width;
        WindowRect.height = height;
        window.position = WindowRect;
        window.minSize = new Vector2(width, height);
        window.maxSize = new Vector2(width, height);
    }

}
