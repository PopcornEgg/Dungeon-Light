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
public class Character : MonoBehaviour {

    public String Name = "null";
    public CharacterType CType = CharacterType.Player;
    public UInt64 UID = 0;
    public UInt32 TabID = 0;

    public UInt16 Level = 1;
    public UInt32 HP = 100;
    public UInt32 MaxHP = 100;
    public UInt32 MP = 100;
    public UInt32 MaxMP = 100;

    public float moveSpeed = 1.0f;
    public float attackSpeed = 1.0f;

    public UInt32 magicDamage = 1;
    public UInt32 physicalDamage = 1;
    public UInt32 magicDefense = 1;
    public UInt32 physicalDefense = 1;

    //一些状态
    public bool isAttack = false;
    public bool isDead;

    
    public Animator anim;                      // Reference to the animator component.

    //头顶信息显示、、、、、、、、、、、、、、、
    //NPC模型高度
    public float modelHeight;
    //主摄像机对象
    Camera maincamera = null;
    //红色血条贴图
    public Texture2D blood_red = null;
    //黑色血条贴图
    public Texture2D blood_black = null;

    void Awake()
    {
        anim = GetComponent<Animator>();

        //得到摄像机对象
        maincamera = Camera.main;

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        //得到模型原始高度
        float size_y = collider.bounds.size.y;
        //得到模型缩放比例
        float scal_y = transform.localScale.y;
        //它们的乘积就是高度
        modelHeight = (size_y * scal_y);

        blood_red = Texture2D.whiteTexture;
        blood_black = Texture2D.blackTexture;

        AwakeEx();
    }

    public virtual void AwakeEx()
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

        //HPText.text = string.Format("{0}/{1}", currentHealth, startingHealth);
    }

    public virtual void Death()
    {
        isDead = true;
        anim.SetTrigger("Die");
    }
}
