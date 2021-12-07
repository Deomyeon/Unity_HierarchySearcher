using UnityEngine;
using System.Collections.Generic;

public class OptionName : ISearchOption
{
    public object obj   {get;set;}

    public List<GameObject> SearchGameObject(List<GameObject> gameObjects, bool first)
    {
        if (obj != null)
        {
            if (first)
            {
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (Object item in objects)
                {
                    if (item.name.ToLower().Contains((obj as string).ToLower()))
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
                    if (!item.name.ToLower().Contains((obj as string).ToLower()))
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
