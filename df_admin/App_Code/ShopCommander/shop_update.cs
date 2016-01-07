using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


public class SHOP_UPDATE : SHOP_BASE
{

    public static string getContainerId(backoffice.admin_shop shop)
    {
        return "shop_container_" + shop.iId.ToString();
    }

    public static string A_get_minimized_dialog(backoffice.admin_shop shop)
    {
        return
            HTML_TOOLBOX.infobox_TWITTER_clickable(
            "", "shop_update_show_shop('" + shop.iId.ToString() + "')",
            shop.sName,
            "Klikk for å se detaljer",
            14, 200, 50, 10, 10, 10, 10, "cursor:pointer;");
    }

    public static string B_get_maximized_dialog(Global global, string sShopId)
    {
        backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

        StringBuilder s = new StringBuilder();

        // 21 okt

        s.Append(HTML_TOOLBOX.infobox_TWITTER_fixed_width_var_height("",
            shop.sParentName +
            "<br> " + shop.sName +
            "<br><br>Lojalitetsprosent:  " + shop.iLoyaltyPercent.ToString() + " %" +
            HTML_TOOLBOX.newline() +
            SHOP_UPDATE.ENROLLMENT_UPDATE.A_get_minimized_dialog(shop, true) +
            HTML_TOOLBOX.newline() +
            SHOP_UPDATE.MERCHANT_UPDATE.A_get_minimized_dialog(shop, true) +
            HTML_TOOLBOX.newline() +
            SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.A_get_minimized_dialog(shop, true) +
            HTML_TOOLBOX.newline() +
            HTML_TOOLBOX.button_GOOGLE("Lukk vindu", 10, 4, 4, 4, 4, "shop_update_close_shop('" + shop.iId.ToString() + "')"), 12, 400, 10, 10, 10, 10, ""));

        return s.ToString();
    }

    public static class MERCHANT_UPDATE
    {
        public static string getContainerId(backoffice.admin_shop shop)
        {
            return "merchant_container_" + shop.iId.ToString();
        }

        public static string A_get_minimized_dialog(backoffice.admin_shop shop, bool bIncludeContainerWrap)
        {
            StringBuilder sb = new StringBuilder();

            if (bIncludeContainerWrap)
                sb.Append("<div style='float:left;' id=" + getContainerId(shop) + " >");

            string sLabel = "BAX-ID";
            string sOldValue = shop.sMerchantId;
            string sJavascriptFunction = "shop_update_merchant_id_click('" + shop.iId.ToString() + "')";

            sb.Append(HTML_TOOLBOX.get_text_input_minimized(sLabel, sOldValue, sJavascriptFunction));

            if (bIncludeContainerWrap)
                sb.Append("</div>");

            return sb.ToString();
        }

        public static string B_get_maximized_dialog(Global global, string sShopId)
        {
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sTextInputId = SHOP_UPDATE.MERCHANT_UPDATE.getContainerId(shop) + "_text";
            string sLabel = "BAX-ID";
            string sOldValue = shop.sMerchantId;
            string sJavascriptFunction_on_save = "shop_update_merchant_id_save('" + shop.iId.ToString() + "','" + sTextInputId + "') ";
            string sJavascriptFunction_on_cancel = "shop_update_merchant_id_cancel('" + shop.iId.ToString() + "') ";

            StringBuilder sb = new StringBuilder();

            sb.Append(HTML_TOOLBOX.get_text_input_maximized(sTextInputId, sLabel, sOldValue, sJavascriptFunction_on_cancel, sJavascriptFunction_on_save));
            return sb.ToString();
        }

    }

    public static class ENROLLMENT_UPDATE
    {
        public static string getContainerId(backoffice.admin_shop shop)
        {
            return "enrollment_container_" + shop.iId.ToString();
        }

        public static string A_get_minimized_dialog(backoffice.admin_shop shop, bool bIncludeContainerWrap)
        {
            StringBuilder sb = new StringBuilder();

            if (bIncludeContainerWrap)
                sb.Append("<div style='float:left;' id=" + getContainerId(shop) + " >");

            string sLabel = "Aksepter enrollment i terminalen";
            bool bOldValue = shop.bAcceptTerminalEnrollment;
            string sJavascriptFunction = "shop_update_enrollment_click('" + shop.iId.ToString() + "')";

            sb.Append(HTML_TOOLBOX.get_checkbox_input_minimized(sLabel, bOldValue, sJavascriptFunction));

            if (bIncludeContainerWrap)
                sb.Append("</div>");

            return sb.ToString();
        }

        public static string B_get_maximized_dialog(Global global, string sShopId)
        {
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sCheckboxId = SHOP_UPDATE.ENROLLMENT_UPDATE.getContainerId(shop) + "_check";
            string sLabel = "Aksepter enrollment i terminalen";
            bool bOldValue = shop.bAcceptTerminalEnrollment;
            string sJavascriptFunction_on_save = "shop_update_enrollment_save('" + shop.iId.ToString() + "','" + sCheckboxId + "') ";
            string sJavascriptFunction_on_cancel = "shop_update_enrollment_cancel('" + shop.iId.ToString() + "') ";

            StringBuilder sb = new StringBuilder();

            sb.Append(HTML_TOOLBOX.get_checkbox_input_maximized(sCheckboxId, sLabel, bOldValue, sJavascriptFunction_on_cancel, sJavascriptFunction_on_save));
            return sb.ToString();
        }

    }


    public static class ENROLLMENT_SMS_UPDATE
    {
        public static string getContainerId(backoffice.admin_shop shop)
        {
            return "enrollment_sms_container_" + shop.iId.ToString();
        }

        public static string A_get_minimized_dialog(backoffice.admin_shop shop, bool bIncludeContainerWrap)
        {
            StringBuilder sb = new StringBuilder();

            if (bIncludeContainerWrap)
                sb.Append("<div style='float:left;' id=" + getContainerId(shop) + " >");

            string sLabel = "Enrollment SMS text";
            string sOldValue = shop.sSmsText;
            string sJavascriptFunction = "shop_update_enrollment_sms_click('" + shop.iId.ToString() + "')";

            sb.Append(HTML_TOOLBOX.get_text_input_minimized(sLabel, sOldValue, sJavascriptFunction));

            if (bIncludeContainerWrap)
                sb.Append("</div>");

            return sb.ToString();
        }

        public static string B_get_maximized_dialog(Global global, string sShopId)
        {
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sTextInputId = SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.getContainerId(shop) + "_text";
            string sLabel = "Enrollment SMS text";
            string sOldValue = shop.sSmsText;
            string sJavascriptFunction_on_save = "shop_update_enrollment_sms_save('" + shop.iId.ToString() + "','" + sTextInputId + "') ";
            string sJavascriptFunction_on_cancel = "shop_update_enrollment_sms_cancel('" + shop.iId.ToString() + "') ";

            StringBuilder sb = new StringBuilder();

            sb.Append(HTML_TOOLBOX.get_text_input_maximized(sTextInputId, sLabel, sOldValue, sJavascriptFunction_on_cancel, sJavascriptFunction_on_save));
            return sb.ToString();
        }

    }




    public static string getAllShopsReport(Global global)
    {
        StringBuilder s = new StringBuilder();

        s.Append(HTML_TOOLBOX.newline());

        s.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 5, 5, "cccccwidth:600px;cccccccheight:200px;"));

        bool bEnrolledAcceptedInTerminal = true;
        List<backoffice.admin_shop> shopList = global.www_backoffice().get_all_shops(global.MASTER_CHAIN_ID,bEnrolledAcceptedInTerminal);
        s.Append(HTML_TOOLBOX.newline());
        s.Append(HTML_TOOLBOX.infobox_TWITTER("", "Rekruttering i terminal", 14, 300, 20, 10, 10, 10, 10, ""));
        s.Append(HTML_TOOLBOX.newline());

        foreach (backoffice.admin_shop shop in shopList)
        {
            s.Append("<div id=" + SHOP_UPDATE.getContainerId(shop) + " style='float:left;' >");
            s.Append(SHOP_UPDATE.A_get_minimized_dialog(shop));
            s.Append("</div>");
        }
        s.Append(HTML_TOOLBOX.div_END());

        s.Append(HTML_TOOLBOX.newline());
        s.Append(HTML_TOOLBOX.newline());

        bEnrolledAcceptedInTerminal = false;
        shopList = global.www_backoffice().get_all_shops(global.MASTER_CHAIN_ID,bEnrolledAcceptedInTerminal);
        s.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 5, 5, "cccccwidth:600px;cccccccheight:200px;"));
        s.Append(HTML_TOOLBOX.newline());
        s.Append(HTML_TOOLBOX.infobox_TWITTER("", "Ingen rekruttering", 14, 300, 20, 10, 10, 10, 10, ""));
        s.Append(HTML_TOOLBOX.newline());

        foreach (backoffice.admin_shop shop in shopList)
        {
            s.Append("<div id=shop_container_" + shop.iId + " style='float:left;' >");
            s.Append(SHOP_UPDATE.A_get_minimized_dialog(shop));
            s.Append("</div>");
        }
        s.Append(HTML_TOOLBOX.div_END());

        return s.ToString();
    }

    public static bool event_catched_and_performed(xAjax ajax, Global global)
    {
        bool bRetVal = true;

        string sProcedure = ajax.getProcedure();
        
        // ajax.WriteVariable("menu_2_click", sProcedure);

        if (sProcedure == "level_1_shop_menu()")
        {            
            StringBuilder sb = new StringBuilder();

            sb.Append(global.chain_level_2_1(global));

            ajax.WriteHtml("menu_2", sb.ToString());
            // ajax.WriteHtml("work_page", sb.ToString());
        }

        else if (sProcedure == "level_2_update_show_all_shops()")
        {
            ajax.WriteHtml("work_page", SHOP_UPDATE.getAllShopsReport(global));
        }

        else if (sProcedure == "shop_update_show_shop()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            ajax.WriteHtml(SHOP_UPDATE.getContainerId(shop), SHOP_UPDATE.B_get_maximized_dialog(global, sShopId));
        }

        else if (sProcedure == "shop_update_close_shop()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            ajax.WriteHtml(SHOP_UPDATE.getContainerId(shop), SHOP_UPDATE.A_get_minimized_dialog(shop));
        }

        else if (sProcedure == "shop_update_enrollment_click()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);
            ajax.WriteHtml(SHOP_UPDATE.ENROLLMENT_UPDATE.getContainerId(shop), SHOP_UPDATE.ENROLLMENT_UPDATE.B_get_maximized_dialog(global, shop.iId.ToString()));
        }

        else if (sProcedure == "shop_update_enrollment_cancel()")
        {
            string sShopId = ajax.getString("parameter_1");

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sEnrollmentContainerId = SHOP_UPDATE.ENROLLMENT_UPDATE.getContainerId(shop);

            ajax.WriteHtml(
                sEnrollmentContainerId,
                SHOP_UPDATE.ENROLLMENT_UPDATE.A_get_minimized_dialog(shop, false));
        }

        else if (sProcedure == "shop_update_enrollment_save()")
        {
            string sShopId = ajax.getString("parameter_1");
            bool bAcceptEnrollment = ajax.getBool("parameter_2");

            bool bOK = global.www_backoffice().update_shop_enrollment(sShopId, bAcceptEnrollment);

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sEnrollmentContainerId = SHOP_UPDATE.ENROLLMENT_UPDATE.getContainerId(shop);

            ajax.WriteHtml(
                sEnrollmentContainerId,
                SHOP_UPDATE.ENROLLMENT_UPDATE.A_get_minimized_dialog(shop, false));
        }


        else if (sProcedure == "shop_update_enrollment_sms_click()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);
            ajax.WriteHtml(SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.getContainerId(shop), SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.B_get_maximized_dialog(global, shop.iId.ToString()));
        }

        else if (sProcedure == "shop_update_enrollment_sms_cancel()")
        {
            string sShopId = ajax.getString("parameter_1");

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sEnrollmentContainerId = SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.getContainerId(shop);

            ajax.WriteHtml(
                sEnrollmentContainerId,
                SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.A_get_minimized_dialog(shop, false));
        }

        else if (sProcedure == "shop_update_enrollment_sms_save()")
        {
            string sShopId = ajax.getString("parameter_1");
            string sSmsEnrollmentText = ajax.getString("parameter_2");

            bool bOK = global.www_backoffice().update_shop_enrollment_sms(sShopId, sSmsEnrollmentText);

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sEnrollmentContainerId = SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.getContainerId(shop);

            ajax.WriteHtml(
                sEnrollmentContainerId,
                SHOP_UPDATE.ENROLLMENT_SMS_UPDATE.A_get_minimized_dialog(shop, false));
        }





        else if (sProcedure == "shop_update_merchant_id_click()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            ajax.WriteHtml(SHOP_UPDATE.MERCHANT_UPDATE.getContainerId(shop), SHOP_UPDATE.MERCHANT_UPDATE.B_get_maximized_dialog(global, shop.iId.ToString()));
        }

        else if (sProcedure == "shop_update_merchant_id_save()")
        {
            string sShopId = ajax.getString("parameter_1");
            string sNewBax = ajax.getString("parameter_2");

            if (!SHOP_UPDATE.isBlank(sNewBax))
            {
                bool bOK = global.www_backoffice().update_shop_merchant_id(sShopId, sNewBax);
            }

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sMerchantContainerId = SHOP_UPDATE.MERCHANT_UPDATE.getContainerId(shop);

            ajax.WriteHtml(
                sMerchantContainerId,
                SHOP_UPDATE.MERCHANT_UPDATE.A_get_minimized_dialog(shop, false));
        }

        else if (sProcedure == "shop_update_merchant_id_cancel()")
        {
            string sShopId = ajax.getString("parameter_1");

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sMerchantContainerId = SHOP_UPDATE.MERCHANT_UPDATE.getContainerId(shop);

            ajax.WriteHtml(
                sMerchantContainerId,
                SHOP_UPDATE.MERCHANT_UPDATE.A_get_minimized_dialog(shop, false));
            return true;
        }
        else
            bRetVal = false;

        return bRetVal;
    }



}
