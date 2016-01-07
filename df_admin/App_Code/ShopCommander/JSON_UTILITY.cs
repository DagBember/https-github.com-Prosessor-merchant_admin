using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for JSON_UTILITY
/// </summary>
public static  class JSON_UTILITY
{

    public static string get_chart_global_info(string sDashboardTitle, int w, int h, int iNofDashboard)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("{");

        sb.Append("'chart_global_info':");

        sb.Append("{");
        sb.Append("'title': '" + sDashboardTitle + "',");
        sb.Append("'dashboard_count': '" + iNofDashboard.ToString() + "',");
        sb.Append("'w': '" + w.ToString() + "',");
        sb.Append("'h': '" + h.ToString() + "'");
        sb.Append("}");

        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'", "\"");

        return sRetVal;
    }

    public static string get_month_data(List<backoffice.dim_value> monthList)
    {
        StringBuilder sb = new StringBuilder();
        bool bFirst = true;

        sb.Append("{");

        sb.Append("'graph_data':");
        sb.Append("[");

        foreach (backoffice.dim_value month in monthList)
        {
            if (bFirst == false) sb.Append(",");
            bFirst = false;

            sb.Append("{");
            sb.Append("'name': '" + month.sDim + "',");
            sb.Append("'value': '" + month.sValue + "',");
            sb.Append("'value2': '51'"); // 24 sept
            sb.Append("}");
        }

        sb.Append("]");
        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'","\"");

        return sRetVal;
    }

    public static string get_conversion_data_3_lines(List<backoffice.name_value_value_value_value> monthList) // 24 sept
    {
        StringBuilder sb = new StringBuilder();
        bool bFirst = true;

        sb.Append("{");

        sb.Append("'graph_data':");
        sb.Append("[");

        foreach (backoffice.name_value_value_value_value month in monthList)
        {
            if (bFirst == false) sb.Append(",");
            bFirst = false;

            sb.Append("{");
            sb.Append("'name': '" + month.sName + "',");
            sb.Append("'value': '" + month.sValue1 + "',");
            sb.Append("'value2': '" + month.sValue2 + "',");
            sb.Append("'value3': '" + month.sValue3 + "'");
            sb.Append("}");
        }

        sb.Append("]");
        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'", "\"");

        return sRetVal;
    }

    public static string get_conversion_data_4_lines(List<backoffice.name_value_value_value_value> monthList) // 24 sept
    {
        StringBuilder sb = new StringBuilder();
        bool bFirst = true;

        sb.Append("{");

        sb.Append("'graph_data':");
        sb.Append("[");

        foreach (backoffice.name_value_value_value_value month in monthList)
        {
            if (bFirst == false) sb.Append(",");
            bFirst = false;

            sb.Append("{");
            sb.Append("'name': '" + month.sName + "',");
            sb.Append("'value': '" + month.sValue1 + "',");
            sb.Append("'value2': '" + month.sValue2 + "',");
            sb.Append("'value3': '" + month.sValue3 + "',");
            sb.Append("'value4': '" + month.sValue4 + "'");
            sb.Append("}");
        }

        sb.Append("]");
        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'", "\"");

        return sRetVal;
    }


    public static string get_two_dim_one_value()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("{");
        //sb.Append("'two_dim_one_value':");
         // sb.Append("{");


        sb.Append("'graph_data_dim_1':");
        sb.Append("[");
        sb.Append("{'name':'Year','type':'string'},");
        sb.Append("{'name':'Sales','type':'number'},");
        sb.Append("{'name':'Expences','type':'number'},");
        sb.Append("{'name':'Profit','type':'number'}");
        sb.Append("],");

        /* OK
        sb.Append("'graph_data_dim_1':");
        sb.Append("[");
        sb.Append("{'0':{'name':'Year','type':'string'}},");
        sb.Append("{'1':{'name':'Sales','type':'number'}},");
        sb.Append("{'2':{'name':'Expences','type':'number'}},");
        sb.Append("{'3':{'name':'Profit','type':'number'}}");
        sb.Append("],");
        */

        /* OK
        sb.Append("'graph_data_dim_1':");
        sb.Append("[");
        sb.Append("{'name':'Year'},");
        sb.Append("{'name':'Sales'},");
        sb.Append("{'name':'Expences'},");
        sb.Append("{'name':'Profit'}");
        sb.Append("],");
        */
        
        /* OK
        sb.Append("'graph_data_dim_1':");
        sb.Append("[");
        sb.Append("{'Year':'string'},");
        sb.Append("{'Sales':'number'},");
        sb.Append("{'Expences':'number'},");
        sb.Append("{'Profit':'number'}");
        sb.Append("],");
        */

        /* OK
        sb.Append("'graph_data_dim_1':");
        sb.Append("{");
        sb.Append("'0':['Year', 'string'],");
        sb.Append("'1':['Sales', 'number'],");
        sb.Append("'2':['Expences', 'number'],");
        sb.Append("'3':['Profit', 'number']");
        sb.Append("},");
        */




        sb.Append("'graph_data_dim_2':");
        sb.Append("[");
        sb.Append("{'name':'2013','values':[{'value':'11'},{'value':'22'},{'value':'33'}]},");
        sb.Append("{'name':'2014','values':[{'value':'111'},{'value':'22'},{'value':'133'}]},");
        sb.Append("{'name':'2015','values':[{'value':'143'},{'value':'22'},{'value':'33'}]}");
        sb.Append("]");

        
        /* OK
        sb.Append("'graph_data_dim_2':");
        sb.Append("{");
        sb.Append("'0':['2014', '1000','400','50'],");
        sb.Append("'1':['2015', '700','200','200'],");
        sb.Append("'2':['2016', '500','100','400']");
        sb.Append("}");
        */
        
        //sb.Append("}");
        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'", "\"");

        return sRetVal;

    }


    public static string get_chart_graphic(string sTitle, int w, int h, CHART_TYPE type, string sHaxis, string sVaxis)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("{");

        sb.Append("'chart_graphic':");
        // sb.Append("[");

        sb.Append("{");
        sb.Append("'w': '" + w.ToString() + "',");
        sb.Append("'h': '" + h.ToString() + "',");
        sb.Append("'type': '" + type.ToString() + "',");
        sb.Append("'title': '" + sTitle + "',");
        sb.Append("'h_axis': '" + sHaxis + "',");
        sb.Append("'v_axis': '" + sVaxis + "'");
        sb.Append("}");

        // sb.Append("]");
        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'", "\"");

        return sRetVal;
    }


    public static string get_one_dim_four_values(String sDim, string sValue1, string sValue2, string sValue3, string sValue4)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("{");

        sb.Append("'columns':");
        // sb.Append("[");

        sb.Append("{");
        sb.Append("'dim_1': '" + sDim + "',");
        sb.Append("'value_1': '" + sValue1 + "',");
        sb.Append("'value_2': '" + sValue2 + "',");
        sb.Append("'value_3': '" + sValue3 + "',");
        sb.Append("'value_4': '" + sValue4 + "'");
        sb.Append("}");

        // sb.Append("]");
        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'", "\"");

        return sRetVal;
    }


    public static string get_one_dim_three_values(String sDim, string sValue1, string sValue2, string sValue3)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("{");

        sb.Append("'columns':");
        // sb.Append("[");

        sb.Append("{");
        sb.Append("'dim_1': '" + sDim + "',");
        sb.Append("'value_1': '" + sValue1 + "',");
        sb.Append("'value_2': '" + sValue2 + "',");
        sb.Append("'value_3': '" + sValue3 + "'");
        sb.Append("}");

        // sb.Append("]");
        sb.Append("}");

        string sRetVal = sb.ToString().Replace("'", "\"");

        return sRetVal;
    }


}