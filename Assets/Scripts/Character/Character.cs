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

public enum PropertyType
{
    HP = 0,//当前HP
    MP,//当前MP
    SP,//当前体力
    EXP,//当前经验
    MONEY,//当前金钱

    MAX,
}
public enum PropertyTypeEx
{
    MAXHP,
    MAXMP,
    MAXSP,
    AD,//attact damage（物理伤害）
    AP, //Ability Power（技能力量）
    ADD,//attact damage defense（物理伤害防御）
    APD, //Ability Power defense（技能力量防御）
    MOVESPEED,

    MAX,
}

public static class PropertyNames
{
    public static string[] Names = new string[(int)PropertyTypeEx.MAX]
    {
        "生命值上限",
        "魔法值上限",
        "体力值上限",
        "物理伤害",
        "技能伤害",
        "物理防御",
        "技能防御",
        "移动速度",
    };
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

    //基础属性信息
    public int[] property = new int[(int)PropertyType.MAX];
    public UInt32 HP {get { return (UInt32)property[(int)PropertyType.HP]; }set { property[(int)PropertyType.HP] = (int)value; }}
    public UInt32 MP { get { return (UInt32)property[(int)PropertyType.MP]; } set { property[(int)PropertyType.MP] = (int)value; } }
    public UInt32 SP { get { return (UInt32)property[(int)PropertyType.SP]; } set { property[(int)PropertyType.SP] = (int)value; } }
    public UInt32 EXP { get { return (UInt32)property[(int)PropertyType.EXP]; } set { property[(int)PropertyType.EXP] = (int)value; } }
    public UInt32 MONEY { get { return (UInt32)property[(int)PropertyType.MONEY]; } set { property[(int)PropertyType.MONEY] = (int)value; } }

    //附加属性信息
    public CharacterProperties characterProperties = new CharacterProperties();
    public float MAXHP { get {  return characterProperties.Get(PropertyTypeEx.MAXHP); } }
    public float MAXMP { get {  return characterProperties.Get(PropertyTypeEx.MAXMP); } }
    public float MAXSP { get {  return characterProperties.Get(PropertyTypeEx.MAXSP); } }
    public float AD { get {  return characterProperties.Get(PropertyTypeEx.AD); } }
    public float ADD { get {  return characterProperties.Get(PropertyTypeEx.ADD); } }
    public float AP { get {  return characterProperties.Get(PropertyTypeEx.AP); } }
    public float APD { get {  return characterProperties.Get(PropertyTypeEx.APD); } }
    public float MOVESPEED { get {  return characterProperties.Get(PropertyTypeEx.MOVESPEED); } }

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

    public void Awake()
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
    }
    public void Start()
    {
        bornPosition = transform.position;
    }
    public void Update()
    {
        characterSkill.OnTick();
    }

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
    public virtual void InitProperty() { }
    public virtual void AddSkillExp(UInt32 _skillid, UInt32 _exp) { }
    public virtual void SkillEnd(UInt32 _skillid){ }
}
