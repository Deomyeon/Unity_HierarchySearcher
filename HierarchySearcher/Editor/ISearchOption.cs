using UnityEngine;
using System.Collections.Generic;

public interface ISearchOption
{
    public object obj   {get;set;}

    public List<GameObject> SearchGameObject(List<GameObject> gameObjects, bool first = false);
}
