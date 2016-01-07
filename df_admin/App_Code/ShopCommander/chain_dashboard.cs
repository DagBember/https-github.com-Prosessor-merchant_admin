using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;

/// <summary>
/// Summary description for chain_dashboard
/// </summary>
/// 

public enum CHART_TYPE { bar_vertical, bar_horisontal, pie, pie_donut, line,line_3_lines,line_4_lines, bember_heading_and_percent }

public class CHAIN_DASHBOARD_ITEM : SHOP_BASE
{
	public CHAIN_DASHBOARD_ITEM()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public static string getContainerId(string sId)
    {
        return "dashboard_item_container_" + sId;
    }


    // Produces a minimized dashboard report ...
    public static string A_get_minimized_dialog(string sId, string sJavascriptCall,string sInnerText)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(HTML_TOOLBOX.infobox_TWITTER_clickable(sId, sJavascriptCall, sInnerText, "Klikk her for å se rapport", 14, 250, 150, 5, 5, 5, 5, "margin:10px;"));

        return sb.ToString();
    }

    public static string B_get_maximized_dialog(Global global, string sId)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("Maximized");

        return sb.ToString();
    }

}

