using UnityEngine;
using System.Collections.Generic;

public class OptionLayer : SearchOption
{

    public override List<GameObject> SearchGameObject(List<GameObject> gameObjects, bool first)
    {
        if (obj != null)
        {
            int value = (obj as int?).Value;
            if (first)
            {
                Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
                foreach (GameObject item in objects)
                {
                    if (item.layer == value)
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
                    if (!(item.layer == value))
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