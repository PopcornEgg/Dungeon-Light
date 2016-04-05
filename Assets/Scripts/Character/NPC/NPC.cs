using UnityEngine;
using System.Collections;
using System;

public class NPC : Character
{
    public static int layerMask;
    Character target = null;
    public CharacterTab npcTab;

    new public void Awake()
    {
        base.Awake();
        attackAbleLayerMask = LayerMask.GetMask("Player");
        CType = CharacterType.Monster;
    }
    new public void Start()
    {
        base.Start();

        float terrainY = Terrain.activeTerrain.SampleHeight(transform.position);
        bornPosition = transform.position = new Vector3(transform.position.x, terrainY, transform.position.z);
        target = Player.Self;
        HeadInfo_Canvas.AddNPCHeadInfo(this);
        //AddRigidbody();
    }
    new public void Update()
    {
        base.Update();

    }

    void AddRigidbody()
    {
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.mass = 1.0f;
        rigidBody.drag = 0;
        rigidBody.angularDrag = 0.05f;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.interpolation = RigidbodyInterpolation.None;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        //rigidBody.constraints = (RigidbodyConstraints)(2 | 8 | 112);
        rigidBody.constraints = (RigidbodyConstraints)(112);
        //rigidBody.constraints = (RigidbodyConstraints)(16 | 64);
    }

    void OnDestroy()
    {
        HeadInfo_Canvas.DelMonsterHeadInfo(this.UID);
    }

    public override void InitProperty()
    {
        if (npcTab != null)
        {
            Name = npcTab.name;
            TabId = npcTab.tabid;
        }
    }
}
