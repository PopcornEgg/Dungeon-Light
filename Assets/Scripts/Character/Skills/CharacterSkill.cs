using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HasSkills
{
    public class SkillData
    {
        public UInt32 skillId;
        public UInt32 level = 1;
        public UInt32 exp = 0;

        public void AddExp(UInt32 _ecp) { }
    }
    public Dictionary<UInt32, SkillData> dicHasSkills = new Dictionary<uint, SkillData>();

    public int AddSkill(UInt32 _skillid)
    {
        if (!dicHasSkills.ContainsKey(_skillid))
        {
            SkillData _sd = new SkillData();
            dicHasSkills.Add(_skillid, _sd);
            return 1;
        }
        return -1;
    }
    public SkillData GetSkill(UInt32 _skillid)
    {
        SkillData _sd;
        if (dicHasSkills.TryGetValue(_skillid, out _sd))
            return _sd;
        return null;
    }
}

public class CharacterSkill
{
    //*********************************************************************************
    //数据部分
    public Character owner;
    public HasSkills hasSkills = new HasSkills();

    public CharacterSkill(Character _o)
    {
        owner = _o;
    }

    //*********************************************************************************
    //冷却时间管理器
    public class CoolDown
    {
        public UInt32 skillId;
        public UInt32 endTime;
    }

    public Dictionary<UInt32, float> dicCoolDown = new Dictionary<uint, float>();
    public void AddCD(UInt32 _skillid, SkillTab _stab)
    {
        if (!dicCoolDown.ContainsKey(_skillid))
        {
            if (_stab != null)
                dicCoolDown.Add(_skillid, Time.time + _stab.cdTime);
        }
    }

    //*********************************************************************************
    //逻辑部分
    public Dictionary<UInt32, BaseInsSkill> dicInsSkills = new Dictionary<UInt32, BaseInsSkill>();

    //释放一个技能
    public void InstanceSkill(UInt32 _skillid, Character _tag)
    {
        //位冷却完
        if (dicCoolDown.ContainsKey(_skillid))
            return;

        //技能表中存在
        SkillTab _stab = SkillTab.Get(_skillid);
        if (_stab == null)
            return;

        //自己拥有该技能
        HasSkills.SkillData _skilldata = hasSkills.GetSkill(_skillid);
        if(_skilldata != null)
        {
            BaseInsSkill insSkill = new BaseInsSkill(owner, _tag);
            insSkill.skillData = _skilldata;
            insSkill.skillTab = _stab;
            dicInsSkills.Add(_skillid, insSkill);
            AddCD(_skillid, _stab);
        }
    }
    public void OnTick()
    {
        //执行技能逻辑
        OnSkillLogic();

        //技能CD
        OnCDLogic();
    }
    public void OnSkillLogic()
    {
        List<UInt32> res = new List<UInt32>();
        foreach (KeyValuePair<UInt32, BaseInsSkill> insSkill in dicInsSkills)
        {
            if (!insSkill.Value.OnTick())
                res.Add(insSkill.Key);
        }
        foreach (UInt32 uid in res)
        {
            dicInsSkills.Remove(uid);
        }
    }
    public void OnCDLogic()
    {
        float currTime = Time.time;
        List<UInt32> res = new List<UInt32>();
        foreach (KeyValuePair<UInt32, float> cd in dicCoolDown)
        {
            if (currTime >= cd.Value)
                res.Add(cd.Key);
        }
        foreach (UInt32 uid in res)
        {
            dicCoolDown.Remove(uid);
        }
    }
}
