using System;
using System.Collections.Generic;
using System.Xml;
using System.Web;

/// <summary>
/// Summary description for User
/// </summary>



public class User
{
	public User()
	{
	}
    public string sId = "";
    public string sFirstName = "";
    public string sLastName = "";
    public string sMailAddress = "";
    public string sPhone = "";

    public static string getUserForm(User user)
    {
        xjcString s = new xjcString();
        
        // *******************************************************************************************
        // ******** Start user form ******************************************************************
        // *******************************************************************************************
        s.append(GlobalGui.html_table_row_start());

        s.append(GlobalGui.html_table_col_start(0));
        s.append(GlobalGui.html_getInputField("Fornavn", "first_name", 30, 30, (user!=null) ? user.sFirstName : ""));
        s.append(GlobalGui.html_table_col_end());

        s.append(GlobalGui.html_table_col_start(100));
        s.append(GlobalGui.html_getInputField("Etternavn", "last_name", 30, 30,(user!=null) ? user.sLastName : ""));
        s.append(GlobalGui.html_table_col_end());

        s.append(GlobalGui.html_table_row_end());

        s.append(GlobalGui.html_table_row_empty(2));

        s.append(GlobalGui.html_table_row_start());

        s.append(GlobalGui.html_table_col_start(0));
        s.append(GlobalGui.html_getInputField("ePost", "mail_address", 40, 40,(user!=null) ? user.sMailAddress : ""));
        s.append(GlobalGui.html_table_col_end());

        s.append(GlobalGui.html_table_col_start(100));
        s.append(GlobalGui.html_getInputField("Telefon", "phone", 15, 15, (user!=null) ? user.sPhone : ""));
        s.append(GlobalGui.html_table_col_end());

        s.append(GlobalGui.html_table_row_end());

        // *******************************************************************************************
        // ******** End user form ******************************************************************
        // *******************************************************************************************
        return s.toString();
    }

    public static User getUserFromXmlDoc(XmlDocument doc)
    {
        bool bResult = DinFordelXml.getXmlResult(doc);
        if (bResult == false) return null;

        try
        {
            XmlNode node = DinFordelXml.getFirstNode(doc);
            User user = new User();
            user.sFirstName = node.Attributes["first_name"].Value;
            user.sLastName = node.Attributes["last_name"].Value;
            user.sMailAddress = node.Attributes["mail_address"].Value;
            user.sPhone = node.Attributes["phone"].Value;
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static User getUserFromXmlNode(XmlNode node)
    {
        try
        {
            if (node == null) return null;
            User user = new User();
            user.sFirstName = node.Attributes["first_name"].Value;
            user.sLastName = node.Attributes["last_name"].Value;
            user.sMailAddress = node.Attributes["mail_address"].Value;
            user.sPhone = node.Attributes["phone"].Value;
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public ValidateMessage getValidation()
    {
        ValidateMessage message = new ValidateMessage();

        if (GlobalGui.isBlank(sFirstName))
        {
            message.sMessage = "Du må oppgi fornavn";
            message.bOK = false;
        }
        else if (GlobalGui.isBlank(sLastName))
        {
            message.sMessage = "Du må oppgi etternavn";
            message.bOK = false;
        }
        else if (GlobalGui.isBlank(sMailAddress))
        {
            message.sMessage = "Du må oppgi ePost adresse";
            message.bOK = false;
        }
        else if (GlobalGui.isBlank(sLastName))
        {
            message.sMessage = "Du må oppgi telefonnummer";
            message.bOK = false;
        }
        else
        {
            message.bOK = true;
        }
        return message;
    }


}