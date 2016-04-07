using UnityEngine;
using System.Collections;

public class PreloadModule : MonoBehaviour
{
    public static bool isPreloaded = false;
    void Awake()
    {
        if (!isPreloaded)
        {
            DropedItem.layerMask = LayerMask.GetMask("DropedItem");
            DropedItem.layer = LayerMask.NameToLayer("DropedItem");
            NPC.layerMask = LayerMask.GetMask("NPC");
            preloadTabs();
            isPreloaded = true;
        }
    }
    void preloadTabs()
    {
        SceneTab.Read();
        ItemTab.Read();
        CharacterTab.Read();
        DropListTab.Read();
        SkillTab.Read();
        PlayerLvTab.Read();
        NPCShopTab.Read();

        SpriteManager.Init(this.transform);
    }
}
