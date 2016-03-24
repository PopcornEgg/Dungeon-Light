using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum SkillRangeType
{
    Single = 0,
    Ray,
    Rect,
    Circle,
    Sector,
}
public class SkillDamage
{
    public readonly PropertyType type;
    public readonly int[] radix;//基数
    public readonly float[] coefficient;// 系数

    public SkillDamage(PropertyType _t, int[] _r, float[] _c)
    {
        type = _t;
        radix = _r;
        coefficient = _c;
    }
}
public class SkillTab
{
    //基础属性
    public const int MaxSkillDamageCount = 2;
    public readonly uint tabid;
    public readonly string name;
    public readonly SkillRangeType SRType;//范围类型
    public readonly float cdTime;//冷却时间
    public readonly float lastTime;//持续时间(技能的动作播放时间)
    public readonly bool skillingCanMove;//释放技能期间能否移动


    //是否是持续伤害
    public readonly bool isLastDamage;
    #region 持续伤害
    public readonly int maxTakeDamage;//最多能造成的伤害 =0表示无限制
    public readonly int maxCountDamage;//造成伤害的次数 =0表示无限制
    public readonly float damageSpaceTime;//伤害间隔时间
    #endregion
    #region 非持续伤害
    public readonly float damageDelayTime;//产生伤害的延迟时间
    #endregion


    //伤害属性
    public readonly bool canCritical;//能否暴击
    public readonly int criticalRate;//暴击率 （百分比）
    public readonly bool isRealCritical;//是否真实伤害
    public readonly int realDamage;//真实伤害数值 >0表示采用该数值 <=0表示按公式计算
    public readonly SkillDamage[] skillDamage = new SkillDamage[MaxSkillDamageCount];//伤害的计算公式数据

    public SkillTab(int i, TabReader tr)
    {
        //基础属性
        tabid = tr.GetItemUInt32(i, "tabid");
        name = tr.GetString(i, "name");
        SRType = (SkillRangeType)tr.GetItemUInt32(i, "srtype");
        cdTime = tr.GetItemFloat(i, "cdtime");
        lastTime = tr.GetItemFloat(i, "lasttime");
        skillingCanMove = tr.GetItemBoolean(i, "skillingcanmove");

        //是否是持续伤害
        isLastDamage = tr.GetItemBoolean(i, "islastdamage");
        maxTakeDamage = tr.GetItemInt32(i, "maxtakedamage");
        maxCountDamage = tr.GetItemInt32(i, "maxcountdamage");
        damageSpaceTime = tr.GetItemFloat(i, "damagespacetime");
        damageDelayTime = tr.GetItemFloat(i, "damagedelaytime");

        //伤害属性
        canCritical = tr.GetItemBoolean(i, "cancritical");
        criticalRate = tr.GetItemInt32(i, "criticalrate");
        isRealCritical = tr.GetItemBoolean(i, "isrealcritical");
        realDamage = tr.GetItemInt32(i, "realdamage");

        for (int j = 0; j < skillDamage.Length; j++)
        {
            int _t = tr.GetItemInt32(i, "skilldamaget" + j.ToString());
            if (_t < 0)
                continue;
            PropertyType _ptype = (PropertyType)_t;

            string[] spradix = tr.GetString(i, "skilldamager" + j.ToString()).Split('|');
            string[] spcoefficient = tr.GetString(i, "skilldamagec" + j.ToString()).Split('|');

            int[] radixs = new int[spradix.Length];
            for(int k = 0; k < radixs.Length; k++)
                radixs[k] = Convert.ToInt32(spradix[k]);
            float[] coefficients = new float[spcoefficient.Length];
            for (int k = 0; k < coefficients.Length; k++)
                coefficients[k] = Convert.ToSingle(spcoefficient[k]);

            skillDamage[j] = new SkillDamage(_ptype, radixs, coefficients);
        }
    }
    public static Dictionary<uint, SkillTab> dicTabs = new Dictionary<uint, SkillTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/skilltab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            SkillTab _item = new SkillTab(i, tr);
            dicTabs.Add(_item.tabid, _item);
        }
    }

    public static SkillTab Get(uint tabid)
    {
        SkillTab _item;
        if (dicTabs.TryGetValue(tabid, out _item))
            return _item;
        return null;
    }
}
