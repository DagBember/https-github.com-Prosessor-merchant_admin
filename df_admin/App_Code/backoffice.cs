using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Configuration;
using System.Text;
using System.Xml;

using abacolla_gui;


/// <summary>
/// Summary description for backoffice
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]



public class backoffice : webservice_common, webservice_database
{

    public enum SHOP_EVENT_TYPE { A_PHONE_ENROLLED, A_PHONE_SKIPPED, A_CONSUMER_EXISTS,A_CONSUMER_EXISTS_AND_IS_MEMBER, B_BASKET_NOT_CONFIRMED, B_BASKET_CONFIRMED, C_MEMBERSHIP_ACCEPTED, UNKNOWN }

    public backoffice()
    {
    }

    private string sSqlException = "";
    public void setSqlException(string s)
    {
        sSqlException = s;
    }


    public string getSqlException()
    {
        return sSqlException;
    }

    public DATABASE_TYPE getDatabaseType()
    {
        return DATABASE_TYPE.POSTGRES;
    }

    public string getConnectionString()
    {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["df_database"];
        if (settings != null)
        {
            return settings.ConnectionString;
        }
        return null;
        // return "Server=127.0.0.1;Port=5432;User Id=din_fordel;Password=din_fordel;Database=din_fordel_local;";
    }

    // select * from webservice_log where parameters like '%BaxNumber=526340%' order by id 
    public class admin_ws_log
    {
        public int iId;
        public string sTimestamp;
    }

    public List<admin_ws_log> get_last_rows_from_log(int iLimit, string sMerchantId)
    {
        StringBuilder sb = new StringBuilder();

        List<admin_ws_log> wsList = new List<admin_ws_log>();

        if (isBlank(sMerchantId))
            return wsList;

        string sSql = "select id,timestamp  from webservice_log where parameters like '%BaxNumber=%" + sMerchantId + "%' order by id desc limit " + iLimit.ToString();


        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                admin_ws_log ws = new admin_ws_log();

                ws.iId = (int)reader.c("id");
                ws.sTimestamp = reader.c("timestamp").ToString();
                wsList.Add(ws);
            }
        }
        catch (Exception e)
        {
            wsList = null;
        }
        finally
        {
            conn.Close();
        }
        return wsList;
    }


    // select basket_id, basket_total_sum,basket_total_discount,confirmed_by_shop,timestamp  from consumer_basket cb where shop_id = 20 order by id desc limit 10
    public class admin_basket
    {
        public int iId;
        public string sBasketId;
        public string sBasket_A;
        public string sBasket_B;
        public string sTotalSum;
        public string sTotalDiscount;
        public string sConfirmedByShop;
        public string sTimestamp; // BAX
    }

    public List<admin_basket> get_last_baskets(int iLimit, string sShopId)
    {
        StringBuilder sb = new StringBuilder();

        List<admin_basket> basketList = new List<admin_basket>();

        string sSql = "select id, basket_id, basket_total_sum,basket_total_discount,confirmed_by_shop,timestamp  from consumer_basket cb where shop_id = " + sShopId + " order by id desc limit " + iLimit.ToString();

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                admin_basket basket = new admin_basket();

                basket.iId = (int)reader.c("id");
                basket.sBasketId = reader.c("basket_id").ToString();
                basket.sTotalSum = reader.c("basket_total_sum").ToString();
                basket.sTotalDiscount = reader.c("basket_total_discount").ToString();
                basket.sConfirmedByShop = reader.c("confirmed_by_shop").ToString();
                if (basket.sConfirmedByShop.ToUpper() == "TRUE") basket.sConfirmedByShop = "Ja";
                else if (basket.sConfirmedByShop.ToUpper() == "FALSE") basket.sConfirmedByShop = "Nei";
                else basket.sConfirmedByShop = "?";

                basket.sTimestamp = reader.c("timestamp").ToString();
                basketList.Add(basket);
            }
        }
        catch (Exception e)
        {
            basketList = null;
        }
        finally
        {
            conn.Close();
        }
        return basketList;
    }

    public admin_basket get_basket(string sBasketRowId)
    {
        admin_basket basket = null;

        List<admin_basket> basketList = new List<admin_basket>();

        string sSql = "select id, basket_id, basket_a, basket_b, basket_total_sum,basket_total_discount,confirmed_by_shop,timestamp from consumer_basket where id = " + sBasketRowId;

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            if (reader.Read())
            {
                basket = new admin_basket();

                basket.iId = (int)reader.c("id");
                basket.sBasketId = reader.c("basket_id").ToString();
                basket.sBasket_A = reader.c("basket_a").ToString();
                basket.sBasket_B = reader.c("basket_b").ToString();
                basket.sTotalSum = reader.c("basket_total_sum").ToString();
                basket.sTotalDiscount = reader.c("basket_total_discount").ToString();
                basket.sConfirmedByShop = reader.c("confirmed_by_shop").ToString();
                if (basket.sConfirmedByShop.ToUpper() == "TRUE") basket.sConfirmedByShop = "Ja";
                else if (basket.sConfirmedByShop.ToUpper() == "FALSE") basket.sConfirmedByShop = "Nei";
                else basket.sConfirmedByShop = "?";
                basket.sTimestamp = reader.c("timestamp").ToString();
                
            }
        }
        catch (Exception e)
        {
            basket = null;
        }
        finally
        {
            conn.Close();
        }
        return basket;
    }


    public class chain_shop_standard_report_item
    {
        public int iShopId;
        public string sShopName;
        public int iNewMembersThisYear = 0;
        public int iNewMembersInPeriod = 0;
        public int iNewPotentialMembersThisYear = 0;
        public int iNewPotentialMembersThisPeriod = 0;
        public decimal dAverageNofBasketsPerMember = 0;

        public decimal dNofBasketsInShop = 0;
        public decimal dMembersOfShop = 0;

    }



    public class campaign_consumer
    {
        public string sPhone;
        public DateTime memberSince;
        public string EnrolledByShop;
        public int EnrolledByShopId;
    }

    public class dim_value
    {
        public string sDim;
        public string sValue;
    }

    public class dim_value_value
    {
        public string sDim;
        public string sValue_1;
        public string sValue_2;
    }


    public class shop_top_bottom
    {
        public string sName;
        public long iNoPhone;
        public long iYesPhoneNotApproved;
        public long iYesPhoneApproved;
        public decimal dLeftMobilePercent;
        public decimal dConvertedFromTotalPercent;
    }


    public class dim_dim_value
    {
        public string sDim_1;
        public string sDim_2;
        public string sValue;
    }

    public class dim_dim_value_value
    {
        public string sDim_1;
        public string sDim_2;
        public string sValue_1;
        public string sValue_2;
        public decimal dValue_1;
        public decimal dValue_2;
    }


    public class name_value_value_value_value
    {
        public string sName;
        public string sValue1;
        public string sValue2;
        public string sValue3;
        public string sValue4;
    }


    public class chain_shop_invoice
    {
        public int iShopId;
        public string sShopName;
        public bool bIsMember = false;
        public int iNewMembersInPeriod = 0;
        public decimal dBemberInvoiceAmount;
        public decimal dTotalInvoiceAmount;
        public DateTime acceptedMembershipAt;
        public DateTime basketDatetime;

        public bool bInThisYear = false;
        public bool bInThisPeriod = false;

    }


    public class admin_shop
    {
        public int iId;
        public string sParentName;
        public string sName;
        public string sMerchantId; // BAX
        public int iLoyaltyPercent = 0;
        public bool bAcceptTerminalEnrollment = false;

        public string sSmsText = "";

        public shop_event firstShopEvent = null;
        public shop_event lastAddedShopEvent = null;


        public void append_event(shop_event shopEvent)
        {
            shop_event looper = firstShopEvent;
            if (looper == null)
            {
                firstShopEvent = shopEvent;
            }
            else
            {                
                while (looper != null)
                {
                    if (looper.Next == null)
                    {
                        looper.Next = shopEvent;
                        return;
                    }
                    else if (looper == firstShopEvent && shopEvent.timestamp <= firstShopEvent.timestamp)
                    {
                        shopEvent.Next = firstShopEvent;
                        firstShopEvent = shopEvent;
                        break;
                    }
                    else if (shopEvent.timestamp <= looper.Next.timestamp)
                    {
                        shopEvent.Next = looper.Next;
                        looper.Next = shopEvent;
                        break;
                    }
                    looper = looper.Next;
                }
            }
        }

    }


    public class shop_event
    {

        public void set_accepted_membership_at(object dtValue)
        {
            string s = dtValue.ToString();
            if (s == null || s == "")
            {
                accepted_membership_at = new DateTime(2000, 1, 1);
                return;
            }

            try
            {
                int iYear = (int)((DateTime)dtValue).Year;
                if (iYear < 2001)
                    accepted_membership_at = new DateTime(2000, 1, 1);
                else
                {
                    accepted_membership_at = (DateTime)dtValue;
                }
            }
            catch (Exception e)
            {
                accepted_membership_at = new DateTime(2000, 1, 1);
            }
        }

        public DateTime accepted_membership_at = new DateTime(2000, 1, 1);
       
        public bool bAdditionalCard = false;
        public SHOP_EVENT_TYPE shopEventType = SHOP_EVENT_TYPE.UNKNOWN;
        public string sRawParameters = "";
        public DateTime timestamp = new DateTime(2000, 1, 1);
        public string sBasketRowId;
        public string sBasket_b;
        public bool bConfirmedByShop = false;
        public bool bErrorBasket = false;
        public string sPhone;
        public string sToken;
        public string sBaxId;
        public string sDescription;
        public string amount_1;
        public string amount_2;

        public shop_event higherPointer = null;

        public int xCenter = 0;
        public int yCenter = 0;

        public shop_event Next;
    }


    public List<chain_shop_standard_report_item> get_chain_shop_total_list(string sParentShopId, DateTime startDate, DateTime endDate)
    {
        StringBuilder sb = new StringBuilder();

        List<chain_shop_standard_report_item> shopList = new List<chain_shop_standard_report_item>();

        string sSql =
            "select s.id, s.name,count(*) members_accepted_in_period " +
            "from consumer c, shop s  " +
            "where  " +
            "s.id = c.enrolled_by_shop_id and  " +
            "c.accepted_membership_at >= '" + buildPostgresDateFromStringDayPrecision(startDate) + "' " +
            "and  " +
            "s.parent_shop=" + sParentShopId + " " +
            "and " +
            "c.accepted_membership_at <= '" + buildPostgresDateFromStringDayPrecision(endDate) + "' " +
            "and " +
            "c.pincode_verified='yes' " + // Just a double check
            "group by enrolled_by_shop_id, s.name,s.id   " +
            "order by s.name";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                chain_shop_standard_report_item shopTotal = new chain_shop_standard_report_item();
                shopTotal.iNewMembersInPeriod = Convert.ToInt32(reader.c("members_accepted_in_period").ToString());
                shopTotal.iShopId = (int)reader.c("id");
                shopTotal.sShopName = (string)reader.c("name");
           
     

                shopList.Add(shopTotal);
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }


    // 7 sept
    public List<chain_shop_standard_report_item> update_shops_with_member_data(string sParentShopId, DateTime startDate, DateTime endDate)
    {
        List<chain_shop_standard_report_item> shopList = new List<chain_shop_standard_report_item>();

        string sSql =
            "select s.id, s.name " +
            "from shop s  " +
            "where  " +
            "s.parent_shop=" + sParentShopId + " " +
            "order by s.name";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        return null;
    }

    public List<chain_shop_standard_report_item> get_chain_shop_list(string sParentShopId)
    {
        StringBuilder sb = new StringBuilder();

        List<chain_shop_standard_report_item> shopList = new List<chain_shop_standard_report_item>();

        string sSql =
            "select s.id, s.name " +
            "from shop s  " +
            "where  " +
            "s.parent_shop=" + sParentShopId + " " +
            "order by s.name";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                chain_shop_standard_report_item shopTotal = new chain_shop_standard_report_item();                
                shopTotal.iShopId = (int)reader.c("id");
                shopTotal.sShopName = (string)reader.c("name");
                shopList.Add(shopTotal);
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }


    public List<chain_shop_standard_report_item> fill_member_count_report(string sChainId, List<chain_shop_standard_report_item> shopList, DateTime startDate, DateTime endDate, bool bIsMember,int iCol)
    {
        StringBuilder sb = new StringBuilder();

        string sMemberLine = " ";
        if (bIsMember)
        {
            sMemberLine =
            " pincode_verified='yes' " +
            " and " +
            " c.accepted_membership_at >= '" + buildPostgresDateFromStringDayPrecision(startDate) + "' " +
            "and  " +
            "c.accepted_membership_at <= '" + buildPostgresDateFromStringDayPrecision(endDate) + "' ";
        }
        else
        {
            sMemberLine =
            " pincode_verified='no' " +
            " and " +
            " c.created_at >= '" + buildPostgresDateFromStringDayPrecision(startDate) + "' " +
            "and  " +
            "c.created_at <= '" + buildPostgresDateFromStringDayPrecision(endDate) + "' ";
        }

        string sSql =
            "select s.id shop_id, count(*) count_number " +
            "from consumer c, shop s  " +
            "where  " +
            "s.id = c.enrolled_by_shop_id " + 
            "and " + 
            sMemberLine + 
            "and  " +
            "s.parent_shop=" + sChainId + " " +
            "group by enrolled_by_shop_id, s.id ";
            
        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                int iShopId = (int)reader.c("shop_id");
                chain_shop_standard_report_item shop = getReportShopFromId(shopList, iShopId.ToString());
                if (shop != null) 
                {
                    int iCountNumber = Convert.ToInt32(reader.c("count_number").ToString());
                    if (iCol == 1) shop.iNewMembersThisYear = iCountNumber;
                    else if (iCol == 2) shop.iNewMembersInPeriod = iCountNumber;
                    else if (iCol == 3) shop.iNewPotentialMembersThisYear = iCountNumber;
                    else if (iCol == 4) shop.iNewPotentialMembersThisPeriod = iCountNumber;
                }
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }

    public List<chain_shop_standard_report_item> fill_baskets_per_member_report(string sChainId, List<chain_shop_standard_report_item> shopList)
    {
        // Antall medlemsbonger pr. butikk
        string sSql =
            "select s.id shop_id, count(*) count_number " +
            "from consumer_basket cb, consumer c, consumer_paytool cp, shop s " +
            "where " +
            "cb.confirmed_by_shop = 'yes' " +
            "and " +
            "cb.consumer_paytool_id = cp.id " +
            "and " +
            "cp.consumer_id = c.id " +
            "and " +
            "cb.shop_id = s.id " +
            "and " +
            "s.parent_shop=" + sChainId + " " +
            "and " +
            "c.pincode_verified='yes' " +
            "group by s.id ";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                int iShopId = (int)reader.c("shop_id");
                chain_shop_standard_report_item shop = getReportShopFromId(shopList, iShopId.ToString());
                if (shop != null)
                {
                    int iCountNumber = Convert.ToInt32(reader.c("count_number").ToString());
                    shop.dNofBasketsInShop = iCountNumber;                    
                    // shop.dAverageNofBasketsPerMember = iCountNumber;                    
                }
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }

        // Antall medlemmer pr. butikk
        
        sSql =
            "select s.id shop_id, count(*) count_number " +
            "from consumer  c, shop s " +
            "where  " +
            "c.enrolled_by_shop_id = s.id  " +
            "and  " +
            "c.pincode_verified='yes'  " +
            "and " +
            "s.parent_shop=" + sChainId + "  " +
            "group by s.id";
        
        conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                int iShopId = (int)reader.c("shop_id");
                chain_shop_standard_report_item shop = getReportShopFromId(shopList, iShopId.ToString());
                if (shop != null)
                {
                    int iCountNumber = Convert.ToInt32(reader.c("count_number").ToString());
                    if (iCountNumber < 1) iCountNumber = 1;
                    shop.dMembersOfShop = iCountNumber;                    
                    shop.dAverageNofBasketsPerMember = shop.dNofBasketsInShop / shop.dMembersOfShop;
                }
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }




    public List<campaign_consumer> get_campaign_week_36_consumers()
    {
        // Used for school start campaign week 36
        StringBuilder sb = new StringBuilder();

        List<campaign_consumer> consumerList = new List<campaign_consumer>();

        // buildPostgresDateFromStringDayPrecision(startDate)

        string sSql =
            "select c.phone, c.accepted_membership_at, c.enrolled_by_shop_id, s.name " +
            "from consumer c, shop s  " +
            "where  " +
            "c.pincode_verified='yes'  " +
            "and " +
            "(c.phone like '9%' or c.phone like '4%')  " +
            "and " +
            "c.accepted_membership_at > '2015-01-01' " +
            "and " +
            "c.enrolled_by_shop_id = s.id  " +
            "order by c.accepted_membership_at";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                campaign_consumer campCons = new campaign_consumer();

                campCons.sPhone = "12345678"; // campCons.sPhone = (string)reader.c("phone"); 
                campCons.memberSince = (DateTime) reader.c("accepted_membership_at");
                campCons.EnrolledByShopId = (int) reader.c("enrolled_by_shop_id");
                campCons.EnrolledByShop = (string) reader.c("name");
                consumerList.Add(campCons);
            }
        }
        catch (Exception e)
        {
            consumerList = null;
        }
        finally
        {
            conn.Close();
        }
        return consumerList;
    }


    public List<dim_value> dash_mini_month_member_old(string sYear, string sChainId)
    {
        List<dim_value> yearList = new List<dim_value>();

        string sSql = 
            "select count(*) year_value from " +
            "consumer c, shop s " + 
            "where pincode_verified='yes' and " + 
            " c.enrolled_by_shop_id=s.id and s.parent_shop=" + sChainId + " " + 
            "EXTRACT(YEAR FROM accepted_membership_at)=" + sYear;

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                dim_value month = new dim_value();

                month.sDim = sYear;
                month.sValue = reader.c("year_value").ToString();
                yearList.Add(month);
            }
        }
        catch (Exception e)
        {
            yearList = null;
        }
        finally
        {
            conn.Close();
        }
        return yearList;
    }


    public List<dim_value> dash_maxi_month_member_old(int iYear, string sChainId)
    {
        List<dim_value> monthList = new List<dim_value>();

        string sSql =
            "select EXTRACT(MONTH FROM accepted_membership_at) accepted_month, count(*) month_value " +
            "from consumer c, shop s " +
            "where " +
            "s.parent_shop=" + sChainId + " and " + 
            "s.id = c.enrolled_by_shop_id and " + 
            "pincode_verified='yes' " +
            "and " +
            "EXTRACT(YEAR FROM accepted_membership_at)=" + iYear.ToString() + " " +
            "group by EXTRACT(MONTH FROM accepted_membership_at) " +
            "order by EXTRACT(MONTH FROM accepted_membership_at) ";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                dim_value month = new dim_value();

                month.sDim = reader.c("accepted_month").ToString();
                month.sValue = reader.c("month_value").ToString();
                monthList.Add(month);
            }
        }
        catch (Exception e)
        {
            monthList = null;
        }
        finally
        {
            conn.Close();
        }
        return monthList;
    }


    public List<name_value_value_value_value> get_conversion_data(DASHBOARD_PERIOD yearMonthPeriod, string sChainId, bool bHalfYear)
    {
        List<name_value_value_value_value> monthList = new List<name_value_value_value_value>();

        DASHBOARD_MONTH yearMonth = new DASHBOARD_MONTH(yearMonthPeriod);

        string sFromMonthDate = "";
        string sBeforeMonthDate = "";

        if (yearMonthPeriod == DASHBOARD_PERIOD.THIS_MONTH || yearMonthPeriod == DASHBOARD_PERIOD.PREV_MONTH || yearMonthPeriod == DASHBOARD_PERIOD.PREV_PREV_MONTH)
        {
            if (bHalfYear)
                sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(-5);
            else
                sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(0);
            sBeforeMonthDate = yearMonth.getThisDbYearNextMonthDay1String();
        }
/*
        else if (yearMonthPeriod == DASHBOARD_PERIOD.HALF_YEAR)
        {
            sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(-5);
            sBeforeMonthDate = yearMonth.getThisDbYearNextMonthDay1String();
        }
*/
        string sSql =
            "select " + 

            "EXTRACT(YEAR FROM ua.timestamp) x_year, " + 
            "EXTRACT(MONTH FROM ua.timestamp) x_month, " + 

            "case " + 
            "when sub_action like 'ecr_new%' then 'yes' " + 
            "when sub_action like 'ecr_ref%' then 'no' " + 
            "when sub_action like 'ecr_already%' then 'already_member' " + 
            "end got_mobile, " + 

            "case " + 
            "when c.pincode_verified = 'yes' then 'yes' " + 
            "when c.pincode_verified = 'no' then 'no_not_yet' " + 
            "else  'no_phone' " + 
            "end approved_sms, " + 
            "count(*) nof_events " + 
            "from user_action ua " + 
            "left JOIN shop s ON (s.shop_external_id = substring(ua.parameters for position(';' in ua.parameters)-position('baxid=' in ua.parameters)-6  from  position('baxid=' in parameters)+6) and " +

            // "(s.id <> 20 and s.id <> 25) and " +
            "s.parent_shop=" + sChainId + ") " + 

            // "s.parent_shop=" + sChainId + ") " + 
            "left JOIN consumer c ON (c.phone = split_part(ua.parameters, ';', 2)) " +
            "where (sub_action like 'ecr_refused_enrollment' or sub_action like 'ecr_new_enrollment' or sub_action like 'ecr_already%') " + 
            "and ua.id > 4785 " + 
            "and ua.timestamp >= '" + sFromMonthDate + "' " + 
            "and " +
            "ua.timestamp < '" + sBeforeMonthDate + "' " +
            "and s.shop_external_id = substring(ua.parameters for position(';' in ua.parameters)-position('baxid=' in ua.parameters)-6  from  position('baxid=' in parameters)+6) " + 
            "group by x_month,x_year, ua.sub_action, c.pincode_verified " + 
            "order by x_month,x_year, ua.sub_action, c.pincode_verified";


        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            string sLastYearMonth = "";
            string sValue_1 = ""; // value_1 : Searching for yes - yes
            string sValue_2 = ""; // value_2 : Searching for yes - no_not_yet
            string sValue_3 = ""; // value_3 : Searching for no - whatever
            string sValue_4 = ""; // value_3 : Searching for no - already_member

            name_value_value_value_value month = null;

            while (reader.Read())
            {

                string sMonth = reader.c("x_month").ToString();
                string sThisYearMonth = reader.c("x_year").ToString() + "_" + sMonth;

                if (sThisYearMonth != sLastYearMonth)
                {
                    month = new name_value_value_value_value();
                    sValue_1 = ""; sValue_2 = ""; sValue_3 = ""; sValue_4 = "";
                }

                string sGotMobile = reader.c("got_mobile").ToString();
                string sApprovedSms = reader.c("approved_sms").ToString();

                if (sGotMobile == "yes" && sApprovedSms == "yes") sValue_1 = reader.c("nof_events").ToString();
                else if (sGotMobile == "yes" && sApprovedSms == "no_not_yet") sValue_2 = reader.c("nof_events").ToString();
                else if (sGotMobile == "no") sValue_3 = reader.c("nof_events").ToString();
                else if (sGotMobile == "already_member") sValue_4 = reader.c("nof_events").ToString();                

                if (sValue_1 != "" && sValue_2 != "" && sValue_3 != "" && sValue_4 != "")
                {
                    // Test if value_4 already member has values. These must be subtracted ...
                    month.sName = DASHBOARD_MONTH.month_text(Convert.ToInt32(sMonth));
                    month.sValue1 = sValue_1; month.sValue2 = sValue_2; month.sValue3 = sValue_3; month.sValue4 = sValue_4;
                    monthList.Add(month);
                }

                sLastYearMonth = sThisYearMonth;
            }
        }
        catch (Exception e)
        {
            monthList = null;
        }
        finally
        {
            conn.Close();
        }
        return monthList;
    }


    public List<shop_top_bottom> get_conversion_data_in_shoplist(DASHBOARD_PERIOD yearMonthPeriod, string sChainId)
    {
        List<shop_top_bottom> shopList = new List<shop_top_bottom>();

        DASHBOARD_MONTH yearMonth = new DASHBOARD_MONTH(yearMonthPeriod);

        string sFromMonthDate = "";
        string sBeforeMonthDate = "";

        sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(0);
        sBeforeMonthDate = yearMonth.getThisDbYearNextMonthDay1String();

        string sSql =
        "select s.name," +
        "case when sub_action like 'ecr_new%' then 'yes' when sub_action like 'ecr_ref%' then 'no' when sub_action like 'ecr_already%' then 'already_member' end got_mobile," +
        "case when c.pincode_verified = 'yes' then 'yes' when c.pincode_verified = 'no' then 'no_not_yet' else  'no_phone' end approved_sms," +
        "count(*) nof_events " +
        "from user_action ua left JOIN shop s ON (s.shop_external_id = substring(ua.parameters for position(';' in ua.parameters)-position('baxid=' in ua.parameters)-6  from  position('baxid=' in parameters)+6) and s.parent_shop=" + sChainId + ")" +
        "left JOIN consumer c ON (c.phone = split_part(ua.parameters, ';', 2)) " +
        "where " + 
        "(sub_action like 'ecr_refused_enrollment' or sub_action like 'ecr_new_enrollment') and " + 
        "ua.id > 4785 and ua.timestamp >= '" + sFromMonthDate + "' and ua.timestamp < '" + sBeforeMonthDate + "' " + 
        "and s.shop_external_id = substring(ua.parameters for position(';' in ua.parameters)-position('baxid=' in ua.parameters)-6  from  position('baxid=' in parameters)+6) " +
        "group by s.name,  ua.sub_action, c.pincode_verified " +
        "order by s.name,  ua.sub_action, c.pincode_verified";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            shop_top_bottom shop = null; // Shop, got_mobile, approved_sms, number_of_events

            string sPrevShopName = "";
            int iii = 0;
            while (reader.Read())
            {
                ++iii;
                try
                {
                    if (iii == 18)
                    {
                        iii = iii;
                    }
                    string sShopName = reader.c("name").ToString();

                    if (sShopName != sPrevShopName)
                    {
                        if (shop != null)
                        {
                            decimal decimal_1 = DASHBOARD_GREEN.get_percent_of_to_decimal(shop.iNoPhone + shop.iYesPhoneApproved + shop.iYesPhoneNotApproved, shop.iYesPhoneApproved + shop.iYesPhoneNotApproved, false);
                            shop.dLeftMobilePercent = decimal_1;
                            decimal decimal_2 = DASHBOARD_GREEN.get_percent_of_to_decimal(shop.iNoPhone + shop.iYesPhoneApproved + shop.iYesPhoneNotApproved, shop.iYesPhoneApproved, false);
                            shop.dConvertedFromTotalPercent = Convert.ToDecimal(decimal_2);
                            shopList.Add(shop);
                        }
                        shop = new shop_top_bottom();
                    }

                    shop.sName = sShopName;

                    string sGotMobile = reader.c("got_mobile").ToString();
                    string sApprovedSms = reader.c("approved_sms").ToString();

                    /* 1 */
                    if (sGotMobile == "yes" && sApprovedSms == "yes") shop.iYesPhoneApproved = Convert.ToInt64(reader.c("nof_events").ToString());
                    /* 2 */
                    else if (sGotMobile == "yes" && sApprovedSms == "no_not_yet") shop.iYesPhoneNotApproved = Convert.ToInt64(reader.c("nof_events").ToString());
                    /* 3 */
                    else if (sGotMobile == "no") shop.iNoPhone = Convert.ToInt64(reader.c("nof_events").ToString());

                    sPrevShopName = sShopName;
                } catch (Exception)
                {
                    int t = 0;
                }



            }
            if (shop != null)
            {
                shop.dLeftMobilePercent = DASHBOARD_GREEN.get_percent_of_to_decimal(shop.iNoPhone + shop.iYesPhoneApproved + shop.iYesPhoneNotApproved, shop.iYesPhoneApproved + shop.iYesPhoneNotApproved, false);
                shop.dConvertedFromTotalPercent = DASHBOARD_GREEN.get_percent_of_to_decimal(shop.iNoPhone + shop.iYesPhoneApproved + shop.iYesPhoneNotApproved, shop.iYesPhoneApproved, false);
                shopList.Add(shop);
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }

    public List<dim_dim_value> dash_maxi_month_member(int iYear, string sChainId)
    {
        List<dim_dim_value> monthList = new List<dim_dim_value>();

        int iLastYear = iYear - 1;

        // Default = this year and the year before, which always will be enough ...
        // 14 okt
        string sSql =
            "select EXTRACT(YEAR FROM accepted_membership_at) accepted_year, EXTRACT(MONTH FROM accepted_membership_at) accepted_month, count(*) year_month_value " +
            "from consumer c, shop s " +
            "where " +
            "s.parent_shop = " + sChainId + " " + 
            "and " +
            "c.enrolled_by_shop_id = s.id " +
            "and " +
            "pincode_verified='yes' and " +
        "(EXTRACT(YEAR FROM accepted_membership_at)=" + iLastYear + " or EXTRACT(YEAR FROM accepted_membership_at)=" + iYear.ToString() + ") " + 
        "group by EXTRACT(YEAR FROM accepted_membership_at), EXTRACT(MONTH FROM accepted_membership_at) " + 
        "order by EXTRACT(YEAR FROM accepted_membership_at), EXTRACT(MONTH FROM accepted_membership_at) ";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                dim_dim_value month = new dim_dim_value();

                month.sDim_1 = reader.c("accepted_year").ToString();
                month.sDim_2 = reader.c("accepted_month").ToString();
                month.sValue = reader.c("year_month_value").ToString();
                monthList.Add(month);
            }
        }
        catch (Exception e)
        {
            monthList = null;
        }
        finally
        {
            conn.Close();
        }
        return monthList;
    }

    public List<dim_dim_value_value> dash_get_average_member_basket(int iYear, string sChainId)
    {
        List<dim_dim_value_value> monthList = new List<dim_dim_value_value>();

        int iLastYear = iYear - 1;

        // Default = this year and the year before, which always will be enough ...
        string sSql =
            "select EXTRACT(YEAR FROM timestamp) accepted_year, EXTRACT(MONTH FROM timestamp) accepted_month, count(*) nof_baskets, sum(cb.basket_total_sum) basket_sum " +
            "from consumer_basket cb, consumer_paytool cp, consumer c, shop s " +
            "where " +
            "s.parent_shop = " + sChainId + " " +
            "and " +
            "s.id = cb.shop_id " +
            "and " +
            "cb.confirmed_by_shop='yes'  " +
            "and " +
            "cb.consumer_paytool_id=cp.id  " +
            "and " +
            "cp.consumer_id=c.id  " +
            "and " +
            "c.pincode_verified='yes' " + 
            "and " +
            "(EXTRACT(YEAR FROM cb.timestamp)=" + iLastYear + " or EXTRACT(YEAR FROM cb.timestamp)=" + iYear.ToString() + ") " +
            "group by EXTRACT(YEAR FROM cb.timestamp), EXTRACT(MONTH FROM cb.timestamp) " +
            "order by EXTRACT(YEAR FROM cb.timestamp), EXTRACT(MONTH FROM cb.timestamp) ";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                dim_dim_value_value month = new dim_dim_value_value();

                month.sDim_1 = reader.c("accepted_year").ToString();
                month.sDim_2 = reader.c("accepted_month").ToString();
                

                month.sValue_1 = reader.c("nof_baskets").ToString();

                month.dValue_1 = Convert.ToInt32(month.sValue_1);

                month.sValue_2 = reader.c("basket_sum").ToString();
                month.dValue_2 = (decimal)reader.c("basket_sum");
                monthList.Add(month);
            }
        }
        catch (Exception e)
        {
            monthList = null;
        }
        finally
        {
            conn.Close();
        }
        return monthList;
    }


    public string get_average_member_age(string sChainId, DASHBOARD_PERIOD currentPeriod)
    {
        string sRetVal = "0.0";

        string s10_Years_ago = DateTime.Now.AddYears(-10).Year.ToString();
        string s110_Years_ago = DateTime.Now.AddYears(-110).Year.ToString();

        // Only accepting age beetween 11 and 109

        DASHBOARD_MONTH yearMonth = new DASHBOARD_MONTH(currentPeriod);

        string sPeriodFilter = " ";
        if (currentPeriod != DASHBOARD_PERIOD.DONT_CARE)
        {
            string sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(0);
            string sBeforeMonthDate = yearMonth.getThisDbYearNextMonthDay1String();

            sPeriodFilter =
            " and " +  // lunch
            "c.accepted_membership_at >= '" + sFromMonthDate + "' " +
            "and " +
            "c.accepted_membership_at < '" + sBeforeMonthDate + "' ";
        }


        string sSql =
        "select avg(age(c.date_of_birth)) average_day_string " +
        "from consumer c, shop s " +
        "where " +
        "EXTRACT(YEAR FROM c.date_of_birth) > " + s110_Years_ago + " " +
        "and " +
        "EXTRACT(YEAR FROM c.date_of_birth) < " + s10_Years_ago + " " +
        sPeriodFilter + 
        "and " + 
        "c.pincode_verified = 'yes' and s.id = c.enrolled_by_shop_id and s.parent_shop=" + sChainId;
        
        // 30 sept

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            if (reader.Read())
            {
                sRetVal = reader.c("average_day_string").ToString();

                if (sRetVal != null && sRetVal.Length > 0)
                {
                    string[] sTab = sRetVal.Split(".".ToCharArray());
                    if (sTab.Length > 0)
                        sRetVal = sTab[0];

                    decimal dDays = Convert.ToDecimal(sRetVal);
                    decimal dYears = decimal.Divide(dDays,(decimal)365.25);
                    sRetVal = dYears.ToString("#.#");
                    if (sRetVal.StartsWith(",") || sRetVal.StartsWith(".")) sRetVal = "0" + sRetVal;
                }
            }
        }
        catch (Exception e)
        {
            sRetVal = "0.0";
        }
        finally
        {
            conn.Close();
        }
        if (sRetVal == "") sRetVal = "0.0";
        return sRetVal;
    }

    public List<dim_value> get_consumer_sex_list(string sChainId, DASHBOARD_PERIOD currentPeriod)
    {
        List<dim_value> sexList = new List<dim_value>();

        DASHBOARD_MONTH yearMonth = new DASHBOARD_MONTH(currentPeriod);
                
        string sPeriodFilter = " ";
        if (currentPeriod != DASHBOARD_PERIOD.DONT_CARE)
        {
            string sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(0);
            string sBeforeMonthDate = yearMonth.getThisDbYearNextMonthDay1String();

            sPeriodFilter =
            " and " +  // lunch
            "c.accepted_membership_at >= '" + sFromMonthDate + "' " +
            "and " +
            "c.accepted_membership_at < '" + sBeforeMonthDate + "' ";
        }

        string sSql =
        "select c.sex, count(*) nof_sex " +
        "from consumer c, shop s " +
        "where c.enrolled_by_shop_id=s.id and s.parent_shop=" + sChainId + " " + 

        sPeriodFilter + 

        "and c.pincode_verified='yes' " + 
        "and " + 
        "(sex = 'female' or sex = 'male') " + 
        "group by c.sex ";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                dim_value sex = new dim_value();

                sex.sDim = reader.c("sex").ToString();
                sex.sValue = reader.c("nof_sex").ToString();
                sexList.Add(sex);
            }
        }
        catch (Exception e)
        {
            sexList = null;
        }
        finally
        {
            conn.Close();
        }
        return sexList;
    }





    public List<dim_value_value> dash_get_members_by_chain_shop(string sParentShopId,DASHBOARD_PERIOD currentPeriod)
    {

        // SKAL BORT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // test_mini();

        DASHBOARD_MONTH yearMonth = new DASHBOARD_MONTH(currentPeriod);

        string sPeriodFilter = "";

        if (currentPeriod != DASHBOARD_PERIOD.DONT_CARE)
        {
            string sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(0);
            string sBeforeMonthDate = yearMonth.getThisDbYearNextMonthDay1String();
            
            sPeriodFilter =
            " and " +  // lunch
            "c.created_at >= '" + sFromMonthDate + "' " +
            "and " +
            "c.created_at < '" + sBeforeMonthDate + "' ";
        }

        /*
        if (yearMonthPeriod == DASHBOARD_PERIOD.THIS_MONTH || yearMonthPeriod == DASHBOARD_PERIOD.PREV_MONTH || yearMonthPeriod == DASHBOARD_PERIOD.PREV_PREV_MONTH)
        {
            sFromMonthDate = yearMonth.getThisDbYearMonthDay1String(0);
            sBeforeMonthDate = yearMonth.getThisDbYearNextMonthDay1String();
        }
        */

        string sSql =
        "select s.id shop_id, s.name,c.pincode_verified membership_status,count(*) consumer_count " +
        "from consumer c, shop s " +
        "where " +
        "s.id = c.enrolled_by_shop_id " + 
        sPeriodFilter + 
        "and " + 
        "s.parent_shop=" + sParentShopId + " " +
        // "and " +
        // "and (s.id <> 20 and s.id <> 25) " + 
        "group by " +
        "enrolled_by_shop_id, s.name,s.id, c.pincode_verified " +
        "order by s.name, c.pincode_verified";

        List<dim_value_value> shopList = new List<dim_value_value>();

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);
            string sOldShopId = "";
            dim_value_value currentShop = null;
            while (reader.Read())
            {

                string sShopId = reader.c("shop_id").ToString();
                if (sShopId != sOldShopId)
                {
                    currentShop = new dim_value_value();
                    currentShop.sDim = reader.c("name").ToString();
                    shopList.Add(currentShop);
                }

                string sMembershipStatus = reader.c("membership_status").ToString();
                string sConsumerCount = reader.c("consumer_count").ToString();

                if (sMembershipStatus == "yes") currentShop.sValue_1 = sConsumerCount; // Has approved SMS // 14 okt = 65
                else if (sMembershipStatus == "no") currentShop.sValue_2 = sConsumerCount; // Not approved SMS

                sOldShopId = sShopId;
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }

    private DateTime getDateTime_from_string(String sDateTime)
    {
        // 0123456789
        // 2001.02.28
        if (sDateTime == null || sDateTime.Length < 19)
            return new DateTime(2000, 1, 1);

        try
        {
            int iYear = Convert.ToInt32(sDateTime.Substring(0, 4));
            string sMonth = sDateTime.Substring(5, 2);
            string sDay = sDateTime.Substring(8, 2);

            


            if (sMonth[0] == '0') sMonth = sMonth.Substring(1, 1);
            if (sDay[0] == '0') sDay = sDay.Substring(1, 1);

            int iMonth = Convert.ToInt32(sMonth);
            int iDay = Convert.ToInt32(sDay);


            string sHour = sDateTime.Substring(11, 2);
            string sMinute = sDateTime.Substring(14, 2);
            string sSecond = sDateTime.Substring(17, 2);
            if (sHour[0] == '0') sHour = sHour.Substring(1, 1);
            if (sMinute[0] == '0') sMinute = sMinute.Substring(1, 1);
            if (sSecond[0] == '0') sSecond = sSecond.Substring(1, 1);


            return new DateTime(iYear, iMonth, iDay,Convert.ToInt32(sHour),Convert.ToInt32(sMinute),Convert.ToInt32(sSecond));
            
        }
        catch (Exception)
        {
            return new DateTime(2000, 1, 1);
        }

    }
    
    public List<backoffice.dim_dim_value> verifone_to_webservice(DateTime checkDate)
    {
        List<backoffice.dim_dim_value> returnList = new List<dim_dim_value>();
        
        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        string sXml = "";

        int iMaxSeconds = -10000;
        int iMinSeconds = 10000;

        int i = 0;

        DateTime ss = getDateTime_from_string("2015.09.12 09:40:24 534");

        string sVerifoneTimeStampRouter = "";
        string sVerifoneTimeStampRouter_1 = "";
        string sVerifoneTimeStampRouter_2 = "";


        string sVerifoneTimeStamp = "";
        string sVerifoneTimeStamp_1 = "";
        string sVerifoneTimeStamp_2 = "";

        try
        {

            // alter table webservice_log add column dt_timestamp TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT to_timestamp(0)
            // alter table webservice_log add column response_time int default 0

            string sSql = 
                "select * from webservice_log where " +
                "(name like '%GetCustomerData%' and " + 
                "parameters like '%_GetCustomerData_%') or " +
                "(name like '%EnrollCustomer%' and " +
                "parameters like '%_EnrollCustomer_%') " +
                "and id > 197000 order by id desc";
            
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                sXml = reader.c("parameters").ToString();

                XmlDocument doc = new XmlDocument();
                sXml = sXml.Replace("PROSESSOR_VAS_NO", "PROSESSOR.VAS.NO");
                sXml = sXml.Replace("VAS_PROSESSOR", "VAS.PROSESSOR");
                sXml = sXml.Replace("VAS_Name_1", "VAS.Name.1");
                sXml = sXml.Replace("_", "'");

                
                sXml = sXml.Replace("&lt;", "<");
                sXml = sXml.Replace("&gt;", ">");
                sXml = sXml.Replace("&quot;", "'");


                try
                {
                    if (sXml.IndexOf("RequestHeader") >= 0)
                    {
                        doc.LoadXml(sXml);

                        XmlNode requestInfo = doc.SelectSingleNode("GetCustomerData/RequestHeader/RequestInfo");

                        if (requestInfo == null)
                            requestInfo = doc.SelectSingleNode("EnrollCustomer/RequestHeader/RequestInfo");

                        sVerifoneTimeStampRouter = getSafeAttribute(requestInfo, "TimeStampRouter");
                        sVerifoneTimeStampRouter_1 = sVerifoneTimeStampRouter.Substring(0, 10);
                        sVerifoneTimeStampRouter_2 = sVerifoneTimeStampRouter.Substring(11, 8);

                        DateTime verifoneTimestamp = getDateTime_from_string(sVerifoneTimeStampRouter_1 + " " + sVerifoneTimeStampRouter_2);

                        string sId = reader.c("id").ToString();

                        string sBemberDate = reader.c("timestamp").ToString();
                        DateTime dtBember = getDateTime_from_string(sBemberDate);

                        dtBember = dtBember.Add(new TimeSpan(1, 0, 0));
                        TimeSpan ts = dtBember.Subtract(verifoneTimestamp);

                        int iSeconds = ts.Seconds;
                        int iMinutes = ts.Minutes;

                        int iResponseInSeconds = iMinutes * 60 + iSeconds;

                        if (iMinutes > 0)
                        {
                            iMinutes = iMinutes;
                        }

                        xSQL_UpdateBuilder ub = new xSQL_UpdateBuilder(this, "webservice_log", "where id=" + sId);
                        ub.add("dt_timestamp", dtBember);
                        ub.add("response_time", iResponseInSeconds);
                        ub.ExecuteSql();




                        if (iSeconds > iMaxSeconds) iMaxSeconds = iSeconds;
                        if (iSeconds < iMinSeconds) iMinSeconds = iSeconds;

                        ++i;
                    }
                }
                catch (Exception eee)
                {
                    eee = eee;
                }


            }
        }
        catch (Exception e)
        {
            e = e;
        }
        finally
        {
            conn.Close();
        }

        return returnList;
    }



    public List<dim_value> dash_mini_month_member(string sYear)
    {
        List<dim_value> yearList = new List<dim_value>();

        string sSql = "select count(*) year_value from consumer c where pincode_verified='yes' and EXTRACT(YEAR FROM accepted_membership_at)=" + sYear;

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                dim_value month = new dim_value();

                month.sDim = sYear;
                month.sValue = reader.c("year_value").ToString();
                yearList.Add(month);
            }
        }
        catch (Exception e)
        {
            yearList = null;
        }
        finally
        {
            conn.Close();
        }
        return yearList;
    }


    
    public List<chain_shop_invoice> get_chain_shop_invoice_list(string sParentShopId, DateTime startDate, DateTime endDate)
    {
        StringBuilder sb = new StringBuilder();

        List<chain_shop_invoice> basketList = new List<chain_shop_invoice>();

        string sSql =
        "select s.id shop_id, s.name, basket_total_sum / 100 bember_profit, c.pincode_verified, cb.timestamp basket_timestamp, c.accepted_membership_at accepted_membership_timestamp " +
        "from " +
        "consumer_basket cb, consumer_paytool cp, consumer c, shop s " +
        "where " +
        "cb.shop_id=s.id " +
        "and " +
        "s.parent_shop=" + sParentShopId + " " +
        "and " +
        "cb.confirmed_by_shop=true " +
        "and " +
        "cb.consumer_paytool_id = cp.id " +
        "and " + 
        "cb.timestamp >= '" + buildPostgresDateFromStringDayPrecision(startDate) + "' " +
        "and " + 
        "cb.timestamp <= '" + buildPostgresDateFromStringDayPrecision(endDate) + "' " +
        "and " +
        "cp.consumer_id =  c.id " +
        "and " +
        "(c.pincode_verified='yes' or pincode_verified='no') " +        
        "order by s.name, cb.id desc";

        /*
            select count(*) members_accepted_in_period 
            from consumer c
            where 
            c.accepted_membership_at > '2015-06-01 00:00'
            and 
            c.accepted_membership_at < '2015-07-01 00:00'
            and 
            enrolled_by_shop_id=19
         */

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                string sPincodeVerified = (string)reader.c("pincode_verified");

                // if (sPincodeVerified == "yes" || sPincodeVerified == "no")
                if (sPincodeVerified == "yes" || sPincodeVerified == "no")
                {
                    chain_shop_invoice chainInvoice = new chain_shop_invoice();
                    chainInvoice.iShopId = (int)reader.c("shop_id");

                    chainInvoice.acceptedMembershipAt = (DateTime)reader.c("accepted_membership_timestamp");
                    chainInvoice.basketDatetime = (DateTime)reader.c("basket_timestamp");

                    if (sPincodeVerified == "yes")
                    {
                        chainInvoice.dBemberInvoiceAmount = (decimal)reader.c("bember_profit");
                        chainInvoice.bIsMember = true;
                    }
                    else
                    {
                        chainInvoice.dBemberInvoiceAmount = 0;
                        chainInvoice.bIsMember = false;
                    }
                    chainInvoice.sShopName = (string)reader.c("name");

                    

                    basketList.Add(chainInvoice);
                }
            }
        }
        catch (Exception e)
        {
            basketList = null;
        }
        finally
        {
            conn.Close();
        }
        return basketList;
    }


    public List<chain_shop_invoice> get_chain_shop_BASKET_list(string sParentShopId, DateTime startDateYear, DateTime endDateYear, DateTime startDatePeriod, DateTime endDatePeriod)
    {
        StringBuilder sb = new StringBuilder();

        List<chain_shop_invoice> basketList = new List<chain_shop_invoice>();

        string sSql =
        "select s.id shop_id, s.name, basket_total_sum, basket_total_sum / 100 bember_profit, c.pincode_verified, cb.timestamp basket_timestamp, c.accepted_membership_at accepted_membership_timestamp " +
        "from " +
        "consumer_basket cb, consumer_paytool cp, consumer c, shop s " +
        "where " +
        "cb.shop_id=s.id " +
        "and " +
        "s.parent_shop=" + sParentShopId + " " +
        "and " +
        "cb.confirmed_by_shop=true " +
        "and " +
        "cb.consumer_paytool_id = cp.id " +
        
        "and ((" +
        "cb.timestamp >= '" + buildPostgresDateFromStringDayPrecision(startDateYear) + "' " +
        "and " +
        "cb.timestamp <= '" + buildPostgresDateFromStringDayPrecision(endDateYear) + "') " +
        " or " + 
        "(cb.timestamp >= '" + buildPostgresDateFromStringDayPrecision(startDatePeriod) + "' " +
        "and " +
        "cb.timestamp <= '" + buildPostgresDateFromStringDayPrecision(endDatePeriod) + "')) " +

        "and " +
        "cp.consumer_id =  c.id " +
        "and " +
        "(c.pincode_verified='yes' or pincode_verified='no') " +
        "order by s.name, cb.id desc";

        /*
            select count(*) members_accepted_in_period 
            from consumer c
            where 
            c.accepted_membership_at > '2015-06-01 00:00'
            and 
            c.accepted_membership_at < '2015-07-01 00:00'
            and 
            enrolled_by_shop_id=19
         */

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                string sPincodeVerified = (string)reader.c("pincode_verified");

                // if (sPincodeVerified == "yes" || sPincodeVerified == "no")
                if (sPincodeVerified == "yes" || sPincodeVerified == "no")
                {
                    chain_shop_invoice chainInvoice = new chain_shop_invoice();
                    chainInvoice.iShopId = (int)reader.c("shop_id");

                    chainInvoice.acceptedMembershipAt = (DateTime)reader.c("accepted_membership_timestamp");
                    chainInvoice.basketDatetime = (DateTime)reader.c("basket_timestamp");

                    if (chainInvoice.basketDatetime >= startDateYear && chainInvoice.basketDatetime <= endDateYear) 
                        chainInvoice.bInThisYear = true;

                    if (chainInvoice.basketDatetime >= startDatePeriod && chainInvoice.basketDatetime <= endDatePeriod) 
                        chainInvoice.bInThisPeriod = true;

                    chainInvoice.dTotalInvoiceAmount = (decimal)reader.c("basket_total_sum");

                    if (sPincodeVerified == "yes")
                    {
                        chainInvoice.dBemberInvoiceAmount = (decimal)reader.c("bember_profit");
                        chainInvoice.bIsMember = true;
                    }
                    else
                    {
                        chainInvoice.dBemberInvoiceAmount = 0;
                        chainInvoice.bIsMember = false;
                    }
                    chainInvoice.sShopName = (string)reader.c("name");

                    basketList.Add(chainInvoice);
                }
            }
        }
        catch (Exception e)
        {
            basketList = null;
        }
        finally
        {
            conn.Close();
        }
        return basketList;
    }




    public List<admin_shop> get_all_shops(string sChainId, bool bEnrollmentAcceptedInTerminal)
    {
        StringBuilder sb = new StringBuilder();

        List<admin_shop> shopList = new List<admin_shop>();

        string sSql = 
            "select id, name,shop_external_id,  accept_terminal_enrollment from shop " + 
            "where parent_shop='" + sChainId + "' and accept_terminal_enrollment=" + bEnrollmentAcceptedInTerminal.ToString() + " order by name";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                admin_shop shop = new admin_shop();

                shop.iId = (int)reader.c("id");
                shop.sName = (string)reader.c("name");
                shop.sMerchantId = (string)reader.c("shop_external_id");
                shop.bAcceptTerminalEnrollment = (bool)reader.c("accept_terminal_enrollment");
                shopList.Add(shop);
            }
        }
        catch (Exception e)
        {
            shopList = null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }

    public admin_shop get_shop(string sShopId)
    {
        StringBuilder sb = new StringBuilder();

        admin_shop shop = new admin_shop();

        string sSql =
            "select s.id, s.parent_name, s.name, s.shop_external_id, c.value_1 percent, accept_terminal_enrollment, enrollment_sms_text " +
            "from shop s, shop_coupon sc, coupon c, coupon_type ct  " +
            "where  " +
            "s.id = " + sShopId + " and  " +
            "sc.shop_id = s.id and  " +
            "c.id = sc.coupon_id and  " +
            "c.coupon_type_id=ct.id and  " +
            "ct.name='enrollment_loyalty_discount'";

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);


            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            if (reader.Read())
            {
                shop.iId = (int)reader.c("id");
                shop.sParentName = (string)reader.c("parent_name");
                shop.sName = (string)reader.c("name");
                shop.sMerchantId = (string)reader.c("shop_external_id");
                shop.sSmsText = (string)reader.c("enrollment_sms_text");
                shop.iLoyaltyPercent = (int)reader.c("percent");
                shop.bAcceptTerminalEnrollment = (bool)reader.c("accept_terminal_enrollment");
            }
            else
                shop = null;
        }
        catch (Exception e)
        {
            shop = null;
        }
        finally
        {
            conn.Close();
        }

        if (shop == null)
            return get_shop_heading_only(sShopId);

        return shop;
    }

    private admin_shop get_shop_heading_only(string sShopId)
    {
        StringBuilder sb = new StringBuilder();

        admin_shop shop = new admin_shop();

        string sSql =
            "select s.id, s.parent_name,s.shop_external_id, s.name, accept_terminal_enrollment, enrollment_sms_text " +
            "from shop s, shop_coupon sc, coupon c, coupon_type ct  " +
            "where  " +
            "s.id = " + sShopId;

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);


            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            if (reader.Read())
            {
                shop.iId = (int)reader.c("id");
                shop.sParentName = (string)reader.c("parent_name");
                shop.sMerchantId = (string)reader.c("shop_external_id");
                shop.sSmsText = (string)reader.c("enrollment_sms_text");
                shop.bAcceptTerminalEnrollment = (bool)reader.c("accept_terminal_enrollment");
                shop.sName = (string)reader.c("name");
            }
        }
        catch (Exception e)
        {
            shop = null;
        }
        finally
        {
            conn.Close();
        }
        return shop;
    }


    public bool update_shop_merchant_id(string sShopId, string sNewMerchantId)
    {
        xSQL_UpdateBuilder ub = new xSQL_UpdateBuilder(this, "shop", "where id=" + sShopId);

        ub.add("shop_external_id", sNewMerchantId);

        bool bOK = ub.ExecuteSql();

        return bOK;
    }


    public bool update_shop_enrollment(string sShopId, bool bAcceptAnrollment)
    {
        xSQL_UpdateBuilder ub = new xSQL_UpdateBuilder(this, "shop", "where id=" + sShopId);

        ub.add("accept_terminal_enrollment", bAcceptAnrollment);

        bool bOK = ub.ExecuteSql();

        return bOK;
    }

    public bool update_shop_enrollment_sms(string sShopId, string sEnrollment_SMS_Text)
    {
        xSQL_UpdateBuilder ub = new xSQL_UpdateBuilder(this, "shop", "where id=" + sShopId);

        ub.add("enrollment_sms_text", sEnrollment_SMS_Text);

        bool bOK = ub.ExecuteSql();

        return bOK;
    }


    
    public bool get_login_status(string sUserName, string sPassword)
    {
        if (isBlank(sUserName)) return false;
        if (isBlank(sPassword)) return false;

        bool bRetVal = false;

        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            string sEncrypted = Dinfordel.Utils.CryptUtils.EncryptPassword(sUserName, sPassword);

            string sSql = "select a.shop_id from administrator a where a.email='" + sUserName + "' and a.password='" + sEncrypted + "' ";

            // b) Aministratorlogin. Få tak i shop_id med sMerchantId ...
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);

            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            if (reader.Read())
            {
                bRetVal = true;
            }
        }
        catch (Exception e)
        {
            bRetVal = false;
        }
        finally
        {
            conn.Close();
        }

        return bRetVal;
    }

    private string buildPostgresDateFromString(DateTime dt)
    {
        return dt.Year.ToString() + "-" + zero_2(dt.Month.ToString()) + "-" + zero_2(dt.Day.ToString()) + " " + zero_2(dt.Hour.ToString()) + zero_2(dt.Minute.ToString());
    }

    private string buildPostgresDateFromStringDayPrecision(DateTime dt)
    {
        return dt.Year.ToString() + "-" + zero_2(dt.Month.ToString()) + "-" + zero_2(dt.Day.ToString());
    }




    public int setRelationsToHigherLevel(List<backoffice.admin_shop> shopList)
    {
        // A_PHONE_ENROLLED, A_PHONE_SKIPPED, A_PHONE_ALREADY_MEMBER, 
        //   B_BASKET_NOT_CONFIRMED, 
        //   B_BASKET_CONFIRMED, 
        //   C_MEMBERSHIP_ACCEPTED, UNKNOWN }
        
        int iRelations = 0;

        return iRelations;
    }

    public int appendAllEventsForAllShops(Global global, List<backoffice.admin_shop> shopList, DateTime fromTimestamp)
    {
        int iNofEvents = 0;
        int iHours = -1;

        iHours = -global.iHours;

        /* COMPLETED BASKETS */
        string sSql =
            // "select shop_id,basket_id, basket_total_sum,basket_total_discount,confirmed_by_shop,timestamp from consumer_basket cb " +
            "select basket_b, shop_id,cb.id basket_row_id, basket_id, basket_total_sum,basket_total_discount,confirmed_by_shop,timestamp,cp.token_id " + 
            "from consumer_basket cb, consumer_paytool cp " +
            "where cb.consumer_paytool_id = cp.id and " +
            "timestamp BETWEEN " +
            "'" + buildPostgresDateFromString(fromTimestamp.AddHours(iHours)) + "'::timestamp and " + 
            "'" + buildPostgresDateFromString(fromTimestamp) + "'::timestamp order by cb.id ";
        
        // 30 juni
        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                string sShopId = reader.c("shop_id").ToString();
                admin_shop currentShop = getShopFromId(shopList, sShopId);

                if (currentShop != null)
                {
                    backoffice.shop_event shopEvent = new shop_event();
                    shopEvent.bConfirmedByShop = (bool)reader.c("confirmed_by_shop");
                    shopEvent.sDescription = reader.c("basket_id").ToString();
                    shopEvent.timestamp = (DateTime)reader.c("timestamp");
                    shopEvent.sBasketRowId = reader.c("basket_row_id").ToString();
                    shopEvent.sBasket_b = reader.c("basket_b").ToString();
                    shopEvent.sToken = (string)reader.c("token_id");
                    shopEvent.amount_1 = reader.c("basket_total_sum").ToString();
                    shopEvent.amount_2 = reader.c("basket_total_discount").ToString();
                    if (shopEvent.bConfirmedByShop)
                        shopEvent.shopEventType = SHOP_EVENT_TYPE.B_BASKET_CONFIRMED;
                    else
                        shopEvent.shopEventType = SHOP_EVENT_TYPE.B_BASKET_NOT_CONFIRMED;
                    ++iNofEvents;
                    
                    
                    currentShop.append_event(shopEvent);
                }
            }
        }
        catch (Exception e)
        {
            e = e;
        }
        finally
        {
            conn.Close();
        }

        
        /* ENROLLED PHONE OR NOT */
        sSql =
            "select " +
            "s.id shop_id, parameters, c.accepted_membership_at, c.pincode_verified, " +
            "case " +
            "when sub_action like 'ecr_new%' then 'Ja' " +
            "when sub_action like 'ecr_ref%' then 'Nei' " +
            "when sub_action like 'ecr_already%' then 'Allerede medlem' " +
            "when sub_action like 'ecr_additional_card%' then 'Nytt kort' " +   // baxid=526595;90187304;additional_card;053670260421520428
            "end Oppga_mobilnummer_dim, " +
            "c.phone, " +
            "ua.timestamp tidspunkt_text, " +
            "ua.id ident_text " +
            "from user_action ua " +
            "left JOIN shop s ON (s.shop_external_id = substring(ua.parameters for position(';' in ua.parameters)-position('baxid=' in ua.parameters)-6  from  position('baxid=' in parameters)+6) ) " +
            // "left JOIN consumer c ON (c.phone = split_part(ua.parameters, ';', 2)) " +
            "left JOIN consumer c ON (c.guid = ua.consumer_guid) " +

            "where (sub_action like 'ecr_refused_enrollment' or sub_action like 'ecr_new_enrollment' or sub_action like 'ecr_already%' or sub_action like 'ecr_additional_card%') " +
            "and ua.id > 4785 " +
            "and s.shop_external_id = substring(ua.parameters for position(';' in ua.parameters)-position('baxid=' in ua.parameters)-6  from  position('baxid=' in parameters)+6) " +
            "and timestamp BETWEEN " +
            "'" + buildPostgresDateFromString(fromTimestamp.AddHours(iHours)) + "'::timestamp and " +
            "'" + buildPostgresDateFromString(fromTimestamp) + "'::timestamp " +
            "order by ua.id";

        conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                string sShopId = reader.c("shop_id").ToString();
                admin_shop currentShop = getShopFromId(shopList, sShopId);

                if (currentShop != null)
                {
                    backoffice.shop_event shopEvent = new shop_event();

                    shopEvent.sRawParameters = (string)reader.c("parameters");
                    shopEvent.sPhone = (string)reader.c("phone");
                    shopEvent.timestamp = (DateTime)reader.c("tidspunkt_text"); // Får forskjellig klokke for de på nederste linjen !!!
                    string sMobilYesNo = (string)reader.c("Oppga_mobilnummer_dim");
                    string sIsMember = (string)reader.c("pincode_verified");

                    shopEvent.set_accepted_membership_at(reader.c("accepted_membership_at"));

                    // Later we will join relations based on token ...
                    if (sMobilYesNo == "Ja")
                    {
                        // "baxid=526588;41293984;enrolled;335475343524073400"
                        shopEvent.shopEventType = SHOP_EVENT_TYPE.A_PHONE_ENROLLED;
                        try
                        {
                            string[] parTab = shopEvent.sRawParameters.Split(";".ToArray());
                            shopEvent.sBaxId = parTab[0].Split("=".ToCharArray())[1];
                            shopEvent.sPhone = parTab[1];
                            shopEvent.sToken = parTab[3];
                        }
                        catch (Exception) { }
                    }
                    else if (sMobilYesNo == "Nei")
                    {
                        // sRawParameters = "baxid=bbb;ttt;"
                        shopEvent.shopEventType = SHOP_EVENT_TYPE.A_PHONE_SKIPPED;
                        try
                        {
                            string[] parTab = shopEvent.sRawParameters.Split(";".ToCharArray());
                            shopEvent.sBaxId = parTab[0].Split("=".ToCharArray())[1];
                            shopEvent.sToken = parTab[1];

                            if (shopEvent.sToken == "053670260421520428") 
                            {
                                int t = 0;
                            }

                        }
                        catch (Exception) { }
                    }
                    else if (sMobilYesNo == "Allerede medlem")
                    {
                        // sRawParameters = "baxid=bbb;token=ttt;"
                        if (sIsMember == "yes")
                        {
                            if (shopEvent.timestamp >= shopEvent.accepted_membership_at && shopEvent.accepted_membership_at.Year > 2001)
                                shopEvent.shopEventType = SHOP_EVENT_TYPE.A_CONSUMER_EXISTS_AND_IS_MEMBER;
                            else
                                shopEvent.shopEventType = SHOP_EVENT_TYPE.A_CONSUMER_EXISTS; // Not member before after ... Have to fix registration data here 
                        }
                        else
                            shopEvent.shopEventType = SHOP_EVENT_TYPE.A_CONSUMER_EXISTS;
                        try
                        {
                            string[] parTab = shopEvent.sRawParameters.Split(";".ToCharArray());
                            shopEvent.sBaxId = parTab[0].Split("=".ToCharArray())[1];
                            shopEvent.sToken = parTab[1].Split("=".ToCharArray())[1];
                        }
                        catch (Exception) { }
                    }
                    else if (sMobilYesNo == "Nytt kort")
                    {
                        // sRawParameters : baxid=526595;90187304;additional_card;053670260421520428
                        if (sIsMember == "yes") 
                            shopEvent.shopEventType = SHOP_EVENT_TYPE.A_CONSUMER_EXISTS_AND_IS_MEMBER;
                        else
                            shopEvent.shopEventType = SHOP_EVENT_TYPE.A_CONSUMER_EXISTS;
                        try
                        {
                            string[] parTab = shopEvent.sRawParameters.Split(";".ToArray());
                            shopEvent.sBaxId = parTab[0].Split("=".ToCharArray())[1];
                            shopEvent.sToken = parTab[3];
                            shopEvent.bAdditionalCard = true;

                        }
                        catch (Exception) { }
                    }


                    ++iNofEvents;
                    currentShop.append_event(shopEvent);
                }
            }
        }
        catch (Exception e)
        {
        }
        finally
        {
            conn.Close();
        }
        

        /* ***** ACCEPTED AGREEMENT */
        sSql =
            "select " +
            "c.id consumer_id, " +
            "s.id shop_id, " +
            "c.accepted_membership_at, " +
            "cp.token_id " + 
            "from consumer c, shop s, consumer_paytool cp " +
            "where " +
            "cp.consumer_id = c.id " +
            "and c.pincode_verified = 'yes' " +
            "and c.enrolled_by_shop_id = s.id " +
            "and length(c.phone) > 2 and length(c.phone) < 11 " +
            "and c.accepted_membership_at BETWEEN " +
            "'" + buildPostgresDateFromString(fromTimestamp.AddHours(iHours)) + "'::timestamp and " +
            "'" + buildPostgresDateFromString(fromTimestamp) + "'::timestamp " +
            "order by c.id";

        conn = new GLOBAL_SQL_CONN(this);

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            string sLastConsumerId = "";
            backoffice.shop_event lastShopEvent = null;
            // We can get more than one row for each consumer since we join with consumer_paytool. Therefore DO NOT make more than one membership_accepted-event
            while (reader.Read())
            {
                string sConsumerId = reader.c("consumer_id").ToString();
                string sShopId = reader.c("shop_id").ToString();
                string sToken = reader.c("token_id").ToString();
                admin_shop currentShop = getShopFromId(shopList, sShopId);

                if (currentShop != null)
                {
                    if (sConsumerId != sLastConsumerId)
                    {
                        backoffice.shop_event shopEvent = new shop_event();
                        shopEvent.timestamp = (DateTime)reader.c("accepted_membership_at");
                        shopEvent.shopEventType = SHOP_EVENT_TYPE.C_MEMBERSHIP_ACCEPTED;
                        shopEvent.sToken = sToken;
                        ++iNofEvents;
                        lastShopEvent = shopEvent;
                        currentShop.append_event(shopEvent);
                    }
                    else
                    {
                        lastShopEvent.sToken += ";" + sToken; // 3 juli TODO (use this match whne testing later ...
                    }
                }
                sLastConsumerId = sConsumerId;
            }
        }
        catch (Exception e)
        {
        }
        finally
        {
            conn.Close();
        }
        
        return iNofEvents;
    }

    backoffice.admin_shop getShopFromId(List<backoffice.admin_shop> shopList, string sId)
    {
        foreach (backoffice.admin_shop shop in shopList)
        {
            if (shop.iId.ToString() == sId)
                return shop;
        }    
        return null;
    }

    public backoffice.chain_shop_standard_report_item getReportShopFromId(List<backoffice.chain_shop_standard_report_item> shopList, string sId)
    {
        foreach (backoffice.chain_shop_standard_report_item shop in shopList)
        {
            if (shop.iShopId.ToString() == sId)
                return shop;
        }
        return null;
    }

    public List <dim_value> get_monthly_invoice(string sChainId)
    {
        string sSql =
            "select " +
            "s.name, sum(cb.basket_total_sum) shop_sum " +
            "from consumer_basket cb, consumer_paytool cp, consumer c, shop s " +
            "where " +
            "cb.shop_id = s.id " +
            "and " +
            "cb.consumer_paytool_id = cp.id " +
            "and " +
            "cp.consumer_id = c.id " +
            "and " +
            "cb.confirmed_by_shop='yes' " +
            "group by s.name " +
            "order by s.name";

        /* Med 60 dagers liggetid ... */
        /*
        string sSql =
            "select " +
            "s.name, sum(cb.basket_total_sum) shop_sum " +
            "from consumer_basket cb, consumer_paytool cp, consumer c, shop s " +
            "where " +
            "cb.shop_id = s.id " +
            "and " +
            "cb.consumer_paytool_id = cp.id " +
            "and " +
            "cp.consumer_id = c.id " +
            "and " +
            "c.pincode_verified='yes' " +
            "and " +
            "cb.confirmed_by_shop='yes' " +
            "and " +
            "(EXTRACT(DOY FROM timestamp) + EXTRACT(YEAR FROM timestamp) * 365) >  (EXTRACT(DOY FROM accepted_membership_at)  + EXTRACT(YEAR FROM accepted_membership_at) * 365) -60  " +
            "group by s.name " +
            "order by s.name";
        */
        GLOBAL_SQL_CONN conn = new GLOBAL_SQL_CONN(this);

        List <dim_value> shopList = new List<dim_value>();

        try
        {
            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND(sSql, conn);
            GLOBAL_SQL_READER reader = new GLOBAL_SQL_READER(command);

            while (reader.Read())
            {
                dim_value dimValue = new dim_value();
                dimValue.sDim = reader.c("name").ToString();
                dimValue.sValue = reader.c("shop_sum").ToString();

                shopList.Add(dimValue);
            }
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            conn.Close();
        }
        return shopList;
    }


}


