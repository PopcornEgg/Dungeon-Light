using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public abstract class BaseItem
{
    //表索引ID
    uint tabId;
    public uint TabId { get { return tabId; } }

    //唯一ID
    UInt64 uId;
    public UInt64 UId { get { return uId; } }

    //配置表中的数据
    [NonSerialized]
    ItemTab tabData;
    public ItemTab TabData { get { return tabData; } set { tabData = value; } }

    //类型
    public ItemType Type {get { return tabData.type; }}

    //装备位置
    public ItemEquipType EquipType { get { return TabData.ietype; } }

    //名字
    public String Name { get { return tabData.name; }}

    //售价
    public int Price{get { return tabData.price; }}

    //装备等级 / 需求等级
    public int Level { get { return tabData.level; } }

    //品质
    public int Quality { get { return tabData.quality; } }

    int overlayCount = 1;
    public int OverlayCount { get { return overlayCount; } set { overlayCount = value; } }

    public void InitItem(ItemTab _tab)
    {
        this.uId = Utils.GuidMaker.GenerateUInt64();
        this.tabId = _tab.tabid;
        this.tabData = _tab;
    }

    public virtual void InitItemEx() { }

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
                    _item = new MedicineItem();
                    break;
                }
            default:
                {
                    _item = new MoneyItem();
                    break;
                }
        }
        if (_item != null)
        {
            _item.InitItem(_tab);
            _item.InitItemEx();
        }
        return _item;
    }
    static public BaseItem newItem(uint tabId)
    {
        ItemTab _tab = ItemTab.Get(tabId);
        return newItem(_tab);
    }
   
}
