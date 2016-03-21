using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class TabReader
{
    public static Encoding Encoding = Encoding.UTF8;
    public static string FLAG_COMMENT = "#";
    public static string FLAG_FIELD = "$";
    public static char SPLIT_CHAR = '\t';
    public static char SPLIT_TOKEN = ',';

    private List<string[]> m_listRecord = new List<string[]>();
    private Dictionary<string, uint> m_dictField = new Dictionary<string, uint>();

    public TabReader(string filename, bool isU3dAsset)
    {
        if (isU3dAsset)
        {
            readU3d(filename);
        }
        else {
            readLocal(filename);
        }
    }

    void readLocal(string filename)
    {
        FileStream aFile = new FileStream(filename, FileMode.Open);
        if (aFile != null)
        {
            StreamReader sr = new StreamReader(aFile);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith(FLAG_COMMENT) || line.Trim() == string.Empty)
                    continue;
                else if (line.StartsWith(FLAG_FIELD))
                {
                    line = line.Remove(0, 1);
                    string[] itemList = line.Split(new char[] { SPLIT_CHAR }, StringSplitOptions.None);
                    for (int i = 0; i < itemList.Length; ++i)
                    {
                        string item = itemList[i];
                        if (item == "")
                            continue;

                        if (m_dictField.ContainsKey(item))
                        {
                            continue;
                        }
                        m_dictField[item] = (uint)i;
                    }
                    continue;
                }
                else
                {
                    string[] itemList = line.Split(new char[] { SPLIT_CHAR }, StringSplitOptions.None);
                    m_listRecord.Add(itemList);
                }
            }
            sr.Close();
        }
    }
    void readU3d(string filename)
    {
        TextAsset tasset = Resources.Load<TextAsset>(filename);
        if (tasset == null)
            return;

        string[] lines = tasset.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        for(int idx=0;idx< lines.Length;idx++)
        {
            string line = lines[idx];
            if (line.StartsWith(FLAG_COMMENT) || line.Trim() == string.Empty)
                continue;
            else if (line.StartsWith(FLAG_FIELD))
            {
                line = line.Remove(0, 1);
                string[] itemList = line.Split(new char[] { SPLIT_CHAR }, StringSplitOptions.None);
                for (int i = 0; i < itemList.Length; ++i)
                {
                    string item = itemList[i];
                    if (item == "")
                        continue;

                    if (m_dictField.ContainsKey(item))
                    {
                        continue;
                    }
                    m_dictField[item] = (uint)i;
                }
                continue;
            }
            else
            {
                string[] itemList = line.Split(new char[] { SPLIT_CHAR }, StringSplitOptions.None);
                m_listRecord.Add(itemList);
            }
        }
    }

    public int recordCount { get { return m_listRecord.Count; } }

    public string GetString(int Idx, string _field)
    {
        return m_listRecord[Idx][m_dictField[_field]];
    }

    public bool GetItemBoolean(int itemIdx, string fieldName)
    {
        string value = GetString(itemIdx, fieldName);
        if (value == "T" || value == "t" || value == "Y" || value == "y")
            return true;
        else if (value == "F" || value == "f" || value == "N" || value == "n" || value == "")
            return false;
       
        return false;
    }

    public UInt32 GetItemUInt32(int itemIdx, string fieldName)
    {
        string value = GetString(itemIdx, fieldName);
        if (value == String.Empty)
            return 0;
        return Convert.ToUInt32(value);
    }

    public float GetItemFloat(int itemIdx, string fieldName)
    {
        string value = GetString(itemIdx, fieldName);
        if (value == String.Empty)
            return 0;
        return (float)Convert.ToDouble(value);
    }

    public Int32 GetItemInt32(int itemIdx, string fieldName)
    {
        string value = GetString(itemIdx, fieldName);
        return Convert.ToInt32(value);
    }

    public UInt64 GetItemUInt64(int itemIdx, string fieldName)
    {
        string value = GetString(itemIdx, fieldName);
        if (value == String.Empty)
            return 0;
        return Convert.ToUInt64(value);
    }
}

