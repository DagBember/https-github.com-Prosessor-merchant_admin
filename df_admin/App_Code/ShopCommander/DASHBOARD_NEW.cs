using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;

/// <summary>
/// Summary description for DASHBOARD_NEW
/// </summary>
public enum XY_STRATEGY { percent, pixels }


public enum DASHBOARD_PERIOD { THIS_MONTH, PREV_MONTH, PREV_PREV_MONTH, PREV_PREV_PREV_MONTH,HALF_YEAR, DONT_CARE}


public class DASHBOARD_CELL
{
    public int w;
    public int h;
    public string sId;

    public CHART_TYPE chartType = CHART_TYPE.bar_vertical;                    
}


public class DASHBOARD_NEW
{
    XY_STRATEGY xyStrategy = XY_STRATEGY.pixels;

    public DASHBOARD_CELL[] cellList = new DASHBOARD_CELL[12];

    Global global = null;

    int iCellPadding = 5;
    int iMargin = 10;
    int iBorder = 1;

    int iCellIdCounter = 0;

    string sPreDashBoardCellName = "dashboard_cell_";

	public DASHBOARD_NEW(Global _global)
	{
        global = _global;
	}

    public string GET_TOP_MENU(DASHBOARD_PERIOD currentDashboardPeriod)
    {
        StringBuilder sb = new StringBuilder();

        DASHBOARD_MONTH dbMonth = new DASHBOARD_MONTH(DASHBOARD_PERIOD.DONT_CARE);

        sb.Append("<td valign=top align=center>");

        sb.Append("<div>");

        // ******START:  TOP GREEN HORISONTAL ***************************************************************
        sb.Append(" <table width=100% cellpadding=0 cellspacing=0 align=center>");

        sb.Append("     <tr>");
        sb.Append("         <td valign=top style='padding:20px;background-color:rgb(247,247,247);' colspan=5>");

        sb.Append("             <div style='float:left;margin-top:12px;'>");
        sb.Append("                 <div class=bember_burger_line></div>");
        sb.Append("                 <div class=bember_burger_line></div>");
        sb.Append("                 <div class=bember_burger_line></div>");
        sb.Append("             </div>");

        sb.Append("             <div class=bember_your_memberclub style='float:left;'>Din Kundeklubb</div>");

        sb.Append("             <div onclick=level_2_report_4('" + ((int)DASHBOARD_PERIOD.THIS_MONTH).ToString() + "') " + getButtonClass(DASHBOARD_PERIOD.THIS_MONTH, currentDashboardPeriod) + " style='float:right;'>Inneværende periode</div>");
        sb.Append("             <div onclick=level_2_report_4('" + ((int)DASHBOARD_PERIOD.PREV_MONTH).ToString() + "') " + getButtonClass(DASHBOARD_PERIOD.PREV_MONTH, currentDashboardPeriod) + " style='float:right;'>" + dbMonth.sPrevMonth + "&nbsp;" + dbMonth.iPrevMonthYear.ToString() + "</div>");
        sb.Append("             <div onclick=level_2_report_4('" + ((int)DASHBOARD_PERIOD.PREV_PREV_MONTH).ToString() + "') " + getButtonClass(DASHBOARD_PERIOD.PREV_PREV_MONTH, currentDashboardPeriod) + " style='float:right;'>" + dbMonth.sPrevPrevMonth + "&nbsp;" + dbMonth.iPrevPrevMonthYear.ToString() + "</div>");

        sb.Append("         </td>");
        sb.Append("     </tr>");
        sb.Append(" </table>");
        sb.Append("</div>");
        sb.Append("</td>");

        return sb.ToString();
    }



    public string get_BEMBER_CHAIN_Dashboard(DASHBOARD_PERIOD currentDashboardPeriod, List<backoffice.name_value_value_value_value> conversionList,List<backoffice.shop_top_bottom> topBottomShops)
    {
        StringBuilder sb = new StringBuilder();

        List<backoffice.dim_value_value> shopList = global.www_backoffice().dash_get_members_by_chain_shop("24", currentDashboardPeriod); // 14 okt = 65
        List<backoffice.dim_value_value> shopListAllPeriods = global.www_backoffice().dash_get_members_by_chain_shop("24", DASHBOARD_PERIOD.DONT_CARE); // 5 okt nå




        sb.Append("<div style='width:100%;'>"); // denne funker !!!
        sb.Append("<table align=center>");
        sb.Append("<tr>");
        sb.Append(GET_TOP_MENU(currentDashboardPeriod));
        sb.Append("</tr>");
        sb.Append("</table>");
        sb.Append("</div>");


        sb.Append("<div style='background-color:rgb(37, 126, 98);width:100%;'>"); // 7 jan 
        sb.Append("<table align=center width=100% cellpadding=0 cellspacing=0>");
        sb.Append("<tr>");
        sb.Append(A_GetHorisontalGreenTop(currentDashboardPeriod, shopList, shopListAllPeriods));
        sb.Append("</tr>");
        sb.Append("</table>");
        sb.Append("</div>");


        sb.Append("<div>");
        sb.Append("<table align=center>");
        sb.Append("<tr>");
        sb.Append(B_GetConversion(shopList, conversionList, topBottomShops)); // 5 okt
        sb.Append("</tr>");
        sb.Append("</table>");
        sb.Append("</div>");

        return sb.ToString();
    }

    public int getPixelWidthAvailable(int iOuterPixelWidth)
    {
        return iOuterPixelWidth - iCellPadding * 2 - iMargin * 2 - iBorder * 2;
    }

    private string get_dashboard_cell(int iPixelWidth, int iPixelHeight, string sContent)
    {
        // id=dashboard_cell_x (starting from 0)

        DASHBOARD_CELL newCell = cellList[iCellIdCounter] = new DASHBOARD_CELL();

        if (iCellIdCounter == 1) newCell.chartType = CHART_TYPE.line_3_lines;
        else if (iCellIdCounter == 2) newCell.chartType = CHART_TYPE.bember_heading_and_percent;
        else if (iCellIdCounter == 3) newCell.chartType = CHART_TYPE.line_4_lines;
        else if (iCellIdCounter == 4) newCell.chartType = CHART_TYPE.pie_donut;
        else newCell.chartType = CHART_TYPE.bember_heading_and_percent;

        StringBuilder sb = new StringBuilder();

        string sId = sPreDashBoardCellName + iCellIdCounter.ToString();

        // Before 19 oct newCell.w = getPixelWidthAvailable(iPixelWidth);
        // Before 19 oct newCell.h = getPixelWidthAvailable(iPixelHeight);

        newCell.w = getPixelWidthAvailable(iPixelWidth);
        newCell.h = getPixelWidthAvailable(iPixelHeight);

        sb.Append(
            "<div onclick=dashboard_cell_clicked('" + sPreDashBoardCellName +
            "','" + iCellIdCounter.ToString() + "') id=" + sId +
            " class=dashboard_cell style='float:left;width:" + newCell.w.ToString() + "px;height:" + newCell.h.ToString() + "px;' " +
            "data-orgwidth='" + newCell.w.ToString() + "px' data-orgheight='" + newCell.h.ToString() + "px' >");
        sb.Append(sContent);
        sb.Append("</div>");

        ++iCellIdCounter;

        return sb.ToString();
    }

    private string getButtonClass(DASHBOARD_PERIOD dashboardPeriodThis,DASHBOARD_PERIOD dashboardPeriodCurrent)
    {
        if (dashboardPeriodThis == dashboardPeriodCurrent)
        return " class=bember_month_box_toggled ";
        else return " class=bember_month_box ";
    }

    private string A_GetHorisontalGreenTop(DASHBOARD_PERIOD currentDashboardPeriod, List<backoffice.dim_value_value> shopList, List<backoffice.dim_value_value> shopListAllPeriods)
    {
        StringBuilder sb = new StringBuilder();

        DASHBOARD_GREEN dg = new DASHBOARD_GREEN(global);

        // 14 okt = 68
        List<backoffice.dim_dim_value> yearMonthList = global.www_backoffice().dash_maxi_month_member((int)DateTime.Now.Year, "24");

        List<backoffice.dim_dim_value_value> yearMonthBasketSumList = global.www_backoffice().dash_get_average_member_basket((int)DateTime.Now.Year, "24");


        List<backoffice.dim_value> sexList = global.www_backoffice().get_consumer_sex_list("24",currentDashboardPeriod);

        string sAverageAge = global.www_backoffice().get_average_member_age("24", currentDashboardPeriod);

        sb.Append("<td align=center>");
        sb.Append("<div>");
        sb.Append("<table align=center cellpadding=0 cellspacing=0>");

        sb.Append("<tr>");
        sb.Append("<td align=center valign=top style='padding:20px;'>");
        sb.Append(dg.A_1_GetGreenPart(global,currentDashboardPeriod, yearMonthList, shopListAllPeriods));
        sb.Append("</td>");
        sb.Append("<td align=center valign=top  style='padding:20px;'>");
        sb.Append(dg.A_2_GreenPart(global,currentDashboardPeriod, yearMonthBasketSumList));
        sb.Append("</td>");
        sb.Append("<td align=center valign=top  style='padding:20px;'>");
        sb.Append(dg.A_3_GreenPart(global,currentDashboardPeriod, yearMonthBasketSumList));
        sb.Append("</td>");
        sb.Append("<td align=center  valign=top  style='padding:20px;'>");
        sb.Append(dg.A_4_GreenPart(sexList, sAverageAge));
        sb.Append("</td>");
        sb.Append("</tr>");
        sb.Append("</table>");
        sb.Append("</div>");
        sb.Append("</td>");
        return sb.ToString();
    }

    private string wrap_in_div_with_class(string sClass, string sContent, string sExtraStyle="")
    {
        StringBuilder sb = new StringBuilder();
        if (sClass != "") sb.Append("<div class=" + sClass + " style='" + sExtraStyle + "' >");
        else sb.Append("<div style='float:left;" + sExtraStyle + "'>");
        sb.Append(sContent);
        sb.Append("</div>");
        return sb.ToString();            
    }

    private string wrap_in_div(string sContent)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div style=''>");
        sb.Append(sContent);
        sb.Append("</div>");
        return sb.ToString();
    }


    private string get_vert_line_1()
    {
        return
        "<svg xmlns='http://www.w3.org/2000/svg' version='1.1' width=12 height=73 > " +
        "<line x1=5 y1=0 x2=5 y2=71 style='stroke:rgb(59,135,224);stroke-width:1' />" +
        "<polygon points='0,66 10,66 5,73 0,66' style='fill:rgb(59,135,224);' />";
    }

    private string get_vert_line_2()
    {
        return
        "<svg xmlns='http://www.w3.org/2000/svg' version='1.1' width=12 height=102 > " +
        "<line x1=5 y1=0 x2=5 y2=98 style='stroke:rgb(244,166,40);stroke-width:1' />" +
        "<polygon points='0,93 10,93 5,100 0,93' style='fill:rgb(244,166,40);' />";
    }

    private string get_vert_line_3()
    {
        return
        "<svg xmlns='http://www.w3.org/2000/svg' version='1.1' width=12 height=103 > " +
        "<line x1=5 y1=0 x2=5 y2=99 style='stroke:rgb(69,144,121);stroke-width:1' />" +
        "<polygon points='0,94 10,94 5,101 0,94' style='fill:rgb(69,144,121);' />";
    }


    private string get_right_conversion(Global global, List<backoffice.dim_value_value> shopList, List<backoffice.name_value_value_value_value> conversionList, List<backoffice.shop_top_bottom> shopConversionList)
    {
        if (shopConversionList == null) return "";

        StringBuilder sb = new StringBuilder();

        int iNofShops = shopConversionList.Count;

        sb.Append("<div style='margin-top:60px;margin-bottom:10px;'><span class=shop_result_left_part>BUTIKKRESULTATER</span>");
        if (iNofShops > 10)
        {
            sb.Append("<span class=shop_result_left_part>&nbsp;TOPP OG BUNN</span>");
        }
        sb.Append("</div>");


        // Sort it ...
        if (shopConversionList != null)
        {
            shopConversionList.Sort
            (
                delegate(backoffice.shop_top_bottom p1, backoffice.shop_top_bottom p2)
                {
                    return p2.dConvertedFromTotalPercent.CompareTo(p1.dConvertedFromTotalPercent);
                }
            );
        }

        int iShopNo = 1;
        

        bool bWhite = true;

        sb.Append("<table cellspacing=0>");

        string sBackgroundClass = "";
        
        foreach (backoffice.shop_top_bottom shop in shopConversionList)
        {
            if (bWhite) sBackgroundClass = " class=shop_result_light "; else sBackgroundClass = " class=shop_result_dark ";

            bool bSkipShop = false;
            if (iShopNo > 5 && iNofShops > 10 && (iNofShops - iShopNo) > 4) bSkipShop = true;

            // bSkipShop = false;

            if (!bSkipShop)
            {
                sb.Append("<tr " + sBackgroundClass + " >");
                sb.Append("<td  class=shop_result_col_1>"); sb.Append(shop.sName); sb.Append("</td>");

                sb.Append("<td class=shop_result_col_2>"); sb.Append((int)shop.dLeftMobilePercent + "%"); sb.Append("</td>");
                sb.Append("<td class=shop_result_col_3>"); sb.Append((int)shop.dConvertedFromTotalPercent + "%"); sb.Append("</td>");
                sb.Append("<td class=shop_result_col_4>" + "<img src='../css/members.png' height=11>" + "</td>");
                sb.Append("<td class=shop_result_col_5>"); sb.Append(shop.iYesPhoneApproved); sb.Append("</td>");

                sb.Append("</tr>");

                if (iShopNo == 5 && iNofShops > 10)
                {
                    bWhite = true;
                    sb.Append("<tr>");
                    sb.Append("<td colspan=5><div class=table_line></div></td>");
                    sb.Append("</tr>");
                }
                else bWhite = !bWhite;
            }
            ++iShopNo;
        }

        sb.Append("<tr>");
        sb.Append("<td colspan=5><div class=table_line></div></td>");
        sb.Append("</tr>");
        
        sb.Append("</table>");
        
        return sb.ToString();
    }

    private string get_left_conversion(Global global, List<backoffice.dim_value_value> shopList, List<backoffice.name_value_value_value_value> conversionList)
    {
        StringBuilder sb = new StringBuilder();

        long iTotalTransactionsWithAndWithoutPhone = 0;
        foreach (backoffice.name_value_value_value_value conversionItem in conversionList)
        {
            iTotalTransactionsWithAndWithoutPhone += Convert.ToInt64(conversionItem.sValue1) + Convert.ToInt64(conversionItem.sValue2) + Convert.ToInt64(conversionItem.sValue3);
        }
        
        int iTotalMembers = 0;
        int iTotalNotMembers = 0;

        foreach (backoffice.dim_value_value shop in shopList)
        {
            iTotalNotMembers += Convert.ToInt32(shop.sValue_2);
            iTotalMembers += Convert.ToInt32(shop.sValue_1); // 14 okt = 65
        }
        int iTotalConsumers = iTotalNotMembers + iTotalMembers;

        string sPercent1 = DASHBOARD_GREEN.get_percent_of(iTotalTransactionsWithAndWithoutPhone, iTotalNotMembers + iTotalMembers, false);
        string sPercent2 = DASHBOARD_GREEN.get_percent_of(iTotalTransactionsWithAndWithoutPhone, iTotalMembers, false);
        string sPercent3 = DASHBOARD_GREEN.get_percent_of(iTotalConsumers, iTotalMembers, false);

        string sLine1 = "<svg xmlns='http://www.w3.org/2000/svg' version='1.1' width=12 height=52 > ";
        sLine1 += "<line x1=6 y1=0 x2=6 y2=48 style='stroke:rgb(59,135,224);stroke-width:3' />";
        sLine1 += "<polygon points='0,40 12,40 6,50 0,40' style='fill:rgb(59,135,224);' />";
        sLine1 += "</svg> ";


        string s_conversion_heading = wrap_in_div_with_class("s_conversion_heading", "KONVERTERING");

        string s_1_1_DownArrow_1 = DASHBOARD_GREEN.centerTableHorVerTextInDiv(50, get_vert_line_1());
        
        string s_1_2_1_NotMembers = wrap_in_div_with_class("s_1_2_1_NotMembers", "IKKE-MEDLEMMER");
        string s_1_2_2_NotMembersValue = wrap_in_div_with_class("s_1_2_2_NotMembersValue", iTotalTransactionsWithAndWithoutPhone.ToString());

        string s_2_1_DownArrow_2 = DASHBOARD_GREEN.centerTableHorVerTextInDiv(50,get_vert_line_2());
        string s_2_2_1_ReturnsMobileNumber = wrap_in_div_with_class("s_2_2_1_ReturnsMobileNumber", "LEGGER IGJEN TLF-NR");
        string s_2_2_2_ReturnsMobileNumberPercent = wrap_in_div_with_class("s_2_2_2_ReturnsMobileNumberPercent", sPercent1 +  " %");
        string s_2_2_3_ReturnsMobilePic1 = wrap_in_div_with_class("", "<img src='../css/members.png' height=13>","margin-top:3px;");
        string s_2_2_4_ReturnsMobileNumberValue = wrap_in_div_with_class("s_2_2_4_ReturnsMobileNumberValue", iTotalConsumers.ToString(), "margin-left:5px;margin-top:3px;");

        string s_3_1_1_SignsDealPrePercent = wrap_in_div_with_class("s_3_1_1_SignsDealPrePercent", sPercent3 + " %");
        string s_3_1_2_DownArrow_3 =  DASHBOARD_GREEN.centerTableHorVerTextInDiv(50,get_vert_line_3());

        string s_3_2_1_SignsDeal = wrap_in_div_with_class("s_3_2_1_SignsDeal", "signerer avtalen");
        string s_3_2_2_TotalConversion = wrap_in_div_with_class("s_3_2_2_TotalConversion", "TOTAL KONVERTERING");
        string s_3_2_3_TotalConversionPercent = wrap_in_div_with_class("s_3_2_3_TotalConversionPercent", sPercent2 + " %");
        string s_3_2_4_ReturnsMobilePic2 = wrap_in_div_with_class("", "<img src='../css/members.png' height=13>", "margin-top:3px;");
        
        // 14 okt = 65
        string s_3_2_5_TotalConversionNumber = wrap_in_div_with_class("s_3_2_5_TotalConversionNumber", iTotalMembers.ToString(), "margin-left:5px;margin-top:3px;");        

        sb.Append("<table align=left style='margin-left:10px;'>");

        sb.Append("<tr>");
        sb.Append("<td colspan=2 valign=top>"); sb.Append(s_conversion_heading); sb.Append("</td>");        
        sb.Append("</tr>");
        
        sb.Append("<tr>");
        sb.Append("<td valign=top align=center>");sb.Append(s_1_1_DownArrow_1);sb.Append("</td>");
        sb.Append("<td valign=top>"); sb.Append(s_1_2_1_NotMembers + "<br>" + s_1_2_2_NotMembersValue); sb.Append("</td>");
        sb.Append("</tr>");

        sb.Append("<tr>");
        sb.Append("<td valign=top align=center>"); sb.Append(s_2_1_DownArrow_2); sb.Append("</td>");
        sb.Append("<td valign=top>"); 
        sb.Append(s_2_2_1_ReturnsMobileNumber + "<br>" + s_2_2_2_ReturnsMobileNumberPercent + "<div style='clear:both;'>" + s_2_2_3_ReturnsMobilePic1 + s_2_2_4_ReturnsMobileNumberValue + "</div>"); sb.Append("</td>");
        sb.Append("</tr>");

        sb.Append("<tr>");
        sb.Append("<td valign=top align=center>"); sb.Append(s_3_1_1_SignsDealPrePercent + "<br>" + s_3_1_2_DownArrow_3); sb.Append("</td>");
        sb.Append("<td valign=top>"); sb.Append(s_3_2_1_SignsDeal + "<br>" + s_3_2_2_TotalConversion + "<br>" + s_3_2_3_TotalConversionPercent + "<div style='clear:both;'>" + s_3_2_4_ReturnsMobilePic2 + s_3_2_5_TotalConversionNumber + "</div>"); sb.Append("</td>");
        sb.Append("</tr>");

        sb.Append("</table>");

        return sb.ToString();
    }

    
    private string B_GetConversion(List<backoffice.dim_value_value> shopList, List<backoffice.name_value_value_value_value> conversionList,List<backoffice.shop_top_bottom> shopConversionList)
    {
        StringBuilder sb = new StringBuilder();

        DASHBOARD_GREEN dg = new DASHBOARD_GREEN(global);

        string sLeftConversion = get_left_conversion(global, shopList, conversionList); // 5 okt

        sb.Append("<td>");

        sb.Append(get_dashboard_cell(300, 400, sLeftConversion));

        sb.Append(get_dashboard_cell(600, 400, "Dette er det store linjediagrammet"));

        string sRightConversion = get_right_conversion(global, shopList, conversionList, shopConversionList);

        sb.Append(get_dashboard_cell(415, 400, sRightConversion));
        sb.Append("</td>");
        return sb.ToString();
    }

    private string append_row_33()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<td>");
        sb.Append(get_dashboard_cell(500, 250,"Hei 1"));
        sb.Append(get_dashboard_cell(480, 250,"Hei 2"));
        sb.Append("</td>");
        return sb.ToString();
    }


    private string append_row_44()
    {
        StringBuilder sb = new StringBuilder();

        List <backoffice.dim_value> shopList = global.www_backoffice().get_monthly_invoice("24");

        StringBuilder sInvoice = new StringBuilder();

        foreach (backoffice.dim_value shop in shopList)
        {
            sInvoice.Append(shop.sDim + ", " + shop.sValue + "<br>");
        }

        sb.Append("<td>");
        sb.Append(get_dashboard_cell(500, 250, sInvoice.ToString()));
        sb.Append(get_dashboard_cell(480, 250, "Fakturagrunnlag 2"));
        sb.Append("</td>");

        return sb.ToString();
    }


}


public class DASHBOARD_GREEN
{
    Global global = null;

    public DASHBOARD_GREEN(Global _global)
    {
        global = _global;
    }

    public static string centerTableHorVerTextInDiv(int iBoxH, string sValue1)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<table cellpadding=0 cellspacing=0 align=center width=100%><tr><td valign=center align=center style='height:" + iBoxH.ToString() + "px;'>" + sValue1 + "</td></tr></table>");
        return sb.ToString();
    }

    public static string centerTableVerTextInDiv(int iBoxH, string sValue1)
    {
        StringBuilder sb = new StringBuilder();
        // Dirty, but it ALWAYS WORKS !!!
        sb.Append("<table cellpadding=0 cellspacing=0  width=100%><tr><td valign=center style='height:" + iBoxH.ToString() + "px;'>" + sValue1 + "</td></tr></table>");
        return sb.ToString();
    }


    public string A_1_GetGreenPart(Global global,DASHBOARD_PERIOD currentPeriod, List<backoffice.dim_dim_value> yearMonthList, List<backoffice.dim_value_value> shopList)
    {
        StringBuilder sb = new StringBuilder();

        DASHBOARD_MONTH dashBoardMonth = new DASHBOARD_MONTH(currentPeriod); // Denne skal dersom parameter er shippet ta periodetype inn 

        int iInPeriod = 0;
        int iFromAncientTimeToLastDateBeforePeriod = 0;

        foreach (backoffice.dim_dim_value yearMonth in yearMonthList)
        {
            if (yearMonth.sDim_1 == dashBoardMonth.iThisMonthYear.ToString() && yearMonth.sDim_2 == dashBoardMonth.iThisMonth.ToString())
            {
                iInPeriod = Convert.ToInt32(yearMonth.sValue); // 14 okt = 68
            }
            else if (yearMonth.sDim_1 == dashBoardMonth.iThisMonthYear.ToString())
            {
                if (Convert.ToInt32(yearMonth.sDim_2) <= dashBoardMonth.iThisMonth)
                {
                    iFromAncientTimeToLastDateBeforePeriod += Convert.ToInt32(yearMonth.sValue);
                }
            }
            else if (Convert.ToInt32(yearMonth.sDim_1) <= dashBoardMonth.iThisMonthYear)
            {
                iFromAncientTimeToLastDateBeforePeriod += Convert.ToInt32(yearMonth.sValue);
            }
        }
        
        int iTotalMembers = 0;
        foreach (backoffice.dim_value_value shop in shopList)
        {            
            iTotalMembers += Convert.ToInt32(shop.sValue_1);
        }
        
        // iInPrevPeriod = antall medlemmer inntil den den flrste i iInPeriod.
        string sIncreasePercent = "+&nbsp;" + get_percent_of(iFromAncientTimeToLastDateBeforePeriod, iInPeriod, false);

        // 14 okt = 68
        sb.Append(XXX_getCenteredGreenFrame(0, 10, 300, 220, 170, "MEDLEMSBASE", "TOTALT ANTALL MEDLEMMER", "I PERIODEN", "ENDRING I %", iTotalMembers.ToString(),
            global.getSvgUpDownArrow("&nbsp;" + iInPeriod.ToString(), true),
            global.getSvgUpDownArrow("&nbsp;" + sIncreasePercent + "&nbsp;%",true)));
        
        
        return sb.ToString();
    }

    public static string get_percent_of(double dTotal, double dSmall, bool bUseSign)
    {
        double dOnePercent = dTotal / 100;

        if (dOnePercent == 0) return "0";

        double dPercent = dSmall / dOnePercent;

        string sRetVal = dPercent.ToString("#.#");

        if (sRetVal == "") sRetVal = "0.00";
        if (sRetVal.StartsWith(",") || sRetVal.StartsWith(".")) sRetVal = "0" + sRetVal;

        string sSign = "";

        if (bUseSign)
        {
            sSign = "+&nbsp;";
            if (dSmall < dTotal) sSign = "-&nbsp;";
        }        

        sRetVal = sSign + sRetVal;

        return sRetVal;
    }

    public static decimal get_percent_of_to_decimal(double dTotal, double dSmall, bool bUseSign)
    {
        
        double dOnePercent = dTotal / 100;

        if (dOnePercent == 0) return 0;

        double dPercent = dSmall / dOnePercent;

        return (decimal)dPercent;
    }


    private string get_percent_increase_from_to(double dFrom, double dTo, bool bUseSign)
    {
        double dOnePercent = dFrom / 100;

        if (dOnePercent == 0) return "0";

        double dPercent = Math.Abs(( (dTo-dFrom) / dOnePercent));

        string sRetVal = dPercent.ToString("#.#");

        if (sRetVal == "") sRetVal = "0.00";

        if (sRetVal.StartsWith(",") || sRetVal.StartsWith(".")) sRetVal = "0" + sRetVal;

        string sSign = "";

        if (bUseSign)
        {
            sSign = "+&nbsp;";
            if (dTo < dFrom) sSign = "-&nbsp;";
        }
        sRetVal = sSign + sRetVal;
        return sRetVal;
    }

    private string XXX_getCenteredGreenFrame(int left, int top, int width, int height, int iTopTwoBoxes, string sHeading1, string sHeading2, string sHeading3,string sHeading4, string sValue1, string sValue2, string sValue3)
    {
        StringBuilder sb = new StringBuilder();

        int iHalfWidth = (width / 2);

        int iHalfTextHeight = 8;

        int iHeightTwoBoxes = height - iTopTwoBoxes + 2;

        int iTextBuffer = 0;

        string sBorderColor = "rgb(136,181,158)";
        string sBackGroundColor = "rgb(37,126,98)";

        string sStyle = "style='float:left;background-color:" + sBackGroundColor + ";color:rgb(136,181,158);'";
        sb.Append("<div " + sStyle + " >");
        sb.Append(get_start_container_relative_xxxx());

        // Ramme 1
        sb.Append("<div style='position:absolute;top:" + top.ToString() + "px;left:" + left.ToString() + "px;height:" + height.ToString() + "px;width:" + (width - 1).ToString() + "px;border: solid 1px " + sBackGroundColor + ";background-color:" + sBackGroundColor + ";'></div>");


        int iBigBoxHeight = iTopTwoBoxes - iHalfTextHeight;
        sb.Append("<div style='position:absolute;top:" + (top + 2).ToString() + "px;left:" + left.ToString() + "px;height:" + (iBigBoxHeight).ToString().ToString() + "px;width:" + (width - 1).ToString() + "px;'>");
        sb.Append("<span class=dashboard_square_0_heading_2>" + centerTableVerTextInDiv(iBigBoxHeight,
            "<span class=member_number_of>" + sHeading2 + "</span>" +
            "<br>" +
            // "<span class=dashboard_square_0_value_1>" + sValue1 + "</span>"));

        "<span class=member_number_of_value>" + sValue1 + "</span>"));

        sb.Append("</div>");

        // Heading 1
        sb.Append("<div style='position:absolute;left:0px;top:0px;background-color:" + sBackGroundColor + ";font-family_arial;font-size:14px;font-weight:bold;color:rgb(255,255,255);'>");
        sb.Append("<span class=member_base_heading>" + sHeading1 + "</span>"); // ettermiddag 1
        sb.Append("</div>");

        // Ramme 2.1 OK?
        sb.Append("<div style='position:absolute;top:" + (iTopTwoBoxes + iHalfTextHeight).ToString() + "px;left:0px;height:" + iHeightTwoBoxes.ToString() + "px;width:" + iHalfWidth.ToString() + "px;border-top: solid 1px " + sBorderColor + ";border-bottom: solid 1px " + sBorderColor + ";'>");
        sb.Append("<span class=member_left_small_value>" + centerTableVerTextInDiv(iHeightTwoBoxes, sValue2) + "</span>");
        sb.Append("</div>");

        // Ramme 2.2 OK?
        sb.Append("<div style='position:absolute;top:" + (iTopTwoBoxes + iHalfTextHeight).ToString() + "px;left:" + iHalfWidth.ToString() + "px;height:" + iHeightTwoBoxes.ToString() + "px;width:" + iHalfWidth.ToString() + "px;border-top: solid 1px " + sBorderColor + ";border-bottom: solid 1px " + sBorderColor + ";'>");
        sb.Append("<span class=member_left_small_value>" + centerTableVerTextInDiv(iHeightTwoBoxes, sValue3) + "</span>");
        sb.Append("</div>");

        // text 2.1
        sb.Append("<div style='position:absolute;top:" + (iTopTwoBoxes+4).ToString() + "px;left:" + iTextBuffer.ToString() + "px;background-color:" + sBackGroundColor + ";'>");
        sb.Append("<span class=member_left_heading_small_value>" + sHeading3 + "&nbsp;</span>");
        sb.Append("</div>");

        // text 2.2
        sb.Append("<div style='position:absolute;top:" + (iTopTwoBoxes+4).ToString() + "px;left:" + (iHalfWidth + iTextBuffer).ToString() + "px;background-color:" + sBackGroundColor + ";'>");
        sb.Append("<span class=member_left_heading_small_value>&nbsp;" + sHeading4 + "&nbsp;</span>");
        sb.Append("</div>");

        sb.Append(get_div_end()); // Container relative

        sb.Append(get_div_end());

        return sb.ToString();

    }

    private string A_1_GreenPart(string sFrame, int left, int top, int width, int height, int iTopTwoBoxes, string sHeading1, string sHeading2, string sHeading3, string sValue1, string sValue2, string sValue3)
    {
        StringBuilder sb = new StringBuilder();

        int iHalfWidth = (width / 2);

        // int iHalfTextHeight = 8;
        int iHalfTextHeight = 8;

        int iHeightTwoBoxes = height - iTopTwoBoxes + 2;

        int iTextBuffer = 30;

        string sBorderColor = "rgb(136,181,158)";
        string sBackGroundColor = "rgb(37,126,98)";

        string sStyle = "style='float:left;background-color:" + sBackGroundColor + ";color:rgb(136,181,158);'";
        sb.Append("<div " + sStyle + " >");
        sb.Append(get_start_container_relative_xxxx());

        // Ramme 1
        sb.Append("<div style='position:absolute;top:" + top.ToString() + "px;left:" + left.ToString() + "px;height:" + height.ToString() + "px;width:" + (width-1).ToString() + "px;border: solid 1px " + sBorderColor + ";background-color:" + sBackGroundColor + ";'></div>");


        int iBigBoxHeight = iTopTwoBoxes - iHalfTextHeight;
        sb.Append("<div style='position:absolute;top:" + (top + 2).ToString() + "px;left:" + left.ToString() + "px;height:" + (iBigBoxHeight).ToString().ToString() + "px;width:" + (width - 1).ToString() + "px;'>");
        sb.Append("<span class=top_boxes_big_number>" + centerTableHorVerTextInDiv(iBigBoxHeight, sValue1) + "</span>");
        sb.Append("</div>");

        // 5 okt ettermiddag

        // Heading 1
        sb.Append("<div id=" + sFrame + "_heading_1 " + 
            "data-center_x='" + (width/2).ToString() + "' " + 
            "class=top_boxes_heading " + 
            "style='left:" + iTextBuffer.ToString() + "px;top:3px;'>");
        sb.Append("&nbsp;" + sHeading1 + "&nbsp;");
        sb.Append("</div>");

        // Ramme 2.1
        sb.Append("<div style='position:absolute;top:" + (iTopTwoBoxes + iHalfTextHeight).ToString() + "px;left:0px;height:" + iHeightTwoBoxes.ToString() + "px;width:" + iHalfWidth.ToString() + "px;border: solid 1px " + sBorderColor + ";'>");
        sb.Append("<span class=top_boxes_small_number>" + centerTableHorVerTextInDiv(iHeightTwoBoxes, sValue2) + "</span>");
        sb.Append("</div>");

        // Ramme 2.2
        sb.Append("<div style='position:absolute;top:" + (iTopTwoBoxes + iHalfTextHeight).ToString() + "px;left:" + iHalfWidth.ToString() + "px;height:" + iHeightTwoBoxes.ToString() + "px;width:" + iHalfWidth.ToString() + "px;border-top: solid 1px " + sBorderColor + ";border-bottom: solid 1px " + sBorderColor + ";'>");
        sb.Append("<span class=top_boxes_small_number>" + centerTableHorVerTextInDiv(iHeightTwoBoxes, sValue3) + "</span>");
        sb.Append("</div>");

        // text 2.1
        sb.Append("<div class=top_boxes_heading id=" + sFrame + "_heading_2 data-center_x='" + (iHalfWidth / 2).ToString() + "' style='top:" + (iTopTwoBoxes).ToString() + "px;left:" + iTextBuffer.ToString() + "px;'>");
        sb.Append("<div class=dashboard_square_1_heading_2>&nbsp;" + sHeading2 + "&nbsp;</div>");
        sb.Append("</div>");

        // text 2.2
        sb.Append("<div class=top_boxes_heading id=" + sFrame + "_heading_3 data-center_x='" + (iHalfWidth + (iHalfWidth / 2)).ToString() + "' style='position:absolute;top:" + (iTopTwoBoxes).ToString() + "px;left:" + (iHalfWidth + iTextBuffer).ToString() + "px;'>");
        sb.Append("<div class=dashboard_square_1_heading_2>&nbsp;" + sHeading3 + "&nbsp;</div>");
        sb.Append("</div>");

        sb.Append(get_div_end()); // Container relative

        sb.Append(get_div_end());

        return sb.ToString();
    }

    public string A_2_GreenPart(Global global,DASHBOARD_PERIOD currentPeriod, List<backoffice.dim_dim_value_value> yearMonthList) 
    {
        StringBuilder sb = new StringBuilder();

        DASHBOARD_MONTH dashBoardMonth = new DASHBOARD_MONTH(currentPeriod); // Denne skal dersom parameter er shippet ta periodetype inn 

        long dInPeriod = 0;
        long dInPrevPeriod = 0;

        foreach (backoffice.dim_dim_value_value yearMonth in yearMonthList)
        {
            if (yearMonth.sDim_1 == dashBoardMonth.iThisMonthYear.ToString() && yearMonth.sDim_2 == dashBoardMonth.iThisMonth.ToString())
            {
                dInPeriod = (long) (yearMonth.dValue_2);
            }
            else if (yearMonth.sDim_1 == dashBoardMonth.iPrevMonthYear.ToString() && yearMonth.sDim_2 == dashBoardMonth.iPrevMonth.ToString())
            {
                dInPrevPeriod = (long)(yearMonth.dValue_2);
            }
        }

        string sPercentChange = get_percent_increase_from_to(dInPrevPeriod, dInPeriod, true);

        sb.Append(A_1_GreenPart("frame_1", 0, 10, 300, 220, 170, "OMSETNING " + new DASHBOARD_MONTH(currentPeriod).sThisMonth, "FORRIGE MND.", "ENDRING I %", dInPeriod.ToString("# ##0") + " kr", dInPrevPeriod.ToString("# ##0") + " kr",global.getSvgUpDownArrow("&nbsp;" + sPercentChange + " %",false)));
        return sb.ToString();
    }

    public string A_3_GreenPart(Global global, DASHBOARD_PERIOD currentPeriod, List<backoffice.dim_dim_value_value> yearMonthList)
    {
        StringBuilder sb = new StringBuilder();

        DASHBOARD_MONTH dashBoardMonth = new DASHBOARD_MONTH(currentPeriod); // Denne skal dersom parameter er shippet ta periodetype inn 

        long dInPeriod = 0;
        long dInPrevPeriod = 0;

        foreach (backoffice.dim_dim_value_value yearMonth in yearMonthList)
        {
            if (yearMonth.sDim_1 == dashBoardMonth.iThisMonthYear.ToString() && yearMonth.sDim_2 == dashBoardMonth.iThisMonth.ToString())
            {
                if (yearMonth.dValue_1 > 0)
                    dInPeriod = (long)(yearMonth.dValue_2 / yearMonth.dValue_1);
                else
                    dInPeriod = 0;
            }
            else if (yearMonth.sDim_1 == dashBoardMonth.iPrevMonthYear.ToString() && yearMonth.sDim_2 == dashBoardMonth.iPrevMonth.ToString())
            {
                if (yearMonth.dValue_1 > 0)
                    dInPrevPeriod = (long)(yearMonth.dValue_2 / yearMonth.dValue_1);
                else
                    dInPrevPeriod = 0;
            }
        }

        string sPercentChange = get_percent_increase_from_to(dInPrevPeriod, dInPeriod, true);
        sb.Append(A_1_GreenPart("frame_2", 0, 10, 300, 220, 170, "GJENNOMSNITTSKJØP " + new DASHBOARD_MONTH(currentPeriod).sThisMonth, "FORRIGE MND.", "ENDRING I %", dInPeriod.ToString() + " kr", dInPrevPeriod.ToString() + " kr", global.getSvgUpDownArrow("&nbsp;" + sPercentChange + "&nbsp;%",false)));
        return sb.ToString();
    }

    public string A_4_GreenPart(List<backoffice.dim_value> sexList, string sAverageAge)
    {
        StringBuilder sb = new StringBuilder();

        int iMale = 0;
        int iFemale = 0;
        int iTotal = 0;
        foreach (backoffice.dim_value sex in sexList)
        {
            iTotal += Convert.ToInt32(sex.sValue);
            if (sex.sDim == "male") iMale = Convert.ToInt32(sex.sValue);
            if (sex.sDim == "female") iFemale = Convert.ToInt32(sex.sValue);
        }

        string sPercentMale = get_percent_of(iTotal, iMale, false);
        string sPercentFemale = get_percent_of(iTotal, iFemale, false);

        sb.Append(A_1_GreenPart("frame_3", 0, 10, 300, 220, 170, "DEMOGRAFI", "KVINNER", "MENN", "<span class='top_boxes_big_number_heading' >GJN. ALDER</span><br>" + sAverageAge + " år", sPercentFemale + " %", sPercentMale + " %"));
        return sb.ToString();
    }


    public string get_div_start(string sClass) {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='" + sClass + "'>");
        return sb.ToString();
    }


    public string get_div_end()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("</div>");
        return sb.ToString();
    }


    public string get_start_dashboard_inner_frame()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_inner_frame>");
        return sb.ToString();
    }

    public string get_square_0_heading_1(string text)
    {
        if (text == "") text = "&nbsp;";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_square_0_heading_1>" + text + "</div>");
        return sb.ToString();
    }

    public string get_square_0_heading_2(string text) {
        if (text == "") text = "&nbsp;";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_square_0_heading_2>" + text + "</div>");
        return sb.ToString();
    }

    public string get_square_0_heading_3_absolute(string text,int left,int top) {
        if (text == "") text = "&nbsp;";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_square_1_heading_2 style='position:absolute;top:" + top.ToString() + "px;left:" + left.ToString() + "px;'>" + text + "</div>");
        return sb.ToString();
    }

    public string get_text_data_03_absolute(string text, int top, int left) {
        if (text == "") text = "&nbsp;";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_square_1_heading_2 style='position:absolute;top:" + top.ToString() + "px;left:" + left.ToString() + "px;'>" + text + "</div>");
        return sb.ToString();
    }


    public string get_text_data_01(string text) {
        if (text == "") text = "&nbsp;";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_text_data_01>" + text + "</div>");
        return sb.ToString();
    }

    public string get_text_data_02(string text) {
        if (text == "") text = "&nbsp;";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_text_data_02>" + text + "</div>");
        return sb.ToString();
    }

    public string get_text_data_03(string text) {
        if (text == "") text = "&nbsp;";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_text_data_03>" + text + "</div>");
        return sb.ToString();
    }

    public string get_line_300()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_line_300 style='margin-top:10px;'></div>");
        return sb.ToString();
    }

    public string get_start_container_relative() {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_container_relative  >");
        return sb.ToString();
    }


    public string get_start_container_relative_xxxx()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=dashboard_container_relative style='height:260px;width:283px;'>");
        return sb.ToString();
    }




    public string get_end_line_300_relative() {
        StringBuilder sb = new StringBuilder();
        sb.Append("</div>");
        return sb.ToString();
    }


}



