using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseInsSkill
{
    public Character owner;
    public Character target;
    public readonly float insTime;//实例化时间
    public HasSkills.SkillData skillData;
    public SkillTab skillTab;

    public BaseInsSkill(Character _o, Character _t)
    {
        owner = _o;
        target = _t;
        insTime = Time.time;
    }
    public bool OnTick()
    {
        //生命周期到了
        if (skillTab.lastTime + insTime >= Time.time)
            return false;

        return OnTickEx();
    }

    public virtual bool OnTickEx()
    {
        // CommonAttack
        int _damages = 0;
        int idx = (int)skillData.level - 1;
        for (int i=0;i< skillTab.skillDamage.Length; i++)
        {
            SkillTab.SkillDamage sdamage = skillTab.skillDamage[i];
            if(sdamage != null)
            {
                int bv = owner.property[(int)sdamage.type];
                _damages += sdamage.radix[idx] + (int)(sdamage.coefficient[idx] * (float)bv);
                target.TakeDamage((UInt32)_damages);
            }
        }
        return true;
    }
}

