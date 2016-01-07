using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for webservice_common
/// </summary>
public class webservice_common : System.Web.Services.WebService
{
    public webservice_common()
    {
    }

    public bool log_call(string sActualParameters)
    {
        try
        {
            if (!isBlank(sActualParameters))
            {
                sActualParameters = sActualParameters.Replace("'", "_");
                // sActualParameters = sActualParameters.Replace("\"", "_");

                if (sActualParameters.Length > 5000) sActualParameters = sActualParameters.Substring(0, 4999);

            } else
                sActualParameters = "";

            bool bOK = false;

            string sDetailedFunction = "Not found ...";
            string sCallStack = Environment.StackTrace;
            if (sCallStack != null && sCallStack.Length > 0)
            {
                string[] callStackTable = sCallStack.Split("\r\n".ToCharArray());

                if (callStackTable != null && callStackTable.Length > 6)
                {
                    string sCall = callStackTable[6];

                    int start = sCall.IndexOf(".");
                    int end = sCall.IndexOf(")");

                    if (start > 0 && end > 0 && end > start)
                    {
                        sDetailedFunction = sCall.Substring(start + 1, end - start);
                    }
                }
            }

            if (isBlank(sActualParameters))
            {
                sActualParameters = "";
            }

            sActualParameters = sActualParameters.Replace("'", "_");
            sActualParameters = sActualParameters.Replace("\"", "_");

            // xSQL_InsertBuilder sb = new xSQL_InsertBuilder(iCommonPointer, "webservice_log");
            xSQL_InsertBuilder sb = new xSQL_InsertBuilder((webservice_database)this, "webservice_log");

            DateTime now = DateTime.Now;

            if (sDetailedFunction.Length > 99) sDetailedFunction = sDetailedFunction.Substring(0, 98);
            sb.add("name", sDetailedFunction);
            // sb.add("timestamp", DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToLongTimeString());
            sb.add("timestamp", now.Year.ToString() + "." + zero_2(now.Month.ToString()) + "." + zero_2(now.Day.ToString()) + " " + zero_2(now.Hour.ToString()) + ":" + zero_2(now.Minute.ToString()) + ":" + zero_2(now.Second.ToString()) + " " + zero_3(now.Millisecond.ToString()));
            sb.add("parameters", sActualParameters);
            bOK = sb.ExecuteSql();
        } catch (Exception)
        {
            return false;
        }
        return true;
    }

    public bool isBlank(string s)
    {
        if (s == null) return true;
        if (s.Trim() == "") return true;
        return false;
    }

    public static string getSafeAttribute(XmlNode node, string sAttribute)
    {
        if (node == null || sAttribute == null || sAttribute.Trim() == "") return "";
        string sValue = "";
        if (node.Attributes[sAttribute] != null)
        {
            sValue = node.Attributes[sAttribute].Value;
            if (sValue == null || sValue.Trim() == "") sValue = "";
        } else return "";

        return sValue;
    }

    public static string getSafeAttributeAsDecimalString(XmlNode node, string sAttribute)
    {
        string sValue = getSafeAttribute(node, sAttribute);

        string sDecimalChar = fwGetDecimalChar();
        if (sDecimalChar == ",")
            sValue = sValue.Replace(".", ",");
        else if (sDecimalChar == ",")
            sValue = sValue.Replace(",", ".");
        return sValue;
    }




    public bool log_user_action(string sConsumerGuid, string sSubAction, string sActualParameters)
    {
        try
        {
            if (sSubAction == null) sSubAction = "";
            if (sSubAction.Length > 50) sSubAction = sSubAction.Substring(0, 49);

            string sCouponId = "";
            if (!isBlank(sActualParameters))
            {
                sActualParameters = sActualParameters.Replace("'", "_");
                sActualParameters = sActualParameters.Replace("\"", "_");
                string[] tab = sActualParameters.Split(";".ToCharArray());
                if (tab != null && tab.Length > 0)
                {
                    for (int i = 0; i < tab.Length; ++i)
                    {
                        string[] tab2 = tab[i].Split("=".ToCharArray());

                        if (tab2 != null && tab2.Length == 2)
                        {
                            if (tab2[0] == "coupon_id")
                            {
                                sCouponId = tab2[1];
                            }
                        }
                    }
                }
            }
            bool bOK = false;

            string sDetailedFunction = "Not found ...";
            string sFunctionCall = "void";
            string sCallStack = Environment.StackTrace;
            if (sCallStack != null && sCallStack.Length > 0)
            {
                string[] callStackTable = sCallStack.Split("\r\n".ToCharArray());

                if (callStackTable != null && callStackTable.Length > 6)
                {
                    string sCall = callStackTable[6];

                    int start = sCall.IndexOf(".");
                    int end = sCall.IndexOf(")");

                    if (start > 0 && end > 0 && end > start)
                    {
                        sDetailedFunction = sCall.Substring(start + 1, end - start);
                        int iStartPar = sDetailedFunction.IndexOf("(");
                        if (iStartPar > 0)
                        {
                            sFunctionCall = sDetailedFunction.Substring(0, iStartPar);
                        }
                    }
                }
            }

            if (isBlank(sActualParameters))
            {
                sActualParameters = "";
            } else if (sActualParameters.Length > 199)
            {
                sActualParameters = sActualParameters.Substring(0, 199);
            }
            sActualParameters = sActualParameters.Replace("'", "_");
            sActualParameters = sActualParameters.Replace("\"", "_");

            xSQL_InsertBuilder sb = new xSQL_InsertBuilder((webservice_database)this, "user_action");

            if (sConsumerGuid.Length > 50) sConsumerGuid = sConsumerGuid.Substring(0, 49);
            if (sActualParameters.Length > 99) sActualParameters = sActualParameters.Substring(0, 98);
            if (sFunctionCall.Length > 50) sFunctionCall = sFunctionCall.Substring(0, 49);
            sb.add("consumer_guid", sConsumerGuid);
            if (sCouponId != "") sb.add("coupon_id", sCouponId);
            sb.add("action", sFunctionCall);
            sb.add("sub_action", sSubAction);
            sb.add("parameters", sActualParameters);
            sb.add("timestamp", DateTime.Now.Subtract(new TimeSpan(1, 0, 0))); // 19 nov fix we get 1 hour wrong on Azure !!!

            bOK = sb.ExecuteSql();
        } catch (Exception e)
        {
            log_call("PROBLEM");
            return false;
        }
        return true;
    }

    public string zero_2(string sIn)
    {
        if (sIn.Length == 1)
            return "0" + sIn;
        else
            return sIn;
    }

    public string zero_3(string sIn)
    {
        if (sIn.Length == 1)
            return "00" + sIn;
        else if (sIn.Length == 2)
            return "0" + sIn;
        else
            return sIn;
    }

    /// <summary>
    /// Made for debug purposes, to see what the webservices return ...
    /// </summary>
    /// <param name="sValueWeReturn"></param>
    /// <returns></returns>
    public string MAIN_STRING_RETURN(bool bSuperDeveloperTesting, string sValueWeReturn)
    {
        if (bSuperDeveloperTesting) return sValueWeReturn;

        try
        {
            string sDetailedFunction = "Not found ...";
            string sCallStack = Environment.StackTrace;
            if (sCallStack != null && sCallStack.Length > 0)
            {
                string[] callStackTable = sCallStack.Split("\r\n".ToCharArray());

                if (callStackTable != null && callStackTable.Length > 6)
                {
                    string sCall = callStackTable[6]; // This is the method we call +1 level higher up in the call stack ...

                    int start = sCall.IndexOf(".");
                    int end = sCall.IndexOf(")");

                    if (start > 0 && end > 0 && end > start)
                    {
                        sDetailedFunction = sCall.Substring(start + 1, end - start);
                    }
                }
            }


            string sSqlReadyText = sValueWeReturn;
            sSqlReadyText = sSqlReadyText.Replace("'", "&quot;");

            xSQL_InsertBuilder sb = new xSQL_InsertBuilder((webservice_database)this, "webservice_log");

            DateTime now = DateTime.Now;

            if (sDetailedFunction.Length > 99) sDetailedFunction = sDetailedFunction.Substring(0, 98);
            sb.add("name", sDetailedFunction);
            sb.add("timestamp", now.Year.ToString() + "." + zero_2(now.Month.ToString()) + "." + zero_2(now.Day.ToString()) + " " + zero_2(now.Hour.ToString()) + ":" + zero_2(now.Minute.ToString()) + ":" + zero_2(now.Second.ToString()) + " " + zero_3(now.Millisecond.ToString()));
            sb.add("parameters", "RETURNED: " + sSqlReadyText);
            bool bOK = sb.ExecuteSql();

        } catch (Exception e)
        {
            return sValueWeReturn;
        }
        return sValueWeReturn;
    }

    public Int32 getIntSum_x_100_from_xml_attribute_text(string sSum)
    {
        // "Correct format = 100.50" >> 10050
        Int32 retval = 0;
        try
        {
            if (isBlank(sSum)) return 0;

            sSum = sSum.Trim();

            char cDot = '.';
            bool bDotFound = false;
            for (int i = 0; i < sSum.Length; ++i)
            {
                if (sSum[i] == cDot && bDotFound) return 0;
                if (sSum[i] == cDot) bDotFound = true;
                if (!Char.IsDigit(sSum[i]) && sSum[i] != cDot) return 0;
            }

            string[] pair = sSum.Split(".".ToCharArray());
            if (pair == null || pair.Length != 2) return 0;

            if (isBlank(pair[0].Trim())) return 0;

            if (pair[1].Trim().Length != 2) return 0;

            int iPre = Convert.ToInt32(pair[0]);
            int iPost = Convert.ToInt32(pair[1]);
            retval = iPre * 100 + iPost;
            if (retval > 10000000) return 0;
            if (retval < 10) return 0;
        } catch (Exception e)
        {
            return 0;
        }
        return retval;
    }

    public static String fwGetDecimalChar()
    {
        String sTest = "123.45";
        double d = 0;

        // Testing for "."
        try
        {
            sTest = "123.45";
            d = Convert.ToDouble(sTest);  // #Java: d = Double.parseDouble(sTest);
            return ".";
        } catch (Exception e)
        {
            // It is not "." !
        }

        // Testing for ","
        try
        {
            sTest = "123,45";
            d = Convert.ToDouble(sTest);  // #Java: d = Double.parseDouble(sTest);
            return ",";
        } catch (Exception e)
        {
            // It is not "," !
        }

        // Anything else ...
        try
        {
            double d1 = 3;
            double d2 = 2;
            d = d1 / d2;
            sTest = d.ToString();  // #Java: sTest = String.valueOf(d);
            for (int i = 0; i < sTest.Length; ++i) // #Java: for (int i = 0; i < sTest.length(); ++i)
            {
                String sChar = xjcString.substr(sTest, i, 1);
                if (sChar != "0" && sChar != "1" && sChar != "2" && sChar != "3" && sChar != "4" && sChar != "5" && sChar != "6" && sChar != "7" && sChar != "8" && sChar != "9")
                    return sChar; // whatever it is ...
            }
        } catch (Exception e)
        {
            return ","; // Error !
        }

        return ",";
    }


    public string getXmlSum(string sSum)
    {
        if (isBlank(sSum) || sSum == "0") return "0.00";

        if (sSum.Length == 1) return "0.0" + sSum;

        string sPre = sSum.Substring(0, sSum.Length - 2);
        string sPost = sSum.Substring(sSum.Length - 2, 2);

        if (sPre == "") sPre = "0";

        return sPre + "." + sPost;
    }

    private string getGoldenXmlSum(string sSum)
    {
        sSum = sSum.Replace(",", ".");

        int t = 0;
        if (isBlank(sSum) || sSum == "0") return "0.00";

        if (sSum.Length == 1) return "0.0" + sSum;

        string sPre = sSum.Substring(0, sSum.Length - 2);
        string sPost = sSum.Substring(sSum.Length - 2, 2);

        if (sPre == "") sPre = "0";

        return sPre + "." + sPost;
    }






}