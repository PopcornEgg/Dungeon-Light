using UnityEngine;
using System.Collections;
using System;



public class Monster : Character
{
    NavMeshAgent nav;
    
    Character target = null;
    UInt32 scanRange = 5;//扫描范围
    UInt32 followRange = 7;//扫描范围

    public override void AwakeEx()
    {
        attackAbleLayer = LayerMask.GetMask("Player");
        CType = CharacterType.Monster;
        nav = GetComponent<NavMeshAgent>();
        characterSkill.hasSkills.AddSkill(0);//测试
        //ai = gameObject.AddComponent<BaseMonsterAI>();

        //AddRigidbody();
    }

    void AddRigidbody()
    {
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.mass = 1.0f;
        rigidBody.drag = 0;
        rigidBody.angularDrag = 0.05f;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.interpolation = RigidbodyInterpolation.None;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rigidBody.constraints = (RigidbodyConstraints)(2 | 8 | 112);
    }

    void Start()
    {
        target = StaticManager.sPlayer;
        StaticManager.sHeadInfo_Canvas.AddMonsterHeadInfo(this);
    }

    void SetAniBool(string _name)
    {
        anim.SetBool("Idle", _name == "Idle");
        anim.SetBool("Walk", _name == "Walk");
        anim.SetBool("Attack", _name == "Attack");
    }

    public override void UpdateEx()
    {
        if (StaticManager.sPlayer == null)
            return;

        if (StaticManager.sPlayer.AIState == CharacterAnimState.Die)
        {
            nav.enabled = false;
            SetAniBool("Idle");
            return;
        }

        switch (AIState)
        {
            case CharacterAnimState.Idle:
                {
                    nav.enabled = false;
                    SetAniBool("Idle");
                    if (target.CType == ScanTarget())
                    {
                        AIState = CharacterAnimState.Walk;
                    }
                }
                break;
            case CharacterAnimState.Walk:
                {
                    SetAniBool("Walk");
                    if (IsOutRange())
                    {
                        AIState = CharacterAnimState.Idle;
                    }
                    else
                    {
                        AutoFollow();
                    }
                }
                break;
            case CharacterAnimState.Attack:
                {
                    SetAniBool("Attack");
                    nav.enabled = false;
                    AutoAttack();
                }
                break;
            case CharacterAnimState.Die:
                {
                    anim.SetTrigger("Die");
                    AIState = CharacterAnimState.Null;
                    break;
                }
                
            default:
                break;
        }

        
    }
    CharacterType ScanTarget()
    {
        CharacterType type = CharacterType.Null;
        if (Vector3.Distance(transform.position, target.transform.position) < scanRange)
        {
            return target.CType;
        }
        return type;
    }
    bool IsOutRange()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > followRange)
        {
            return true;
        }
        return false;
    }

    void AutoFollow()
    {
        if (!CheckAttack())
        {
            nav.enabled = true;
            Vector3 despos = new Vector3(StaticManager.sPlayer.transform.position.x,
                transform.position.y,
                StaticManager.sPlayer.transform.position.z);
            nav.SetDestination(despos);
            //anim.SetBool("Walk", true);
        }
        else
        {
            AIState = CharacterAnimState.Attack;
        }
    }
    
    bool CheckAttack()
    {
        GenerateNextSkill();
        if(nextSkillTab.skillRange.CheckInRange(this, target))
        {
            return true;
        }
        return false;
    }
    SkillTab nextSkillTab;
    void GenerateNextSkill()
    {
        if (nextSkillTab == null || characterSkill.IsCoolDown(nextSkillTab.tabid))
        {
            int idx = UnityEngine.Random.Range(0, characterSkill.hasSkills.lsHasSkills.Count);
            nextSkillTab = SkillTab.Get(characterSkill.hasSkills.lsHasSkills[idx].skillId);
        }
    }

    void AutoAttack()
    {
        if (InsSkillRetType.OK == characterSkill.InstanceSkill(nextSkillTab, null))
        {
            //isAttack = true;
            
        }
    }

    public override void SkillEnd(uint _skillid)
    {
        //播发动作
        if (AIState == CharacterAnimState.Attack)
        {
            //   isAttack = false;
            AIState = CharacterAnimState.Idle;
        }
    }

    public override void Death()
    {
        AIState = CharacterAnimState.Die;
        DropedItem.Drop(new Vector3(transform.position.x, 0, transform.position.z), TabId);
        StaticManager.sHeadInfo_Canvas.DelMonsterHeadInfo(this.UID);
        Destroy(gameObject, 3.0f);
    }

    public override void InitTab()
    {
        TabId = 2;
        MonsterTab _mtab = MonsterTab.Get(TabId);
        if(_mtab != null)
        {
            Name = _mtab.name;
            Level = (UInt32)_mtab.level;
        }
    }
}
