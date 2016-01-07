
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;


using abacolla_gui;


public partial class tilbud : System.Web.UI.Page
{
    OuterBankEventController sa_eventController = new OuterBankEventController();

    protected void Page_Load(object sender, EventArgs e)
    {
        sa_eventController.initializeAjaxController(Request, Session, Response, Server);
        sa_eventController.handlePageEvents();
    }
}


public class OuterBankEventController : BankEventController
{

    public override bool handlePageEvents()
    {
        return true;
    }

}
