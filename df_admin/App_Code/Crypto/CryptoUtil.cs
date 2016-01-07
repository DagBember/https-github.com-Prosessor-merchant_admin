using System;
using System.Collections.Generic;
using System.Text;

class CryptoUtil
{
    public static byte[] GetEncryptedBytesFromSQL(string sDiskEncrypted)
    {
        // Receiving 001234123014
        //           000111222333
        byte[] bytesEncrypted = new byte[sDiskEncrypted.Length / 3];
        int iBytePos = 0;
        for (int i = 0; i < sDiskEncrypted.Length; i = i + 3)
        {
            string s3Byte = sDiskEncrypted.Substring(i, 3);
            int iByte = Convert.ToInt32(s3Byte);
            bytesEncrypted[iBytePos] = (byte)iByte;
            ++iBytePos;
        }

        return bytesEncrypted;
    }

    public static string PrepareEncryptedBytesForSQL(byte[] bytes)
    {
        string s = "";
        for (int i = 0; i < bytes.Length; ++i)
        {
            string sByte = ((int)bytes[i]).ToString();
            sByte = sByte;
            if (sByte.Length == 1)
                sByte = "00" + sByte;
            else if (sByte.Length == 2)
                sByte = "0" + sByte;
            else if (sByte.Length == 3)
                sByte = sByte;
            s += sByte;
        }
        return s;
    }

    public static string DeCrypt(byte[] encrypted)
    {
        CryptoStat ct = new CryptoStat();
        
        ct.Initialize("tobeornot", "tobealive", 128);

        Byte[] deCrypted = ct.DecryptBytes(encrypted, encrypted.Length);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < deCrypted.Length; ++i)
        {
            sb.Append((char)deCrypted[i]);
        }
        return sb.ToString();
    }

    public static byte[] EnCrypt(string sClearText)
    {
        CryptoStat ct = new CryptoStat();
        ct.Initialize("tobeornot", "tobealive", 128);

        Byte[] inBytes = new Byte[sClearText.Length];

        for (int i = 0; i < sClearText.Length; ++i)
        {
            inBytes[i] = (byte)sClearText[i];
        }

        Byte[] encrypted = ct.GetEncryptedBytes(inBytes);
        StringBuilder sbencrypted = new StringBuilder();
        for (int i = 0; i < encrypted.Length; ++i)
        {
            sbencrypted.Append(encrypted[i]);
            if (i < (encrypted.Length - 1))
                sbencrypted.Append(";");
        }
        return encrypted;
    }
}

