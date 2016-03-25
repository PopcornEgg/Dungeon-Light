using UnityEngine;
using System;
using System.Collections;

public enum AIState
{
    Null = 0,
    Idle,
    Follow,
    Attack,
}
public class BaseMonsterAI : MonoBehaviour
{
    //获取要追踪的对象
    Character self = null;
    Character target = null;
    UInt32 scanRange = 5;//扫描范围
    UInt32 followRange = 7;//扫描范围

    DebugDrawCircle scanCircle = null;
    DebugDrawCircle followCircle = null;
    public AIState aiState = AIState.Idle;

    //追踪范围
    //是否回到原位
    void Start()
    {
        self = GetComponent<Character>();
        SetTarget(StaticManager.sPlayer);

        scanCircle = gameObject.AddComponent<DebugDrawCircle>();
        scanCircle.SetRadius(scanRange);
        followCircle = gameObject.AddComponent<DebugDrawCircle>();
        followCircle.SetRadius(followRange);
    }
    void Update()
    {
        if (target == null || self == null)
            return;
        switch (aiState)
        {
            case AIState.Idle:
                {
                    if (target.CType == ScanTarget())
                    {
                        aiState = AIState.Follow;
                    }
                }
                break;
            case AIState.Follow:
                {
                    if (IsOutRange())
                    {
                        aiState = AIState.Idle;
                    }
                    else
                    {
                        AutoAttack();
                    }
                }
                break;
            case AIState.Attack:
                {
                   
                }
                break;
            default:
                break;
        }
    }

    public void SetTarget(Character _target)
    {
        target = _target;
    }
    public CharacterType ScanTarget()
    {
        CharacterType type = CharacterType.Null;
        if ( Vector3.Distance(transform.position, target.transform.position) < scanRange )
        {
            return target.CType;
        }
        return type;
    }
    public bool IsOutRange()
    {
        if ( Vector3.Distance(transform.position, target.transform.position) > followRange )
        {
            return true;
        }
        return false;
    }

    bool canDoNextSkill = true;
    SkillTab nextSkillTab;
    public SkillTab NextSkillTab
    {
        get { return nextSkillTab; }
        set
        {

        }
    }
    public void GenerateNextSkill()
    {
        if (NextSkillTab == null || self.characterSkill.IsCoolDown(NextSkillTab.tabid))
        {
            int idx = UnityEngine.Random.Range(0, self.characterSkill.hasSkills.lsHasSkills.Count);
            NextSkillTab = SkillTab.Get(self.characterSkill.hasSkills.lsHasSkills[idx].skillId);
        }
    }

    public void AutoAttack()
    {
        GenerateNextSkill();
        if (canDoNextSkill)
        {
            self.characterSkill.InstanceSkill(nextSkillTab);
            canDoNextSkill = false;
        }
    }
}
