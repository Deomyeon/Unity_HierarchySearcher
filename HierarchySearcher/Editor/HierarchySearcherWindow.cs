using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class HierarchySearcherWindow : EditorWindow
{

    public static List<SearchOption> searchOptions = new List<SearchOption>();
    private List<Object> objects = new List<Object>();

    private List<GameObject> gameObjects = new List<GameObject>();

    private List<bool> selectedGameObjects = new List<bool>();

    private string[] hierarchySearchOption = {"Option Name", "Option Component", "Option Tag", "Option Layer", "Option Function", "Option Field", "Option Property", "Option Active", "Option Has Children", "Option Has Parent" };

    private int selectedIndex = 0;

    private int selectedGameObject = -1;

    Vector2 scrollViewPosition = Vector2.zero;
    Vector2 optionScrollViewPosition = Vector2.zero;
    Vector2 gameObjectScrollViewPosition = Vector2.zero;


    private void OnGUI()
    {
        if (HierarchySearcher.window == null) HierarchySearcher.window = EditorWindow.GetWindow<HierarchySearcherWindow>();

        if (HierarchySearcherObjectSearchWindow.window != null)
        {
            if (EditorWindow.focusedWindow != HierarchySearcherObjectSearchWindow.window)
            {
                HierarchySearcherObjectSearchWindow.window.Close();
            }
        }
        if (HierarchySearcher.window.position.width - HierarchySearcher.width > 0.01f)
        {
            Close();
            HierarchySearcher.HierarchySearcher_Show();
        }

        if (Event.current.keyCode == KeyCode.Tab)
        {
            EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow")).Focus();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Hierarchy Search");

        scrollViewPosition = EditorGUILayout.BeginScrollView(scrollViewPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none, GUILayout.Height(HierarchySearcher.window.position.height - 20));

        EditorGUILayout.Space(40);

        
        EditorGUI.BeginChangeCheck();

        selectedIndex = Mathf.Clamp(selectedIndex, 0, hierarchySearchOption.Length - 1);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(380), GUILayout.Height(20));
        
        EditorGUILayout.Space(2);
        selectedIndex = EditorGUILayout.Popup(selectedIndex, hierarchySearchOption, GUILayout.Width(200), GUILayout.Height(20));
        EditorGUILayout.Space(10);
        bool addOption = GUILayout.Button("Add Option", GUILayout.Width(100), GUILayout.Height(20));
        
        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            if (addOption)
            {
                switch (hierarchySearchOption[selectedIndex])
                {
                    case "Option Name":
                    searchOptions.Add(new OptionName());
                    break;
                    case "Option Component":
                    searchOptions.Add(new OptionComponent());
                    break;
                    case "Option Tag":
                    searchOptions.Add(new OptionTag());
                    break;
                    case "Option Layer":
                    searchOptions.Add(new OptionLayer());
                    break;
                    case "Option Function":
                    searchOptions.Add(new OptionFunction());
                    break;
                    case "Option Field":
                    searchOptions.Add(new OptionField());
                    break;
                    case "Option Property":
                    searchOptions.Add(new OptionProperty());
                    break;
                    case "Option Active":
                    searchOptions.Add(new OptionActive());
                    break;
                    case "Option Has Children":
                    searchOptions.Add(new OptionHasChildren());
                    break;
                    case "Option Has Parent":
                    searchOptions.Add(new OptionHasParent());
                    break;
                }
            }
        }

        EditorGUILayout.Space(20);
        
        optionScrollViewPosition = EditorGUILayout.BeginScrollView(optionScrollViewPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.box, GUILayout.Height((int)HierarchySearcher.window.position.height >> 1));

        for (int i = 0; i < searchOptions.Count; i++)
        {
            switch (searchOptions[i].GetType().ToString())
            {
                case "OptionName":
                OptionNameContent(i);
                break;
                case "OptionComponent":
                OptionComponentContent(i);
                break;
                case "OptionTag":
                OptionTagContent(i);
                break;
                case "OptionLayer":
                OptionLayerContent(i);
                break;
                case "OptionFunction":
                OptionFunctionContent(i);
                break;
                case "OptionField":
                OptionFieldContent(i);
                break;
                case "OptionProperty":
                OptionPropertyContent(i);
                break;
                case "OptionActive":
                OptionActiveContent(i);
                break;
                case "OptionHasChildren":
                OptionHasChildrenContent(i);
                break;
                case "OptionHasParent":
                OptionHasParentContent(i);
                break;
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space(20);
        
        EditorGUI.BeginChangeCheck();

        bool searchGameObject = GUILayout.Button("Search GameObject", GUILayout.Width(380), GUILayout.Height(20));

        if (EditorGUI.EndChangeCheck())
        {
            if (searchGameObject)
            {
                gameObjects.Clear();
                selectedGameObjects.Clear();
                selectedGameObject = -1;
                
                if (searchOptions.Count > 0)
                {
                    gameObjects = searchOptions[0].SearchGameObject(gameObjects, true);
                    if (searchOptions.Count > 1)
                    {
                        for (int i = 1; i < searchOptions.Count; i++)
                        {
                            gameObjects = searchOptions[i].SearchGameObject(gameObjects);
                        }
                    }
                    
                    gameObjects.Sort((a, b) => {return a.name.CompareTo(b.name);});
                    

                    for (int i = 0; i < gameObjects.Count; i++)
                    {
                        selectedGameObjects.Add(false);
                    }
                }
            }
        }

        EditorGUILayout.Space(40);

        gameObjectScrollViewPosition = EditorGUILayout.BeginScrollView(gameObjectScrollViewPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.box, GUILayout.Height((int)HierarchySearcher.window.position.height >> 1));

        EditorGUILayout.Space(10);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] == null) continue;
            if (selectedGameObjects[i])
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(370));
            }
            else
            {
                EditorGUILayout.BeginVertical(EditorStyles.numberField, GUILayout.Width(370));
            }

            EditorGUI.BeginChangeCheck();
            bool selectGameObject = GUILayout.Button(gameObjects[i].name, GUI.skin.label);

            if (EditorGUI.EndChangeCheck())
            {
                if (selectGameObject)
                {
                    if (Event.current.control)
                    {
                        selectedGameObjects[i] = !selectedGameObjects[i];
                        selectedGameObject = i;
                    }
                    else if (Event.current.shift)
                    {
                        if (selectedGameObject != -1)
                        {
                            int min = 0;
                            int max = 0;

                            if (selectedGameObject > i)
                            {
                                max = selectedGameObject;
                                min = i;
                            }
                            else
                            {
                                max = i;
                                min = selectedGameObject;
                            }

                            for (int j = min; j < max + 1; j++)
                            {
                                selectedGameObjects[j] = true;
                            }
                            selectedGameObject = i;
                        }
                        else
                        {
                            selectedGameObjects[i] = true;
                            selectedGameObject = i;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < selectedGameObjects.Count; j++)
                        {
                            selectedGameObjects[j] = false;
                        }
                        selectedGameObjects[i] = true;
                        selectedGameObject = i;
                    }

                    List<GameObject> selectObject = new List<GameObject>();
                    for (int j = 0; j < selectedGameObjects.Count; j++)
                    {
                        if (selectedGameObjects[j])
                        {
                            selectObject.Add(gameObjects[j]);
                        }
                    }
                    Selection.objects = selectObject.ToArray();
                    
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(100);

        EditorGUILayout.EndScrollView();
    }
    
    private void OptionHasParentContent(int idx)
    {
        BeginOptionContent();
        bool? temp = searchOptions[idx].obj as bool?;
        searchOptions[idx].obj = EditorGUILayout.Toggle("Has Parent", temp.HasValue ? temp.Value : false);

        EndOptionContent(idx);
    }

    private void OptionHasChildrenContent(int idx)
    {
        BeginOptionContent();
        bool? temp = searchOptions[idx].obj as bool?;
        searchOptions[idx].obj = EditorGUILayout.Toggle("Has Children", temp.HasValue ? temp.Value : false);

        EndOptionContent(idx);
    }

    private void OptionActiveContent(int idx)
    {
        BeginOptionContent();
        bool? temp = searchOptions[idx].obj as bool?;
        searchOptions[idx].obj = EditorGUILayout.Toggle("Is Active", temp.HasValue ? temp.Value : false);

        EndOptionContent(idx);
    }

    private void OptionPropertyContent(int idx)
    {
        BeginOptionContent();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Property Name", GUILayout.Width(200));
        if (searchOptions[idx].obj == null)
        {
            searchOptions[idx].obj = "None";
        }
        bool pressed = GUILayout.Button(searchOptions[idx].obj as string, EditorStyles.numberField, GUILayout.Width(150));

        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            if (pressed)
            {
                HashSet<string> items = new HashSet<string>();
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
                foreach (GameObject item in objects)
                {
                    Component[] components = item.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        foreach (var member in component.GetType().GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (!items.Contains(member.Name))
                            {
                                items.Add(string.Concat(component.GetType().Name, ".", member.Name));
                            }
                        }
                    }
                }
                string[] selectItems = items.ToArray();
                System.Array.Sort(selectItems);
                HierarchySearcherObjectSearchWindow.OpenWindow(selectItems, idx);
            }
        }

        EndOptionContent(idx);
    }

    private void OptionFieldContent(int idx)
    {
        BeginOptionContent();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Field Name", GUILayout.Width(200));
        if (searchOptions[idx].obj == null)
        {
            searchOptions[idx].obj = "None";
        }
        bool pressed = GUILayout.Button(searchOptions[idx].obj as string, EditorStyles.numberField, GUILayout.Width(150));

        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            if (pressed)
            {
                HashSet<string> items = new HashSet<string>();
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
                foreach (GameObject item in objects)
                {
                    Component[] components = item.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        foreach (var member in component.GetType().GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (!items.Contains(member.Name))
                            {
                                items.Add(string.Concat(component.GetType().Name, ".", member.Name));
                            }
                        }
                    }
                }
                string[] selectItems = items.ToArray();
                System.Array.Sort(selectItems);
                HierarchySearcherObjectSearchWindow.OpenWindow(selectItems, idx);
            }
        }

        EndOptionContent(idx);
    }

    private void OptionFunctionContent(int idx)
    {
        BeginOptionContent();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Function Name", GUILayout.Width(200));
        if (searchOptions[idx].obj == null)
        {
            searchOptions[idx].obj = "None";
        }
        bool pressed = GUILayout.Button(searchOptions[idx].obj as string, EditorStyles.numberField, GUILayout.Width(150));

        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            if (pressed)
            {
                HashSet<string> items = new HashSet<string>();
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
                foreach (GameObject item in objects)
                {
                    Component[] components = item.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        foreach (var member in component.GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                        {
                            string checkMethod = member.Name.Substring(0, 4);
                            if (checkMethod == "set_" || checkMethod == "get_") continue;
                            if (!items.Contains(member.Name))
                            {
                                items.Add(string.Concat(component.GetType().Name, ".", member.Name));
                            }
                        }
                    }
                }
                string[] selectItems = items.ToArray();
                System.Array.Sort(selectItems);
                HierarchySearcherObjectSearchWindow.OpenWindow(selectItems, idx);
            }
        }

        EndOptionContent(idx);
    }

    private void OptionLayerContent(int idx)
    {
        BeginOptionContent();
        int? temp = searchOptions[idx].obj as int?;
        searchOptions[idx].obj = EditorGUILayout.LayerField("Layer", temp.HasValue ? temp.Value : 0);

        EndOptionContent(idx);
    }

    private void OptionTagContent(int idx)
    {
        BeginOptionContent();
        searchOptions[idx].obj = EditorGUILayout.TagField("Tag", searchOptions[idx].obj as string);

        EndOptionContent(idx);
    }

    private void OptionComponentContent(int idx)
    {
        BeginOptionContent();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Component Name", GUILayout.Width(200));
        if (searchOptions[idx].obj == null)
        {
            searchOptions[idx].obj = "None";
        }
        bool pressed = GUILayout.Button(searchOptions[idx].obj as string, EditorStyles.numberField, GUILayout.Width(150));

        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            if (pressed)
            {
                HashSet<string> items = new HashSet<string>();
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
                foreach (GameObject item in objects)
                {
                    Component[] components = item.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        if (!items.Contains(component.GetType().Name))
                        {
                            items.Add(component.GetType().Name);
                        }
                    }
                }
                string[] selectItems = items.ToArray();
                System.Array.Sort(selectItems);
                HierarchySearcherObjectSearchWindow.OpenWindow(selectItems, idx);
            }
        }

        EndOptionContent(idx);
    }

    private void OptionNameContent(int idx)
    {
        BeginOptionContent();
        searchOptions[idx].obj = EditorGUILayout.TextField("Name", searchOptions[idx].obj as string);

        EndOptionContent(idx);
    }

    private void BeginOptionContent()
    {
        EditorGUILayout.BeginVertical(GUI.skin.window, GUILayout.Width(HierarchySearcher.window.position.width - HierarchySearcher.window.position.width / 10), GUILayout.Height(50));
        EditorGUILayout.BeginVertical(GUILayout.Width(HierarchySearcher.window.position.width - HierarchySearcher.window.position.width / 10 - 20), GUILayout.Height(20));

    }

    private void EndOptionContent(int idx)
    {
        EditorGUILayout.EndVertical();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space(10);
        bool removeOption = GUILayout.Button("Remove Option", GUILayout.Width(HierarchySearcher.window.position.width - HierarchySearcher.window.position.width / 10), GUILayout.Height(20));
        
        if (EditorGUI.EndChangeCheck())
        {
            if (removeOption)
            {
                gameObjects.Clear();
                searchOptions.RemoveAt(idx);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(4);
    }
}
