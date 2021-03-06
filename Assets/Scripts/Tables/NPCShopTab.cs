﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ShopType
{
    General = 0,
    Equip,

    Max = 20,
}
public enum ShopBuyType
{
    Money = 0,
}
public class NPCShopTab
{
    //表索引ID
    public readonly uint tabid;
    //类型
    public readonly ShopType stype;

    //攻防属性
    public readonly uint[] idlist;
    public readonly uint[] price;
    public readonly ShopBuyType[] buytype;
    public readonly int[] buycount;

    public NPCShopTab(int i, TabReader tr)
    {
        tabid = tr.GetItemUInt32(i, "tabid");
        stype = (ShopType)tabid;

        string[] sp = tr.GetString(i, "idlist").Split('|');
        idlist = new uint[sp.Length];
        for (int j = 0; j < sp.Length; j++)
            idlist[j] = Convert.ToUInt32(sp[j]);

        sp = tr.GetString(i, "price").Split('|');
        price = new uint[sp.Length];
        for (int j = 0; j < sp.Length; j++)
            price[j] = Convert.ToUInt32(sp[j]);

        sp = tr.GetString(i, "buytype").Split('|');
        buytype = new ShopBuyType[sp.Length];
        for (int j = 0; j < sp.Length; j++)
            buytype[j] = (ShopBuyType)Convert.ToUInt32(sp[j]);

        sp = tr.GetString(i, "buycount").Split('|');
        buycount = new int[sp.Length];
        for (int j = 0; j < sp.Length; j++)
            buycount[j] = Convert.ToInt32(sp[j]);
    }
    public static List< NPCShopTab > lsTabs = new List< NPCShopTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/npcshoptab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            NPCShopTab _item = new NPCShopTab(i, tr);
            lsTabs.Add(_item);
        }
    }

    public static int Count {
        get { return lsTabs.Count; }
    }
    public static NPCShopTab Get(uint tabid)
    {
        if(tabid < lsTabs.Count)
            return lsTabs[(int)tabid];
        return null;
    }
}
