using UnityEngine;
using System.Collections;

//player逻辑处理部分
public partial class Player : Character
{
    //移动部分
    public float speed = 6f;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    Vector2 moveDir;
    int attackAbleMask;

#if !MOBILE_INPUT
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
#endif

    Player()
    {
        CType = CharacterType.Player;
    }

    public override void AwakeEx()
    {
        StaticManager.sPlayer = this;

#if !MOBILE_INPUT
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
#endif

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        attackAbleMask = LayerMask.GetMask("Monster");
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
    void FixedUpdate()
    {
#if !MOBILE_INPUT
        // Store the input axes.
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
            Move(h, v);
            Turning(h, v);
        }

        Animating(h, v);
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);

        //transform.position += movement;

    }


    void Turning(float h, float v)
    {
      

        movement.Set(h, 0f, v);
        Quaternion newRotatation = Quaternion.LookRotation(movement * Time.deltaTime);
        playerRigidbody.MoveRotation(newRotatation);
    }


    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool running = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("Run", running);
    }

    
    public void Attack()
    {
        if (isAttack || isDead)
            return;

        isAttack = true;
        anim.SetTrigger("Attack");
        Invoke("AttackEnd", attackSpeed);
        Invoke("AttackTarget", 0.5f);
    }
    void AttackTarget()
    {
        Vector3 orgPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Ray shootRay = new Ray(orgPos, transform.forward);
        RaycastHit shootHit;
        float range = 5.0f;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, attackAbleMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            Character enemyHealth = shootHit.collider.GetComponent<Character>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(this.physicalDamage);
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

        StaticManager.sHUD_Canvas.SetHP_Slider((float)HP / (float)MaxHP);

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
