using UnityEngine;
using System.Collections;
using System;

public enum MonsterType
{
    //普通
    Normal = 0,
    //强化
    Enchanted,
    //精英
    Elitist,
    //终极
    Boss,
    //无
    Max,
}

public class Monster : Character
{
    public MonsterTab monsterTab;
    public int createdPositionIdx = -1;

    NavMeshAgent nav;
    Character target = null;
    DebugDrawCircle scanCircle = null;
    DebugDrawCircle followCircle = null;

    new public void Awake()
    {
        base.Awake();
        attackAbleLayerMask = LayerMask.GetMask("Player");
        CType = CharacterType.Monster;
        characterSkill.hasSkills.AddSkill(0);//测试
    }
    new public void Start()
    {
        base.Start();

        bornPosition = transform.position;
        target = Player.Self;
        HeadInfo_Canvas.AddMonsterHeadInfo(this);
        AddNavMeshAgent();
        AddRigidbody();

        scanCircle = gameObject.AddComponent<DebugDrawCircle>();
        scanCircle.SetRadius(monsterTab.scanrange);
        followCircle = gameObject.AddComponent<DebugDrawCircle>();
        followCircle.SetRadius(monsterTab.followrange);
    }
    new public void Update()
    {
        base.Update();

        if (Player.Self == null)
            return;

        if (AIState == CharacterAnimState.Die)
            return;

        if (Player.Self.AIState == CharacterAnimState.Die)
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
            default:
                break;
        }
    }
    void AddNavMeshAgent()
    {
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        nav = gameObject.AddComponent<NavMeshAgent>();
        nav.radius = collider.radius;
        nav.height = collider.height;
        nav.baseOffset = 0;
        nav.speed = MOVESPEED;
        nav.angularSpeed = 360;
        nav.acceleration = MOVESPEED * 2.0f;
        nav.stoppingDistance = 1;
        nav.autoBraking = true;
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
        //rigidBody.constraints = (RigidbodyConstraints)(2 | 8 | 112);
        rigidBody.constraints = (RigidbodyConstraints)(112);
        //rigidBody.constraints = (RigidbodyConstraints)(16 | 64);
    }
    void SetAniBool(string _name)
    {
        anim.SetBool("Idle", _name == "Idle");
        anim.SetBool("Walk", _name == "Walk");
        anim.SetBool("Attack", _name == "Attack");
    }
    CharacterType ScanTarget()
    {
        CharacterType type = CharacterType.Null;
        if (Vector3.Distance(transform.position, target.transform.position) < monsterTab.scanrange)
        {
            return target.CType;
        }
        return type;
    }
    bool IsOutRange()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > monsterTab.followrange)
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
//             Vector3 despos = new Vector3(StaticManager.sPlayer.transform.position.x,
//                 transform.position.y,
//                 StaticManager.sPlayer.transform.position.z);
            nav.SetDestination(Player.Self.transform.position);
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
            transform.LookAt(new Vector3(target.transform.position.x, 
                transform.position.y, 
                target.transform.position.z));
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

    public override void Death(Character killer)
    {
        AIState = CharacterAnimState.Die;
        anim.SetTrigger("Die");
        DropedItem.Drop(new Vector3(transform.position.x, 0, transform.position.z), TabId);
        HeadInfo_Canvas.DelMonsterHeadInfo(this.UID);
        SceneManager.Self.MonsterDie(this);
        killer.AddExp((uint)monsterTab.exp);
        Destroy(gameObject, 2.0f);
    }

    void OnDestroy()
    {
        HeadInfo_Canvas.DelMonsterHeadInfo(this.UID);
    }

    public override void InitProperty()
    {
        if(monsterTab != null)
        {
            characterProperties.AddProperty(new MonsterBaseProperty(this));
            Name = monsterTab.name;
            Level = (UInt32)monsterTab.level;
            TabId = monsterTab.tabid;
            HP = (uint)MAXHP;
            MP = (uint)MAXMP;
            SP = (uint)MAXSP;
        }
    }
}
