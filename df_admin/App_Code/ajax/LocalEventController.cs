using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Globalization;          // #Java: 
using System.Threading;              // #Java: 


public enum menu_1_item
{
    empty,

    admin_terminal,
    admin_general,
    admin_shop

}
public enum menu_2_item
{
    empty,

    admin_user_login_email,
    admin_user_login_facebook,
    admin_user_login_phone,
    admin_user_coupon_view,
    admin_user_demo_card,

    admin_user_create_consumer_from_facebook,
    admin_user_create_consumer_from_email,
    admin_user_verify_consumer_from_pincode,

    admin_user_consumer_get_unique_link_from_email,
    admin_user_consumer_email_set_new_password,

    admin_user_consumer_get_unique_link_from_phone,



    admin_user_create,
    admin_user_view,
    admin_user_shop_connect,
    admin_chain_create,
    
    admin_coupon_approve,
    admin_coupon_create,
    admin_coupon_view,
    admin_shop_create,
    admin_shop_view,
    admin_consumer_view,
    admin_x_set_google_font,
    admin_show_consumer_coupon,
    admin_create_consumer_coupon,

    admin_single_consumer_coupon,
    admin_set_consumer_coupon_subscription,
    admin_get_consumer_progress,
    admin_consumer_interests,
    admin_veriphone_pilot,
    admin_pos_pilot,

    admin_consumer_last_webservice_created,


    shop_coupon_create,
    shop_coupon_view,
    shop_admin_details,

    terminal_get_shop_consumer_token_coupons,
    terminal_ping_din_fordel_string,
    terminal_ping_din_fordel_string_in_string_out,
    terminal_ping_din_fordel_int,
    terminal_ping_din_fordel_bool,

    point_interface_form,
    admin_user_show_webservices,
}


public abstract class LocalEventController : xEventController
{
    public Global global;

    public bool stop_intruder()
    {
        if (global == null) return true;
        if (global.bLoggedIn == false)
        {
            return true;
        }
        return false;
    }

    public void initializeAjaxController(
    System.Web.HttpRequest _request,
    System.Web.SessionState.HttpSessionState _session,
    System.Web.HttpResponse _response,
    System.Web.HttpServerUtility _server)
    {
        try
        {
            this.response = _response;
            this.request = _request;
            this.session = _session;
            this.server = _server;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO", false);   

            www.fwInitialize("", "", request, session, response);
            ajax.Initialize(www);



            sGlobalAjaxPrefix = ajax.getString("global_session_prefix");
            string sGlobalSessionPrefix = (string)www.fwGetSessionVariable("global_session_prefix");

            // A) NONE Ajax - prefix and NONE session : Just return.            
            if (isBlank(sGlobalAjaxPrefix) && isBlank(sGlobalSessionPrefix))
            {
                return;
            }

            // B) Ajax - prefix and NONE session - prefix. SAVE NEW.
            if (!isBlank(sGlobalAjaxPrefix) && isBlank(sGlobalSessionPrefix))
            {
                www.fwSetSessionVariable("global_session_prefix", sGlobalAjaxPrefix);
                ajax.WriteVariable("initiating", "true");

                global = (Global)www.fwGetSessionVariable(sGlobalAjaxPrefix + "_global");
                if (global == null)
                {
                    global = new Global(this);
                    www.fwSetSessionVariable(sGlobalAjaxPrefix + "_global", global);
                    ajax.WriteVariable("initiating", "true");
                    return;
                }


                return;
            }

            
            // C) session - prefix. Just Go On
            if (!isBlank(sGlobalSessionPrefix))
            {
                global = (Global)www.fwGetSessionVariable(sGlobalSessionPrefix + "_global");
                return;
            }
        }
        catch (Exception)
        {
        }
    }


    


    public bool isBlank(string s)
    {
        if (s == null) return true;
        if (s.Trim() == "") return true;
        return false;
    }

    public static string html_getTopMenu(menu_1_item higlightedItem)
    {
        xjcString s = new xjcString("");
        
        // s.append(html_getMenuItem_level_1(higlightedItem, "Din Fordel - Simulator", higlightedItem));
        s.append(html_getMenuItem_level_1(menu_1_item.admin_terminal, "Din Fordel - Simulator", higlightedItem));
        // s.append(html_getMenuItem_level_1(menu_1_item.admin_general, "Din Fordel Admin", higlightedItem));
        // s.append(GlobalGui.html_getMenuItem_level_1(menu_1_item.admin_shop, "Butikk eier", higlightedItem));

        return s.toString();
    }



    public static string html_getMenuItem_level_1(menu_1_item menuItem, string sText, menu_1_item higlightedItem)
    {
        xjcString s = new xjcString();

        string sFontWeightAndSize = "font-weight:normal;font-size:" + GlobalGui.getFontSize(1) + "px";
        string sBackColor = "background-color:rgb(220,220,220)";
        if (menuItem == higlightedItem) sFontWeightAndSize = "font-weight:bold;font-size:" + GlobalGui.getFontSize(1) + "px";
        s.append("<div onclick=" + menuItem.ToString() + "()" + " style='");
        s.append("float:left;" + sBackColor + ";cursor:pointer;margin:4px;border-radius:8px;xheight:100px;xwidth:200px;padding:20px;color:rgb(20,20,20);" + sFontWeightAndSize + ";border: 4px rgb(200,200,200) solid;");
        s.append("'>");
        s.append(sText);

        s.append("</div>");
        return s.toString();
    }

    public static string html_getMenuItem_level_2(menu_2_item menuItem, menu_2_item currentItem)
    {
        xjcString s = new xjcString();

        string sFontWeight = "font-weight:normal";
        if (currentItem == menuItem)
            sFontWeight = "font-weight:bold";
        s.append("<div onclick=" + menuItem.ToString() + "()" + " style='");
        s.append("float:left;cursor:pointer;padding:3px;padding-right:20px;font-weight:bold;text-decoration:underline;font-size:" + GlobalGui.getFontSize(4) + "px;" + sFontWeight + ";");
        s.append("'>");
        s.append(getWorkPageHeading(menuItem));

        s.append("</div>");
        return s.toString();
    }

    public static string html_getTerminalMenu(menu_2_item currentMenuItem)
    {
        xjcString s = new xjcString();
        s.append("<div style='");
        s.append("float:left;margin:4px;border-radius:8px;padding:20px;background-color:rgb(220,220,220);color:rgb(50,50,50);font-weight:bold;font-size:" + GlobalGui.getFontSize(2) + "px;border: 4px rgb(200,200,200) solid;");
        s.append("'>");

        s.append(GlobalGui.html_hor_space(70));
        // s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_shop_view, currentMenuItem));

        s.append(GlobalGui.html_hor_space(70));
        // s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_consumer_view, currentMenuItem));

        // s.append(GlobalGui.html_hor_space(70));
        s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_coupon_view, currentMenuItem));

        // s.append(GlobalGui.html_hor_space(70));
        s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_show_consumer_coupon, currentMenuItem));

        s.append(html_getMenuItem_level_2(menu_2_item.admin_single_consumer_coupon, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_set_consumer_coupon_subscription, currentMenuItem));
        s.append(GlobalGui.html_get_newline());
        s.append(html_getMenuItem_level_2(menu_2_item.admin_get_consumer_progress, currentMenuItem));
        
        s.append(html_getMenuItem_level_2(menu_2_item.admin_consumer_interests, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_veriphone_pilot, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_pos_pilot, currentMenuItem));


        s.append(GlobalGui.html_get_newline());
        s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.terminal_ping_din_fordel_string, currentMenuItem));
        s.append(GlobalGui.html_hor_space(70));
        s.append(html_getMenuItem_level_2(menu_2_item.terminal_ping_din_fordel_string_in_string_out, currentMenuItem));

        s.append(GlobalGui.html_get_newline());

        // s.append(GlobalGui.html_getMenuItem_level_2(menu_2_item.terminal_get_shop_consumer_token_coupons, currentMenuItem));

        // s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.point_interface_form, currentMenuItem));

        s.append(GlobalGui.html_get_newline());



        /* s.append(GlobalGui.html_getMenuItem_level_2(menu_2_item.terminal_ping_din_fordel_int, currentMenuItem));
        s.append(GlobalGui.html_hor_space(70));

        s.append(GlobalGui.html_getMenuItem_level_2(menu_2_item.terminal_ping_din_fordel_bool, currentMenuItem));
        s.append(GlobalGui.html_hor_space(70)); */


        s.append("</div>");
        return s.toString();
    }

    public static string getCurrent_level_2_menu(menu_1_item topMenu, menu_2_item currentMenu2)
    {
        if (topMenu == menu_1_item.admin_general) return html_getAdminMenu(currentMenu2);
        else if (topMenu == menu_1_item.admin_terminal) return html_getTerminalMenu(currentMenu2);
        else if (topMenu == menu_1_item.admin_shop) return html_getShopMenu(currentMenu2);
        return "??? Menu2 ...";
    }

    // Mountain BYTE Menypunkter i work page
    public static string html_getAdminMenu(menu_2_item currentMenuItem)
    {
        xjcString s = new xjcString();
        s.append("<div style='");
        s.append("float:left;margin:4px;border-radius:8px;padding:20px;background-color:rgb(220,220,220);color:rgb(50,50,50);font-weight:bold;font-size:" + GlobalGui.getFontSize(2) + "px;border: 4px rgb(200,200,200) solid;");
        s.append("'>");

        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_login_email, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_login_facebook, currentMenuItem));
        // s.append(html_getMenuItem_level_2(menu_2_item.admin_user_login_phone, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_coupon_view, currentMenuItem));

        s.append(GlobalGui.html_get_newline());


        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_create_consumer_from_facebook, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_create_consumer_from_email, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_verify_consumer_from_pincode, currentMenuItem));

        s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_consumer_get_unique_link_from_email, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_consumer_email_set_new_password, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_consumer_get_unique_link_from_phone, currentMenuItem));
        
        s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_demo_card, currentMenuItem));



        s.append(GlobalGui.html_get_newline());
        s.append(GlobalGui.html_get_newline());

        // 16 mai s.append(html_getMenuItem_level_2(menu_2_item.admin_user_create, currentMenuItem));
        // 16 mai s.append(html_getMenuItem_level_2(menu_2_item.admin_user_view, currentMenuItem));

        // s.append(GlobalGui.html_hor_space(30));
        s.append(GlobalGui.html_get_newline());

        // 16 mai s.append(html_getMenuItem_level_2(menu_2_item.admin_chain_create, currentMenuItem));

        // s.append(GlobalGui.html_hor_space(30));
        s.append(GlobalGui.html_get_newline());

        // 16 mai s.append(html_getMenuItem_level_2(menu_2_item.admin_shop_create, currentMenuItem));
        // 16 mai s.append(html_getMenuItem_level_2(menu_2_item.admin_user_shop_connect, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_shop_view, currentMenuItem));

        s.append(GlobalGui.html_get_newline());
        s.append(html_getMenuItem_level_2(menu_2_item.admin_consumer_view, currentMenuItem));
        s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_coupon_view, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_coupon_create, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_coupon_approve, currentMenuItem));

        s.append(GlobalGui.html_get_newline());

        s.append(html_getMenuItem_level_2(menu_2_item.admin_show_consumer_coupon, currentMenuItem));

        s.append(html_getMenuItem_level_2(menu_2_item.admin_single_consumer_coupon, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_set_consumer_coupon_subscription, currentMenuItem));
        s.append(GlobalGui.html_get_newline());
        s.append(html_getMenuItem_level_2(menu_2_item.admin_get_consumer_progress, currentMenuItem));

        
        s.append(html_getMenuItem_level_2(menu_2_item.admin_consumer_interests, currentMenuItem));

        s.append(html_getMenuItem_level_2(menu_2_item.admin_veriphone_pilot, currentMenuItem));
        s.append(html_getMenuItem_level_2(menu_2_item.admin_pos_pilot, currentMenuItem));


        s.append(html_getMenuItem_level_2(menu_2_item.admin_user_show_webservices, currentMenuItem));



        s.append("</div>");
        return s.toString();
    }

    public static string html_getShopMenu(menu_2_item currentMenuItem)
    {
        xjcString s = new xjcString();
        s.append("<div style='");
        s.append("float:left;margin:4px;border-radius:8px;padding:20px;background-color:rgb(220,220,220);color:rgb(50,50,50);font-weight:bold;font-size:" + GlobalGui.getFontSize(2) + "px;border: 4px rgb(200,200,200) solid;");
        s.append("'>");
        s.append(GlobalGui.html_getText("Ingen elementer her", 3));
        /*s.append(GlobalGui.html_getMenuItem_level_2(menu_2_item.shop_coupon_view, currentMenuItem));
        s.append(GlobalGui.html_getMenuItem_level_2(menu_2_item.shop_coupon_create, currentMenuItem));
        s.append(GlobalGui.html_getMenuItem_level_2(menu_2_item.shop_admin_details, currentMenuItem));
         */
        s.append("</div>");
        return s.toString();
    }

    public static string getWorkPageHeading(menu_2_item menuItem)
    {
        if (menuItem == menu_2_item.admin_user_login_email) return "Login epost";
        else if (menuItem == menu_2_item.admin_user_login_facebook) return "Login Facebook";
        else if (menuItem == menu_2_item.admin_user_login_phone) return "Login Mobilnr (Vent med denne ...)";
        else if (menuItem == menu_2_item.admin_user_coupon_view) return "Vis kuponger";
        else if (menuItem == menu_2_item.admin_user_demo_card) return "Registrere kort";
        
        else if (menuItem == menu_2_item.admin_user_create_consumer_from_facebook) return "Lag bruker fra Facebook-ID";
        else if (menuItem == menu_2_item.admin_user_create_consumer_from_email) return "Lag bruker fra eMail";
        else if (menuItem == menu_2_item.admin_user_verify_consumer_from_pincode) return "Verifiser bruker fra Pincode";

        else if (menuItem == menu_2_item.admin_user_consumer_get_unique_link_from_email) return "Hent unik link fra ePost";
        else if (menuItem == menu_2_item.admin_user_consumer_email_set_new_password) return "Sett nytt passord";
        else if (menuItem == menu_2_item.admin_user_consumer_get_unique_link_from_phone) return "Hent unik link fra Mobil";


        else if (menuItem == menu_2_item.admin_chain_create) return "Opprette kjede";
        
        else if (menuItem == menu_2_item.admin_coupon_approve) return "Godkjenne kupong";
        else if (menuItem == menu_2_item.admin_coupon_create) return "Lage ny kupong";
        else if (menuItem == menu_2_item.admin_coupon_view) return "Vis kupong";
        else if (menuItem == menu_2_item.admin_show_consumer_coupon) return "Vis ALLE forbruker kupong-abonnementer";

        else if (menuItem == menu_2_item.admin_single_consumer_coupon) return "Klikk på en bruker og vis kuponger";
        else if (menuItem == menu_2_item.admin_set_consumer_coupon_subscription) return "Klikk her for å sette abonnement status forbruker/kupong";
        else if (menuItem == menu_2_item.admin_get_consumer_progress) return "Vis ForbrukerProgress";
        else if (menuItem == menu_2_item.admin_consumer_interests) return "DIVERSE TEST-KALL";
        else if (menuItem == menu_2_item.admin_veriphone_pilot) return "Verifone pilot";
        else if (menuItem == menu_2_item.admin_pos_pilot) return "POS pilot";
        else if (menuItem == menu_2_item.admin_consumer_last_webservice_created) return "Siste webservice som er laget";
        
        else if (menuItem == menu_2_item.admin_create_consumer_coupon) return "Koble et forbrukerkort til en kupong";
        else if (menuItem == menu_2_item.admin_shop_create) return "Opprett butikk";
        else if (menuItem == menu_2_item.admin_shop_view) return "Vis butikker";
        else if (menuItem == menu_2_item.admin_consumer_view) return "Vis forbrukere";
        else if (menuItem == menu_2_item.admin_user_create) return "Opprett bruker";
        else if (menuItem == menu_2_item.admin_user_shop_connect) return "Koble bruker til butikk";
        else if (menuItem == menu_2_item.admin_user_view) return "Vis brukere";
        else if (menuItem == menu_2_item.admin_x_set_google_font) return "#";
        else if (menuItem == menu_2_item.empty) return "Velg fra menyen";
        else if (menuItem == menu_2_item.shop_admin_details) return "Min side";
        else if (menuItem == menu_2_item.shop_coupon_create) return "Butikk lage kupong";
        else if (menuItem == menu_2_item.shop_coupon_view) return "Vis kuponger";
        else if (menuItem == menu_2_item.admin_user_show_webservices) return "Vis de siste webservice kallene";
        
        else if (menuItem == menu_2_item.terminal_get_shop_consumer_token_coupons) return "Simuler kupong-oppslag fra <i>SalesConnector</i> mot <i>Din Fordel</i>";

        else if (menuItem == menu_2_item.point_interface_form) return "Point web_services";

        else if (menuItem == menu_2_item.terminal_ping_din_fordel_string) return "string <b>PingDinFordel_1()</b>";
        else if (menuItem == menu_2_item.terminal_ping_din_fordel_string_in_string_out) return "string <b>PingDinFordel_2(</b>string sMessage<b>)</b>";
        // else if (menuItem == menu_2_item.terminal_ping_din_fordel_int) return "int  <b>DinFordel_3(</b>int iNumber<b>)</b>";
        // else if (menuItem == menu_2_item.terminal_ping_din_fordel_bool) return "bool <b>DinFordel_4(</b>bool bStatus<b>)</b>";


        else return "???";
    }

    public static string html_getWorkPageNoWidth(menu_2_item menuItem, string sText)
    {
        xjcString s = new xjcString();

        s.append("<div id=work_page_heading style='clear:both;margin-top:30px;margin-left:10px;padding:0px;border-radius:8px;'>");
        s.append(GlobalGui.html_getText("<b>" + getWorkPageHeading(menuItem) + "</b>", 1));
        s.append("</div>");

        s.append(GlobalGui.html_get_newline());

        s.append("<div style='");
        s.append("float:left;margin:4px;border-radius:8px;height:600px;overflow:auto;padding:20px;background-color:rgb(250,250,250);color:rgb(50,50,50);font-size:" + GlobalGui.getFontSize(3) + "px;border: 4px rgb(230,230,230) solid;");
        s.append("'>");
        s.append("<div id=message_line>");
        s.append("</div>");
        s.append(sText);

        s.append("</div>");
        return s.toString();
    }


    public static string html_getDemoWorkPage(menu_2_item menuItem, string sText)
    {
        xjcString s = new xjcString();

        s.append("<div id=work_page_heading style='clear:both;margin-top:30px;margin-left:10px;padding:0px;border-radius:8px;'>");
        s.append(GlobalGui.html_getText("<b>" + getWorkPageHeading(menuItem) + "</b>", 1));
        s.append("</div>");

        s.append(GlobalGui.html_get_newline());

        s.append("<div class=demo_work_page >");
        s.append("<div id=message_line>");
        s.append("</div>");
        s.append(sText);

        s.append("</div>");
        return s.toString();
    }



    public static string html_getWorkPage(menu_2_item menuItem, string sText)
    {
        xjcString s = new xjcString();

        s.append("<div id=work_page_heading style='clear:both;margin-top:30px;margin-left:10px;padding:0px;border-radius:8px;'>");
        s.append(GlobalGui.html_getText("<b>" + getWorkPageHeading(menuItem) + "</b>", 1));
        s.append("</div>");

        s.append(GlobalGui.html_get_newline());

        s.append("<div class=work_page >");
        s.append("<div id=message_line>");
        s.append("</div>");
        s.append(sText);

        s.append("</div>");
        return s.toString();
    }


    public static string html_getWorkPage_no_heading(string sText)
    {
        xjcString s = new xjcString();

        s.append("<div class=work_page >");
        s.append("<div id=message_line>");
        s.append("</div>");
        s.append(sText);

        s.append("</div>");
        return s.toString();
    }


    
    public static string html_getWorkPageScroll(menu_2_item menuItem, string sText)
    {
        xjcString s = new xjcString();

        s.append("<div id=work_page_heading style='clear:both;margin-top:30px;margin-left:10px;padding:0px;border-radius:8px;'>");
        s.append(GlobalGui.html_getText("<b>" + getWorkPageHeading(menuItem) + "</b>", 1));
        s.append("</div>");

        s.append(GlobalGui.html_get_newline());

        s.append("<div class=work_page >");
        s.append("<div id=message_line>");
        s.append("</div>");
        s.append(sText);

        s.append("</div>");
        return s.toString();
    }


    public string transformXmltextToDiv(string sXml)
    {
        sXml = sXml.Replace("<", "&#x003c;");
        sXml = sXml.Replace(">", "&#62;");
        return sXml;
    }



}

