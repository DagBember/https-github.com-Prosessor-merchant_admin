using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using System.Globalization;
using System.Threading;


using abacolla_gui;


public enum bank_image_type { start_commercial, end_steg_1, end_steg_2, end_steg_3, lmc_logo, commercial }
public abstract class BankEventController : xEventController
{


    public BankGlobal global;

    public bool stop_intruder()
    {
        if (global == null) return true;
        if (global.bLoggedIn == false)
        {
            return true;
        }
        return false;
    }





    private string get_image(bank_image_type imageType, int iSize)
    {
        if (imageType == bank_image_type.end_steg_1) return "<image width='170px;' src='css/bank/images/" + iSize.ToString() + "x/steg-1.png'>";
        else if (imageType == bank_image_type.end_steg_2) return "<image width='170px;' src='css/bank/images/" + iSize.ToString() + "x/steg-2.png'>";
        else if (imageType == bank_image_type.end_steg_3) return "<image width='170px;' src='css/bank/images/" + iSize.ToString() + "x/steg-3.png'>";
        else if (imageType == bank_image_type.lmc_logo) return "<image src='css/bank/images/" + iSize.ToString() + "x/lmc_logo.png'>";
        else if (imageType == bank_image_type.commercial) return "<image width='470px;' src='css/bank/images/" + iSize.ToString() + "x/reklamebilde.png'>";
        return "";
    }

    private string phone_saved()
    {
        StringBuilder sb = new StringBuilder();

        string s1 =
        "Velg 'JA' på terminalen når " +
        "du blir spurt om du ønsker " +
        "rabatt.";

        string s2 =
        "Tast ditt mobilnummer slik " +
        "at vi kan identifisere deg. " +
        "Din rabatt vil bli automatisk " +
        "trukket.";

        string s3 =
        "Fullfør betalingen på vanlig måte.";

        // #257e62

        sb.Append("<table align=center><tr><td>");
        sb.Append("<div class=global_frame>");

        sb.Append("<table id=main_table_page_2 align=center class=main_table_outline_free>");

        sb.Append("<tr>");
        sb.Append("<td colspan=2 class=bember_thanks_field>");
        sb.Append("<div  width='100%'>");
        sb.Append("Takk!<br><br>");
        sb.Append("Ditt mobilnummer er nå registrert  hos oss, og ");
        sb.Append("du vil få 20% på alt du handler hos oss i ");
        sb.Append("november.");
        sb.Append("</div>");
        sb.Append("</td>");
        sb.Append("</tr>");


        sb.Append("<tr>");
        sb.Append("<td colspan=2>");

        sb.Append("<div style='margin-top:25px;position:relative;'>");
        sb.Append("<div class=line_100>");
        sb.Append("</div>");
        sb.Append("<div class=i_butikken_heading>");
        sb.Append("I BUTIKKEN");
        sb.Append("</div>");
        sb.Append("</div>"); // End relative

        sb.Append("</td>");
        sb.Append("</tr>");



        sb.Append("<tr>");
        sb.Append("<td colspan=2>");

        sb.Append("<div>");
        sb.Append("<br>");
        sb.Append("</div>");

        sb.Append("</td>");
        sb.Append("</tr>");

        sb.Append("<tr>");



        sb.Append("<td colspan=2>");

        sb.Append("<div class=i_butikken>");
        sb.Append("Når du skal betale for en vare hos LMC, følger du disse ");
        sb.Append("instruksjonene for å få rabatten fratrukket.");
        sb.Append("</div>");


        sb.Append("</td>");
        sb.Append("</tr>");


        sb.Append("<tr>");
        sb.Append("<td>");
        sb.Append(get_image(bank_image_type.end_steg_1, 1));
        sb.Append("</td>");

        sb.Append("<td >");
        sb.Append("<div class=bember_end_col_2>");
        sb.Append(s1);
        sb.Append("</div>");
        sb.Append("</td>");


        sb.Append("</tr>");
        sb.Append("<tr>");
        sb.Append("<td>");
        sb.Append(get_image(bank_image_type.end_steg_2, 1));
        sb.Append("</td>");

        sb.Append("<td  >");
        sb.Append("<div class=bember_end_col_2>");
        sb.Append(s2);
        sb.Append("</div>");
        sb.Append("</td>");

        sb.Append("</tr>");
        sb.Append("<tr>");
        sb.Append("<td>");
        sb.Append(get_image(bank_image_type.end_steg_3, 1));
        sb.Append("</td>");

        sb.Append("<td >");
        sb.Append("<div class=bember_end_col_2>");
        sb.Append(s3);
        sb.Append("</div>");
        sb.Append("</td>");

        sb.Append("</tr>");



        sb.Append("<tr>");
        sb.Append("<td colspan=2>");

        sb.Append("<div style='margin-top:25px;position:relative;'>");
        sb.Append("<div class=line_100>");
        sb.Append("</div>");
        sb.Append("<div class=i_butikken_heading>");
        sb.Append("FINN EN LMC BUTIKK");
        sb.Append("</div>");
        sb.Append("</div>"); // End relative

        sb.Append("</td>");
        sb.Append("</tr>");


        sb.Append("<tr height=0>");
        sb.Append("<td colspan=2>");
        sb.Append("<div style='color:rgb(0,0,0);'>");
        sb.Append(""); // This one gets eaten ...
        sb.Append("</div>");
        sb.Append("</td>");
        sb.Append("</tr>");

        sb.Append(addFylke_TR("OSLO"));
        sb.Append(addShop_TR("LMC Karl Johan", "Ligger i sentrum"));
        sb.Append(addShop_TR("LMC Sentrum", "Ligger også i sentrum"));

        sb.Append(addFylke_TR("AKERSHUS"));
        sb.Append(addShop_TR("LMC Metro", "Ligger i Lørenskog"));



        sb.Append("</table>");
        sb.Append("</div>");

        sb.Append("</td");
        sb.Append("</tr");
        sb.Append("</table");

        return sb.ToString();
    }

    private string addFylke_TR(string sFylke)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<tr>");
        sb.Append("<td colspan=2>");
        sb.Append("<div class=lmc_area>");
        sb.Append(sFylke);
        sb.Append("</div>");
        sb.Append("</td>");
        sb.Append("</tr>");
        return sb.ToString();
    }

    private string addShop_TR(string sName, string sText)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<tr>");
        sb.Append("<td colspan=2>");
        sb.Append("<div class=lmc_shop>");
        sb.Append(sName);
        sb.Append("<br>");
        sb.Append("<span class=lmc_shop_text>");
        sb.Append(sText);
        sb.Append("</span>");

        sb.Append("</div>");
        sb.Append("</td>");
        sb.Append("</tr>");
        return sb.ToString();
    }


    private string phone_not_saved()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<br>");
        sb.Append("Noe gikk galt");
        sb.Append("<br>");

        return sb.ToString();
    }

    private string get_start_form(string sPhone)
    {
        StringBuilder sb = new StringBuilder();


        sb.Append("<table align=center><tr><td>");
        sb.Append("<div class=global_frame>");


        sb.Append("<table id=main_table_page_1 align=center class=main_table_outline_free>");
        sb.Append("<tr>");
        sb.Append("<td>");

        sb.Append("<div style='position:relative;height:50px;'>");
        // sb.Append("<div style='position:absolute;top:0px;left:0px;'>");
        sb.Append("<div style='position:relative;top:0;left:0;'>");
        sb.Append(get_image(bank_image_type.commercial, 1)); // main_table_outline_max_width_pixels.width
        sb.Append("</div>");
        sb.Append("<div style='position:absolute;top:120px;left:10px;'>");
        sb.Append(get_image(bank_image_type.lmc_logo, 1));
        sb.Append("</div>");

        sb.Append("</div>"); // End relative

        // sb.Append("<img src='layout/images/idea.png'>");
        sb.Append("</td>");
        sb.Append("</tr>");




        sb.Append("<tr height=116>");
        sb.Append("<td>");
        sb.Append("</td>");
        sb.Append("</tr>");



        sb.Append("<tr>");
        sb.Append("<td>");
        sb.Append("<div class=intro_text_frame_01>");
        sb.Append("<br><br>20 % på alt i butikken<br><br>");

        sb.Append("<div class=intro_text_02>");
        sb.Append("Ja takk, jeg vil gjerne ha 20 % på alle mine kjøp hos LMC i november. Gjelder også allerede nedsatte varer.<br><br>");
        sb.Append("</div>");

        sb.Append("<div class=intro_text_03>");
        sb.Append("Tast inn mobilnummeret ditt i feltet under.<br>");
        sb.Append("Vi benytter det til å gjenkjenne deg i kassen.");
        sb.Append("</div>");
        sb.Append("</div>"); // End intro_text_frame_01

        sb.Append("</td>");
        sb.Append("</tr>");


        sb.Append("<tr>");

        sb.Append("<td>");
        sb.Append("<div class=phone_wrapper>");
        if (sPhone == "")
            sb.Append("<input class=phone_input type=text  maxlength=8  value='Tast inn ditt mobilnummer' id=bank_phone onkeyup=testInteger(this,event); />");
        else
            sb.Append("<input class=phone_input type=text  maxlength=8  value='" + sPhone + "' id=bank_phone onkeyup=testInteger(this,event); />");
        sb.Append("</div>");
        sb.Append("</td>");

        sb.Append("</tr>");
        sb.Append("<tr>");
        sb.Append("<td align=center>");

        sb.Append("<div class=send_phone_button_passive id=send_phone_button onclick=send_phone() >FÅ RABATT</div>");

        sb.Append("</td>");
        sb.Append("</tr>");
        sb.Append("</table>");


        sb.Append("</div>");
        sb.Append("</table>");

        BankDatabaseService bankService = new BankDatabaseService();
        bool bOK = bankService.save_page_view();

        return sb.ToString();
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


            if (ajax.sProcedure == "init_ajax_web_form()")
            {
                ajax.setProcedure("init_bank()");

                StringBuilder sb = new StringBuilder();

                sb.Append(get_start_form(""));

                ajax.WriteHtml("bank_content", sb.ToString());
                ajax.WriteXmlToClient();
            }

            else if (ajax.sProcedure == "send_phone()")
            {
                string sPhone = ajax.getString("parameter_1");

                if (sPhone.Trim().Length != 8)
                {
                    ajax.WriteHtml("bank_content", get_start_form(sPhone));
                }
                else
                {
                    BankDatabaseService bankDatabase = new BankDatabaseService();

                    bool bExists = bankDatabase.bankPhoneExist(sPhone);

                    bankDatabase.save_phone(sPhone);

                    ajax.WriteVariable("status", "true");
                    ajax.WriteHtml("bank_content", phone_saved());
                }
                ajax.WriteXmlToClient();
                return;
            }

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

                global = (BankGlobal)www.fwGetSessionVariable(sGlobalAjaxPrefix + "_global");
                if (global == null)
                {
                    global = new BankGlobal();
                    www.fwSetSessionVariable(sGlobalAjaxPrefix + "_global", global);
                    ajax.WriteVariable("initiating", "true");
                    return;
                }
                return;
            }

            // C) session - prefix. Just Go On
            if (!isBlank(sGlobalSessionPrefix))
            {
                global = (BankGlobal)www.fwGetSessionVariable(sGlobalSessionPrefix + "_global");
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

}



