using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Xml;


/// <summary>
/// Summary description for shop_live
/// </summary>


public class SHOP_LIVE : SHOP_BASE
{
    public static string getContainerId(backoffice.admin_shop shop)
    {
        return "shop_container_" + shop.iId.ToString();
    }

    public static string build_live_table(Global global)
    {
        StringBuilder s = new StringBuilder();

        List<backoffice.admin_shop> shopList = global.www_backoffice().get_all_shops(global.MASTER_CHAIN_ID,true);

        // DateTime startFromTimestamp = DateTime.Now;

        if (global.shop_live_current_timestamp.Year == 2000)
        {
            global.shop_live_current_timestamp = DateTime.Now;
            if (global.bDEBUG_TESTING_OLD_DATABASE)
            {
                global.shop_live_current_timestamp = new DateTime(2015, 6, 30, 20, 0, 0);
            }
        }
        

        // DateTime startFromTimestamp = new DateTime(2015, 6, 30, 20, 0, 0);
        // global.shop_live_current_timestamp = startFromTimestamp;

        if (!global.bRunningLive)
        {
            global.iToMinute = 0;
            global.iToSecond = 0;
        }

        global.shop_live_current_timestamp = new DateTime(global.shop_live_current_timestamp.Year, global.shop_live_current_timestamp.Month, global.shop_live_current_timestamp.Day, global.iToHour, global.iToMinute, global.iToSecond);

        global.www_backoffice().appendAllEventsForAllShops(global, shopList, global.shop_live_current_timestamp);

        // { A_PHONE_ENROLLED, A_PHONE_SKIPPED, A_PHONE_ALREADY_MEMBER, B_BASKET_NOT_CONFIRMED, C_BASKET_CONFIRMED, D_MEMBERSHIP_ACCEPTED, UNKNOWN }
        global.www_backoffice().setRelationsToHigherLevel(shopList);

        if (global.bShopLiveTimeDialogIsOpen)
            s.Append(SHOP_LIVE_DATE_MODIFY.B_get_maximized_dialog(global));
        else
            s.Append(SHOP_LIVE_DATE_MODIFY.A_get_minimized_dialog(global.shop_live_current_timestamp, true));


        s.Append("<div class=live_body>");

        s.Append("<table class=live_table>");
        s.Append("  <tr>");
        s.Append("      <td valign=top>");
        s.Append(GET_TIMELINE_PART(global,shopList, global.shop_live_current_timestamp, 16));
        s.Append("      </td>");
        s.Append("  </tr>");
        s.Append("  <tr>");
        s.Append("      <td>");
        s.Append(GET_LAST_EVENTS_PART());
        s.Append("      </td>");
        s.Append("  </tr>");
        s.Append("<table>");

        s.Append("</div>");

        global.www_backoffice().log_call("live_table_called " + global.shop_live_current_timestamp.ToShortDateString());

        return s.ToString();
    }

    public static int GET_X_START_FROM_TIME(Global global, int iTotalW, DateTime startFromTimestamp, DateTime eventTimestamp, int iNofSlices)
    {
        double dxEvent = 0;

        double dBuffer = (iTotalW / iNofSlices) / 2;

        double dDivider = 3600;


        dDivider = 3600 * global.iHours;

        TimeSpan ts = startFromTimestamp.Subtract(eventTimestamp);
        double secondsBeforeStartFrom = ts.TotalSeconds;
        // double secondsBeforeStartFrom = 3600;

        // 30 juni
        double dFactor = secondsBeforeStartFrom / (dDivider);

        dxEvent = dFactor * (iTotalW - dBuffer*2);

        // return (int)(iTotalW - dxEvent + dBuffer);
        return (int)(iTotalW - (dxEvent + dBuffer));
    }

    public static string GET_TIMELINE_HEADING(Global global, int timezone_width, DateTime startFrom, int iSlices)
    {
        // 800px : [__16:00__]  [__16:10__]  [__16:20__]  [__16.30__]  [__16:40__]  [__16:50__]  [__17:00__] 
        // Width of each 10-minute = (798/7)
        // Real X-Start = (800/7)/2
        // Real X-End = 800 - ((800/7)/2)

        double dDivSlices = iSlices - 1;
        
        StringBuilder sb = new StringBuilder();

        double iNumberOfMinutesPerTick = (double)(global.iHours*60 / dDivSlices);


        for (int i = -(iSlices-1); i <= 0; ++i)
        {

            DateTime minutesAgo = startFrom.AddMinutes(i * iNumberOfMinutesPerTick);
         
            int divWidth = (timezone_width / iSlices);
             
            sb.Append("<div class=live_graph_shop_heading_ticks style='width:" + divWidth.ToString() + "px;'>");

            if (global.iHours >= 24)
            {
                sb.Append(getDateMonthYearNorwegianPretty(minutesAgo) + " " + 
                    zero_2(minutesAgo.Hour.ToString()) + ":" +
                    zero_2(minutesAgo.Minute.ToString()));
            }
            else
            {
                sb.Append(
                    zero_2(minutesAgo.Hour.ToString()) + ":" +
                    zero_2(minutesAgo.Minute.ToString()));
            }

            sb.Append("</div>");
             
        }


        return sb.ToString();
    }

    public static string GET_TIMELINE_PART(Global global, List<backoffice.admin_shop> shopList, DateTime startFromTimestamp, int iNofSlices)
    {
        int TOTAL_WIDTH = 1000;

        bool bAllShops = false;

        StringBuilder s = new StringBuilder();


        s.Append("<div class=live_timeline>");

        s.Append("<table cellpadding=0 cellspacing=0");

        s.Append("<tr>");

        s.Append("<td valign=top id=visible_time onclick=shop_live_date_clicked() class=live_timeline_heading>");
        if (global.bRunningLive)
        {
            s.Append(getHourMinuteSecond(DateTime.Now));
        }
        else
        {
            s.Append(getDateMonthYearNorwegianPretty(startFromTimestamp));
        }
        s.Append("</td>");

        s.Append("<td>");
        s.Append(GET_TIMELINE_HEADING(global,TOTAL_WIDTH, startFromTimestamp,iNofSlices));
        s.Append("</td>");

        s.Append("<td valign=top>");
        s.Append("<div class=live_grand_total >");
        s.Append("grand_total_to_be_replaced");
        s.Append("</div>");
        s.Append("</td>");

        s.Append("<td valign=top>");
        s.Append("<div class=live_grand_total >");
        s.Append("grand_total_gold_to_be_replaced");
        s.Append("</div>");

        s.Append("</td>");
        s.Append("</tr>");


        int wBasket = 6; int hBasket = 6;
        int rCircle = 4;

        int w = TOTAL_WIDTH;
        int h = 50;
        if (bAllShops) h = 500;

        int iNofNewMembersGrandTotal = 0;
        int iGoldMemberReturnedGlobal = 0;


        if (bAllShops)
        {
            s.Append("<tr>");
            s.Append("<td>");

            s.Append("<div id='shop_timeline_all_shops' class=live_shop>");
            s.Append("Alle butikker");
            s.Append("</div>");

            s.Append("</td>");

            // s.Append("<td valign=top colspan=" + iNofSlices.ToString() + ">");
            s.Append("<td valign=top >");


            string sShopClass = " class=live_graph_shop_timeline ";

            if (bAllShops) sShopClass = " class=live_graph_all_shops_timeline ";

            s.Append("<div " + sShopClass + " >");


            s.Append("<svg xmlns='http://www.w3.org/2000/svg' version='1.1' ");
            s.Append("width='" + w.ToString() + "' height='" + h.ToString() + "' > ");
        }       
        
        
        foreach (backoffice.admin_shop shop in shopList)
        {
            if (bAllShops == false)
            {
                s.Append("<tr>");
                s.Append("<td>");

                s.Append("<div id='shop_timeline_" + shop.iId.ToString() + "' class=live_shop>");
                s.Append(shop.sName);
                s.Append("</div>");

                s.Append("</td>");

                // s.Append("<td valign=top colspan=" + iNofSlices.ToString() + ">");
                s.Append("<td valign=top >");


                string sShopClass = " class=live_graph_shop_timeline ";

                if (bAllShops) sShopClass = " class=live_graph_all_shops_timeline ";

                s.Append("<div " + sShopClass + " >");


                s.Append("<svg xmlns='http://www.w3.org/2000/svg' version='1.1' ");
                s.Append("width='" + w.ToString() + "' height='" + h.ToString() + "' > ");
            }
            StringBuilder sShopEvents = new StringBuilder();

            int iNofNewMembers = 0;
            int iGoldMemberReturned = 0;
            backoffice.shop_event looper = shop.firstShopEvent;
            while (looper != null)
            {
                // 16 sept
                // Merkelig feil på testserver gjør at man må legge til en time ...
                bool bFixTestServerProblem = false;

                if (bFixTestServerProblem)
                {
                    if (looper.shopEventType.ToString().StartsWith("A"))
                    {
                        looper.timestamp = looper.timestamp.AddHours(1);
                    }
                }
                
                int xFromTime = GET_X_START_FROM_TIME(global, TOTAL_WIDTH, startFromTimestamp, looper.timestamp, iNofSlices);
                int startBasketY = ((h / 2) - hBasket / 2);

                int startBasketX = ((xFromTime) - wBasket / 2);

                int level_2 = (-(int) (double) (h*0.3));
                int level_1 = 0;
                int level_0 = (int)(double)(h * 0.3);
                
                string sFillColor = "rgb(128,128,128)";

                looper.xCenter = xFromTime;

                // string sHourMinuteSecond = zero_2(looper.timestamp.Hour.ToString()) + ":" + zero_2(looper.timestamp.Minute.ToString()) + ":" + zero_2(looper.timestamp.Second.ToString());
                string sHourMinuteSecond = getHourMinuteSecond(looper.timestamp);

                string sOnClick = ""; 
                string sSmallCircle = "";

                if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.C_MEMBERSHIP_ACCEPTED)
                {
                    sOnClick = " onclick=shop_live_accepted_membership_event('" + looper.sToken + "','" + looper.shopEventType.ToString() + "','" + sHourMinuteSecond + "') ";
                    looper.yCenter = level_2 + (h / 2);
                    // sFillColor = "rgb(0,255,0)";
                    sFillColor = "rgb(255,255,255)";
                    sSmallCircle = "<circle cx='" + xFromTime + "' cy='" + (looper.yCenter).ToString() + "' r='" + (2).ToString() + "' fill='rgb(255,255,255)' " +
                    sOnClick +
                    " /> ";
                    ++iNofNewMembers;
                    ++iNofNewMembersGrandTotal;
                }
                else if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.B_BASKET_CONFIRMED)
                {
                    sOnClick = " onclick=shop_live_basket_event('" + looper.sBasketRowId + "','" + looper.sToken + "','" + looper.shopEventType.ToString() + "','" + sHourMinuteSecond + "') ";
                    looper.yCenter = level_1 + (h / 2);
                    sFillColor = "rgb(0,255,0)";
                    if (!isBasketModified(looper.sBasket_b)) sFillColor = "rgb(255,0,0)"; // No discount
                    if (!isErrorBasket(looper.sBasket_b)) looper.bErrorBasket = true;
                }
                else if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.B_BASKET_NOT_CONFIRMED)
                {
                    sOnClick = " onclick=shop_live_basket_event('" + looper.sBasketRowId + "','" + looper.sToken + "','" + looper.shopEventType.ToString() + "','" + sHourMinuteSecond + "') ";
                    looper.yCenter = level_1 + (h / 2);
                    sFillColor = "rgb(0,255,0)";
                    if (!isBasketModified(looper.sBasket_b)) sFillColor = "rgb(255,0,0)"; // No discount
                    if (!isErrorBasket(looper.sBasket_b)) looper.bErrorBasket = true;
                }
                else if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_PHONE_ENROLLED)
                {
                    sOnClick = " onclick=shop_live_card_inserted_event('" + looper.sBaxId + "','" + looper.sToken + "','" + looper.shopEventType.ToString() + "','" + sHourMinuteSecond + "') ";
                    looper.yCenter = level_0 + (h / 2);
                    sFillColor = "rgb(0,255,0)";
                }
                else if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_PHONE_SKIPPED)
                {
                    sOnClick = " onclick=shop_live_card_inserted_event('" + looper.sBaxId + "','" + looper.sToken + "','" + looper.shopEventType.ToString() + "','" + sHourMinuteSecond + "') ";
                    looper.yCenter = level_0 + (h / 2);
                    sFillColor = "rgb(255,0,0)";
                }
                else if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_CONSUMER_EXISTS)
                {
                    sOnClick = " onclick=shop_live_card_inserted_event('" + looper.sBaxId + "','" + looper.sToken + "','" + looper.shopEventType.ToString() + "','" + sHourMinuteSecond + "') ";
                    looper.yCenter = level_0 + (h / 2);
                    sFillColor = "yellow";
                    if (looper.bAdditionalCard)
                    {
                        sSmallCircle = "<circle cx='" + xFromTime + "' cy='" + (looper.yCenter).ToString() + "' r='" + (2).ToString() + "' fill='purple' " +
                        sOnClick +
                        " /> ";
                    }
                }
                else if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_CONSUMER_EXISTS_AND_IS_MEMBER)
                {
                    sOnClick = " onclick=shop_live_card_inserted_event('" + looper.sBaxId + "','" + looper.sToken + "','" + looper.shopEventType.ToString() + "','" + sHourMinuteSecond + "') ";
                    looper.yCenter = level_0 + (h / 2);
                    
                    sFillColor = "rgb(255,255,255)";
                    ++iGoldMemberReturned;
                    ++iGoldMemberReturnedGlobal;
                }


                 
                sShopEvents.Append(
                "<circle cx='" + xFromTime + "' cy='" + (looper.yCenter).ToString() + "' r='" + (rCircle).ToString() + "' fill='" + sFillColor + "' " +
                    sOnClick +
                    " /> ");

                if (sSmallCircle != "")
                {
                    sShopEvents.Append(sSmallCircle);
                }

                if (looper.shopEventType == backoffice.SHOP_EVENT_TYPE.B_BASKET_NOT_CONFIRMED)
                {
                    sShopEvents.Append("<line " +
                               " x1=" + (xFromTime - rCircle).ToString() + " y1=" + looper.yCenter.ToString() +
                               " x2=" + (xFromTime + rCircle).ToString() + " y2=" + looper.yCenter.ToString() + " style='stroke:rgb(0,0,0);stroke-width:1' " + 
                               sOnClick + 
                               " />");
                }

                if (looper.bErrorBasket)
                {
                    sShopEvents.Append("<line " +
                               " x1=" + (xFromTime).ToString() + " y1=" + (looper.yCenter - rCircle).ToString() +
                               " x2=" + (xFromTime).ToString() + " y2=" + (looper.yCenter + rCircle).ToString() + " style='stroke:rgb(0,0,0);stroke-width:1' " +
                               sOnClick +
                               " />");
                }

                looper = looper.Next;
            }

            // Before closing the shop, we need to add all lines ...
            // Den vi skal peke på ligger lenger frem i tid ... det er bare å lete fremover til første match.

            s.Append(add_SVG_gRelationsFromCardInsertToBasket(shop));
            
            s.Append(add_SVG_gRelationsFromBasketToAcceptMembership(shop));

            s.Append(sShopEvents);


            if (bAllShops == false)
            {
                s.Append("</svg>");

                s.Append("</div>");
                s.Append("</td>");

                s.Append("<td class=live_timeline_sum >");

                // 10 juli
                if (iNofNewMembers > 0)
                    s.Append(iNofNewMembers.ToString());

                s.Append("</td>");

                s.Append("<td class=live_timeline_sum >");

                if (iGoldMemberReturned > 0)
                    s.Append(iGoldMemberReturned.ToString());

                s.Append("</td>");

                s.Append("</tr>");
            }
        }

        if (bAllShops)
        {
            s.Append("</svg>");

            s.Append("</div>");
            s.Append("</td>");

            s.Append("<td class=live_timeline_sum >");

            // 10 juli
            // if (iNofNewMembers > 0)
            //    s.Append(iNofNewMembers.ToString());

            s.Append("</td>");

            s.Append("<td class=live_timeline_sum >");

            // if (iGoldMemberReturned > 0)
            //    s.Append(iGoldMemberReturned.ToString());

            s.Append("</td>");

            s.Append("</tr>");
        }



        s.Append("</table>");

        s.Append("</div>");

        string sTotalText = (s.ToString().Replace("grand_total_to_be_replaced",iNofNewMembersGrandTotal.ToString()));

        sTotalText = (sTotalText.Replace("grand_total_gold_to_be_replaced", iGoldMemberReturnedGlobal.ToString()));


        return sTotalText;
    }

    public static string add_SVG_gRelationsFromCardInsertToBasket(backoffice.admin_shop shop)
    {
        StringBuilder s = new StringBuilder();
        backoffice.shop_event rightLooper = shop.firstShopEvent;
        while (rightLooper != null)
        {
            /* 
            A_PHONE_ENROLLED, 
                B_BASKET_NOT_CONFIRMED, 
                B_BASKET_CONFIRMED, 
                *C_MEMBERSHIP_ACCEPTED, 
                 
            A_PHONE_ALREADY_MEMBER, Can be a 2 or a 1
                B_BASKET_NOT_CONFIRMED, 
                B_BASKET_CONFIRMED, 
                *C_MEMBERSHIP_ACCEPTED, 

             A_PHONE_SKIPPED, 
                END

            B_BASKET_CONFIRMED,                 
                *C_MEMBERSHIP_ACCEPTED, 

            B_BASKET_NOT_CONFIRMED,                 
                *C_MEMBERSHIP_ACCEPTED, 
                 
             C_MEMBERSHIP_ACCEPTED, 
                END
             */

            // We can have more than one terminal, so we have to search to the end ...

            backoffice.shop_event anchorLooper = rightLooper.Next;
            while (anchorLooper != null)
            {
                if (rightLooper.sToken.IndexOf(anchorLooper.sToken) >= 0)
                {
                    if ((rightLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_PHONE_ENROLLED || rightLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_CONSUMER_EXISTS || rightLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_CONSUMER_EXISTS_AND_IS_MEMBER || rightLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.A_PHONE_SKIPPED) &&
                        (
                            anchorLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.B_BASKET_CONFIRMED ||
                            anchorLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.B_BASKET_NOT_CONFIRMED)
                         && rightLooper.higherPointer == null)
                    {
                        rightLooper.higherPointer = anchorLooper;
                        s.Append(
                           "<line " +
                           " x1=" + rightLooper.xCenter.ToString() + " y1=" + rightLooper.yCenter +
                           " x2=" + anchorLooper.xCenter.ToString() + " y2=" + anchorLooper.yCenter.ToString() + " style='stroke:rgb(255,255,255);stroke-width:1' />");
                        break;
                    }
                }
                anchorLooper = anchorLooper.Next;
            }

            rightLooper = rightLooper.Next;
        }
        return s.ToString();
    }

    public static string add_SVG_gRelationsFromBasketToAcceptMembership(backoffice.admin_shop shop)
    {
        StringBuilder s = new StringBuilder();
        backoffice.shop_event rightLooper = shop.firstShopEvent;
        while (rightLooper != null)
        {
            // a) We can have more than one terminal, so we have to search to the end ...
            // b) Membership can have been accepted earlier so we start even before the basket
            backoffice.shop_event anchorLooper = shop.firstShopEvent;
            while (anchorLooper != null)
            {
                if (anchorLooper != rightLooper) // Don't relate to yourself ...
                {
                    if (anchorLooper.sToken == rightLooper.sToken)
                    {
                        if ((rightLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.B_BASKET_CONFIRMED || rightLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.B_BASKET_NOT_CONFIRMED) &&
                                anchorLooper.shopEventType == backoffice.SHOP_EVENT_TYPE.C_MEMBERSHIP_ACCEPTED)
                                // && rightLooper.higherPointer == null)
                        {
                            rightLooper.higherPointer = anchorLooper;
                            s.Append(
                               "<line " +
                               " x1=" + rightLooper.xCenter.ToString() + " y1=" + rightLooper.yCenter +
                               " x2=" + anchorLooper.xCenter.ToString() + " y2=" + anchorLooper.yCenter.ToString() + " style='stroke:rgb(180,180,180);stroke-width:2' />");
                        }
                    }
                }
                anchorLooper = anchorLooper.Next;
            }

            rightLooper = rightLooper.Next;
        }
        return s.ToString();
    }



    public static string GET_LAST_EVENTS_PART()
    {
        StringBuilder s = new StringBuilder();
        s.Append("<div id=live_shop_single_events class=live_events>");
        s.Append("Hendelser");
        return s.ToString();
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
        return "MAX";
    }

    public static bool event_catched_and_performed(xAjax ajax, Global global)
    {
        bool bRetVal = true;

        string sProcedure = ajax.getProcedure();

        if (sProcedure == "level_2_live_start_show()")
        {
            StringBuilder s = new StringBuilder();

            string sLiveTable = build_live_table(global);

            s.Append("<div id=shop_live_main>");
            s.Append(sLiveTable);
            s.Append("</div>");

            ajax.WriteHtml("work_page", s.ToString());
        }
        else if (sProcedure == "shop_live_card_inserted_event()")
        {
            string sBax = ajax.getString("parameter_1");
            string sToken = ajax.getString("parameter_2");
            string sEventType = ajax.getString("parameter_3");
            string sHourMinuteSecond = ajax.getString("parameter_4");
            ajax.WriteHtml("live_shop_single_events", sBax + ";" + sToken + ";" + sEventType + ";" + sHourMinuteSecond);
            int t = 0;
        }
        else if (sProcedure == "shop_live_accepted_membership_event()")
        {
            string sToken = ajax.getString("parameter_1");
            string sEventType = ajax.getString("parameter_2");
            string sHourMinuteSecond = ajax.getString("parameter_3");

            ajax.WriteHtml("live_shop_single_events",sToken + ";" + sEventType + ";" + sHourMinuteSecond);
        }
        else if (sProcedure == "shop_live_basket_event()")
        {
            string sBasketRowId = ajax.getString("parameter_1");
            string sToken = ajax.getString("parameter_2");
            string sEventType = ajax.getString("parameter_3");
            string sHourMinuteSecond = ajax.getString("parameter_4");

            backoffice.admin_basket basket = global.www_backoffice().get_basket(sBasketRowId);
            string sModified = isBasketModified(basket.sBasket_B).ToString();
            ajax.WriteHtml("live_shop_single_events", sBasketRowId + ";" + sToken + ";" + sEventType + ";" + sHourMinuteSecond + ";Modifisert?" + sModified);
        }
        else if (sProcedure == "shop_live_date_clicked()")
        {
            global.bShopLiveTimeDialogIsOpen = true;
            ajax.WriteHtml(SHOP_LIVE_DATE_MODIFY.getContainerId(), SHOP_LIVE_DATE_MODIFY.B_get_maximized_dialog(global));
        }
        else if (sProcedure == "shop_live_date_clicked_cancel()")
        {
            global.bShopLiveTimeDialogIsOpen = false;
            ajax.WriteHtml("shop_live_main", build_live_table(global));
            // ajax.WriteHtml(SHOP_LIVE_DATE_MODIFY.getContainerId(), SHOP_LIVE_DATE_MODIFY.A_get_minimized_dialog(global.shop_live_current_timestamp,false));
        }

        else if (sProcedure == "shop_live_refresh_hours_and_to_hour()")
        {
            global.iHours = ajax.getInt("parameter_1");
            global.iToHour = ajax.getInt("parameter_2");
            ajax.WriteHtml("shop_live_main", build_live_table(global));
        }
        else if (sProcedure == "shop_live_refresh()")
        {
            int year = ajax.getInt("parameter_1");
            int month = ajax.getInt("parameter_2");
            int day = ajax.getInt("parameter_3");
            int iHours = ajax.getInt("parameter_4");
            int iToHours = ajax.getInt("parameter_5");
            global.shop_live_current_timestamp = new DateTime(year, month, day, iToHours, 0, 0);
            ajax.WriteHtml("shop_live_main", build_live_table(global));
        }
        else if (sProcedure == "shop_live_start_timer_job()")
        {
            // global.shop_live_current_timestamp = DateTime.Now.Subtract(new TimeSpan(178,0,0));
            global.shop_live_current_timestamp = DateTime.Now;




            global.iToHour = (int)global.shop_live_current_timestamp.Hour;
            global.iToMinute = (int)global.shop_live_current_timestamp.Minute;
            global.iToSecond = (int)global.shop_live_current_timestamp.Second;
            
            /*
            if (global.iToHour < 22)
                ++global.iToHour;
            else
                global.iToHour = 23;
            */

            global.iHours = global.iToHour - 10;

            if (global.iHours < 1) global.iHours = 1;

            global.bRunningLive = true;
            ajax.WriteHtml("shop_live_main", build_live_table(global));
        }
        else if (sProcedure == "shop_live_stop_timer_job()")
        {
            global.bRunningLive = false;
            ajax.WriteHtml("shop_live_main", build_live_table(global));
        }
        else
            bRetVal = false;

        return bRetVal;
    }
}


public static class SHOP_LIVE_DATE_MODIFY 
{
    public static string getContainerId()
    {
        return "shop_live_date_modify";
    }



    private static string getDayButton_TD_PART(DateTime currentDate, DateTime day)
    {
        string sLinkText = day.Day.ToString() + "." + day.Month.ToString();

        string sStyle = "font-size:12px;bold;background-color:rgb(240,240,240);text-align:center;width:100%;";
        if (currentDate.Year == day.Year && currentDate.Month == day.Month && currentDate.Day == day.Day)
            sStyle = "font-size:12px;bold;background-color:rgb(255,255,255);text-align:center;width:100%;";
        // else if ((int)day.DayOfWeek == 6 || (int)day.DayOfWeek == 0) sStyle = sStyle + "background-color:rgb(245,245,245);";
        
        string sLink = HTML_TOOLBOX.link_TWITTER_call_javascript_function(sLinkText, "shop_live_refresh(" + day.Year.ToString() + "," + day.Month.ToString() + "," + day.Day.ToString() + ",10,20)", 10, true,sStyle);


        string sTdStyle = "";
        if ((int)day.DayOfWeek == 6 || (int)day.DayOfWeek == 0) sTdStyle = "background-color:rgb(245,245,245);";

        return
            HTML_TOOLBOX.td_START_cell(sTdStyle) +
            sLink +
            HTML_TOOLBOX.td_END("");
    }

    private static string getHourToButton(int iHour)
    {
        return HTML_TOOLBOX.button_GOOGLE(iHour.ToString(), 10, 3, 3, 3, 3, "alert('Hei')");
    }

    private static string getHoursSinceButton(int iHours)
    {
        return HTML_TOOLBOX.button_GOOGLE(iHours.ToString(), 10, 3, 3, 3, 3, "alert('Hei')");
    }

    public static string A_get_minimized_dialog(DateTime dateTime, bool bIncludeContainerWrap)
    {
        StringBuilder sb = new StringBuilder();

        if (bIncludeContainerWrap)
            sb.Append("<div style='display:none;' id=" + getContainerId() + " class=xxxxxxxlive_timeline_heading >");

        if (bIncludeContainerWrap)
            sb.Append("</div>");

        return sb.ToString();
    }

    public static string B_get_maximized_dialog(Global global)
    {

        string sTextInputId = getContainerId() + "_text";
        string sOldValue = global.shop_live_current_timestamp.ToString();

        StringBuilder sb = new StringBuilder();

        // ******** overflow:hidden; Important, it makes all the tables wrap INSIDE the div !!! **************
        sb.Append("<div style='overflow:hidden;ccccccborder:2px solid rgb(200,200,200);xxxxxborder-radius:6px;xxxxxxxbackground-color:rgb(100,100,100);'>"); // Rundt alle tabellene


        sb.Append("<div style='float:left;padding:10px;'>");
        sb.Append(HTML_TOOLBOX.table_START_with_class("live_date_class"));

        sb.Append(HTML_TOOLBOX.tr_START("Timer bakover"));
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        sb.Append("Antall timer bakover");
        sb.Append(HTML_TOOLBOX.td_END(""));

        
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));

        StringBuilder select_hours = new StringBuilder();
        select_hours.Append("<select id=nof_hours onchange=shop_live_refresh_hours_and_to_hour() style='font-size:10px;font-family:arial;'>");
        select_hours.Append("<option value=1>1 time</option>");
        select_hours.Append("<option value=2>2 timer</option>");
        select_hours.Append("<option value=3>3 timer</option>");
        select_hours.Append("<option value=4>4 timer</option>");
        select_hours.Append("<option value=5>5 timer</option>");
        select_hours.Append("<option value=6>6 timer</option>");
        select_hours.Append("<option value=7>7 timer</option>");
        select_hours.Append("<option value=8>8 timer</option>");
        select_hours.Append("<option value=9>9 timer</option>");
        select_hours.Append("<option value=10>10 timer</option>");
        select_hours.Append("<option value=11>11 timer</option>");
        select_hours.Append("<option value=12>12 timer</option>");
        select_hours.Append("<option value=24>24 timer</option>");
        select_hours.Append("<option value=48>2 dager</option>");
        select_hours.Append("<option value=72>3 dager</option>");
        select_hours.Append("<option value=96>4 dager</option>");
        select_hours.Append("<option value=120>5 dager</option>");
        select_hours.Append("<option value=144>6 dager</option>");
        select_hours.Append("<option value=168>1 uke</option>");
        select_hours.Append("</select>");

        string sss = select_hours.ToString();

        sss = sss.Replace("value=" + global.iHours.ToString() + ">", " selected value=" + global.iHours.ToString() + ">");

        sb.Append(sss);
        sb.Append(HTML_TOOLBOX.td_END(""));


        /*
        for (int iHour = 1; iHour <= 24; ++iHour)
        {
            sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
            sb.Append(getHoursSinceButton(iHour));
            sb.Append(HTML_TOOLBOX.td_END(""));
        }
        */
        sb.Append(HTML_TOOLBOX.tr_END(""));
        sb.Append(HTML_TOOLBOX.table_END(""));
        sb.Append("</div>");


        sb.Append("<div style='float:left;padding:14px;padding-left:5px;'>"); // Tilogmed klokken
        
        if (global.bRunningLive)
            sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Stop live data!", "shop_live_stop_timer_job()", 14, true, "padding-right:10px;"));
        else
            sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Live data!", "shop_live_start_timer()", 14, true, "padding-right:10px;"));
        
        sb.Append("</div>"); // Tilogmed klokken


        sb.Append("<div style='float:left;padding:10px;'>"); // Tilogmed klokken
        sb.Append(HTML_TOOLBOX.table_START_with_class("live_date_class"));
        sb.Append(HTML_TOOLBOX.tr_START("Tilogmed klokken"));
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        sb.Append("Inntil klokken");
        sb.Append(HTML_TOOLBOX.td_END(""));

        // sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        // sb.Append(getHourToButton(0));
        // sb.Append(HTML_TOOLBOX.td_END(""));

        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));

        StringBuilder sSelect = new StringBuilder();
        sSelect.Append("<select id=to_hour onchange=shop_live_refresh_hours_and_to_hour() style='font-size:10px;font-family:arial;'>");
        sSelect.Append("<option value=10>10:00</option>");
        sSelect.Append("<option value=11>11:00</option>");
        sSelect.Append("<option value=12>12:00</option>");
        sSelect.Append("<option value=13>13:00</option>");
        sSelect.Append("<option value=14>14:00</option>");
        sSelect.Append("<option value=15>15:00</option>");
        sSelect.Append("<option value=16>16:00</option>");
        sSelect.Append("<option value=17>17:00</option>");
        sSelect.Append("<option value=18>18:00</option>");
        sSelect.Append("<option value=19>19:00</option>");
        sSelect.Append("<option value=20>20:00</option>");
        sSelect.Append("<option value=21>21:00</option>");
        sSelect.Append("<option value=22>22:00</option>");
        sSelect.Append("<option value=23>23:00</option>");
        sSelect.Append("</select>");

        string sss2 = sSelect.ToString();

        sss2 = sss2.Replace("value=" + global.iToHour.ToString() + ">", " selected value=" + global.iToHour.ToString() + ">");

        sb.Append(sss2);
        sb.Append(HTML_TOOLBOX.td_END(""));

        /*
        for (int iHour = 10; iHour <= 24; ++iHour)
        {
            sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
            sb.Append(getHourToButton(iHour));
            sb.Append(HTML_TOOLBOX.td_END(""));
        }
        */
        sb.Append(HTML_TOOLBOX.tr_END(""));
        sb.Append(HTML_TOOLBOX.table_END(""));
        sb.Append("</div>"); // end tilogmedklokken




        sb.Append("<div style='float:left;padding:14px;;padding-left:5px;'>"); // Tilogmed klokken
        sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Skjul dialog", "shop_live_date_clicked_cancel()", 14, true, "padding-right:10px;"));
        sb.Append("</div>"); // Tilogmed klokken







        sb.Append("<div style='clear:both;padding:10px;'>");
        sb.Append(HTML_TOOLBOX.table_START_with_class("xxxxxxxxxxxxxxxxxxxxxxxlive_date_class"));

        sb.Append(HTML_TOOLBOX.tr_START("Ukedag"));
        
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        // sb.Append(HTML_TOOLBOX.button_GOOGLE("Tilbake",14,10,10,10,10, "shop_live_date_clicked_cancel()"));
        // sb.Append(HTML_TOOLBOX.link_TWITTER_call_javascript_function("Live data!", "shop_live_date_clicked_cancel()", 14, true,"padding-right:10px;"));
        sb.Append(HTML_TOOLBOX.td_END(""));

        DateTime now = DateTime.Now; 
        
        if (global.bDEBUG_TESTING_OLD_DATABASE)
        {
            now = new DateTime(2015, 6, 30, 20, 0, 0);
        }


        int iDayNoToday = (int)now.DayOfWeek;


        string sCellStyle = "font-size:12px;bold;background-color:rgb(240,240,240);padding-right:10px;padding-left:10px;";
        sb.Append(HTML_TOOLBOX.td_START_cell(sCellStyle)); sb.Append("Mandag"); sb.Append(HTML_TOOLBOX.td_END(""));
        sb.Append(HTML_TOOLBOX.td_START_cell(sCellStyle)); sb.Append("Tirsdag"); sb.Append(HTML_TOOLBOX.td_END(""));
        sb.Append(HTML_TOOLBOX.td_START_cell(sCellStyle)); sb.Append("Onsdag"); sb.Append(HTML_TOOLBOX.td_END(""));
        sb.Append(HTML_TOOLBOX.td_START_cell(sCellStyle)); sb.Append("Torsdag"); sb.Append(HTML_TOOLBOX.td_END(""));
        sb.Append(HTML_TOOLBOX.td_START_cell(sCellStyle)); sb.Append("Fredag"); sb.Append(HTML_TOOLBOX.td_END(""));
        sb.Append(HTML_TOOLBOX.td_START_cell(sCellStyle)); sb.Append("Lørdag"); sb.Append(HTML_TOOLBOX.td_END(""));
        sb.Append(HTML_TOOLBOX.td_START_cell(sCellStyle)); sb.Append("Søndag"); sb.Append(HTML_TOOLBOX.td_END(""));                

        sb.Append(HTML_TOOLBOX.tr_END(""));


        sb.Append(HTML_TOOLBOX.tr_START("Ukedag"));

        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        sb.Append("Denne uken");
        sb.Append(HTML_TOOLBOX.td_END(""));

        // Finn mandag denne uken og finn ut hvilken dato det er ...
        DateTime thisMonday = now.AddDays(1 - (iDayNoToday));
        for (int iWeekDay = 1; iWeekDay <= iDayNoToday; ++iWeekDay)
        {
            DateTime day = thisMonday.AddDays(iWeekDay-1);
            sb.Append(getDayButton_TD_PART(global.shop_live_current_timestamp, day));
        }
        sb.Append(HTML_TOOLBOX.tr_END(""));


        // Finn mandag Forrige uke og finn ut hvilken dato det er ...
        DateTime thisMondayMinus_1_week = now.AddDays(1 - (iDayNoToday) - 7);
        sb.Append(HTML_TOOLBOX.tr_START("Ukedag"));
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        sb.Append("Forrige uke");
        sb.Append(HTML_TOOLBOX.td_END(""));

        for (int iWeekDay = 1; iWeekDay <= 7; ++iWeekDay)
        {
            DateTime day = thisMondayMinus_1_week.AddDays(iWeekDay-1);
            sb.Append(getDayButton_TD_PART(global.shop_live_current_timestamp,day));
        }

        sb.Append(HTML_TOOLBOX.tr_END(""));

        // Finn mandag Forrige Forrige uke og finn ut hvilken dato det er ...
        DateTime thisMondayMinus_2_weeks = now.AddDays(1 - (iDayNoToday) - 14);
        sb.Append(HTML_TOOLBOX.tr_START("Ukedag"));
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        sb.Append("Uken før det");
        sb.Append(HTML_TOOLBOX.td_END(""));

        for (int iWeekDay = 1; iWeekDay <= 7; ++iWeekDay)
        {
            DateTime day = thisMondayMinus_2_weeks.AddDays(iWeekDay-1);
            sb.Append(getDayButton_TD_PART(global.shop_live_current_timestamp,day));
        }
        sb.Append(HTML_TOOLBOX.tr_END(""));

        // Finn mandag Forrige Forrige uke og finn ut hvilken dato det er ...
        DateTime thisMondayMinus_3_weeks = now.AddDays(1 - (iDayNoToday) - 21);
        sb.Append(HTML_TOOLBOX.tr_START("Ukedag"));
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        sb.Append("Uken før det ...");
        sb.Append(HTML_TOOLBOX.td_END(""));

        for (int iWeekDay = 1; iWeekDay <= 7; ++iWeekDay)
        {
            DateTime day = thisMondayMinus_3_weeks.AddDays(iWeekDay - 1);
            sb.Append(getDayButton_TD_PART(global.shop_live_current_timestamp, day));
        }
        sb.Append(HTML_TOOLBOX.tr_END(""));



        // Finn mandag Forrige Forrige uke og finn ut hvilken dato det er ...
        DateTime thisMondayMinus_4_weeks = now.AddDays(1 - (iDayNoToday) - 28);
        sb.Append(HTML_TOOLBOX.tr_START("Ukedag"));
        sb.Append(HTML_TOOLBOX.td_START_cell("font-size:12px;bold;"));
        sb.Append("Uken før det ... ...");
        sb.Append(HTML_TOOLBOX.td_END(""));

        for (int iWeekDay = 1; iWeekDay <= 7; ++iWeekDay)
        {
            DateTime day = thisMondayMinus_4_weeks.AddDays(iWeekDay - 1);
            sb.Append(getDayButton_TD_PART(global.shop_live_current_timestamp, day));
        }
        sb.Append(HTML_TOOLBOX.tr_END(""));



        sb.Append(HTML_TOOLBOX.table_END(""));
        sb.Append("</div>"); // Slutt på datovalg


        sb.Append("</div>");
        return sb.ToString();
    }

}



