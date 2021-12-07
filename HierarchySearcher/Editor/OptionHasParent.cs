using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHasParent : ISearchOption
{
    public object obj   {get;set;}

    public List<GameObject> SearchGameObject(List<GameObject> gameObjects, bool first)
    {
        if (obj != null)
        {
            if (first)
            {
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (GameObject item in objects)
                {
                    if ((item.transform.parent != null) == (obj as bool?).Value)
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
                    if (!((item.transform.parent != null) == (obj as bool?).Value))
                    {
                        removeItems.Add(item);
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