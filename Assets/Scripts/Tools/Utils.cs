using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Utils
{
    public class GuidMaker
    {
        //生成string
        public static string GenerateString()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        //生成UInt64 
        public static UInt64 GenerateUInt64()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
    public class Colors
    {
        static Color[] colors = new Color[]
        {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow,
        };
        public static Color GetRandColor()
        {
            int idx = UnityEngine.Random.Range(0, colors.Length);
            return colors[idx];
        }
    }
}
