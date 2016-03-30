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
    public readonly float maxRange;//范围类型

    public Character owner;
    public BaseSkillRange(SkillRangeType _srtype, float _mr) { SRType = _srtype; maxRange = _mr; }

    static public BaseSkillRange New(SkillRangeType _srtype, float _mr, float v1, float v2, float v3, float v4)
    {
        switch (_srtype)
        {
            case SkillRangeType.Single:
                return new SingleSkillRange(_srtype, _mr, v1);
            case SkillRangeType.Circle:
                return new CircleSkillRange(_srtype, _mr, v1, v2);
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

    public virtual bool CheckInRange(Character _o, Character _tag)
    {
        if (_tag == null || _o == null)
            return false;
        return maxRange >= Vector3.Distance(_o.transform.position, _tag.transform.position);
    }
}
public class SingleSkillRange : BaseSkillRange
{
    public readonly float distance;//距离

    public SingleSkillRange(SkillRangeType _srtype, float _mr, float v1) : base(_srtype, _mr)
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

    public CircleSkillRange(SkillRangeType _srtype, float _mr, float v1, float v2) : base(_srtype, _mr)
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
        Vector3 orgPos = new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z) + 
            owner.transform.forward * startDistance;
        RaycastHit[] shootHits = Physics.SphereCastAll(orgPos, radius, owner.transform.forward, 0, owner.attackAbleLayerMask);

        if (shootHits != null)
        {
            for(int i = 0; i < shootHits.Length; i++)
            {
                //Debug.Log("shootHits[i].collider : " + shootHits[i].collider.name);
                if (shootHits[i].collider.isTrigger)//排除用来范围检测的collider
                    continue;
                Character _tag = shootHits[i].collider.GetComponent<Character>();
                if (owner == _tag )//排除自己
                    continue;
                if (_tag != null)
                {
                    lsChar.Add(_tag);
                }
            }
        }
        return lsChar.ToArray();
    }
    //public override bool CheckInRange(Character _o, Character _tag)
    //{
    //    if (_tag != null && _o != null)
    //    {
    //        Vector3 v1 = new Vector3(_o.transform.position.x, 0, _o.transform.position.z);
    //        Vector3 v2 = new Vector3(_tag.transform.position.x, 0, _tag.transform.position.z); ;
    //        Ray ray = new Ray(v1, (v2 - v1).normalized);
    //        if (Physics.SphereCast(ray, radius, 0, _o.attackAbleLayer))
    //        {
    //            return true;
    //        }
    //    }
    //    //Physics.CheckSphere()
    //    return false;
    //}
}

