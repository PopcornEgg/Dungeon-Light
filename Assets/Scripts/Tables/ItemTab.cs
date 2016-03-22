using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class ItemTab
{
    #region ***属性数据***
    //表索引ID
    public uint tabid;
    //类型
    public ItemType type;
    //名字
    public String name;
    //售价
    public int price;
    //装备等级 / 需求等级
    public int level;
    //品质
    public int quality;
    #endregion

    #region*** 外观数据***
    //掉落外观
    public string dropModel;
    //掉落高度
    public float dropHeight;
    //掉落缩放
    public float dropScale;
    #endregion

    public static Dictionary<uint, ItemTab> dicTabs = new Dictionary<uint, ItemTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/itemtab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            ItemTab _item = new ItemTab();
            //属性数据
            _item.tabid = tr.GetItemUInt32(i, "tabid");
            _item.type = (ItemType)tr.GetItemUInt32(i, "type");
            _item.name = tr.GetString(i, "name");
            _item.price = tr.GetItemInt32(i, "price");
            _item.level = tr.GetItemInt32(i, "level");
            _item.quality = tr.GetItemInt32(i, "quality");
            //外观数据
            _item.dropModel = tr.GetString(i, "dropmodel");
            _item.dropHeight = tr.GetItemFloat(i, "dropheight");
            _item.dropScale = tr.GetItemFloat(i, "dropscale");

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
