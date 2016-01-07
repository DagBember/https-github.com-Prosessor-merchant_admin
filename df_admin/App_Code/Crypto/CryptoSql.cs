using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for how_to_crypt_and_encrypt
/// </summary>
public class CryptoSql
{
	public CryptoSql()
	{
	}

    public static string getMachinePassword(string sHumanPassword)
    {
        byte[] encryptedPassword = CryptoUtil.EnCrypt(sHumanPassword);
        return CryptoUtil.PrepareEncryptedBytesForSQL(encryptedPassword);
    }

    public static string getHumanPassword(string sMachinePassword)
    {
        byte[] encrypted = CryptoUtil.GetEncryptedBytesFromSQL(sMachinePassword);
        return CryptoUtil.DeCrypt(encrypted);
    }


    public void test()
    {

        string sMachinePassword = CryptoSql.getMachinePassword("Dag Bergesen");
        string sHumanPassword = CryptoSql.getHumanPassword(sMachinePassword);

    }

    string get_update(string sMail, string sPassword)
    {

        string sMachinePassword = CryptoSql.getMachinePassword(sPassword);
        
        string sql = "update users set crypto_pass='" + sMachinePassword  + "' where mail_address='" + sMail + "' ";
        return sql;
    }


}
