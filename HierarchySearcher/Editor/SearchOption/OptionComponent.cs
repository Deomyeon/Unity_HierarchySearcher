using UnityEngine;
using System.Collections.Generic;

public class OptionComponent : SearchOption
{

    public override List<GameObject> SearchGameObject(List<GameObject> gameObjects, bool first)
    {
        if (obj != null)
        {
            if (first)
            {
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
                foreach (GameObject item in objects)
                {
                    bool contains = false;
                    Component[] components = item.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        if (component.GetType().Name == (obj as string))
                        {
                            contains = true;
                        }
                    }
                    if (contains)
                    {
                        gameObjects.Add(item as GameObject);
                    }
                }
            }
            else
            {
                List<GameObject> removeItems = new List<GameObject>();
                foreach (GameObject item in gameObjects)
                {
                    bool contains = false;
                    Component[] components = item.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        if (component.GetType().Name == (obj as string))
                        {
                            contains = true;
                        }
                    }
                    if (!contains)
                    {
                        removeItems.Add(item as GameObject);
                    }
                }
                foreach (GameObject item in removeItems)
                {
                    gameObjects.Remove(item);
                }
            }
        }

        return gameObjects;
    }
}
