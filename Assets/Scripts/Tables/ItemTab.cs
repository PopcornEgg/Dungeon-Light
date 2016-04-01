using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class ItemTab
{
    #region ***属性数据***
    //表索引ID
    public readonly uint tabid;
    //类型
    public readonly ItemType type;
    //类型
    public readonly ItemEquipType ietype;
    //名字
    public readonly String name;
    //售价
    public readonly int price;
    //装备等级 / 需求等级
    public readonly int level;
    //品质
    public readonly int quality;
    #endregion

    //攻防属性
    public readonly float[] propertyEx = new float[(int)PropertyTypeEx.MAX];

    #region*** 显示数据***
    //掉落外观
    public readonly string icon;
    //掉落外观
    public readonly string model;
    //掉落高度
    public readonly float dropHeight;
    //掉落缩放
    public readonly float dropScale;
    #endregion

    public ItemTab(int i, TabReader tr)
    {
        tabid = tr.GetItemUInt32(i, "tabid");
        type = (ItemType)tr.GetItemUInt32(i, "type");
        ietype = (ItemEquipType)tr.GetItemUInt32(i, "ietype");
        name = tr.GetString(i, "name");
        price = tr.GetItemInt32(i, "price");
        level = tr.GetItemInt32(i, "level");
        quality = tr.GetItemInt32(i, "quality");

        propertyEx[(int)PropertyTypeEx.MAXHP] = tr.GetItemFloat(i, "maxhp");
        propertyEx[(int)PropertyTypeEx.MAXMP] = tr.GetItemFloat(i, "maxmp");
        propertyEx[(int)PropertyTypeEx.AD] = tr.GetItemFloat(i, "ad");
        propertyEx[(int)PropertyTypeEx.AP] = tr.GetItemFloat(i, "ap");
        propertyEx[(int)PropertyTypeEx.ADD] = tr.GetItemFloat(i, "add");
        propertyEx[(int)PropertyTypeEx.APD] = tr.GetItemFloat(i, "apd");
        propertyEx[(int)PropertyTypeEx.MOVESPEED] = tr.GetItemFloat(i, "movespeed");

        //显示数据
        icon = tr.GetString(i, "icon");
        model = tr.GetString(i, "model");
        dropHeight = tr.GetItemFloat(i, "dropheight");
        dropScale = tr.GetItemFloat(i, "dropscale");
    }
    public static Dictionary<uint, ItemTab> dicTabs = new Dictionary<uint, ItemTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/itemtab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            ItemTab _item = new ItemTab(i, tr);
            dicTabs.Add(_item.tabid, _item);
        }
    }

    public static ItemTab Get(uint tabid)
    {
        ItemTab _item;
        if (dicTabs.TryGetValue(tabid, out _item))
            return _item;
        return null;
    }
}
