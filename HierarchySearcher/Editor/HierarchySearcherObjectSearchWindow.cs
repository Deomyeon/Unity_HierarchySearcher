using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HierarchySearcherObjectSearchWindow : EditorWindow
{
    public static EditorWindow window;
    public const int width = 200;
    public const int height = 400;
    
    private static string[] selectObjects;
    private static int idx;

    private Vector2 scrollPosition = Vector2.zero;

    private string findText = string.Empty;

    private void OnGUI()
    {
        findText = EditorGUILayout.TextField(findText, EditorStyles.toolbarSearchField);
        HashSet<string> removeItems = new HashSet<string>();
        string[] text = findText.Split(' ');
        foreach (string item in selectObjects)
        {
            if (findText != string.Empty)
            {
                foreach (string exp in text)
                {
                    if (!item.ToLower().Contains(exp.ToLower()))
                    {
                        removeItems.Add(item);
                    }
                }
            }
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.window);
        foreach (string item in selectObjects)
        {
            if (!removeItems.Contains(item))
            {
                EditorGUI.BeginChangeCheck();
                bool pressed = GUILayout.Button(item, GUI.skin.box, GUILayout.Width(window.position.width - window.position.width / 10));
                if (EditorGUI.EndChangeCheck())
                {
                    if (pressed)
                    {
                        HierarchySearcherWindow.searchOptions[idx].obj = item;
                        window.Close();
                    }
                }
            }
        }
        EditorGUILayout.Space(20);
        EditorGUILayout.EndScrollView();
    }

    public static void OpenWindow(string[] items, int index)
    {
        window = EditorWindow.GetWindow<HierarchySearcherObjectSearchWindow>();
        window.titleContent = new GUIContent("Select Object");
        window.minSize = new Vector2(width, height);
        window.maxSize = new Vector2(width, height);
        Vector2 mousePosition = Event.current.mousePosition;

        selectObjects = items;
        idx = index;

        window.position = new Rect(mousePosition.x + HierarchySearcher.window.position.x, mousePosition.y + HierarchySearcher.window.position.y, width, height);
        window.Focus();
    }

    public static void CloseWindow()
    {
        window.Close();
    }
}
