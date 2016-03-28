using UnityEngine;
using System.Collections;

public enum PropertyType
{
    HP = 0,
    MAXHP,
    MP,
    MAXMP,
    AD,//attact damage（物理伤害）
    AP, //Ability Power（技能力量）
    ADD,//attact damage defense（物理伤害防御）
    APD, //Ability Power defense（技能力量防御）
    MOVESPEED,

    MAX,
}

public enum ItemType
{
    MONEY = 0,
    MEDICINE,
    EQUIP,

    MAX,
}

//装备使用的位置
public enum ItemEquipType
{
    Head = 0,
    Body,
    Hand,
    Leg,
    Accessory = Leg + 2,//饰品两个

    MAX,
}