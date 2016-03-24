using UnityEngine;
using System.Collections;
using System;
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

    //*********************************************************************************
    //身上装备
    public EquipItem[] bodyEuiqpItems = new EquipItem[(int)ItemEquipType.MAX];

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
}
