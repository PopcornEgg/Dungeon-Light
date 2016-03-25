using UnityEngine;
using System.Collections;

public class PreloadModule : MonoBehaviour {

    void Awake()
    {
        DropedItem.dropedItemLayer = LayerMask.GetMask("DropedItem");
        preloadTabs();
    }
    
    void preloadTabs()
    {
        ItemTab.Read();
        MonsterTab.Read();
        DropListTab.Read();
        SkillTab.Read();

        SpriteManager.Init(this.transform);
    }
}
