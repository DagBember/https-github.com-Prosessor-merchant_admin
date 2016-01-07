using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


public enum STANDARD_REPORT_TYPE { A_COUNT_MEMBERS, B_BASKET_SUM, C_BASKET_AVERAGE, D_BASKET_COUNT, E_BASKET_AVERAGE_SUM }


public class CHAIN_REPORT : SHOP_BASE
{

    public static string getContainerId(backoffice.admin_shop shop)
    {
        return "shop_container_" + shop.iId.ToString();
    }

    public static string A_get_minimized_dialog(backoffice.admin_shop shop)
    {
        return
            HTML_TOOLBOX.infobox_TWITTER_clickable(
            "", "shop_report_show_shop('" + shop.iId.ToString() + "')",
            shop.sName,
            "Klikk for å se detaljer",
            14, 200, 50, 10, 10, 10, 10, "cursor:pointer;font-weight:bold;");
    }

    public static string B_get_maximized_dialog(Global global, string sShopId)
    {
        backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);


        StringBuilder s = new StringBuilder();

        StringBuilder sLog = new StringBuilder();

        List<backoffice.admin_ws_log> wsList = global.www_backoffice().get_last_rows_from_log(5, shop.sMerchantId);

        sLog.Append("<div style='background-color:rgb(255,255,255);border: 2px rgb(200,180,180) solid;border-radius:5px;' >");
        sLog.Append("<table>");
        sLog.Append("<tr>");

        sLog.Append(HTML_TOOLBOX.td_START_header(""));
        sLog.Append("ID");
        sLog.Append(HTML_TOOLBOX.td_END(""));

        sLog.Append(HTML_TOOLBOX.td_START_header(""));
        sLog.Append("Tidspunkt");
        sLog.Append(HTML_TOOLBOX.td_END(""));
        sLog.Append("</tr>");

        bool bExists = false;
        foreach (backoffice.admin_ws_log ws in wsList)
        {
            bExists = true;
            sLog.Append("<tr>");
            sLog.Append(HTML_TOOLBOX.td_START_cell(""));
            sLog.Append(ws.iId.ToString());
            sLog.Append(HTML_TOOLBOX.td_END(""));

            sLog.Append(HTML_TOOLBOX.td_START_cell_right_aligned(""));
            sLog.Append(ws.sTimestamp);
            sLog.Append(HTML_TOOLBOX.td_END(""));
            sLog.Append("</tr>");
        }
        sLog.Append("</table>");
        sLog.Append("</div>");

        string sLast_10_log = "Siste kall fra Verifone-terminal til GetCustomerData<br>";
        if (!bExists)
        {
            sLog = new StringBuilder("<div>Terminalen har ikke kommunisert med Bember</div>");
            sLast_10_log = "";
        }




        List<backoffice.admin_basket> basketList = global.www_backoffice().get_last_baskets(10, sShopId);

        StringBuilder sBaskets = new StringBuilder();
        sBaskets.Append("<div style='background-color:rgb(255,255,255);border: 2px rgb(200,180,180) solid;border-radius:5px;' >");
        sBaskets.Append("<table>");
        sBaskets.Append("<tr>");
        
        sBaskets.Append(HTML_TOOLBOX.td_START_header(""));
        sBaskets.Append("Bongnr");
        sBaskets.Append(HTML_TOOLBOX.td_END(""));
        
        sBaskets.Append(HTML_TOOLBOX.td_START_header(""));
        sBaskets.Append("Totalsum");
        sBaskets.Append(HTML_TOOLBOX.td_END(""));
        
        sBaskets.Append(HTML_TOOLBOX.td_START_header(""));
        sBaskets.Append("Totalrabatt");
        sBaskets.Append(HTML_TOOLBOX.td_END(""));
        
        sBaskets.Append(HTML_TOOLBOX.td_START_header(""));
        sBaskets.Append("Godkjent");
        sBaskets.Append(HTML_TOOLBOX.td_END(""));
        
        sBaskets.Append(HTML_TOOLBOX.td_START_header(""));
        sBaskets.Append("Tidspunkt");
        sBaskets.Append(HTML_TOOLBOX.td_END(""));
        sBaskets.Append("</tr>");

        bExists = false;
        foreach (backoffice.admin_basket basket in basketList)
        {
            bExists = true;
            sBaskets.Append("<tr>");
            sBaskets.Append(HTML_TOOLBOX.td_START_cell(""));
            sBaskets.Append(basket.sBasketId);
            sBaskets.Append(HTML_TOOLBOX.td_END(""));

            sBaskets.Append(HTML_TOOLBOX.td_START_cell_right_aligned(""));
            sBaskets.Append(basket.sTotalSum);
            sBaskets.Append(HTML_TOOLBOX.td_END(""));

            sBaskets.Append(HTML_TOOLBOX.td_START_cell_right_aligned(""));
            sBaskets.Append(basket.sTotalDiscount);
            sBaskets.Append(HTML_TOOLBOX.td_END(""));

            sBaskets.Append(HTML_TOOLBOX.td_START_cell(""));
            sBaskets.Append(basket.sConfirmedByShop);
            sBaskets.Append(HTML_TOOLBOX.td_END(""));

            sBaskets.Append(HTML_TOOLBOX.td_START_cell(""));
            sBaskets.Append(basket.sTimestamp);
            sBaskets.Append(HTML_TOOLBOX.td_END(""));
            sBaskets.Append("</tr>");
        }
        sBaskets.Append("</table>");
        sBaskets.Append("</div>");

        string sLast_10 = "De 10 siste bongene ...<br>";
        if (!bExists)
        {
            sBaskets = new StringBuilder("<div>Butikken har ingen bonger</div>");
            sLast_10 = "";
        }

        s.Append(HTML_TOOLBOX.infobox_TWITTER_fixed_width_var_height("",
            shop.sParentName +
            "<br>" + shop.sName +
            "<br><br>" +
            sLast_10_log +
            sLog.ToString() +
            "<br><br>" + 
            sLast_10 + 
            sBaskets.ToString() + 
            HTML_TOOLBOX.newline() +
            HTML_TOOLBOX.button_GOOGLE("Lukk vindu", 10, 4, 4, 4, 4, "shop_report_close_shop('" + shop.iId.ToString() + "')"), 12, 400, 10, 10, 10, 10, ""));

        return s.ToString();
    }

    public static class MERCHANT_REPORT
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
            string sJavascriptFunction = "shop_report_merchant_id_click('" + shop.iId.ToString() + "')";

            sb.Append(HTML_TOOLBOX.get_text_input_minimized(sLabel, sOldValue, sJavascriptFunction));

            if (bIncludeContainerWrap)
                sb.Append("</div>");

            return sb.ToString();
        }

        public static string B_get_maximized_dialog(Global global, string sShopId)
        {
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sTextInputId = CHAIN_REPORT.MERCHANT_REPORT.getContainerId(shop) + "_text";
            string sLabel = "BAX-ID";
            string sOldValue = shop.sMerchantId;
            string sJavascriptFunction_on_save = "shop_report_merchant_id_save('" + shop.iId.ToString() + "','" + sTextInputId + "') ";
            string sJavascriptFunction_on_cancel = "shop_report_merchant_id_cancel('" + shop.iId.ToString() + "') ";

            StringBuilder sb = new StringBuilder();

            sb.Append(HTML_TOOLBOX.get_text_input_maximized(sTextInputId, sLabel, sOldValue, sJavascriptFunction_on_cancel, sJavascriptFunction_on_save));
            return sb.ToString();
        }

    }


    public static class ENROLLMENT_REPORT
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
            string sJavascriptFunction = "shop_report_enrollment_click('" + shop.iId.ToString() + "')";

            sb.Append(HTML_TOOLBOX.get_checkbox_input_minimized(sLabel, bOldValue, sJavascriptFunction));

            if (bIncludeContainerWrap)
                sb.Append("</div>");

            return sb.ToString();
        }

        public static string B_get_maximized_dialog(Global global, string sShopId)
        {
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sCheckboxId = CHAIN_REPORT.ENROLLMENT_REPORT.getContainerId(shop) + "_check";
            string sLabel = "Aksepter enrollment i terminalen";
            bool bOldValue = shop.bAcceptTerminalEnrollment;
            string sJavascriptFunction_on_save = "shop_report_enrollment_save('" + shop.iId.ToString() + "','" + sCheckboxId + "') ";
            string sJavascriptFunction_on_cancel = "shop_report_enrollment_cancel('" + shop.iId.ToString() + "') ";

            StringBuilder sb = new StringBuilder();

            sb.Append(HTML_TOOLBOX.get_checkbox_input_maximized(sCheckboxId, sLabel, bOldValue, sJavascriptFunction_on_cancel, sJavascriptFunction_on_save));
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
            s.Append("<div id=" + CHAIN_REPORT.getContainerId(shop) + " style='float:left;' >");
            s.Append(CHAIN_REPORT.A_get_minimized_dialog(shop));
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
            s.Append("<div id=" + CHAIN_REPORT.getContainerId(shop) + " style='float:left;' >");
            s.Append(CHAIN_REPORT.A_get_minimized_dialog(shop));
            s.Append("</div>");
        }
        s.Append(HTML_TOOLBOX.div_END());

        return s.ToString();
    }


    public static string get_text_cell(string sText)
    {
        StringBuilder s = new StringBuilder();
        s.Append("<td class=bember_table_text_cell >" + sText + "</td>");
        return s.ToString();
    }

    public static string get_number_cell(decimal dNumber)
    {
        long lNumber = (long)dNumber;
        StringBuilder s = new StringBuilder();
        s.Append("<td class=bember_table_number_cell >" + getThousandDots(lNumber) + "</td>");
        return s.ToString();
    }


    public static string get_decimal_2_cell(decimal dNumber)
    {        
        StringBuilder s = new StringBuilder();
        string sSpecifier = "#.##";
        s.Append("<td class=bember_table_number_cell >" + dNumber.ToString(sSpecifier) + "</td>");
        return s.ToString();
    }


    public static string get_sum_decimal_2_cell(decimal dNumber)
    {
        StringBuilder s = new StringBuilder();
        string sSpecifier = "#.##";
        s.Append("<td class=bember_table_sum_number >" + dNumber.ToString(sSpecifier) + "</td>");
        return s.ToString();
    }

    public static string get_sum_number_cell(decimal dNumber)
    {
        long lNumber = (long)dNumber;
        StringBuilder s = new StringBuilder();
        s.Append("<td class=bember_table_sum_number >" + getThousandDots(lNumber) + "</td>");
        return s.ToString();
    }



    public static string get_sum_text_cell(string sText)
    {
        StringBuilder s = new StringBuilder();
        s.Append("<td class=bember_table_sum_text >" + sText + "</td>");
        return s.ToString();
    }



    public static string get_shop_line(string sShopName, int iNewMembersInPeriod, decimal dShopSum, int iNofBaskets, int iMemberBaskets, int iNotMemberBaskets)
    {
        StringBuilder s = new StringBuilder();
        s.Append("<tr>");
        s.Append(get_text_cell(sShopName));
        s.Append(get_number_cell((decimal)iNewMembersInPeriod));
        s.Append(get_number_cell(dShopSum));
        s.Append(get_number_cell(iNofBaskets));
        s.Append(get_number_cell(iMemberBaskets));
        s.Append(get_number_cell(iNotMemberBaskets));
        dShopSum = 0;
        s.Append("</tr>");
        return s.ToString();
    }

    public static int get_new_members_in_period(List<backoffice.chain_shop_standard_report_item> shopList, int iShopId)
    {
        foreach (backoffice.chain_shop_standard_report_item shop in shopList)
        {
            if (shop.iShopId == iShopId)
            {
                return shop.iNewMembersInPeriod;
            }
        }
        return 0;
    }

    public static int get_sum_new_members_in_chain(List<backoffice.chain_shop_standard_report_item> shopList)
    {
        int iNewMembers = 0;
        foreach (backoffice.chain_shop_standard_report_item shop in shopList)
        {
            iNewMembers += shop.iNewMembersInPeriod;
        }
        return iNewMembers;
    }

    public static string getCampaign(Global global)
    {
        StringBuilder sb = new StringBuilder();

        List<backoffice.campaign_consumer> consumerList = global.www_backoffice().get_campaign_week_36_consumers();

        sb.Append("<div style='height:20px;padding:20px;'>");
        sb.Append("Alle medlemmer tilogmed: " + DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString());
        sb.Append("</div>");

        consumerList.Count.ToString();
        sb.Append("<div style='height:20px;padding:20px;'>");
        sb.Append("Antall: " + consumerList.Count.ToString());
        sb.Append("</div>");


        sb.Append("<div>");
        sb.Append("<table border=1 cellpadding=5>");


        sb.Append("<tr>");

        sb.Append("<td>"); sb.Append("Telefon"); sb.Append("</td>");
        // sb.Append("<td>"); sb.Append("Butikk"); sb.Append("</td>");
        sb.Append("<td>"); sb.Append("Registrerte seg"); sb.Append("</td>");

        sb.Append("</tr>");


        foreach (backoffice.campaign_consumer camCons in consumerList)
        {

            sb.Append("<tr>");

            sb.Append("<td>"); sb.Append(camCons.sPhone); sb.Append("</td>");
            // sb.Append("<td>"); sb.Append(camCons.EnrolledByShop); sb.Append("</td>");
            sb.Append("<td>"); sb.Append(camCons.memberSince); sb.Append("</td>");

            sb.Append("</tr>");
        }
        sb.Append("</table>");
        sb.Append("</div>");

        return sb.ToString();
    }

    public static string header_td_cell(string sHeading, int iColspan = 1)
    {
        return "<td colspan=" + iColspan + "><div class=bember_table_header>" + sHeading + "</div></td>";
    }

    public static string getChainInvoice(Global global)
    {
        StringBuilder s = new StringBuilder();

        /*
            select s.id, s.name,count(*) members_accepted_id_period 
            from consumer c, shop s 
            where 
            s.id = c.enrolled_by_shop_id and 
            c.accepted_membership_at > '2015-06-01 00:00'
            and 
            s.parent_shop=24
            and
            c.accepted_membership_at < '2015-07-01 00:00'
            group by enrolled_by_shop_id, s.name,s.id  
            order by s.name
        */


        // SQL Here
        string sChainId = "24";
        DateTime startDate = new DateTime(2015,1,1);
        DateTime endDate = new DateTime(2099,10,1);

        List<backoffice.chain_shop_standard_report_item> chainShopTotal = global.www_backoffice().get_chain_shop_total_list(sChainId, startDate, endDate);

        List<backoffice.chain_shop_invoice> chainInvoiceList  = global.www_backoffice().get_chain_shop_invoice_list(sChainId,startDate,endDate);

        s.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 5, 5, "cccccwidth:600px;cccccccheight:200px;"));
        s.Append(HTML_TOOLBOX.newline());
        s.Append(HTML_TOOLBOX.infobox_TWITTER("", "Faktureringsgrunnlag LMC", 14, 300, 20, 10, 10, 10, 10, ""));
        s.Append(HTML_TOOLBOX.newline());

        int iPrevShop = -1;
        string sPrevShop = "";

        // s.Append("<div style='margin:10px;font-size:12px;margin-bottom:20px;'>Kjede = LMC");
        // s.Append("</div>");
        /*
        s.Append("<div style='margin:10px;' >StartDato&nbsp;&nbsp;<input type=text value='2015-06-01'></div>");

        s.Append("<div style='margin:10px;' >SluttDato&nbsp;&nbsp;<input type=text value='2015-07-01'></div>");
        */
        s.Append("<div>");
        s.Append("<table cellspacing=0 cellpadding=0>");
        s.Append("<tr>");
        s.Append(header_td_cell("Butikk"));
        s.Append(header_td_cell("Antall nye medlemmer"));
        s.Append(header_td_cell("Bember 1%"));
        s.Append(header_td_cell("Antall bonger totalt"));
        s.Append(header_td_cell("Antall bonger med medlemskap"));
        s.Append(header_td_cell("Antall bonger uten medlemskap"));
        s.Append("</tr>");

        decimal dShopSum = 0;
        decimal dGrandTotalShopSum = 0;
        int iTotalShopInvoices = 0;
        int iGrandTotalShopInvoices = 0;

        int iTotalShopMemberInvoices = 0;
        int iTotalShopMemberNotInvoices = 0;

        int iGrandTotalShopMemberInvoices = 0;
        int iGrandTotalShopMemberNotInvoices = 0;

        foreach (backoffice.chain_shop_invoice invoice_line in chainInvoiceList)
        {
            if (invoice_line.iShopId != iPrevShop && iPrevShop != -1)
            {
                int iNewMembersInPeriod = get_new_members_in_period(chainShopTotal,iPrevShop);
                s.Append(get_shop_line(
                            sPrevShop, 
                            iNewMembersInPeriod, 
                            dShopSum,
                            iTotalShopInvoices,
                            iTotalShopMemberInvoices,
                            iTotalShopMemberNotInvoices
                            ));
                dShopSum = 0;
                iTotalShopInvoices = 0;
                iTotalShopMemberInvoices = 0;
                iTotalShopMemberNotInvoices = 0;
            }
            
            // Start Accumulate ...
            ++iTotalShopInvoices;
            if (invoice_line.bIsMember)
            {
                ++iGrandTotalShopMemberInvoices;
                ++iTotalShopMemberInvoices;                

                // Only Bembermoney if member
                dShopSum += invoice_line.dBemberInvoiceAmount;
                dGrandTotalShopSum += invoice_line.dBemberInvoiceAmount;

                // if (invoice_line.basketDatetime > invoice_line.acceptedMembershipAt)
                // {
                    // Kjøpte noe etter at medlemskap var akseptert
                    // ++iTotalMembersRevisited;
                    // ++iGrandTotalMembersRevisited;
                    // Riktige måten å gjøre dette på er å plukke opp DENNE bongen og så se om consumer har en tidligere bong som resulterte i rabatt.
                    // Ble enige om å vente med denne statistikken ...
                // }
            }
            else
            {
                ++iGrandTotalShopMemberNotInvoices;
                ++iTotalShopMemberNotInvoices;
            }
            // Stop Accumulate ...

            ++iGrandTotalShopInvoices;

            
            sPrevShop = invoice_line.sShopName;
            iPrevShop = invoice_line.iShopId;
        }
        
        s.Append(get_shop_line(
                        sPrevShop, 
                        get_new_members_in_period(chainShopTotal, iPrevShop), 
                        dShopSum,
                        iTotalShopInvoices,
                        iTotalShopMemberInvoices,
                        iTotalShopMemberNotInvoices
                        ));

        s.Append("<tr>");
        s.Append(get_sum_text_cell("Totalt"));
        s.Append(get_sum_number_cell((decimal)get_sum_new_members_in_chain(chainShopTotal)));
        s.Append(get_sum_number_cell((decimal)dGrandTotalShopSum));
        s.Append(get_sum_number_cell((decimal)iGrandTotalShopInvoices));

        s.Append(get_sum_number_cell((decimal)iGrandTotalShopMemberInvoices));
        s.Append(get_sum_number_cell((decimal)iGrandTotalShopMemberNotInvoices));

        s.Append("</tr>");
        
        s.Append("</table>");
        s.Append("</div>");

        s.Append(HTML_TOOLBOX.div_END());

        return s.ToString();
    }



    public static bool event_catched_and_performed(xAjax ajax, Global global)
    {
        bool bRetVal = true;

        string sProcedure = ajax.getProcedure();

        // ajax.WriteVariable("menu_2_click", sProcedure);


        if (sProcedure == "level_2_report_show_all_shops()")
        {
            ajax.WriteHtml("work_page", CHAIN_REPORT.getAllShopsReport(global));
        }
        else if (sProcedure == "level_1_report()")
        {
            // MENY 2 A
            StringBuilder sb = new StringBuilder();

            sb.Append(global.chain_level_2_2(global));

            ajax.WriteHtml("menu_2", sb.ToString());
            ajax.WriteHtml("work_page", "");
        }
        else if (sProcedure == "level_1_analyze()")
        {
            // MENY 2 A
            StringBuilder sb = new StringBuilder();

            sb.Append(global.chain_level_2_4());

            ajax.WriteHtml("menu_2", sb.ToString());
            ajax.WriteHtml("work_page", "");
        }

        else if (sProcedure == "level_2_report_2()")
        {
            StringBuilder sb = new StringBuilder();

            ChainStandardReport csr = new ChainStandardReport(global, 24);

            DateTime startDate = new DateTime(2015, 7, 1);
            DateTime endDate = new DateTime(2015, 8, 30);

            sb.Append("<div style='padding:10px;overflow:hidden;clear:both;'>");
            sb.Append(csr.A_StandardReport("A Antall medlemmer", global, startDate, endDate));
            sb.Append("</div>");

            sb.Append("<div style='padding:10px;overflow:hidden;clear:both;'>");
            sb.Append(csr.getReport_B_basket_sums("B Beløp Sum", (int)DateTime.Now.Year, startDate, endDate, STANDARD_REPORT_TYPE.B_BASKET_SUM));
            sb.Append("</div>");



            sb.Append("<div style='padding:10px;overflow:hidden;clear:both;'>");
            sb.Append(csr.getReport_B_basket_sums("C Beløp Snitt", (int)DateTime.Now.Year, startDate, endDate, STANDARD_REPORT_TYPE.C_BASKET_AVERAGE));
            sb.Append("</div>");

            sb.Append("<div style='padding:10px;overflow:hidden;clear:both;'>");
            sb.Append(csr.getReport_B_basket_sums("D Antall bonger", (int)DateTime.Now.Year, startDate, endDate, STANDARD_REPORT_TYPE.D_BASKET_COUNT));
            sb.Append("</div>");

            ajax.WriteHtml("work_page", sb.ToString());
        }

        else if (sProcedure == "level_2_report_3()")
        {
            StringBuilder sb = new StringBuilder();
            /*
            sb.Append("<table>");
            for (int i = 1; i <= 12; ++i)
            {
                if (iCol == 1 || iCol == 5 || iCol == 9)
                {
                    if (iCol == 1) sb.Append("<tr>"); else { sb.Append("</tr>"); sb.Append("<tr>"); }
                    sb.Append("<tr>");
                }
                sb.Append("<td><div id=" + CHAIN_DASHBOARD_ITEM.getContainerId(i.ToString()) + " style='float:left;' >");
                sb.Append(CHAIN_DASHBOARD_ITEM.A_get_minimized_dialog(i.ToString()));
                sb.Append("</div></td>");
                ++iCol;           
            }
            sb.Append("</tr>");
            sb.Append("</table>");
            */
            string sTitle = "";

            sb.Append("<div id=google_chart_sample_01 style='border:2px rgb(200,200,200) solid';width:100%;border-radius:6px;height:115px;background-color:rgb(255,200,200);margin-top:5px;overflow:hidden;'>Sample chart</div>");
            sb.Append("<div id=google_chart_sample_02 style='border:2px rgb(200,200,200) solid';width:100%;height:115px;background-color:rgb(255,200,200);margin-top:5px;overflow:hidden;'>Sample chart</div>");
            sb.Append("<div id=google_chart_sample_03 style='width:100%;height:115px;background-color:rgb(255,200,200);margin-top:5px;overflow:hidden;'>Sample chart</div>");
            
            for (int i = 0; i <= 8; ++i)
            {
                StringBuilder s = new StringBuilder();

                if (i == 0) sTitle = "Medlemsutvikling";
                else if (i == 1) sTitle = "Medlemskonvertering";
                else if (i == 2) sTitle = "Dasboard 3";
                else if (i == 3) sTitle = "Dasboard 4";
                else if (i == 4) sTitle = "Dasboard 5";
                else if (i == 5) sTitle = "Dasboard 6";
                else if (i == 6) sTitle = "Dasboard 7";
                else if (i == 7) sTitle = "Dasboard 8";
                else if (i == 8) sTitle = "Dasboard 9";

                string sMaximized = " data-maximized='false' ";

                s.Append("<div style='font-family:arial;font-size:14px;color:rgb(150,150,150);margin-top:5px;margin-bottom:5px;'>" + sTitle + "</div>");
                s.Append("<div style='width:100%;background-color:rgb(170,170,170);height:2px;'>&nbsp;</div>");
                s.Append("<div id=chart_div_" + i.ToString() + sMaximized + " style='width:100%;height:115px;background-color:rgb(255,255,255);margin-top:5px;'>&nbsp;</div>");

                if (i == 3 || i == 6)
                {
                    sb.Append(HTML_TOOLBOX.newline());                    
                }
                // sb.Append(CHAIN_DASHBOARD_ITEM.A_get_minimized_dialog("show_google_chart('" + i.ToString() + "')","<div>" + s.ToString() + "</div>"));
                
                sb.Append(CHAIN_DASHBOARD_ITEM.A_get_minimized_dialog("outer_chart_id_" + i.ToString(),"chain_get_dashboard_data('" + i.ToString() + "',false)", "<div>" + s.ToString() + "</div>"));

                // sb.Append(CHAIN_DASHBOARD_ITEM.A_get_minimized_dialog("outer_chart_id_" + i.ToString(), "chain_get_dashboard_data('" + i.ToString() + "',false)", "<div>" + s.ToString() + "</div>"));

            }
            
            // 9 sept

            ajax.WriteHtml("work_page", sb.ToString());
        }

        else if (sProcedure == "chain_get_dashboard_data()")
        {
            int iIndex = ajax.getInt("parameter_1");
            bool bMaximized = ajax.getBool("parameter_2");

            ajax.WriteVariable("dashboard_index", iIndex.ToString());
            ajax.WriteVariable("maximized", bMaximized.ToString());

            if (iIndex == 0)
            {
                if (bMaximized == false)
                {
                    List<backoffice.dim_value> monthList = global.www_backoffice().dash_mini_month_member_old(DateTime.Now.Year.ToString(),"24");
                    ajax.WriteVariable("json_chart_data", JSON_UTILITY.get_month_data(monthList));
                }
                else
                {
                    List<backoffice.dim_value> monthList = global.www_backoffice().dash_mini_month_member_old(DateTime.Now.Year.ToString(),"24");
                    ajax.WriteVariable("json_chart_data", JSON_UTILITY.get_month_data(monthList));
                }
            }
        }

        else if (sProcedure == "level_2_analyze_1()")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Analyze 1");

            ajax.WriteHtml("work_page", sb.ToString());
        }
        else if (sProcedure == "level_2_analyze_2()")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Analyze 2");

            ajax.WriteHtml("work_page", sb.ToString());
        }

        else if (sProcedure == "level_2_analyze_3()")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Analyze 3");

            ajax.WriteHtml("work_page", sb.ToString());
        }

        else if (sProcedure == "level_2_analyze_4()")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Analyze 4");

            ajax.WriteHtml("work_page", sb.ToString());
        }


        else if (sProcedure == "level_2_shop_report_show_chain_invoice()")
        {
            ajax.WriteHtml("work_page", CHAIN_REPORT.getChainInvoice(global));
        }
        else if (sProcedure == "level_1_campaign()")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(global.chain_menu_2_3());

            ajax.WriteHtml("menu_2", sb.ToString());

            ajax.WriteHtml("work_page", "");
        }

        else if (sProcedure == "level_2_report_verifone_to_webservice()")
        {

            // List<backoffice.dim_dim_value> shop = global.www_backoffice().verifone_to_webservice(DateTime.Now);

            ajax.WriteHtml("work_page", "Not implemented");

            // 20 okt

            // ajax.WriteHtml("work_page", CHAIN_REPORT.getCampaign(global));
        }
        else if (sProcedure == "level_2_report_azure_deployment()")
        {

            // List<backoffice.dim_dim_value> shop = global.www_backoffice().verifone_to_webservice(DateTime.Now);

            StringBuilder sb = new StringBuilder();

            sb.Append("<br><a href='https://ws.bember.no/ecr/prosessor_vas.asmx'>Prod ECR</a>");
            sb.Append("<br><a href='http://test.ws.bember.no/ecr/prosessor_vas.asmx'>Test ECR</a><br>");
            sb.Append("<br><a href='https://ws.bember.no/frontend/frontend_webservice.asmx'>Prod Byte</a>");
            sb.Append("<br><a href='http://test.ws.bember.no/frontend/frontend_webservice.asmx'>Test Byte</a><br>");
            sb.Append("<br><a href='https://ws.bember.no/verifone/df_admin_webservice.asmx'>Prod Verifone</a>");
            sb.Append("<br><a href='http://test.ws.bember.no/verifone/df_admin_webservice.asmx'>Test Verifone</a>");

            ajax.WriteHtml("work_page", sb.ToString());

            // 20 okt

            // ajax.WriteHtml("work_page", CHAIN_REPORT.getCampaign(global));
        }

        
        else if (sProcedure == "shop_report_show_shop()")
        {
            string sShopId = ajax.getString("parameter_1");

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            ajax.WriteHtml(CHAIN_REPORT.getContainerId(shop), CHAIN_REPORT.B_get_maximized_dialog(global, sShopId));
        }

        else if (sProcedure == "shop_report_close_shop()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            ajax.WriteHtml(CHAIN_REPORT.getContainerId(shop), CHAIN_REPORT.A_get_minimized_dialog(shop));
        }

        else if (sProcedure == "shop_report_enrollment_click()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);
            ajax.WriteHtml(CHAIN_REPORT.ENROLLMENT_REPORT.getContainerId(shop), CHAIN_REPORT.ENROLLMENT_REPORT.B_get_maximized_dialog(global, shop.iId.ToString()));
        }

        else if (sProcedure == "shop_report_enrollment_cancel()")
        {
            string sShopId = ajax.getString("parameter_1");

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sEnrollmentContainerId = CHAIN_REPORT.ENROLLMENT_REPORT.getContainerId(shop);

            ajax.WriteHtml(
                sEnrollmentContainerId,
                CHAIN_REPORT.ENROLLMENT_REPORT.A_get_minimized_dialog(shop, false));
        }

        else if (sProcedure == "shop_report_enrollment_save()")
        {
            string sShopId = ajax.getString("parameter_1");
            bool bAcceptEnrollment = ajax.getBool("parameter_2");

            bool bOK = global.www_backoffice().update_shop_enrollment(sShopId, bAcceptEnrollment);

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sEnrollmentContainerId = CHAIN_REPORT.ENROLLMENT_REPORT.getContainerId(shop);

            ajax.WriteHtml(
                sEnrollmentContainerId,
                CHAIN_REPORT.ENROLLMENT_REPORT.A_get_minimized_dialog(shop, false));
        }

        else if (sProcedure == "shop_report_merchant_id_click()")
        {
            string sShopId = ajax.getString("parameter_1");
            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            ajax.WriteHtml(CHAIN_REPORT.MERCHANT_REPORT.getContainerId(shop), CHAIN_REPORT.MERCHANT_REPORT.B_get_maximized_dialog(global, shop.iId.ToString()));
        }

        else if (sProcedure == "shop_report_merchant_id_save()")
        {
            string sShopId = ajax.getString("parameter_1");
            string sNewBax = ajax.getString("parameter_2");

            if (!CHAIN_REPORT.isBlank(sNewBax))
            {
                bool bOK = global.www_backoffice().update_shop_merchant_id(sShopId, sNewBax);
            }

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sMerchantContainerId = CHAIN_REPORT.MERCHANT_REPORT.getContainerId(shop);

            ajax.WriteHtml(
                sMerchantContainerId,
                CHAIN_REPORT.MERCHANT_REPORT.A_get_minimized_dialog(shop, false));
        }

        else if (sProcedure == "level_2_report_4()")
        {
            int iDashboardPeriod = ajax.getInt("parameter_1");

            StringBuilder sb = new StringBuilder();

            DASHBOARD_NEW dbn = new DASHBOARD_NEW(global);

            DASHBOARD_PERIOD currentDashboardPeriod = DASHBOARD_PERIOD.THIS_MONTH;
            if (iDashboardPeriod == 1) currentDashboardPeriod = DASHBOARD_PERIOD.PREV_MONTH;
            else if (iDashboardPeriod == 2) currentDashboardPeriod = DASHBOARD_PERIOD.PREV_PREV_MONTH;

            List<backoffice.name_value_value_value_value> conversionList_6_months = global.www_backoffice().get_conversion_data(currentDashboardPeriod, "24", true); // 5 okt
            
            List<backoffice.name_value_value_value_value> conversionListCurrentPeriod = global.www_backoffice().get_conversion_data(currentDashboardPeriod, "24",false); // 5 okt

            List<backoffice.shop_top_bottom> shopConversionList = global.www_backoffice().get_conversion_data_in_shoplist(currentDashboardPeriod, "24");


            StringBuilder ss = new StringBuilder();


            sb.Append("<table style='width:100%;background-color:rgb(247,247,247);'>");
            sb.Append("<tr>");
            sb.Append("<td align=center>");
            sb.Append(dbn.get_BEMBER_CHAIN_Dashboard(currentDashboardPeriod, conversionListCurrentPeriod, shopConversionList)); 
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            List<backoffice.dim_value> monthList = global.www_backoffice().dash_maxi_month_member_old((int) DateTime.Now.Year,"24");

            ajax.WriteVariable("json_chart_global_info", JSON_UTILITY.get_chart_global_info("Dashboard for kjeden", 980, 800, dbn.cellList.Length));

            ajax.WriteVariable("json_two_dim_one_value", JSON_UTILITY.get_two_dim_one_value());

            for (int i = 0; i < dbn.cellList.Length; ++i)
            {
                // See also: Removed 19.oct.2015 to bind it together again ...
                if (dbn.cellList[i] != null)
                {
                    if (i == 1) // 3 lines 
                    {
                        ajax.WriteVariable("json_dim_and_values_" + i.ToString(), JSON_UTILITY.get_one_dim_three_values("Rekruttering","Godkjent avtale","Mobilnummer","Handler")); // 24 sept
                        ajax.WriteVariable("json_chart_data_" + i.ToString(), JSON_UTILITY.get_conversion_data_3_lines(conversionList_6_months)); // 24 sept
                        ajax.WriteVariable("json_chart_graphic_" + i.ToString(), JSON_UTILITY.get_chart_graphic("", dbn.cellList[i].w, dbn.cellList[i].h, dbn.cellList[i].chartType, "X-Akse", "Y-Akse"));
                    }
                    
                    else if (i == 3) // 4 lines 
                    {
                        
                        ajax.WriteVariable("json_dim_and_values_" + i.ToString(), JSON_UTILITY.get_one_dim_four_values("Rekruttering", "linje_1", "linje_2", "linje_3", "linje_4")); // 24 sept
                        ajax.WriteVariable("json_chart_data_" + i.ToString(), JSON_UTILITY.get_conversion_data_4_lines(conversionList_6_months)); // 24 sept
                        ajax.WriteVariable("json_chart_graphic_" + i.ToString(), JSON_UTILITY.get_chart_graphic("4 lines", dbn.cellList[i].w, dbn.cellList[i].h, dbn.cellList[i].chartType, "X-Akse", "Y-Akse"));
                    }
                    else if (i == 4)
                    {
                        ajax.WriteVariable("json_chart_data_" + i.ToString(), JSON_UTILITY.get_month_data(monthList));
                        ajax.WriteVariable("json_chart_graphic_" + i.ToString(), JSON_UTILITY.get_chart_graphic("Fin tittel", dbn.cellList[i].w, dbn.cellList[i].h, dbn.cellList[i].chartType, "X-Akse", "Y-Akse"));
                    }
                    else
                    {
                        ajax.WriteVariable("json_chart_data_" + i.ToString(), JSON_UTILITY.get_month_data(monthList));
                        ajax.WriteVariable("json_chart_graphic_" + i.ToString(), JSON_UTILITY.get_chart_graphic("Fin tittel", dbn.cellList[i].w, dbn.cellList[i].h, dbn.cellList[i].chartType, "X-Akse", "Y-Akse"));
                    }
                    
                }
            }

            // ajax.WriteHtml("one_and_only_global_page", sb.ToString());
            

            ajax.WriteHtml("work_page",sb.ToString());
        }


        else if (sProcedure == "shop_report_merchant_id_cancel()")
        {
            string sShopId = ajax.getString("parameter_1");

            backoffice.admin_shop shop = global.www_backoffice().get_shop(sShopId);

            string sMerchantContainerId = CHAIN_REPORT.MERCHANT_REPORT.getContainerId(shop);

            ajax.WriteHtml(
                sMerchantContainerId,
                CHAIN_REPORT.MERCHANT_REPORT.A_get_minimized_dialog(shop, false));
            return true;
        }
        else
            bRetVal = false;

        return bRetVal;
    }

    public static string ReverseString(string s)
    {
        char[] arr = s.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }


    public static string getThousandDots(long dNumber)
    {
        string sNumber = ((Int64)(dNumber)).ToString();
        StringBuilder s = new StringBuilder();
        
        int iDigits = 0;
        for (int i = sNumber.Length - 1; i >= 0; --i)
        {
            if (sNumber[i] == '-')
            {
                s.Append('-');
                return ReverseString(s.ToString());
            }
            else
                ++iDigits;
            s.Append(sNumber[i]);
            if ((iDigits == 3 || iDigits == 6 || iDigits == 9 || iDigits == 12)
                &&
                i > 0 && sNumber[i - 1] != '-')
                s.Append('.');
        }
        return ReverseString(s.ToString());
    }



}





