using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

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
        int bodyIdx = (int)_item.itemEquipType;
        if (_item.itemEquipType == ItemEquipType.Accessory)//特殊处理
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
    }

    //*********************************************************************************
    //身上装备
    public EquipItem[] bodyEuiqpItems = new EquipItem[(int)ItemEquipType.MAX];
    public void UseBodyItem(EquipItem _eitem, int idx)
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
    public String GetPropertyString()
    {
        StringBuilder txt = new StringBuilder();
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.HP], HP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.MP], MP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.MAXHP], MAXHP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.MAXMP], MAXMP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.AD], AD));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.AP], AP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.ADD], ADD));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyType.APD], APD));
        txt.Append(String.Format("{0}：{1}", PropertyNames.Names[(int)PropertyType.MOVESPEED], (int)(MOVESPEED * 100.0f)));

        return txt.ToString();
    }
}
