using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for ChainStandardReport
/// </summary>
public class ChainStandardReport : CHAIN_REPORT
{
    Global global = null;

    int iChainId;

    public ChainStandardReport(Global _global, int _iChainId)
	{
        global = _global;
        iChainId = _iChainId;
	}

    private string getShopLine(STANDARD_REPORT_TYPE reportType, 
                decimal dAvgBasketsPerMember,    
                decimal dBongSumShopThisYear,
                decimal dBongSumShopThisPeriod,
                decimal dBongSumShopThisYear_half_members,
                decimal dBongSumShopThisPeriod_half_members,

                decimal dBongSumShopThisYear_counter,
                decimal dBongSumShopThisPeriod_counter,
                decimal dBongSumShopThisYear_half_members_counter,
                decimal dBongSumShopThisPeriod_half_members_counter)
    {
        StringBuilder s = new StringBuilder();

        if (reportType == STANDARD_REPORT_TYPE.B_BASKET_SUM)
        {
            // Members                
            s.Append(get_number_cell(dBongSumShopThisYear));
            s.Append(get_number_cell(dBongSumShopThisPeriod));

            // Half members
            s.Append(get_number_cell(dBongSumShopThisYear_half_members));
            s.Append(get_number_cell(dBongSumShopThisPeriod_half_members));

            s.Append(get_number_cell(0));
            s.Append(get_number_cell(0));
        }
        else if (reportType == STANDARD_REPORT_TYPE.C_BASKET_AVERAGE)
        {
            // COUNTER
            // Members
            if (dBongSumShopThisYear_counter < 1) dBongSumShopThisYear_counter = 1;
            if (dBongSumShopThisPeriod_counter < 1) dBongSumShopThisPeriod_counter = 1;
            if (dBongSumShopThisYear_half_members_counter < 1) dBongSumShopThisYear_half_members_counter = 1;
            if (dBongSumShopThisPeriod_half_members_counter < 1) dBongSumShopThisPeriod_half_members_counter = 1;

            s.Append(get_number_cell(dBongSumShopThisYear / dBongSumShopThisYear_counter));
            s.Append(get_number_cell(dBongSumShopThisPeriod / dBongSumShopThisPeriod_counter));
            
            // Half members
            s.Append(get_number_cell(dBongSumShopThisYear_half_members / dBongSumShopThisYear_half_members_counter));
            s.Append(get_number_cell(dBongSumShopThisPeriod_half_members / dBongSumShopThisPeriod_half_members_counter));

            s.Append(get_number_cell(0));
            s.Append(get_number_cell(0));
        }
        else if (reportType == STANDARD_REPORT_TYPE.D_BASKET_COUNT)
        {
            // COUNTER
            s.Append(get_decimal_2_cell((decimal)dAvgBasketsPerMember));
            s.Append(get_number_cell(dBongSumShopThisYear_counter));
            s.Append(get_number_cell(dBongSumShopThisPeriod_counter));
            
            // Half members
            s.Append(get_number_cell(dBongSumShopThisYear_half_members_counter));
            s.Append(get_number_cell(dBongSumShopThisPeriod_half_members_counter));

            s.Append(get_number_cell(0));
            s.Append(get_number_cell(0));
        }

        return s.ToString();
    }



    public string getReport_B_basket_sums(String sHeading, int iYear, DateTime startDatePeriod, DateTime endDatePeriod, STANDARD_REPORT_TYPE reportType)
    {
        StringBuilder s = new StringBuilder();

        DateTime thisYearStart = new DateTime(iYear,1,1);
        DateTime thisYearEnd = new DateTime(iYear,DateTime.Now.Month,DateTime.Now.Day);


        List<backoffice.chain_shop_standard_report_item> chainShopList = global.www_backoffice().get_chain_shop_total_list(iChainId.ToString(), thisYearStart, thisYearEnd);

        if (reportType == STANDARD_REPORT_TYPE.D_BASKET_COUNT)
            chainShopList = global.www_backoffice().fill_baskets_per_member_report(iChainId.ToString(), chainShopList);


        List<backoffice.chain_shop_invoice> chainBasketList = global.www_backoffice().get_chain_shop_BASKET_list( iChainId.ToString(), thisYearStart, thisYearEnd, startDatePeriod,endDatePeriod);

        s.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 5, 5, ""));
        s.Append(HTML_TOOLBOX.newline());
        s.Append(HTML_TOOLBOX.infobox_TWITTER("", sHeading, 14, 300, 20, 10, 10, 10, 10, ""));
        s.Append(HTML_TOOLBOX.newline());

        int iPrevShop = -1;
        string sPrevShop = "";

        s.Append("<div>");
        s.Append("<table cellspacing=0 cellpadding=0>");
        s.Append("<tr>");

        s.Append(header_td_cell("&nbsp;", 1));

        if (reportType == STANDARD_REPORT_TYPE.B_BASKET_SUM)
        {        
            s.Append(header_td_cell("Nye medlemmer", 2));
            s.Append(header_td_cell("Potensielle medlemmer", 2));
            s.Append(header_td_cell("Andre kortbrukere", 2));
        }
        else if (reportType == STANDARD_REPORT_TYPE.C_BASKET_AVERAGE)
        {            
            s.Append(header_td_cell("SNITT Nye medlemmer", 2));
            s.Append(header_td_cell("SNITT Potensielle medlemmer", 2));
            s.Append(header_td_cell("SNITT Andre kortbrukere", 2));
        }
        else if (reportType == STANDARD_REPORT_TYPE.D_BASKET_COUNT)
        {
            s.Append(header_td_cell("Nye medlemmer", 3));
            s.Append(header_td_cell("Potensielle medlemmer", 2));
            s.Append(header_td_cell("Andre kortbrukere", 2));
        }
        
        s.Append("</tr>");
        
        s.Append("<tr>");
        s.Append(header_td_cell("Butikk"));

        if (reportType == STANDARD_REPORT_TYPE.B_BASKET_SUM)
        {
            s.Append(header_td_cell("Hittil i år"));
            s.Append(header_td_cell("Denne perioden"));
            s.Append(header_td_cell("Hittil i år"));
            s.Append(header_td_cell("Denne perioden"));
            s.Append(header_td_cell("Hittil i år"));
            s.Append(header_td_cell("Denne perioden"));
        }
        else if (reportType == STANDARD_REPORT_TYPE.C_BASKET_AVERAGE)
        {
            s.Append(header_td_cell("SNITT Hittil i år"));
            s.Append(header_td_cell("SNITT Denne perioden"));
            s.Append(header_td_cell("SNITT Hittil i år"));
            s.Append(header_td_cell("SNITT Denne perioden"));
            s.Append(header_td_cell("SNITT Hittil i år"));
            s.Append(header_td_cell("SNITT Denne perioden"));
        }
        else if (reportType == STANDARD_REPORT_TYPE.D_BASKET_COUNT)
        {
            s.Append(header_td_cell("Ant. kjøp pr. medlem"));
            s.Append(header_td_cell("Hittil i år"));
            s.Append(header_td_cell("Denne perioden"));
            s.Append(header_td_cell("Hittil i år"));
            s.Append(header_td_cell("Denne perioden"));
            s.Append(header_td_cell("Hittil i år"));
            s.Append(header_td_cell("Denne perioden"));
        }
        s.Append("</tr>");

        decimal dBongSumShopThisYear = 0;
        decimal dBongSumShopThisPeriod = 0;

        decimal dBongSumShopThisYear_half_members = 0;
        decimal dBongSumShopThisPeriod_half_members = 0;

        decimal dBongSumShopThisYear_counter = 0;
        decimal dBongSumShopThisPeriod_counter = 0;

        decimal dBongSumShopThisYear_half_members_counter = 0;
        decimal dBongSumShopThisPeriod_half_members_counter = 0;

        decimal dBongSumShopThisYear_GT = 0;
        decimal dBongSumShopThisPeriod_GT = 0;

        decimal dBongSumShopThisYear_half_members_GT = 0;
        decimal dBongSumShopThisPeriod_half_members_GT = 0;

        decimal dBongSumShopThisYear_counter_GT = 0;
        decimal dBongSumShopThisPeriod_counter_GT = 0;

        decimal dBongSumShopThisYear_half_members_counter_GT = 0;
        decimal dBongSumShopThisPeriod_half_members_counter_GT = 0;


        decimal dMembersPerShop_GT = 0;
        decimal dBasketsPerShop_GT = 0;

        foreach (backoffice.chain_shop_invoice invoice_line in chainBasketList)
        {
            if (invoice_line.iShopId != iPrevShop && iPrevShop != -1)
            {
                s.Append("<tr>");
                
                backoffice.chain_shop_standard_report_item shop = global.www_backoffice().getReportShopFromId(chainShopList,iPrevShop.ToString());



                s.Append(get_text_cell(sPrevShop));

                decimal dAvgBasketsPerMember = 1;
                if (shop != null)
                {
                    dMembersPerShop_GT += shop.dMembersOfShop;
                    dBasketsPerShop_GT += shop.dNofBasketsInShop;

                    dAvgBasketsPerMember = shop.dAverageNofBasketsPerMember;
                }



                s.Append(getShopLine(reportType, dAvgBasketsPerMember, dBongSumShopThisYear,
                dBongSumShopThisPeriod,
                dBongSumShopThisYear_half_members,
                dBongSumShopThisPeriod_half_members,

                dBongSumShopThisYear_counter,
                dBongSumShopThisPeriod_counter,
                dBongSumShopThisYear_half_members_counter,
                dBongSumShopThisPeriod_half_members_counter));

                s.Append("</tr>");
                dBongSumShopThisYear = 0;
                dBongSumShopThisPeriod = 0;
                dBongSumShopThisYear_half_members = 0;
                dBongSumShopThisPeriod_half_members = 0;

                dBongSumShopThisYear_counter = 0;
                dBongSumShopThisPeriod_counter = 0;
                dBongSumShopThisYear_half_members_counter = 0;
                dBongSumShopThisPeriod_half_members_counter = 0;
            }
            if (invoice_line.bInThisPeriod)
            {
                if (invoice_line.bIsMember)
                {
                    dBongSumShopThisPeriod += invoice_line.dTotalInvoiceAmount;
                    ++dBongSumShopThisPeriod_counter;
                    ++dBongSumShopThisPeriod_counter_GT;
                    
                    dBongSumShopThisPeriod_GT += invoice_line.dTotalInvoiceAmount;
                }
                else
                {
                    dBongSumShopThisPeriod_half_members += invoice_line.dTotalInvoiceAmount;
                    ++dBongSumShopThisPeriod_half_members_counter;
                    ++dBongSumShopThisPeriod_half_members_counter_GT;

                    dBongSumShopThisPeriod_half_members_GT += invoice_line.dTotalInvoiceAmount;
                }
            }
            if (invoice_line.bInThisYear)
            {
                if (invoice_line.bIsMember)
                {
                    dBongSumShopThisYear += invoice_line.dTotalInvoiceAmount;
                    ++dBongSumShopThisYear_counter;
                    ++dBongSumShopThisYear_counter_GT;

                    dBongSumShopThisYear_GT += invoice_line.dTotalInvoiceAmount;
                }
                else
                {
                    dBongSumShopThisYear_half_members += invoice_line.dTotalInvoiceAmount;
                    ++dBongSumShopThisYear_half_members_counter;
                    ++dBongSumShopThisYear_half_members_counter_GT;

                    dBongSumShopThisYear_half_members_GT += invoice_line.dTotalInvoiceAmount;
                }
            }
            
            sPrevShop = invoice_line.sShopName;
            iPrevShop = invoice_line.iShopId;
        }

        // Siste butikk
        s.Append("<tr>");
        
        
        s.Append(get_text_cell(sPrevShop));

        backoffice.chain_shop_standard_report_item shopAvg = global.www_backoffice().getReportShopFromId(chainShopList, iPrevShop.ToString());

        decimal dAvg = 1;
        if (shopAvg != null)
        {
            dAvg = shopAvg.dAverageNofBasketsPerMember;
            dMembersPerShop_GT += shopAvg.dMembersOfShop;
            dBasketsPerShop_GT += shopAvg.dNofBasketsInShop;
        }

        s.Append(getShopLine(reportType, dAvg,dBongSumShopThisYear,
        dBongSumShopThisPeriod,
        dBongSumShopThisYear_half_members,
        dBongSumShopThisPeriod_half_members,

        dBongSumShopThisYear_counter,
        dBongSumShopThisPeriod_counter,
        dBongSumShopThisYear_half_members_counter,
        dBongSumShopThisPeriod_half_members_counter));

        
        s.Append("</tr>");

        s.Append("<tr>");
        s.Append(get_sum_text_cell("Totalt"));
        
        if (reportType == STANDARD_REPORT_TYPE.B_BASKET_SUM)
        {
            s.Append(get_sum_number_cell(dBongSumShopThisYear_GT));
            s.Append(get_sum_number_cell(dBongSumShopThisPeriod_GT));
            s.Append(get_sum_number_cell(dBongSumShopThisYear_half_members_GT));
            s.Append(get_sum_number_cell(dBongSumShopThisPeriod_half_members_GT));

            s.Append(get_sum_number_cell(0));
            s.Append(get_sum_number_cell(0));

        }
        else if (reportType == STANDARD_REPORT_TYPE.C_BASKET_AVERAGE)
        {
            if (dBongSumShopThisYear_counter_GT < 1) dBongSumShopThisYear_counter_GT = 1;
            if (dBongSumShopThisPeriod_counter_GT < 1) dBongSumShopThisPeriod_counter_GT = 1;
            if (dBongSumShopThisYear_half_members_counter_GT < 1) dBongSumShopThisYear_half_members_counter_GT = 1;
            if (dBongSumShopThisPeriod_half_members_counter_GT < 1) dBongSumShopThisPeriod_half_members_counter_GT = 1;

            s.Append(get_sum_number_cell(dBongSumShopThisYear_GT / dBongSumShopThisYear_counter_GT));
            s.Append(get_sum_number_cell(dBongSumShopThisPeriod_GT / dBongSumShopThisPeriod_counter_GT));
            // Half members
            s.Append(get_sum_number_cell(dBongSumShopThisYear_half_members_GT / dBongSumShopThisYear_half_members_counter_GT));
            s.Append(get_sum_number_cell(dBongSumShopThisPeriod_half_members_GT / dBongSumShopThisPeriod_half_members_counter_GT));

            s.Append(get_sum_number_cell(0));
            s.Append(get_sum_number_cell(0));
        }
        else if (reportType == STANDARD_REPORT_TYPE.D_BASKET_COUNT)
        {
            decimal dSuperAvg = 1;
            if (dMembersPerShop_GT != 0)
                dSuperAvg = dBasketsPerShop_GT / dMembersPerShop_GT;
            
            s.Append(get_sum_decimal_2_cell(dSuperAvg));
            s.Append(get_sum_number_cell(dBongSumShopThisYear_counter_GT));
            s.Append(get_sum_number_cell(dBongSumShopThisPeriod_counter_GT));
            // Half members
            s.Append(get_sum_number_cell(dBongSumShopThisYear_half_members_counter_GT));
            s.Append(get_sum_number_cell(dBongSumShopThisPeriod_half_members_counter_GT));

            s.Append(get_sum_number_cell(0));
            s.Append(get_sum_number_cell(0));
        }
        s.Append("</tr>");

        s.Append("</table>");
        s.Append("</div>");

        s.Append(HTML_TOOLBOX.div_END());

        return s.ToString();
    }


    public string A_StandardReport(string sHeading, Global global, DateTime startDate, DateTime endDate)
    {

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

        DateTime thisYearStart = new DateTime(DateTime.Now.Year, 1, 1);
        DateTime thisYearEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        // SQL Here
        string sChainId = "24";

        List<backoffice.chain_shop_standard_report_item> shopList = global.www_backoffice().get_chain_shop_list(sChainId);
        
        // Send in shoplist to method where we fill inn detalis from member-statistics ...
        shopList = global.www_backoffice().fill_member_count_report(sChainId, shopList, thisYearStart, thisYearEnd, true, 1);
        shopList = global.www_backoffice().fill_member_count_report(sChainId, shopList, startDate, endDate, true, 2);
        shopList = global.www_backoffice().fill_member_count_report(sChainId, shopList, thisYearStart, thisYearEnd, false, 3);
        shopList = global.www_backoffice().fill_member_count_report(sChainId, shopList, startDate, endDate, false, 4);


        StringBuilder s = new StringBuilder();

        s.Append(HTML_TOOLBOX.div_START_input_container_TWITTER(10, 10, 5, 5, ""));
        s.Append(HTML_TOOLBOX.newline());
        s.Append(HTML_TOOLBOX.infobox_TWITTER("", sHeading, 14, 300, 20, 10, 10, 10, 10, ""));
        s.Append(HTML_TOOLBOX.newline());

        s.Append("<div>");
        s.Append("<table cellspacing=0 cellpadding=0>");

        s.Append("<tr>");
        s.Append(header_td_cell("&nbsp;", 1));
        s.Append(header_td_cell("Nye medlemmer", 2));
        s.Append(header_td_cell("Potensielle medlemmer", 2));
        s.Append("</tr>");

        s.Append("<tr>");
        s.Append(header_td_cell("Butikk", 1));
        s.Append(header_td_cell("Hittil i år"));
        s.Append(header_td_cell("Denne perioden"));
        s.Append(header_td_cell("Hittil i år"));
        s.Append(header_td_cell("Denne perioden"));
        s.Append("</tr>");

        long iNewMembersThisYear_TOTAL = 0;
        long iNewMembersInPeriod_TOTAL = 0;
        long iNewPotentialMembersThisYear_TOTAL = 0;
        long iNewPotentialMembersThisPeriod_TOTAL = 0;

        foreach (backoffice.chain_shop_standard_report_item chainShop in shopList)
        {
            s.Append("<tr>");
            s.Append(get_text_cell(chainShop.sShopName));
            s.Append(get_number_cell(chainShop.iNewMembersThisYear));
            s.Append(get_number_cell(chainShop.iNewMembersInPeriod));
            s.Append(get_number_cell(chainShop.iNewPotentialMembersThisYear));
            s.Append(get_number_cell(chainShop.iNewPotentialMembersThisPeriod));

            iNewMembersThisYear_TOTAL += chainShop.iNewMembersThisYear;
            iNewMembersInPeriod_TOTAL += chainShop.iNewMembersInPeriod;
            iNewPotentialMembersThisYear_TOTAL += chainShop.iNewPotentialMembersThisYear;
            iNewPotentialMembersThisPeriod_TOTAL += chainShop.iNewPotentialMembersThisPeriod;

            s.Append("</tr>");
        }

        s.Append("<tr>");
        s.Append(get_sum_text_cell("Totalt"));
        s.Append(get_sum_number_cell(iNewMembersThisYear_TOTAL));
        s.Append(get_sum_number_cell(iNewMembersInPeriod_TOTAL));
        s.Append(get_sum_number_cell(iNewPotentialMembersThisYear_TOTAL));
        s.Append(get_sum_number_cell(iNewPotentialMembersThisPeriod_TOTAL));
        s.Append("</tr>");
        
        s.Append("</table>");
        s.Append("</div>");

        s.Append(HTML_TOOLBOX.div_END());

        return s.ToString();
    }
}

