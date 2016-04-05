using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Self;
    System.Random random = new System.Random();
    public class Scene
    {
        public SceneTab _sceneTab;
        //记录怪物种类最大个数
        public int[] monsterCount = new int[(int)MonsterType.Max];
        //间隔时间
        public int[] spaces;
        //间隔时间
        public bool[] isCreated;

        public SceneTab sceneTab {
            get { return _sceneTab; }
            set
            {
                _sceneTab = value;
                spaces = new int[_sceneTab.levelTab.Count];
                isCreated = new bool[_sceneTab.levelTab.Count];
                for (int i = 0; i < spaces.Length; i++)
                {
                    spaces[i] = Int32.MaxValue;
                    isCreated[i] = false;
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

    //测试用
    public List<Vector3> EnchantedPositions = new List<Vector3>();
    public List<Vector3> ElitistPositions = new List<Vector3>();
    public List<Vector3> BossPositions = new List<Vector3>();

    void Start()
    {
        currScene = new Scene(0);
        Self = this;

        GameObject gameobj = PrefabsManager.Instantiate(PrefabsType.Players, "Player");
        gameobj.SetActive(true);

        //测试用
        for (int i = 0; i < currScene.sceneTab.levelTab.Count; i++)
        {
            LevelTab.Data _data = currScene.sceneTab.levelTab.lsTabs[i];
            MonsterTab _mtab = MonsterTab.Get(_data.monsterid);
            if (_mtab != null)
            {
                if(_mtab.mtype == MonsterType.Enchanted) { EnchantedPositions.Add(new Vector3(_data.x, _data.y, _data.z + 3)); }
                else if (_mtab.mtype == MonsterType.Elitist) { ElitistPositions.Add(new Vector3(_data.x, _data.y, _data.z + 3)); }
                else if (_mtab.mtype == MonsterType.Boss) { BossPositions.Add(new Vector3(_data.x, _data.y, _data.z + 3)); }
            }
        }
    }
    void Update()
    {
        if (!PreloadModule.isPreloaded)
            return;

        if (Player.Self == null)
            return;
        
        for(int i=0; i< currScene.sceneTab.levelTab.Count; i++)
        {
            LevelTab.Data _data = currScene.sceneTab.levelTab.lsTabs[i];
            int rate = random.Next(0, 100);
            if(rate <= _data.rate)
            {
                MonsterTab _mtab = MonsterTab.Get(_data.monsterid);
                if(_mtab != null)
                {
                    if(currScene.monsterCount[(int)_mtab.mtype] > 0 && 
                        (currScene.spaces[i] + _data.space) <= Time.time &&
                        !currScene.isCreated[i])
                    {
                        Character _char = Character.New(_mtab, i);
                        _char.gameObject.SetActive(true);
                        _char.transform.position = new Vector3(_data.x, _data.y, _data.z);
                        currScene.spaces[i] = (int)Time.time;
                        currScene.isCreated[i] = true;
                        currScene.monsterCount[(int)_mtab.mtype]--;
                    }
                }
            }
        }
    }
    public void MonsterDie(Monster _char)
    {
        currScene.monsterCount[(int)_char.monsterTab.mtype]++;
        currScene.isCreated[_char.createdPositionIdx] = false;
    }
}

