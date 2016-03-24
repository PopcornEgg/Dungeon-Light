using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SkillData
{
    //技能范畴
    public float attackLastTime = 2.0f;//攻击持续时间
    public float attackSpaceTime = 20.5f;//攻击间隔时间
    public float attackSpeed
    {
        get { return attackSpaceTime + attackLastTime; }
    }
    public float nextAttackTime = 0;

    public int id;
    public float CDTime;
    public float damageDelay = 1.0f;
    public float attackDistance = 3.0f;

}
public class CharacterSkill
{
    //技能范畴
    public float attackLastTime = 2.0f;//攻击持续时间
    public float attackSpaceTime = 20.5f;//攻击间隔时间
    public float attackSpeed {
        get { return attackSpaceTime + attackLastTime; }
    }
    public float nextAttackTime = 0;
   

    public float CDTime;
    public float damageDelay = 1.0f;
    public float attackDistance = 3.0f;


    public void Tick()
    {

    }
}
