using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
/// <summary>
/// Summary description for shop_base
/// </summary>
public class SHOP_BASE
{

    public static bool isBlank(string s)
    {
        if (s == null) return true;
        if (s.Trim() == "") return true;
        return false;
    }

    public static string getHourMinuteSecond(DateTime timestamp)
    {
        return zero_2(timestamp.Hour.ToString()) + ":" + zero_2(timestamp.Minute.ToString()) + ":" + zero_2(timestamp.Second.ToString());
    }

    public static string getDateMonthYear(DateTime timestamp)
    {
        return zero_2(timestamp.Day.ToString()) + ":" + zero_2(timestamp.Month.ToString()) + ":" + zero_2(timestamp.Year.ToString());
    }

    public static string getDateMonthYearNorwegianPretty(DateTime timestamp)
    {

        if (timestamp.Month == 1) return timestamp.Day.ToString() + "." + " januar";
        else if (timestamp.Month == 2) return timestamp.Day.ToString() + "." + " februar";
        else if (timestamp.Month == 3) return timestamp.Day.ToString() + "." + " mars";
        else if (timestamp.Month == 4) return timestamp.Day.ToString() + "." + " april";
        else if (timestamp.Month == 5) return timestamp.Day.ToString() + "." + " mai";
        else if (timestamp.Month == 6) return timestamp.Day.ToString() + "." + " juni";
        else if (timestamp.Month == 7) return timestamp.Day.ToString() + "." + " juli";
        else if (timestamp.Month == 8) return timestamp.Day.ToString() + "." + " august";
        else if (timestamp.Month == 9) return timestamp.Day.ToString() + "." + " september";
        else if (timestamp.Month == 10) return timestamp.Day.ToString() + "." + " oktober";
        else if (timestamp.Month == 11) return timestamp.Day.ToString() + "." + " november";
        else if (timestamp.Month == 12) return timestamp.Day.ToString() + "." + " desember";
        else return timestamp.Day.ToString() + "." + timestamp.Month.ToString();
        // return zero_2(timestamp.Day.ToString()) + ":" + zero_2(timestamp.Month.ToString()) + ":" + zero_2(timestamp.Year.ToString());
    }

    public static string zero_2(string sIn)
    {
        if (sIn.Length == 1)
            return "0" + sIn;
        else
            return sIn;
    }

    public static bool isBasketModified(string sBasket_B)
    {
        bool bRetval = false;
        try
        {
            sBasket_B = sBasket_B.Replace("&quot;", "'");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sBasket_B);
            XmlNode testNode2 = doc.SelectSingleNode("xml/basket/header");
            string sModified = webservice_common.getSafeAttribute(testNode2, "modified");
            if (sModified.ToUpper() == "TRUE") bRetval = true;
        }
        catch (Exception e)
        {
            bRetval = false;
        }
        return bRetval;
    }

    public static bool isErrorBasket(string sBasket_B)
    {
        bool bRetval = false;
        try
        {
            sBasket_B = sBasket_B.Replace("&quot;", "'");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sBasket_B);
            XmlNode testNode2 = doc.SelectSingleNode("xml/basket/header");
            string sStatus = webservice_common.getSafeAttribute(testNode2, "status");
            if (sStatus.ToUpper() == "TRUE") bRetval = true;
        }
        catch (Exception e)
        {
            bRetval = false;
        }
        return bRetval;
    }




}