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
    ItemType type = ItemType.NULL;
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

    public void InitItem(uint tabId)
    {
        this.UId = tabId;
    }
    public virtual void InitItemEx() { }
   

    static public BaseItem newItem(uint tabId)
    {
        BaseItem _item = null;
        ItemType _type = ItemType.EQUIP;
        switch (_type)
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
        if(_item != null)
        {
            _item.InitItem(tabId);
            _item.InitItemEx();
        }
        return _item;
    }
   
}
