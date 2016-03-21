using UnityEngine;
using System.Collections;

public enum PropertyType
{
    AD = 0,//attact damage（物理伤害）
    AP, //Ability Power（技能力量）

    MAX,
}

public enum ItemType
{
    NULL = 0,
    EQUIP,
    MEDICINE,

    MAX,
}

//装备使用的位置
public enum ItemEquipType
{
    Head = 0,
    Body,
    Hand,
    Ring,//戒指有两个
    Leg = Ring + 2,

    MAX,
}