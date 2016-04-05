using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class PlayerLvTab
{
    static System.Random random = new System.Random();

    //表索引ID
    public readonly uint level;

    //攻防属性
    public readonly int maxhp;
    public readonly int maxmp;
    public readonly int ad;
    public readonly int ap;
    public readonly int add;
    public readonly int apd;
    public readonly float movespeed;
    //经验
    public readonly int exp;

    public PlayerLvTab(int i, TabReader tr)
    {
        //属性数据
        level = tr.GetItemUInt32(i, "lv");
    
        //攻防属性
        maxhp = tr.GetItemInt32(i, "maxhp");
        maxmp = tr.GetItemInt32(i, "maxmp");
        ad = tr.GetItemInt32(i, "ad");
        ap = tr.GetItemInt32(i, "ap");
        add = tr.GetItemInt32(i, "add");
        apd = tr.GetItemInt32(i, "apd");
        movespeed = tr.GetItemFloat(i, "movespeed");

        exp = tr.GetItemInt32(i, "exp");
    }
    //静态
    public static List<PlayerLvTab> lsTabs = new List<PlayerLvTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/playerlvtab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            PlayerLvTab _item = new PlayerLvTab(i, tr);
            lsTabs.Add( _item);
        }
    }

    public static PlayerLvTab Get(uint lv)
    {
        if (lv < lsTabs.Count)
            return lsTabs[(int)lv];
        return null;
    }
}
