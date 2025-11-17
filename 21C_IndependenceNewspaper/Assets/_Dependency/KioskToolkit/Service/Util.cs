using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Util
{
    public static byte[] HexStringToByteHex(string strHex)
    {
        if (strHex.Length % 2 != 0)
            Debug.Log("HexString는 홀수일 수 없습니다. - " + strHex);

        byte[] bytes = new byte[strHex.Length / 2];

        for (int count = 0; count < strHex.Length; count += 2)
        {
            bytes[count / 2] = System.Convert.ToByte(strHex.Substring(count, 2), 16);
        }
        return bytes;
    }

    public static string ByteHexToHexString(byte[] hex)
    {
        string result = string.Empty;
        foreach (byte c in hex)
            result += c.ToString("x2").ToUpper();
        return result;
    }

}

