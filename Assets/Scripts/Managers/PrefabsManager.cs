using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum PrefabsType {

    Players = 0,
    Monsters,
    Items,
    NPCs,

    Max,
}
public class PrefabsData
{
    public String Name;
    public PrefabsType type;
    public Dictionary<String, GameObject> dicDatas = new Dictionary<string, GameObject>();

    public PrefabsData(PrefabsType _type)
    {
        this.type = _type;
        this.Name = String.Format("Prefabs/{0}/",_type.ToString());
    }
    public GameObject Get(String _name)
    {
        GameObject obj;
        if (!dicDatas.TryGetValue(_name, out obj))
        {
            obj = Resources.Load<GameObject>(Name + _name);
            dicDatas.Add(_name, obj);
        }
        return obj;
    }
}
public class PrefabsManager
{
    static PrefabsData[] lsDatas = new PrefabsData[] {
        new PrefabsData(PrefabsType.Players),
        new PrefabsData(PrefabsType.Monsters),
        new PrefabsData(PrefabsType.Items),
    };
    static GameObject Get(PrefabsType _type, String _name)
    {
        int idx = (int)_type;
        return lsDatas[idx].Get(_name);
    }
    public static GameObject Instantiate(PrefabsType _type, String _name)
    {
        GameObject obj = Get(_type, _name);
        if(obj != null)
            return GameObject.Instantiate<GameObject>(obj);
        return null;
    }
}

