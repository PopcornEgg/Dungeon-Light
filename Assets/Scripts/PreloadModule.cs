using UnityEngine;
using System.Collections;

public class PreloadModule : MonoBehaviour {

    void Awake()
    {
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
