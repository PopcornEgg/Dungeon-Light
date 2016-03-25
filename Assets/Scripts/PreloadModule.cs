using UnityEngine;
using System.Collections;

public class PreloadModule : MonoBehaviour {

    static bool isPreloaded = false;
    void Awake()
    {
        if (!isPreloaded)
        {
            DropedItem.dropedItemLayer = LayerMask.GetMask("DropedItem");
            preloadTabs();
            isPreloaded = true;
        }
    }
    
    void preloadTabs()
    {
        SceneTab.Read();
        ItemTab.Read();
        MonsterTab.Read();
        DropListTab.Read();
        SkillTab.Read();
        PlayerLvTab.Read();

        SpriteManager.Init(this.transform);
    }
}
