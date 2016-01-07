using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DASHBOARD_MONTH
/// </summary>
public class DASHBOARD_MONTH
{
    public int iThisMonthYear = 0;
    public int iPrevMonthYear = 0;
    public int iPrevPrevMonthYear = 0;
    public int iPrevPrevPrevMonthYear = 0;
    public int iThisMonth = 1;
    public int iPrevMonth = 1;
    public int iPrevPrevMonth = 1;
    public int iPrevPrevPrevMonth = 1;

    public string sThisMonth = "";
    public string sPrevMonth = "";
    public string sPrevPrevMonth = "";
    public string sPrevPrevPrevMonth = "";

    public DASHBOARD_MONTH(DASHBOARD_PERIOD period)
	{
        DateTime dt = DateTime.Now;

        if (period == DASHBOARD_PERIOD.PREV_MONTH)
        {
            dt = dt.AddMonths(-1);
        }
        else if (period == DASHBOARD_PERIOD.PREV_PREV_MONTH)
        {
            dt = dt.AddMonths(-2);
        }
        else if (period == DASHBOARD_PERIOD.PREV_PREV_PREV_MONTH)
        {
            dt = dt.AddMonths(-3);
        }
        else if (period == DASHBOARD_PERIOD.DONT_CARE)
        {
        }

        iThisMonthYear = dt.Year;
        iPrevMonthYear = dt.Year;
        iPrevPrevMonthYear = dt.Year;
        iThisMonth = dt.Month;

        iPrevMonth = iThisMonth - 1;
        iPrevPrevMonth = iThisMonth - 2;

        if (iPrevMonth == 0)
        {
            iPrevMonth = 12; // Not in use ...
            iPrevPrevMonth = 11; // Not in use ...
            iPrevPrevPrevMonth = 10; // Not in use ...
            iPrevMonthYear = iThisMonthYear - 1;
            iPrevPrevMonthYear = iThisMonthYear - 1;
            iPrevPrevPrevMonthYear = iThisMonthYear - 1;
        }
        else if (iPrevMonth == 1)
        {
            iPrevPrevMonth = 12; // Not in use ...
            iPrevPrevPrevMonth = 11; // Not in use ...
            iPrevPrevMonthYear = iThisMonthYear - 1;
            iPrevPrevPrevMonthYear = iThisMonthYear - 1;
        }
        else if (iPrevMonth == 2)
        {
            iPrevPrevPrevMonth = 12; // Not in use ...
            iPrevPrevPrevMonthYear = iThisMonthYear - 1;
        }

        sThisMonth = month_text(iThisMonth).ToUpper();
        sPrevMonth = month_text(iPrevMonth).ToUpper();
        sPrevPrevMonth = month_text(iPrevPrevMonth).ToUpper();
    }

    public static string month_text(int i)
    {
        // A bit uncertant where the calender locale will be defined ... therefore a little native approach ...
        if (i == 1) return "Januar";
        else if (i == 2) return "Februar";
        else if (i == 3) return "Mars";
        else if (i == 4) return "April";
        else if (i == 5) return "Mai";
        else if (i == 6) return "Juni";
        else if (i == 7) return "Juli";
        else if (i == 8) return "August";
        else if (i == 9) return "September";
        else if (i == 10) return "Oktober";
        else if (i == 11) return "November";
        else if (i == 12) return "Desember";
        else return "Month???";
    }

    public string getThisDbYearMonthDay1String(int iBackwardCounter)
    {
        if (iBackwardCounter == 0)
        {
            return iThisMonthYear.ToString() + "-" + zero_2(iThisMonth.ToString()) + "-" + "01";
        }
        else
        {
            DateTime dt = new DateTime(iThisMonthYear, iThisMonth, 1);
            dt = dt.AddMonths(iBackwardCounter);
            return dt.Year.ToString() + "-" + zero_2((dt.Month).ToString()) + "-" + "01";
        }
    }

    public string getThisDbYearNextMonthDay1String()
    {
        if ((iThisMonth) < 12) 
            return iThisMonthYear.ToString() + "-" + zero_2((iThisMonth+1).ToString()) + "-" + "01";
        else
            return (iThisMonthYear+1).ToString() + "-01-" + "01";
    }


    private string zero_2(string sIn)
    {
        if (sIn.Length == 1)
            return "0" + sIn;
        else
            return sIn;
    }


}


