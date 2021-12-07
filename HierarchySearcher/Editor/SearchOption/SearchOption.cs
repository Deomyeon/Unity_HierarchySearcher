using UnityEngine;
using System.Collections.Generic;

public class SearchOption
{
    public object obj;

    public virtual List<GameObject> SearchGameObject(List<GameObject> gameObjects, bool first = false)
    {
        return gameObjects;
    }
}
