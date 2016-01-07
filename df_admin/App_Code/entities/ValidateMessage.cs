using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

 

public class ValidateMessage
{
	public ValidateMessage()
	{
	}

    public void setStatus(bool _bOK, string _sMessage)
    {
        sMessage = _sMessage;
        bOK = _bOK;
    }

    public bool isOK()
    {
        return bOK;
    }

    public string getMessage()
    {
        return sMessage;
    }

    public bool bOK = false;
    public string sMessage;
}

