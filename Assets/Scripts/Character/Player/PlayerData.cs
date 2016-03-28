using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

//player数据处理部分
public partial class Player : Character
{
    //*********************************************************************************
    //背包
    public static int bagSpace = 50;//背包容量
    public BaseItem[] bagItems = new BaseItem[bagSpace];
    public int CurrBagItemCount {
        get {
            int _count = 0;
            for (int i = 0; i < bagItems.Length; i++)
            {
                if (bagItems[i] != null)
                    _count++;
            }
            return _count;
        }
    }
    public bool AddBagItem(BaseItem _item)
    {
        for(int i=0;i< bagItems.Length; i++)
        {
            if (bagItems[i] == null)
            {
                bagItems[i] = _item;
                return true;
            }
        }
        return false;
    }
    public void DelBagItem(int idx)
    {
        if (idx < 0 || idx >= bagItems.Length)
            return;

        bagItems[idx] = null;
    }
    public void UseBagItem(int idx)
    {
        BaseItem _item = bagItems[idx];
        if (_item == null)
            return;

        switch (_item.Type)
        {
            case ItemType.EQUIP:
                {
                    UseBagEquip((EquipItem)_item, idx);
                    break;
                }
            default:
                break;
        }
    }
    public void UseBagEquip(EquipItem _item, int bagIdx)
    {
        int bodyIdx = (int)_item.EquipType;
        if (_item.EquipType == ItemEquipType.Accessory)//特殊处理
        {
            if (bodyEuiqpItems[bodyIdx - 1] == null)
            {
                bodyEuiqpItems[bodyIdx - 1] = _item;
                bagItems[bagIdx] = null;
            }
            else if (bodyEuiqpItems[bodyIdx] == null)
            {
                bodyEuiqpItems[bodyIdx] = _item;
                bagItems[bagIdx] = null;
            }
            else//默认替换第一个饰品
            {
                BaseItem buffer = bodyEuiqpItems[bodyIdx - 1];
                bodyEuiqpItems[bodyIdx - 1] = _item;
                bagItems[bagIdx] = buffer;
            }
        }
        else
        {
            if (bodyEuiqpItems[bodyIdx] == null)
            {
                bodyEuiqpItems[bodyIdx] = _item;
                bagItems[bagIdx] = null;
            }
            else
            {
                BaseItem buffer = bodyEuiqpItems[bodyIdx];
                bodyEuiqpItems[bodyIdx] = _item;
                bagItems[bagIdx] = buffer;
            }
        }
        StaticManager.sSecond_Canvas.RefreshPlayerBag();
        StaticManager.sSecond_Canvas.RefreshPlayerProperty();
        playerEquipProperty.IsDirty = true;
    }
    public void SaveBagItems()
    {
        Utils.BinarySerialize.Serialize<BaseItem[]>(bagItems, "c:/bagItems.data");
    }
    public void LoadBagItems()
    {
        BaseItem[] datas = Utils.BinarySerialize.DeSerialize<BaseItem[]>("c:/bagItems.data");
        if (datas != null)
            bagItems = datas;
    }
    //*********************************************************************************
    //身上装备
    PlayerEquipProperty playerEquipProperty;
    public BaseItem[] bodyEuiqpItems = new BaseItem[(int)ItemEquipType.MAX];
    public void UseBodyItem(EquipItem _eitem, int idx)
    {

    }
    public void SaveBodyItems()
    {

    }

    //*********************************************************************************
    //技能
    public override void AddSkillExp(UInt32 _skillid, UInt32 _exp)
    {
        HasSkills.SkillData _csd = characterSkill.hasSkills.GetSkill(_skillid);
        if (_csd != null)
        {
            _csd.AddExp(_exp);
        }
    }

    //*********************************************************************************
    //人物属性
    PlayerBaseProperty playerBaseProperty;
    public String GetPropertyString()
    {
        StringBuilder txt = new StringBuilder();
        txt.Append(String.Format("{0}：{1}/{2}\n", PropertyNames.Names[(int)PropertyTypeEx.MAXHP], HP, MAXHP));
        txt.Append(String.Format("{0}：{1}/{2}\n", PropertyNames.Names[(int)PropertyTypeEx.MAXMP], MP, MAXMP));
        txt.Append(String.Format("{0}：{1}/{2}\n", PropertyNames.Names[(int)PropertyTypeEx.MAXSP], SP, MAXSP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.AD], AD));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.AP], AP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.ADD], ADD));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.APD], APD));
        txt.Append(String.Format("{0}：{1}", PropertyNames.Names[(int)PropertyTypeEx.MOVESPEED], MOVESPEED));

        return txt.ToString();
    }
}
