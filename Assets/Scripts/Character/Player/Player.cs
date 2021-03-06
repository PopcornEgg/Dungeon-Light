﻿using UnityEngine;
using System.Collections;

//player逻辑处理部分
public partial class Player : Character
{
    static public Player Self = null;

    Vector3 movement;
    Vector2 moveDir;

#if !MOBILE_INPUT
#endif

    new public void Awake()
    {
        base.Awake();
        characterSkill.hasSkills.AddSkill(0);//测试
        attackAbleLayerMask = LayerMask.GetMask("Monster");
        CType = CharacterType.Player;
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }
    new public void Start()
    {
        base.Start();
        //测试坐标
        transform.position = bornPosition;
        HeadInfo_Canvas.AddPlayerHeadInfo(this);

        Self = this;
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
    new public void Update()
    {
        base.Update();

        //拾取道具
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100.0f, DropedItem.layerMask | NPC.layerMask))
            {
                //Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
                GameObject gameObj = hitInfo.collider.gameObject;
                if(gameObj.tag == "Item")
                {
                    DropedItem _ditem = gameObj.GetComponentInParent<DropedItem>();
                    if (_ditem != null)
                        _ditem.OnPickuped();
                }
                else if (gameObj.tag == "Character")
                {
                    NPC _npc = gameObj.GetComponentInParent<NPC>();
                    if (_npc != null)
                        Second_Canvas.npcShopPanel.Show(true, (ShopType)_npc.npcTab.mtype);
                }
            }
        }
    }
    void FixedUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

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


    float lastTestT = 0;
    public void Attack()
    {
        Debug.Log("STime: " + (Time.time - lastTestT));

        lastTestT = Time.time;

        Debug.Log("AIState: " + AIState.ToString());

        if (AIState == CharacterAnimState.Die || AIState == CharacterAnimState.Attack)
            return;

        InsSkillRetType isrt = characterSkill.InstanceSkill(0, null);
        if (InsSkillRetType.OK == isrt)
        {
            AIState = CharacterAnimState.Attack;
            anim.SetTrigger("Attack");
        }

        Debug.Log("InsSkillRetType: " + isrt.ToString());
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

    public override void Death(Character killer)
    {
        AIState = CharacterAnimState.Die;
        anim.SetTrigger("Die");
        Invoke("ShowOver", 1.0f);
    }
    void ShowOver()
    {
        HUD_Canvas.Self.DieSetUp();
    }
}
