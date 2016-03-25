using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SceneManager
{
    System.Random random = new System.Random();
    public class Scene
    {
        public SceneTab _sceneTab;
        public int[] monsterCount = new int[(int)MonsterType.Max];
        public float[] spaces;

        public SceneTab sceneTab {
            get { return _sceneTab; }
            set
            {
                _sceneTab = value;
                spaces = new float[_sceneTab.levelTab.Count];
                monsterCount[0] = _sceneTab.normal;
                monsterCount[1] = _sceneTab.enchanted;
                monsterCount[2] = _sceneTab.elitist;
                monsterCount[3] = _sceneTab.boss;
            }
        }
        public Scene(uint id)
        {
            sceneTab = SceneTab.Get(id);
        }
    }
    public Scene currScene;
    public Dictionary<UInt64, Character> dicMonsters = new Dictionary<UInt64, Character>();

    void Start()
    {
        currScene = new Scene(0);
    }
    public void OnTick()
    {
        foreach(LevelTab.Data _data in currScene.sceneTab.levelTab.lsTabs)
        {
            int rate = random.Next(0, 100);
            if(rate <= _data.rate)
            {
                MonsterTab _mtab = MonsterTab.Get(_data.monsterid);
                if(_mtab != null)
                {
                    if(currScene.monsterCount[(int)_mtab.mtype] > 0)
                    {

                        currScene.monsterCount[(int)_mtab.mtype]--;
                    }
                }
            }
        }
    }

    delegate bool CheckMonsterCount(); 
}

