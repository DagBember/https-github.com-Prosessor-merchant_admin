

//	********************************************************
//	**************** Global variables **********************
//	********************************************************
var sa_ajax_cube_request = sa_init_ajax();
var sCubeRequest = "";
var xmlResponse = null;
var sLastRequest = "";

var sa_AJAX_READYSTATE_UNINITIALIZED    = 0;
var sa_AJAX_READYSTATE_LOADING          = 1;
var sa_AJAX_READYSTATE_RH_RECEIVED      = 2;
var sa_AJAX_READYSTATE_SOME_RB_RECEIVED = 3;
var sa_AJAX_READYSTATE_COMPLETE         = 4;
var sa_AJAX_JAVA_COMPILE_ERROR          = 500;
var sa_AJAX_HTTP_OK         = 200;
var http_request = null;

var NOF_BACKBUTTON_ANCHORS = 200;

var focusInterval;
var scrollInterval;

function superaktiv_main_event_loop(http_request)
{
    ajax.initialize(http_request);

    var procedure = ajax.get_server_value("procedure");

    if (procedure != null)
    {
        try
        {
            eval(procedure);
           
            var postAnalyzerSearch = ajax.get_server_value("post_analyzer_operation");

            if (postAnalyzerSearch && postAnalyzerSearch == "rerun_analyzer_search")
            {
                // Using timer to avoid ajax-error ...
                window.setTimeout("queryAnalyzer.runAnalyzerSearch(null, null)", 1);
            }
        }
        catch (error)
        {
            ajax.log('ERROR - ' + procedure + " : " + error);
        }
    }
}




String.prototype.replaceAll = function (str1, str2, ignore)
{
    return this.replace(new RegExp(str1.replace(/([\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, function (c) { return "\\" + c; }), "g" + (ignore ? "i" : "")), str2);
}

function backbutton_RenameAnchor(anchorid, anchorname)
{
    var e = document.getElementById(anchorid);
    if (e != null)
    {
	    document.getElementById(anchorid).name = anchorname; //this renames the anchor
	}
}

function backbutton_RedirectLocation(anchorid, anchorname, HashName)
{
	backbutton_RenameAnchor(anchorid, anchorname);
	document.location = HashName;
}

function backbutton_createBackbuttonPlaceholders()
{
    document.write("<a id='backbutton_LocationAnchor' name='backbutton_LocationAnchor'></a>");

    for (var i = 0; i < NOF_BACKBUTTON_ANCHORS; ++i) // relocate ...
    {
        document.write("<a href='javascript:backbutton_RedirectLocation('backbutton_LocationAnchor', 'x" + i + "'," + "'#x" + i + "');'></a>");
    }

    for (var ii = 0; ii < NOF_BACKBUTTON_ANCHORS; ++ii)
    {
        document.write("<span id='x" + ii + "' style='display:none;'></span>");
    }

    if (bbHelper.CURRENT_PAGE == ABACOLLA_PAGE)
    {
        for (var xxx = 0; xxx < 5; ++xxx)
        {
            backbutton_RedirectLocation("backbutton_LocationAnchor", "x" + xxx, "#x" + xxx)
            backbutton_iScreenNo = xxx;
        }
    }
}

function backbutton_timer()
{
    // Has the location.hash changed?
}

var backbutton_iScreenNo = 0;

function processARequest(http_request,url,callback) 
{
  http_request.onreadystatechange = callback;

  if (backbutton_iScreenNo <= NOF_BACKBUTTON_ANCHORS) // 20. april 
  {
      ++backbutton_iScreenNo;
    backbutton_RedirectLocation("backbutton_LocationAnchor", "x" + backbutton_iScreenNo, "#x" + backbutton_iScreenNo)
  }

  http_request.open('GET', url, true); 
  // Fra egen OC Det krasher på http_request.send(null) dersom url er større enn 2087 tegn.
  // Dette gjelder både IE og Firefox. Jeg har prøvd å endre diverse maxstørrelser i web.config uten hell.
  // På kc.superaktiv.no FUNGERER det, men jeg mener å ha sett 404 feiler der også. 
  // Uansett : Fint om man kan få diagnostisert dette ordentlig.
  http_request.send(null);
}

function processPostRequest(http_request, url,data , callback) {
    http_request.onreadystatechange = callback;

    if (backbutton_iScreenNo <= NOF_BACKBUTTON_ANCHORS) // 20. april 
    {
        ++backbutton_iScreenNo;

        // 8 sept 2012. We get here EACH time an ajax function has been called in ALL forms ...        

        backbutton_RedirectLocation("backbutton_LocationAnchor", "x" + +backbutton_iScreenNo, "#x" + backbutton_iScreenNo)
    }
    http_request.open('POST', url, true);
    http_request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    http_request.send(data);
}


function log(message)
{
    window.status='Logging : ' + message
}


// Asynchrounous init call will return http_request object
function sa_init_ajax()
{
    // setHtmlTo("ajax_timer","Working&nbsp;...");
    // setHtmlTo("ajax_timer", "Working&nbsp;...");
    var sWaitingGif = "http://www.abacolla.com/layout/images/ajax_loader.gif";
    if (runningLocalhost())
        sWaitingGif = "/web/layout/images/ajax_loader.gif";
    else
        sWaitingGif = "/layout/images/ajax_loader.gif";

    setHtmlTo("ajax_timer", "<img style='border-radius: 4px;box-shadow: 5px 5px 15px rgb(100,100,100);margin-top:2px;' height=11 src='" + sWaitingGif + "'>");


    var http_request;
    if (window.XMLHttpRequest) 
    { 
        // Mozilla, Safari,... 
        http_request = new XMLHttpRequest(); 
    } 
    else if (window.ActiveXObject) 
    { 
        // Internet Explorer 
        http_request = new ActiveXObject("Microsoft.XMLHTTP"); 
    }

    return http_request;
}


function sa_get_ajax_cube_response(my_http_request,my_handler) 
{

    try
    {    
        if (sa_ajax_cube_request.readyState == sa_AJAX_READYSTATE_COMPLETE) 
        {
            if (sa_ajax_cube_request.status == sa_AJAX_HTTP_OK) 
            {
                my_handler(my_http_request);
            } 
            else if (sa_ajax_cube_request.status == sa_AJAX_JAVA_COMPILE_ERROR)
            { 
                document.write('SA Ajax Java compile error : ' + sa_ajax_cube_request.responseText); 
            }
            else 
            {
                document.write("<div style='border-top:solid 1px rgb(70,70,70);padding:10px;background-color:rgb(100,100,100);color:rgb(250,250,250);font-weight:bold;font-size:12px;' >");
                document.write('Bad internet communication. Please login again.');
                document.write('</div>');
                window.location = "http://www.vg.no"; // Getting out ...
            } 
            return sa_ajax_cube_request.status;
        } 
    }
    catch(error)
    {
        document.write("<div style='border-top:solid 1px rgb(70,70,70);padding:10px;background-color:rgb(100,100,100);color:rgb(250,250,250);font-weight:bold;font-size:12px;' >");
        document.write('An error occurred. Please login again.');
        document.write('</div>');
    }
} 


function initializeAjaxPage()
{
	// Preloading everything you need here
	// The example below will ensure that super_picture.gif gets preloaded
	// Image0= new Image(); Image0.src = "super_picture.gif";

	// Sends the first request to the ajax_controller to test Ajax - configuration
	ajax_init_request();
		
	setVar("MasterAction","Initialize");
	processCubeRequest();
}



//	********************************************************
//	**************** Callback function *********************
//	********************************************************
function sa_ajax_callback() 
{
	var retVal = sa_get_ajax_cube_response(sa_ajax_cube_request,superaktiv_main_event_loop);
};

function sa_ajax_handshake_callback() 
{
	var retVal = sa_get_ajax_cube_response(sa_ajax_cube_request,superaktiv_handshake_main_event_loop);
	
};


//	********************************************************
//	**************** Process CUBE request ******************
//	********************************************************

 
function ajax_send_request_to_server()
{
    MAIN_CONTROLLER_PAGE = window.location;
      
	sa_ajax_cube_request = sa_init_ajax();
    
	new processPostRequest(sa_ajax_cube_request, MAIN_CONTROLLER_PAGE, sCubeRequest, sa_ajax_callback);
}




//	********************************************************
//	**************** Initialize sCubeRequest string ********
//	**************** before assigning variables     ********
//	********************************************************
function ajax_init_request()
{
	sCubeRequest = "?";
}

//	********************************************************
//	**************** Assign request variables **************
//	********************************************************
function ajax_set_variable(sVariable,sValue)
{
	if (sCubeRequest == "") 
		sCubeRequest = "?"; 
	
	if (sCubeRequest == "?")
		sCubeRequest += sVariable + "=" + sValue;
	else	
	{	
		if (sVariable.indexOf("?") >= 0) sVariable=sVariable.replace("?","_");
		if (sVariable.indexOf("&") >= 0) sVariable=sVariable.replace("&","_");
		sCubeRequest += "&" + sVariable + "=" + sValue;
	}
}

//	****************************************************
//	** Gets an xml-value from the server xml-response **
//	****************************************************
function ajax_get_server_value(sName)
{
	if (xmlResponse == null)
	{
		// Internet Information Server
		// Firefox
		startText = "<" + sName + ">";
		endText = "</" + sName + ">";
		retVal = getTextBeetween(startText,endText);
		if (retVal.length > 11)
		{
			if (retVal.indexOf("<![CDATA[") == 0)
			{
				retVal = getT_Beetween(retVal,"<![CDATA[","]]>");
			}
		}
		if (retVal == '') return null;
		return retVal;
	}
	else
	{
		var retVal = xmlResponse.getElementsByTagName(sName);
		if (retVal[0] == null)
		{
			startText = "<" + sName + ">";
			endText = "</" + sName + ">";
			retVal = getTextBeetween(startText,endText);
			if (retVal.length > 11)
			{
				if (retVal.indexOf("<![CDATA[") >= 0)
				{
					retVal = getT_Beetween(retVal,"<![CDATA[","]]>");
				}
			}
			// Internet Information Server
			// Internet Explorer
			// Firefox and Apache
			if (retVal.length == 0) 
			{
				return null;
			}
			if (retVal == '') return null;
			return retVal;
		}
		// Apache
		// Internet Explorer
		var x = retVal[0].firstChild.nodeValue;
		if (x == '') return null;
		return x;
	}
}

//	********************************************************************************************************************
//	** Text parsing help method,                                                         *******************************
//  ** used for unknown browsers with unsolved XML-compability issues                    *******************************
//	** Returns text beetween startText and endText from the global responseText-variable *******************************
//	********************************************************************************************************************
function getTextBeetween(startText,endText)
{
    try
    {
        var start = responseText.indexOf(startText)
        var end = responseText.indexOf(endText)
        return responseText.substr(start + startText.length, end - (start + startText.length));
    }
    catch (error)
    {
        return "";
    }

}

//	********************************************************************************************************************
//	** Text parsing help method,                                      **************************************************
//  ** used for unknown browsers with unsolved XML-compability issues **************************************************
//	** Returns text beetween startText and endText from sInText       **************************************************
//	********************************************************************************************************************
function getT_Beetween(sInText,startText,endText)
{
	var start = sInText.indexOf(startText)
	var end = sInText.indexOf(endText)
	var retVal = sInText.substr(start + startText.length,end - (start + startText.length));
	return retVal;
}
 

//	**********************************************************
//	** Fills a cell in the html-document with new html-code **
//	**********************************************************

function elementExists(sID)
{
	var element = document.getElementById(sID);
	if (element != null)
		return true;
	return false;		
}


function setHtmlTo(sID,sHtmlCode)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		if (sHtmlCode == null)
			element.innerHTML = "";	
		 else
			element.innerHTML = sHtmlCode;
        return true;			
	}
	else
	{
        // window.status='AJAX LOG : ' + sID + " not found";
        return false;
    }
    return false;
}		

function setElementTo(element,sHtmlCode)
{
	if (element != null)
	{
		if (sHtmlCode == null)
			element.innerHTML = "";	
		 else
			element.innerHTML = sHtmlCode;	
        return true;			
	}
	else
	{
        // window.status='AJAX LOG : ' + sID + " not found";
        return false;
    }
    return false;
}		


// Same as SetHtmlTo, but no warning if the element does not exist ...
function ifExistsSetHtmlTo(sID,sHtmlCode)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		if (sHtmlCode == null)
			element.innerHTML = "";	
		 else
			element.innerHTML = sHtmlCode;	
	}
}		

function isInteger(textNumber)
{
    if (textNumber == null)
        return false;
    for (var i=0;i<textNumber.length;++i)
    {
        var c = textNumber.substring(i,i+1);
        if (c != "0" 
        && c != "1"
        && c != "2"
        && c != "3"
        && c != "4"
        && c != "5"
        && c != "6"
        && c != "7"
        && c != "8"
        && c != "9")
        {
            return false;
        }        
    }        
    return true;
}

function getTextFieldValue(sID)
{
    var returnValue = "";
	var element = document.getElementById(sID);
	if (element != null)
	{
	    var s = element.value;
        // If error element.value will be undefined ...	    
        try
        {
	        for (var i=0;i<s.length;++i)
	        {
	           var c = s.charAt(i);

	           if (c == '[')
	           {
	               returnValue = returnValue + "*START_BRACKET*"; 
               }
	           else if (c == ']')
	           {
	               returnValue = returnValue + "*END_BRACKET*"; 
               }
	           else if (c == '+')
	           {
	               returnValue = returnValue + "*PLUS*";
	           }
	           else
               {
	               returnValue = returnValue + c; 
               }
            }
		    return returnValue;
        }
        catch(error)
        {
            alert(element.innerHTML);
        }
	}
	else
		log(sID + " not found");
}		

// Works for IE, not Firefox ...
function getTextAreaValue(sID)
{
    var returnValue = "";
	var element = document.getElementById(sID);
	if (element != null)
	{
	    
        var s = element.value;
	    
	    for (var i=0;i<s.length;++i)
	    {
	       var c = s.charAt(i);
	       if (c == '\r') {
	           c = ""; // Fix for bug with /7 
                // Nothing ... waiting for \n
	       }
	       else if (c == '\n')
	       {
	           returnValue = returnValue + "*ENTER*"; // Enter sent to server. 
	       }
           
           if (c == '\u00E6') 
           {
	           returnValue = returnValue + "*ae*"; 
           }
           else if (c == '\u00F8')
           {
	           returnValue = returnValue + "*oe*"; 
           }
           else if (c == '\u00E5')
           {
	           returnValue = returnValue + "*aa*"; 
           }
           else if (c == '\u00C6')
           {
	           returnValue = returnValue + "*AE*"; 
           }
           else if (c == '\u00D8')
           {
	           returnValue = returnValue + "*OE*"; 
           }
           else if (c == '\u00C5')
           {
	           returnValue = returnValue + "*AA*"; 
           }
	       else if (c == 'æ')
	       {
	           returnValue = returnValue + "*ae*"; 
           }
	       else if (c == 'ø')
	       {
	           returnValue = returnValue + "*oe*"; 
           }
	       else if (c == 'å')
	       {
	           returnValue = returnValue + "*aa*"; 
           }
	       else if (c == 'Æ')
	       {
	           returnValue = returnValue + "*AE*"; 
           }
	       else if (c == 'Ø')
	       {
	           returnValue = returnValue + "*OE*"; 
           }
	       else if (c == 'Å')
	       {
	           returnValue = returnValue + "*AA*"; 
           }
	       else if (c == '[')
	       {
	           returnValue = returnValue + "*START_BRACKET*"; 
           }
	       else if (c == ']')
	       {
	           returnValue = returnValue + "*END_BRACKET*"; 
           }
	       else if (c == '+')
	       {
	           returnValue = returnValue + "*PLUS*";
	       }
	       else if (c == '#')
	       {
	           returnValue = returnValue + "*hash*";
	       }

	       else
	       {
                returnValue = returnValue + c;
	       }
	       c = c;
	    }
		// return element.value;
		return returnValue;
	}
	else
		log(sID + " not found");
}		


function getPaypalHiddenFieldValue(sID)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 return element.value;
	}
	else
	{
		log(sID + " not found");
    }
}		


function setFocusTo(sID)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 element.focus();
	}
	else
	{
		// log(sID + " not found - setFocusTo");
    }
}		


function getTextAreaText(sID)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 return element.innerHTML;
	}
	else
		log(sID + " not found");
}		

function setTextFieldValue(sID,sValue)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 return element.value = sValue;
	}
	else
		log(sID + " not found");
}		

function blankTextField(sId)
{
    var element = document.getElementById(sId);
    if (element != null)
        element.value='';
}

function PasteFromClipboard(sId)
{ 
   var element = document.getElementById(sId);
   element.focus();
   PastedText = element.createTextRange();
   PastedText.execCommand("Paste");
}

function setClassTo(sID,sClass)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 element.className = sClass;
         return true;
	}
	else
    {
		log(sID + " not found");
        return false;
    }
}		

function setThisClassTo(element,sClass)
{
	if (element != null)
	{
		 element.className = sClass;
	}
	else
		log(sID + " not found");
}		


function getCheckBoxValue(sID)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 return element.checked;
	}
	else
	{
		log(sID + " not found");
		return false;
	}
}		

function setCheckBoxValue(sID,bValue)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 element.checked = bValue;
	}
	else
	{
		log(sID + " not found");
		return false;
	}
}		

function enableFieldIfNotBlank(sId, sStringValue)
{
    if (sStringValue != '' && sStringValue != ' ' && sStringValue != '  ' && sStringValue != '   ' && sStringValue != '    ')
        enableField(sId,true);
    else
        enableField(sId,false);
}

function enableField(sID,bValue)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 element.disabled = !bValue;
	}
	else
	{
		log(sID + " not found");
		return false;
	}
}		

function setPaypalHiddenFieldValue(sID,sValue)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 return element.value = sValue;
	}
	else
	    alert('Field ' + sId + ' not found');
}		


function setDropDownValue(sID,sValue)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		 return element.value = sValue;
	}
	else
		log(sID + " not found");
}		

function getDropDownValue(sID)
{
	var element = document.getElementById(sID);
	if (element != null)
	{
		if (element.selectedIndex >= 0)
		{
			return element.options[element.selectedIndex].value;
        }
		else return "";			
	}
	else
		log(sID + " not found");
}		

function getDropDownValueFromCombo(combo)
{
	if (combo != null)
	{
		if (combo.selectedIndex >= 0)
			return combo.options[combo.selectedIndex].value;
		else return "";			
	}
	else
		log("Combo not found");
}		

function setSelectedComboValue(comboId, sValue)
{
    if (!sValue) return;
    var combo = document.getElementById(comboId);
    if (!combo) return;

    for (var i=0;i<combo.options.length;++i)
    {
        if (combo.options[i].value == sValue)
        {
            combo.selectedIndex = i;
            return;
        }
    }
}

function enterKeyHit(ev)
{
	if(ev && ev.which)
	{ 
		charkey = ev.which 
	}
	else
	{		
		charkey = ev.keyCode 
	}
	
	if(charkey == 13)
	{ 
		return true;
	}
	return false;
}



function setCursorAtEnd(textId) 
{   
    if (textId == null) return;
    var txt = document.getElementById(textId);
    if(!txt) return;
    if (txt.createTextRange) 
    {   
        //IE   
        var FieldRange = txt.createTextRange();   
        FieldRange.moveStart('character', txt.value.length);   
        FieldRange.collapse();   
        FieldRange.select();   
    }   
    else 
    {   
        //Firefox and Opera   
        txt.focus();   
        var length = txt.value.length;   
        txt.setSelectionRange(length, length);   
    }   
}


function getSelectedComboValue(select_id)
{
    // Works for both single and multiple choices. Multiple choices are separated by ";"
    var select_box = document.getElementById(select_id);
    var i = 0;
    if (!select_box) return "";

    for (i = 0; i < select_box.options.length; ++i)
    {
        if (select_box.options[i].selected)
        {            
            return select_box.options[i].value;
        }
    }
    return "";
}



function scrollToDiv(id)
{
    try
    {
        var element = document.getElementById(id);
        element.scrollIntoView(true);
        clearInterval(scrollInterval);
    }
    catch (error)
    {
        clearInterval(scrollInterval);
    }
}


function set_border_color(e, rgb_color)
{
    e.style.border = "3px " + rgb_color + " solid";
}

function runningLocalhost()
{
    var x = window.location.toString();
    if (x.indexOf("localhost") >= 0)
        return true;
    return false;
}

function create_global_session_prefix()
{
    now = new Date();
    return now.getHours() + "_" + now.getMinutes() + "_" + now.getSeconds() + "_" + now.getMilliseconds();
}

var renderHtmlPageFunction = "";

function init_ajax_web_form()
{
    if (ClientCall(arguments))
    {
        ajax.call_server("init_ajax_web_form()",renderHtmlPageFunction);
    }
    else if (ServerCall(arguments))
    {
        // sFontFamily = " style='font-family:verdana;' "; 
        // sFontFamily = " style='font-family:Varela Round, sans-serif;' ";
        // sFontFamily = " style='font-family: Gabriela, serif;' ";
        ajax.render_server_html();
    }
}

function init_ajax_web_form_special() // For card demo ...
{
    if (ClientCall(arguments))
    {
        ajax.call_server("init_ajax_web_form()",renderHtmlPageFunction,location.search);
    }
    else if (ServerCall(arguments))
    {
        // sFontFamily = " style='font-family:verdana;' "; 
        // sFontFamily = " style='font-family:Varela Round, sans-serif;' ";
        // sFontFamily = " style='font-family: Gabriela, serif;' ";
        ajax.render_server_html();
    }
}

