using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum CharacterType
{
    Monster = 0,
    NPC,
    //这里添加新的类型

    Player,
    Null,
}
public enum CharacterAniState
{
    Idle = 0,
    Walk,
    Attack,
    Die,
}
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
    Null,
}
public class Character : MonoBehaviour
{
    //基本信息(所有角色都具有的)
    //GUID
    UInt64 uId = 0;
    public UInt64 UID { get { return uId; } set { uId = value; } }
    //对于player来说无用
    UInt32 tabId = 0;
    public UInt32 TabId { get { return tabId; } set { tabId = value; } }
    //名字
    public String Name = "null";
    //类型
    CharacterType cType = CharacterType.Player;
    public CharacterType CType { get { return cType; } set { cType = value; } }
    //等级
    UInt32 level = 1;
    public UInt32 Level { get { return level; } set { level = value; } }

    //属性信息
    public int[] property = new int[(int)PropertyType.MAX];
    public UInt32 HP {get { return (UInt32)property[(int)PropertyType.HP]; }set { property[(int)PropertyType.HP] = (int)value; }}
    public UInt32 MAXHP {get { return (UInt32)property[(int)PropertyType.MAXHP]; }set { property[(int)PropertyType.MAXHP] = (int)value; }}
    public UInt32 MP { get { return (UInt32)property[(int)PropertyType.MP]; } set { property[(int)PropertyType.MP] = (int)value; } }
    public UInt32 MAXMP { get { return (UInt32)property[(int)PropertyType.MAXMP]; } set { property[(int)PropertyType.MAXMP] = (int)value; } }
    public UInt32 AD { get { return (UInt32)property[(int)PropertyType.AD]; } set { property[(int)PropertyType.AD] = (int)value; } }
    public UInt32 ADD { get { return (UInt32)property[(int)PropertyType.ADD]; } set { property[(int)PropertyType.ADD] = (int)value; } }
    public UInt32 AP { get { return (UInt32)property[(int)PropertyType.AP]; } set { property[(int)PropertyType.AP] = (int)value; } }
    public UInt32 APD { get { return (UInt32)property[(int)PropertyType.APD]; } set { property[(int)PropertyType.APD] = (int)value; } }
    public float MOVESPEED { get { return (float)property[(int)PropertyType.MOVESPEED]/100.0f; } set { property[(int)PropertyType.MOVESPEED] = (int)(value * 100.0f); } }

    //AI范畴
    public CharacterAniState CAniType = CharacterAniState.Idle;
    public bool isAttack = false;
    public bool isDead;
    public Animator anim;

    //技能范畴
    public CharacterSkill characterSkill;
    public float attackLastTime = 2.0f;//攻击持续时间
    public float attackSpaceTime = 20.5f;//攻击间隔时间
    public float attackSpeed {
        get { return attackSpaceTime + attackLastTime; }
    }
    public float nextAttackTime = 0;
    public float damageDelay = 1.0f;
    public float attackDistance = 3.0f;

    //头顶信息显示
    public float modelHeight;//模型高度
    public Rigidbody rigidbody;

    void Awake()
    {
        anim = GetComponent<Animator>();

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        //得到模型原始高度
        float size_y = collider.bounds.size.y;
        //得到模型缩放比例
        float scal_y = transform.localScale.y;
        //它们的乘积就是高度
        modelHeight = (size_y * scal_y);

        characterSkill = new CharacterSkill(this);

        AwakeEx();
    }

    public virtual void AwakeEx()
    {
    }
    void Update()
    {
        characterSkill.OnTick();
        UpdateEx();
    }
    public virtual void UpdateEx()
    {
    }
    /*
    void OnGUI()
    {
        //得到NPC头顶在3D世界中的坐标
        //默认NPC坐标点在脚底下，所以这里加上modelHeight它模型的高度即可
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + modelHeight, transform.position.z);
        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        Vector2 position = maincamera.WorldToScreenPoint(worldPosition);
        //得到真实NPC头顶的2D坐标
        position = new Vector2(position.x, Screen.height - position.y);

        //计算出血条的宽高
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));

        //通过血值计算红色血条显示区域
        float bloodHeight = 5.0f;
        float bloodWidth = 60.0f;
        float currBloodWidth = bloodWidth * (float)HP / 100.0f;
        //先绘制黑色血条
        GUI.DrawTexture(new Rect(position.x - bloodWidth / 2.0f, position.y , bloodWidth, bloodHeight), blood_black);
        //在绘制红色血条
        GUI.DrawTexture(new Rect(position.x - bloodWidth / 2.0f, position.y , currBloodWidth, bloodHeight), blood_red);

        //计算NPC名称的宽高
        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(Name));
        //设置显示颜色为黄色
        GUI.color = Color.yellow;
        //绘制NPC名称
        GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - bloodSize.y, nameSize.x, nameSize.y), Name);

        //Image
    }
    */

    public virtual void TakeDamage(uint amount)
    {
        if (amount == 0)
            return;

        if (amount >= HP)
            HP = 0;
        else
            HP -= amount;

        if (HP == 0 && !isDead)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        isDead = true;
        anim.SetTrigger("Die");
    }

    public virtual void AddSkillExp(UInt32 _skillid, UInt32 _exp)
    {

    }
    public virtual void SkillEnd(UInt32 _skillid)
    {

    }
}
