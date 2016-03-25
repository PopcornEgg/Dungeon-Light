using UnityEngine;
using System.Collections;

//player逻辑处理部分
public partial class Player : Character
{
    public static int dropedItemLayer;
    public static int attackAbleLayer;

    Vector3 movement;
    Vector2 moveDir;

#if !MOBILE_INPUT
#endif

    public override void AwakeEx()
    {
        characterSkill.hasSkills.AddSkill(0);

        dropedItemLayer = LayerMask.GetMask("DropedItem");
        attackAbleLayer = LayerMask.GetMask("AttackAble");

        StaticManager.sPlayer = this;
        CType = CharacterType.Player;

        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
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
            if (Physics.Raycast(ray, out hitInfo, 100.0f, dropedItemLayer))
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
#endif

        if(moveDir != null && !isAttack && !isDead)
            OnMoving(moveDir.x, moveDir.y);
    }

    void OnMoving(float h, float v)
    {
        if (v != 0f || h != 0f)
        {
            //移动
            movement.Set(h, 0f, v);
            movement = movement.normalized * MOVESPEED * Time.deltaTime;
            rigidbody.MovePosition(transform.position + movement);

            //转向movement
            movement.Set(h, 0f, v);
            Quaternion newRotatation = Quaternion.LookRotation(movement * Time.deltaTime);
            rigidbody.MoveRotation(newRotatation);
        }
        //播放动作
        anim.SetBool("Run", h != 0f || v != 0f);
    }

    public void Attack()
    {
        if (isAttack || isDead)
            return;

        if (InsSkillRetType.OK == characterSkill.InstanceSkill(0, null))
        {
            isAttack = true;
            anim.SetTrigger("Attack");
        }
    }
    public override void SkillEnd(uint _skillid)
    {
        //播发动作
        if (isAttack)
        {
            isAttack = false;
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

        if (HP == 0 && !isDead)
        {
            Death();
        }
    }

    public override void Death()
    {
        isDead = true;
        anim.SetTrigger("Die");
        Invoke("ShowOver", 1.0f);
    }
    void ShowOver()
    {
        StaticManager.sHUD_Canvas.DieSetUp();
    }
}
