using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SpriteManager
{
    //const String iconEquipsHeadName = "Icons/Equips/";
    public static Dictionary<String, SpriteRenderer> dicIconEquips = new Dictionary<string, SpriteRenderer>();

    public static void Init(Transform _root)
    {
        //初始化装备ICON
        SpriteRenderer[] sp = _root.FindChild("Icons").FindChild("Equips").GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < sp.Length; i++)
        {
            dicIconEquips.Add(sp[i].name, sp[i]);
        }
    }

    public static Sprite GetIconEquip(String _name)
    {
        SpriteRenderer sp;
        if (dicIconEquips.TryGetValue(_name, out sp))
            return sp.sprite;
        return null;
    }
}

