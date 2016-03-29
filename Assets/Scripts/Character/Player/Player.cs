﻿using UnityEngine;
using System.Collections;

//player逻辑处理部分
public partial class Player : Character
{
    Vector3 movement;
    Vector2 moveDir;

#if !MOBILE_INPUT
#endif

    public override void AwakeEx()
    {
        characterSkill.hasSkills.AddSkill(0);//测试
        
        attackAbleLayer = LayerMask.GetMask("Monster");
        StaticManager.sPlayer = this;
        CType = CharacterType.Player;
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    public void SetMoveDir(Vector2 v)
    {
        moveDir = v;
    }
    public void SetMoveDir(float h, float v)
    {
        moveDir = new Vector2(h, v);
    }
    public void ClearMoveDir()
    {
        moveDir = new Vector2(0, 0);
    }
    public override void UpdateEx()
    {
        //拾取道具
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100.0f, DropedItem.dropedItemLayer))
            {
                Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
                GameObject gameObj = hitInfo.collider.gameObject;
                /*
                Debug.Log("click object name is " + gameObj.name);
                if (gameObj.tag == "Player")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    Debug.Log("pick up!");
                }
                */
                DropedItem _ditem = gameObj.GetComponent<DropedItem>();
                if (_ditem != null )
                    _ditem.OnPickuped();
            }
        }
    }
    void FixedUpdate()
    {
#if !MOBILE_INPUT

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(h, v);

        if (Input.GetKeyUp(KeyCode.J)){
            Attack();
        }
#endif

        if (moveDir != null && AIState == CharacterAnimState.Idle || AIState == CharacterAnimState.Walk)
            OnMoving(moveDir.x, moveDir.y);
    }

    void OnMoving(float h, float v)
    {
        if (v != 0f || h != 0f)
        {
            //移动
            movement.Set(h, 0f, v);
            movement = movement.normalized * MOVESPEED * Time.deltaTime;
            rigidBody.MovePosition(transform.position + movement);

            //转向movement
            movement.Set(h, 0f, v);
            Quaternion newRotatation = Quaternion.LookRotation(movement * Time.deltaTime);
            rigidBody.MoveRotation(newRotatation);
        }
        //播放动作
        anim.SetBool("Run", h != 0f || v != 0f);
    }

    public void Attack()
    {
        if (AIState == CharacterAnimState.Die || AIState == CharacterAnimState.Attack)
            return;

        if (InsSkillRetType.OK == characterSkill.InstanceSkill(0, null))
        {
            AIState = CharacterAnimState.Attack;
            anim.SetTrigger("Attack");
        }
    }
    public override void SkillEnd(uint _skillid)
    {
        //播发动作
        if (AIState == CharacterAnimState.Attack)
        {
            AIState = CharacterAnimState.Walk;
            anim.SetBool("Attack", false);
        }
    }

    public override void TakeDamage(uint amount)
    {
        if (amount == 0)
            return;

        if (amount >= HP)
            HP = 0;
        else
            HP -= amount;

        StaticManager.sHUD_Canvas.SetHP_Slider((float)HP / (float)MAXHP);

        if (HP == 0 && CharacterAnimState.Die != AIState)
        {
            Death();
        }
    }

    public override void Death()
    {
        AIState = CharacterAnimState.Die;
        anim.SetTrigger("Die");
        Invoke("ShowOver", 1.0f);
    }
    void ShowOver()
    {
        StaticManager.sHUD_Canvas.DieSetUp();
    }

    
    PlayerLvTab currPlayerLvTab;
    public PlayerLvTab CurrPlayerLvTab{get { return currPlayerLvTab; }set { currPlayerLvTab = value; playerBaseProperty.IsDirty = true; } }
    public override void InitProperty()
    {
        //加载一些存档
        LoadAll();

        //注册等级属性模块
        playerBaseProperty = new PlayerBaseProperty(this);
        characterProperties.AddProperty(playerBaseProperty);
        //注册装备属性模块
        playerEquipProperty = new PlayerEquipProperty(this);
        characterProperties.AddProperty(playerEquipProperty);

        UpdateLevelProperty();
    }
    public void UpdateLevelProperty()
    {
        PlayerLvTab _pltab = PlayerLvTab.Get(Level);
        if (_pltab != null)
        {
            CurrPlayerLvTab = _pltab;
            HP = (uint)MAXHP;
            MP = (uint)MAXMP;
            SP = (uint)MAXSP;
        }
    }
}
