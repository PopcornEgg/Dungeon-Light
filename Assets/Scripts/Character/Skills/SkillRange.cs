using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum SkillRangeType
{
    Single = 0,
    Ray,
    Rect,
    Circle,
    Sector,
}

public class BaseSkillRange
{
    public readonly SkillRangeType SRType;//范围类型
    public Character owner;
    public BaseSkillRange(SkillRangeType _srtype) { SRType = _srtype; }

    static public BaseSkillRange New(SkillRangeType _srtype, float v1, float v2, float v3, float v4)
    {
        switch (_srtype)
        {
            case SkillRangeType.Single:
                return new SingleSkillRange(_srtype, v1);
            case SkillRangeType.Circle:
                return new CircleSkillRange(_srtype, v1, v2);
        }
        return null;
    }
    public Character[] GetInRangeTargets(Character _o, Character _tag = null)
    {
        owner = _o;
        List<Character> lsChar = new List<Character>();
        if (SRType == SkillRangeType.Single)//指向性技能
        {
            if (InRangeI(_tag))
                return new Character[] { _tag };
        }
        else
            return InRangeII();

        return null;
    }
    public virtual bool InRangeI(Character _tar) { return false; }
    public virtual Character[] InRangeII() { return null; }
}
public class SingleSkillRange : BaseSkillRange
{
    public readonly float distance;//距离

    public SingleSkillRange(SkillRangeType _srtype, float v1) : base(_srtype)
    {
        distance = v1;
    }

    public override bool InRangeI(Character _tar)
    {
        return false;
    }
}

public class CircleSkillRange : BaseSkillRange
{
    public readonly float startDistance;//起始位置 =0表示在释放者的位置
    public readonly float radius;//半径

    public CircleSkillRange(SkillRangeType _srtype, float v1, float v2) : base(_srtype)
    {
        startDistance = v1;
        radius = v2;
    }
    /*
    public override Character[] InRangeII()
    {
        List<Character> lsChar = new List<Character>();
        Vector3 orgPos = new Vector3(owner.transform.position.x, owner.transform.position.y + 0.5f, owner.transform.position.z);
        Ray shootRay = new Ray(orgPos, owner.transform.forward);
        RaycastHit shootHit;
        float range = 5.0f;

        if (Physics.Raycast(shootRay, out shootHit, range, Player.attackAbleMask))
        {
            Character _tag = shootHit.collider.GetComponent<Character>();
            if (_tag != null)
            {
                lsChar.Add(_tag);
            }
        }
        return lsChar.ToArray();
    }*/
    public override Character[] InRangeII()
    {
        List<Character> lsChar = new List<Character>();
        Vector3 orgPos = new Vector3(owner.transform.position.x + startDistance, owner.transform.position.y + 0.5f, owner.transform.position.z + startDistance);
        RaycastHit[] shootHits = Physics.SphereCastAll(orgPos, radius, owner.transform.forward, radius, Player.attackAbleMask);

        if (shootHits != null)
        {
            for(int i = 0; i < shootHits.Length; i++)
            {
                Character _tag = shootHits[i].collider.GetComponent<Character>();
                if (_tag != null)
                {
                    lsChar.Add(_tag);
                }
            }
        }
        return lsChar.ToArray();
    }
}

