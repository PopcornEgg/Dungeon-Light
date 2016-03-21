using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class ItemTab
{
    //表索引ID
    public uint tabid;
    //类型
    public ItemType type = ItemType.NULL;
    //名字
    public String name;
    //售价
    public int price;
    //装备等级 / 需求等级
    public int level;
    //品质
    public int quality;

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/itemtab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            ItemTab _item = new ItemTab();
            _item.tabid = tr.GetItemUInt32(i, "tabid");
            _item.type = (ItemType)tr.GetItemUInt32(i, "type");
            _item.name = tr.GetString(i, "name");
            _item.price = tr.GetItemInt32(i, "price");
            _item.level = tr.GetItemInt32(i, "level");
            _item.quality = tr.GetItemInt32(i, "quality");

            Add(_item.tabid, _item );
        }
    }

    public static Dictionary<uint, ItemTab> dicTabs = new Dictionary<uint, ItemTab>();

    public static void Add(uint idx , ItemTab _item)
    {
        if (_item != null)
        {
            dicTabs.Add(idx, _item);
        }
    }
    public static ItemTab GetByDic(uint tabid)
    {
        ItemTab _item;
        if (dicTabs.TryGetValue(tabid, out _item))
            return _item;
        return null;
    }
}
