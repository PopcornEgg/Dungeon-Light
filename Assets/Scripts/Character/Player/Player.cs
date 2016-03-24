using UnityEngine;
using System.Collections;

//player逻辑处理部分
public partial class Player : Character
{
    public static int dropedItemLayer;
    public static int attackAbleMask;

    Vector3 movement;
    Rigidbody playerRigidbody;
    Vector2 moveDir;

#if !MOBILE_INPUT
#endif

    public override void AwakeEx()
    {
        dropedItemLayer = LayerMask.GetMask("DropedItem");
        attackAbleMask = LayerMask.GetMask("Monster");

        StaticManager.sPlayer = this;
        CType = CharacterType.Player;

        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
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
            playerRigidbody.MovePosition(transform.position + movement);
            //转向
            movement.Set(h, 0f, v);
            Quaternion newRotatation = Quaternion.LookRotation(movement * Time.deltaTime);
            playerRigidbody.MoveRotation(newRotatation);
        }
        //播放动作
        anim.SetBool("Run", h != 0f || v != 0f);
    }
    
    public void Attack()
    {
        if (isAttack || isDead)
            return;

        isAttack = true;
        anim.SetTrigger("Attack");
        Invoke("AttackEnd", attackSpeed);
        Invoke("DamageDelay", damageDelay);
    }
    void DamageDelay()
    {
        Vector3 orgPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Ray shootRay = new Ray(orgPos, transform.forward);
        RaycastHit shootHit;
        float range = 5.0f;

        if (Physics.Raycast(shootRay, out shootHit, range, attackAbleMask))
        {
            Character enemyHealth = shootHit.collider.GetComponent<Character>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(this.AD);
            }
        }

        //         LineRenderer gunLine = GetComponent<LineRenderer>();
        //         gunLine.enabled = true;
        //         gunLine.SetPosition(0, orgPos);
        //         gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
    }
    void AttackEnd()
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
