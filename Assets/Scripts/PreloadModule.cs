using UnityEngine;
using System.Collections;

public class PreloadModule : MonoBehaviour {

    static bool isPreloaded = false;
    void Awake()
    {
        if (!isPreloaded)
        {
            DropedItem.dropedItemLayerMask = LayerMask.GetMask("DropedItem");
            DropedItem.dropedItemLayer = LayerMask.NameToLayer("DropedItem");
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
