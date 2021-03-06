﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SpriteManager
{
    //const String iconEquipsHeadName = "Icons/Equips/";
    public static Dictionary<String, SpriteRenderer> dicIconEquips = new Dictionary<string, SpriteRenderer>();
    public static Dictionary<String, SpriteRenderer> dicIconMedicines = new Dictionary<string, SpriteRenderer>();

    public static void Init(Transform _root)
    {
        //初始化装备ICON
        Transform Icons = _root.FindChild("Icons");
        SpriteRenderer[] sp = Icons.FindChild("Equips").GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < sp.Length; i++)
        {
            dicIconEquips.Add(sp[i].name, sp[i]);
        }

        sp = Icons.FindChild("Medicines").GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sp.Length; i++)
        {
            dicIconMedicines.Add(sp[i].name, sp[i]);
        }

        //最后隐藏Icons
        Icons.gameObject.SetActive(false);
    }

    public static Sprite GetIcon(String _name)
    {
        SpriteRenderer sp;
        if (dicIconEquips.TryGetValue(_name, out sp))
            return sp.sprite;
        else if (dicIconMedicines.TryGetValue(_name, out sp))
            return sp.sprite;
        return null;
    }
}

