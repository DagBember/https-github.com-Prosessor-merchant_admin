using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;

/// <summary>
/// Summary description for Global
/// </summary>
public enum BROWSER { NONE, INTERNET_EXPLORER, INTERNET_EXPLORER_8, INTERNET_EXPLORER_9, INTERNET_EXPLORER_10, FIREFOX, SAFARI, IPAD, IPHONE }

public enum USER_PROFILE { POINT, BYTE, ZINO, BUTIKK_DEMO, NONE }

public class Global
{
    public bool bDEBUG_TESTING_OLD_DATABASE = false;

    public string MASTER_CHAIN_ID = "";

    public string sLevel_1_menu = "";
    public string sLevel_2_menu = "";
    public bool bRunningLive = false;
    public int iToHour = 20;
    public int iToMinute = 0;
    public int iToSecond = 0;
    public int iHours = 10;
    public bool bShopLiveTimeDialogIsOpen = false;
    
    public DateTime shop_live_current_timestamp = new DateTime(2000, 1, 1, 1, 1, 1);

    public string sAfterCardPage = "";

    public string sConsumerGuid = "";

    public USER_PROFILE userProfile = USER_PROFILE.NONE;
    
    public bool bLoggedIn = false;
    public bool bSuperUser = false;

    public int iShopId_admin_coupon_create = 0;

    public menu_1_item currentTopMenuItem = menu_1_item.empty;

    public menu_2_item currentMenu_2_Item = menu_2_item.empty;

    public LocalEventController controller = null;
    public Global(LocalEventController _controller)
    {
        controller = _controller;
    }


    bool bRunningLocal = false;


    public void init(System.Web.HttpRequest request)
    {
        if (request.Url.Host.ToUpper().IndexOf("LOCALHOST") >= 0)
        {
            // bRunningLocal = true;
            bRunningLocal = false;
        }
        else
        {
            // bRunningLocal = false;
            bRunningLocal = false;
        }


    }

     

    public backoffice www_backoffice()
    {
        if (bRunningLocal)
            return new backoffice();
        else
        {
            return new backoffice();
        }
    }



    // public static string home_page = "http://localhost:49405/df_admin/";
    public static string home_page = "http://www.gimeg.no/";


    public bool topMenuItemCalled(xAjax ajax)
    {
        if (ajax.clientCalled("admin_terminal()"))
        {
            currentTopMenuItem = menu_1_item.admin_terminal;
            currentMenu_2_Item = menu_2_item.empty;
            ajax.WriteVariable("action", "call_aspx");
            return true;
        }
        else if (ajax.clientCalled("admin_general()"))
        {
            currentTopMenuItem = menu_1_item.admin_general;
            currentMenu_2_Item = menu_2_item.empty;
            ajax.WriteVariable("action", "call_aspx");
            return true;
        }
        else if (ajax.clientCalled("admin_shop()"))
        {
            currentTopMenuItem = menu_1_item.admin_shop;
            currentMenu_2_Item = menu_2_item.empty;
            ajax.WriteVariable("action", "call_aspx");
            return true;
        }
        return false;
    }

    // TOP MENU
    public string chain_level_1(Global global)
    {
        StringBuilder sb = new StringBuilder();

        string s1 = HTML_TOOLBOX.left_buffer(10) + HTML_TOOLBOX.link_TWITTER_call_javascript_function("Butikker oversikt", "level_1_shop_menu()", 14, true);
        string s2 = HTML_TOOLBOX.left_buffer(40) + HTML_TOOLBOX.link_TWITTER_call_javascript_function("Rapporter til kjeden", "level_1_report()", 14, true);        
        string s3 = HTML_TOOLBOX.left_buffer(40) + HTML_TOOLBOX.link_TWITTER_call_javascript_function("Kjede - Analyse", "level_1_analyze()", 14, true);
        string s4 = HTML_TOOLBOX.left_buffer(40) + HTML_TOOLBOX.link_TWITTER_call_javascript_function("Bember Admin", "level_1_campaign()", 14, true);

        if (!global.bSuperUser)
        {
            s3 = "";
            s4 = "";
        }

        sb.Append(
        HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 10, 10, "") +
                    HTML_TOOLBOX.left_buffer(10) +
                    s1 +
                    s2 +
                    s3 +
                    s4 +
                    HTML_TOOLBOX.left_buffer(10) +
                    HTML_TOOLBOX.div_END());
        return sb.ToString();
    }

    // BUTIKKER
    public string chain_level_2_1(Global global)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 20, 20, "background-color:rgb(250,250,250);", "menu_2"));
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Butikker - Live", "level_2_live_start_show()", 12, true));

        sb.Append(HTML_TOOLBOX.left_buffer(20));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Status", "level_2_report_show_all_shops()", 12, true));

        if (global.bSuperUser)
        {
            sb.Append(HTML_TOOLBOX.left_buffer(20));
            sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Oppdater nøkkelinformasjon", "level_2_update_show_all_shops()", 12, true));
        }
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        sb.Append(HTML_TOOLBOX.div_END());
        return sb.ToString();
    }

    // Standardrapporter
    public string chain_level_2_2(Global global)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 20, 20, "background-color:rgb(250,250,250);", "menu_x"));
        sb.Append(HTML_TOOLBOX.left_buffer(10));

        if (global.bSuperUser)
        {
            sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Standardrapport", "level_2_report_2()", 12, true));
            sb.Append(HTML_TOOLBOX.left_buffer(20));
            sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Fakturagrunnlag", "level_2_shop_report_show_chain_invoice()", 12, true));
            sb.Append(HTML_TOOLBOX.left_buffer(20));
            sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Dashboard OLD", "level_2_report_3()", 12, true));
            sb.Append(HTML_TOOLBOX.left_buffer(20));
        }
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Dashboard", "level_2_report_4(" + ((int) DASHBOARD_PERIOD.THIS_MONTH).ToString() + ")", 12, true));
        
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        sb.Append(HTML_TOOLBOX.div_END());
        return sb.ToString();
    }

    // Standardrapporter Analyze
    public string chain_level_2_4()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 20, 20, "background-color:rgb(250,250,250);", "menu_x"));
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Analyse 1", "level_2_analyze_1()", 12, true));
        sb.Append(HTML_TOOLBOX.left_buffer(20));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Analyse 2", "level_2_analyze_2()", 12, true));
        sb.Append(HTML_TOOLBOX.left_buffer(20));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Analyse 3", "level_2_analyze_3()", 12, true));
        sb.Append(HTML_TOOLBOX.left_buffer(20));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Analyse 4", "level_2_analyze_4()", 12, true));
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        sb.Append(HTML_TOOLBOX.div_END());
        return sb.ToString();
    }


    // Uttrekk - kampanjer
    public string chain_menu_2_3()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 20, 20, "background-color:rgb(250,250,250);", "menu_x"));
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        // sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Uttrekk LMC skolekampanje", "level_2_report_lmc_campaign()", 12, true));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Responstid terminal - webservices", "level_2_report_verifone_to_webservice()", 12, true));
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Azure deployment", "level_2_report_azure_deployment()", 12, true));
        sb.Append(HTML_TOOLBOX.left_buffer(10));
        sb.Append(HTML_TOOLBOX.div_END());

        return sb.ToString();
    }

    public string getSvgUpDownArrow(string sText, bool bLeft)
    {
        bool bUp = true;
        StringBuilder sb = new StringBuilder();
        if (sText.IndexOf("-") >= 0) bUp = false;
        string sColor = "rgb(255,255,255)";

        if (!bLeft)
        {
            sb.Append("<table align=center>");
            sb.Append("<tr>");
            sb.Append("<td>");
        }
        sb.Append("<div style='float:left;'>");
        sb.Append("<svg xmlns='http://www.w3.org/2000/svg' version='1.1' width=10 height=25 > ");


        if (bUp)
            sb.Append(
                "<line x1=4 y1=5 x2=4 y2=20 style='stroke:" + sColor + ";stroke-width:1' />" +
                "<line x1=0 y1=9 x2=4 y2=5 style='stroke:" + sColor + ";stroke-width:1' />" +
                "<line x1=4 y1=5 x2=8 y2=9 style='stroke:" + sColor + ";stroke-width:1' />");

        /*
                "<line x1=4 y1=5 x2=4 y2=20 style='stroke:" + sColor + ";stroke-width:1' />" +
                "<line x1=0 y1=16 x2=4 y2=20 style='stroke:" + sColor + ";stroke-width:1' />" +
                "<line x1=4 y1=20 x2=8 y2=16 style='stroke:" + sColor + ";stroke-width:1' />");
        */
        
        else
            sb.Append(
                "<line x1=4 y1=5 x2=4 y2=20 style='stroke:" + sColor + ";stroke-width:1' />" +
                "<line x1=0 y1=16 x2=4 y2=20 style='stroke:" + sColor + ";stroke-width:1' />" +
                "<line x1=4 y1=20 x2=8 y2=16 style='stroke:" + sColor + ";stroke-width:1' />");

        sb.Append("</svg>");

        sb.Append("</div>");

        sb.Append("<div style='float:left;'>");
        sb.Append(sText);
        sb.Append("</div>");

        if (!bLeft)
        {
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
        }

        return sb.ToString();
    }



}