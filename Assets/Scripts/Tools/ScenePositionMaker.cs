using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class ScenePositionMaker : MonoBehaviour
{

    const string Path = "D:/";
    public string SaveName = "default.txt";
    public Transform scanLayer;
    public Transform Temp;

    //随机生成
    public int randomCount = 20;
    public float scaled = 1.0f;
    public Vector3 rangeStart;
    public Vector3 rangeEnd;

    public bool IsNeedCfg = false;
    public int[] monsterids;
    public int[] rates;
    public int[] spaces;

    // Use this for initialization
    void Start ()
    {
        if (!IsNeedCfg)
        {
            monsterids = new int[randomCount];
            rates = new int[randomCount];
            spaces = new int[randomCount];
        }
    }
    //void OoGUI()
    //{

    //    if (GUI.Button(new Rect(500, 250, 80, 40), "退出"))//创建文本按钮
    //    {
    //    }

    //}
    public void MakeTXT()
    {
        Transform[] positions = scanLayer.GetComponentsInChildren<Transform>();
        StringBuilder sb = new StringBuilder("#id\t坐标x\t坐标y\t坐标z\t怪物id\t出生概率（百分比）\t时间间隔\n");
        sb.Append("$tabid\tx\ty\tz\tmonsterid\trate\tspace\n");
        for(int i=0;i< positions.Length; i++)
        {
            Vector3 pos = positions[i].position;
            sb.Append(string.Format("{3}\t{0}\t{1}\t{2}\t", pos.x, pos.y, pos.z, i));

            //monterid
            if(monsterids != null && monsterids.Length > i)
                sb.Append(string.Format("{0}\t", monsterids[i]));
            else
                sb.Append("0\t");

            if (rates != null && rates.Length > i)
                sb.Append(string.Format("{0}\t", rates[i]));
            else
                sb.Append("0\t");

            if (spaces != null && spaces.Length > i)
                sb.Append(string.Format("{0}\n", spaces[i]));
            else
                sb.Append("0\n");
        }

        using (FileStream fs = new FileStream(Path + SaveName, FileMode.Create))
        {
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
    public void RandomPositions()
    {
        for (int i = 0; i < randomCount; i++)
        {
            Transform tf = Instantiate<Transform>(Temp);
            tf.SetParent(scanLayer, false);
            tf.gameObject.SetActive(true);
            tf.position = new Vector3(UnityEngine.Random.Range(rangeStart.x, rangeEnd.x),
                UnityEngine.Random.Range(rangeStart.y, rangeEnd.y),
                UnityEngine.Random.Range(rangeStart.z, rangeEnd.z));
            tf.localScale = new Vector3(scaled, scaled, scaled);
        }
    }
}
