using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShaderManager
{
    static Dictionary<String, Shader> dicDatas = new Dictionary<string, Shader>();

    public static void Add(String _name, Shader _sd)
    {
        if (!dicDatas.ContainsKey(_name))
            dicDatas.Add(_name, _sd);
    }
    public static Shader Get(String _name)
    {
        Shader _sd;
        if (!dicDatas.TryGetValue(_name, out _sd))
        {
            _sd = Resources.Load<Shader>("Shaders/" + _name);
            dicDatas.Add(_name, _sd);
        }
        return _sd;
    }
}

