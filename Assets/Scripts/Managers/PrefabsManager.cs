using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PrefabsManager
{
    static Dictionary<String, GameObject> dicDatas = new Dictionary<string, GameObject>();

    public static GameObject Get(String _name)
    {
        GameObject obj;
        if (!dicDatas.TryGetValue(_name, out obj))
        {
            obj = Resources.Load<GameObject>("Prefabs/" + _name);
            dicDatas.Add(_name, obj);
        }
        return obj;
    }
}

