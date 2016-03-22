using UnityEngine;
using System.Collections;

public class Monster_Skeleton : Character
{
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    bool playerInRange;             // Whether player is within the trigger collider and can be attacked.
    int attackAbleMask;
    BaseMonsterAI ai = null;

    public override void AwakeEx()
    {
        TabID = 2;
        CType = CharacterType.Monster;

        nav = GetComponent<NavMeshAgent>();
        attackAbleMask = LayerMask.GetMask("Player");

        ai = gameObject.AddComponent<BaseMonsterAI>();
    }

    void Start()
    {
        StaticManager.sThird_Canvas.AddMonsterHeadInfo(this);
    }

	void Update ()
    {
        if (StaticManager.sPlayer == null)
            return;

        if (StaticManager.sPlayer.isDead)
        {
            anim.SetBool("Idle", true);
            return;
        }

        if (isDead)
            return;

        if (!isAttack)
        {
            if ( playerInRange )
            {
                Attack();
            }
            else
            {
                if (this.HP > 0 && StaticManager.sPlayer.HP > 0 && ai.aiState == AIState.Follow)
                {
                    nav.enabled = true;
                    Vector3 despos = new Vector3(StaticManager.sPlayer.transform.position.x,
                        transform.position.y,
                        StaticManager.sPlayer.transform.position.z);
                    nav.SetDestination(despos);
                    anim.SetBool("Walk", true);
                }
                else
                {
                    nav.enabled = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (StaticManager.sPlayer == null && !isDead)
            return;

        // If the entering collider is the player...
        if (other.gameObject.name == StaticManager.sPlayer.name)
        {
            // ... the player is in range.
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (StaticManager.sPlayer == null)
            return;

        // If the exiting collider is the player...
        if (other.gameObject.name == StaticManager.sPlayer.name)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }

    void Attack()
    {
        //攻击时停止移动
        nav.enabled = false;

        if (StaticManager.sPlayer.HP > 0)
        {
            Invoke("RealTakeDamage", 1.2f);

            //播发动作
            isAttack = true;
            anim.SetBool("Attack", true);

            Invoke("AttackEnd", attackSpeed);
        }
    }
   
    void RealTakeDamage()
    {
        Vector3 orgPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Ray shootRay = new Ray(orgPos, transform.forward);
        RaycastHit shootHit;
        float range = 3.0f;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, attackAbleMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            Character health = shootHit.collider.GetComponent<Character>();

            // If the EnemyHealth component exist...
            if (health != null)
            {
                health.TakeDamage(this.physicalDamage);
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

    public override void Death()
    {
        isDead = true;
        anim.SetTrigger("Die");

        DropedItem.Drop(new Vector3(transform.position.x, 0, transform.position.z), TabID);
        StaticManager.sThird_Canvas.DelMonsterHeadInfo(this.UID);
        Destroy(gameObject, 3.0f);
    }
}
