using UnityEngine;
using System.Collections;
using System;

public abstract class BaseItem
{
    //表索引ID
    uint tabId;
    public uint TabId { get { return tabId; } set { tabId = value; } }

    //唯一ID
    UInt64 uId;
    public UInt64 UId { get { return uId; } set { uId = value; } }

    //类型
    ItemType type = ItemType.MAX;
    public ItemType Type {get { return type; }set { type = value; }}

    //名字
    String name;
    public String Name { get { return name; } set { name = value; } }

    //售价
    int price;
    public int Price{get { return price; }set { price = value; } }

    //装备等级 / 需求等级
    int level;
    public int Level { get { return level; } set { level = value; } }

    //品质
    int quality;
    public int Quality { get { return quality; } set { quality = value; } }

    public void InitItem(ItemTab _tab)
    {
        this.UId = Utils.GuidMaker.GenerateUInt64();
        this.tabId = _tab.tabid;
        this.type = _tab.type;
        this.Name = _tab.name;
        this.price = _tab.price;
        this.level = _tab.level;
        this.quality = _tab.quality;
    }

    public virtual void InitItemEx(ItemTab _tab) { }

    static public BaseItem newItem(ItemTab _tab)
    {
        if (_tab == null)
            return null;

        BaseItem _item = null;
        switch (_tab.type)
        {
            case ItemType.EQUIP:
                {
                    _item = new EquipItem();
                    break;
                }
            case ItemType.MEDICINE:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
        if (_item != null)
        {
            _item.InitItem(_tab);
            _item.InitItemEx(_tab);
        }
        return _item;
    }
    static public BaseItem newItem(uint tabId)
    {
        ItemTab _tab = ItemTab.Get(tabId);
        return newItem(_tab);
    }
   
}
