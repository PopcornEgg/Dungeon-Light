using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum CharacterType
{
    Monster = 0,
    NPC,
    //这里添加新的类型

    Player,
    Null,
}

public enum CharacterAnimState
{
    Null = 0,
    Idle,
    Walk,
    Attack,
    Die,
}

public class Character : MonoBehaviour
{
    static Dictionary<string, GameObject> dicLoadedCharacter = new Dictionary<string, GameObject>();
    public static Character New(UInt32 id)
    {
        MonsterTab _mtab = MonsterTab.Get(id);
        if (_mtab != null)
            return New(_mtab);
        return null;
    }
    public static Character New(MonsterTab _mtab)
    {
        if(_mtab.type == CharacterType.Monster)
        {
            GameObject obj = null;
            if (!dicLoadedCharacter.TryGetValue(_mtab.model, out obj))
            {
                obj = Resources.Load<GameObject>("Prefabs/Monsters/" + _mtab.model);
                dicLoadedCharacter.Add(_mtab.model, obj);
            }
            GameObject gameobj = GameObject.Instantiate<GameObject>(obj);
            gameobj.SetActive(false);
            Monster _monster = gameobj.AddComponent<Monster>();
            _monster.monsterTab = _mtab;
            return _monster;
        }
        else if(_mtab.type == CharacterType.NPC)
        {
            return null;
        }
        return null;
    }
    public static Character NewPlayer()
    {
        return null;
    }

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
    CharacterType cType = CharacterType.Null;
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
    CharacterAnimState aiState = CharacterAnimState.Idle;
    public CharacterAnimState AIState { get { return aiState; } set { aiState = value; } }

    public Animator anim;

    //技能范畴
    public CharacterSkill characterSkill;

    //头顶信息显示
    public float modelHeight;//模型高度

    //刚体
    public Rigidbody rigidBody = null;

    //可攻击layer
    public int attackAbleLayer = -1;

    public Vector3 bornPosition;

    void Awake()
    {
        UID = Utils.GuidMaker.GenerateUInt64();

        InitProperty();

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

    public virtual void InitProperty() { }
    public virtual void AwakeEx(){}

    void Update()
    {
        characterSkill.OnTick();
        UpdateEx();
    }
    public virtual void UpdateEx(){ }
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

        if (HP == 0 && AIState != CharacterAnimState.Die)
        {
            Death();
        }
    }

    public virtual void Death(){ }
    public virtual void AddSkillExp(UInt32 _skillid, UInt32 _exp) { }
    public virtual void SkillEnd(UInt32 _skillid){ }
}
