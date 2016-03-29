using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    System.Random random = new System.Random();
    public class Scene
    {
        public SceneTab _sceneTab;
        public int[] monsterCount = new int[(int)MonsterType.Max];
        public int[] spaces;

        public SceneTab sceneTab {
            get { return _sceneTab; }
            set
            {
                _sceneTab = value;
                spaces = new int[_sceneTab.levelTab.Count];
                for (int i = 0; i < spaces.Length; i++)
                {
                    spaces[i] = Int32.MaxValue;
                }
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
        StaticManager.sSceneManager = this;
        currScene = new Scene(0);
    }
    void Update()
    {
        for(int i=0; i< currScene.sceneTab.levelTab.Count; i++)
        {
            LevelTab.Data _data = currScene.sceneTab.levelTab.lsTabs[i];
            int rate = random.Next(0, 100);
            if(rate <= _data.rate)
            {
                MonsterTab _mtab = MonsterTab.Get(_data.monsterid);
                if(_mtab != null)
                {
                    if(currScene.monsterCount[(int)_mtab.mtype] > 0 && (currScene.spaces[i] + _data.space) <= Time.time)
                    {
                        Character _char = Character.New(_mtab);
                        _char.gameObject.SetActive(true);
                        _char.bornPosition = _char.transform.position = new Vector3(_data.x, _data.y, _data.z);
                        currScene.spaces[i] = (int)Time.time;
                        currScene.monsterCount[(int)_mtab.mtype]--;
                    }
                }
            }
        }
    }
    public void MonsterDie(Monster _char)
    {
        currScene.monsterCount[(int)_char.monsterTab.mtype]++;
    }
    delegate bool CheckMonsterCount(); 
}

