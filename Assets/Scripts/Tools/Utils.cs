using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
}

