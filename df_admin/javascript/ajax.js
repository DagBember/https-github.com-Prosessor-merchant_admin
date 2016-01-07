

 // alert('including ajax');
var ajax = new Ajax();

var globalSessionPrefix = "";

// Init 7 jan
function Ajax()
{
} 
 
Ajax.prototype.showGuiMessage = function()
{
	var gui_message = ajax_get_server_value("gui_message");
	if (gui_message != null)
		setHtmlTo("gui_message",ajax_get_server_value("gui_message"));
	else		
	{
		if (elementExists("gui_message"))
			setHtmlTo("gui_message","&nbsp;");
	}
}

Ajax.prototype.init_request = function()
{
	ajax_init_request();
}

Ajax.prototype.set_variable = function(variable,value)
{
	ajax_set_variable(variable,value);
}

Ajax.prototype.setFocusTo = function(variable)
{
	setFocusTo(variable);
}

Ajax.prototype.send_request_to_server = function()
{
	ajax_send_request_to_server();
}

Ajax.prototype.get_server_value = function(variable)
{
	return ajax_get_server_value(variable);
}

Ajax.prototype.set_html = function(client_and_server_id)
{
    try
    {
        serverValue = this.get_server_value(client_and_server_id);
        if (serverValue == null) 
            serverValue = '';
	    
	    // hei dag æøå
	    serverValue = createNorwegian(serverValue);
	    
	    setHtmlTo(client_and_server_id,serverValue);
    }
    catch(error)
    {
        log('set_html(' + client_and_server_id + ') has not been not sent from the server');
    }	    
}



Ajax.prototype.set_element = function(element)
{
    try
    {
	    setElementTo(element,this.get_server_value(client_and_server_id));
    }
    catch(error)
    {
        log('set_html(' + client_and_server_id + ') has not been not sent from the server');
    }	    
}



Ajax.prototype.set_html_to = function(client_id,value)
{
	setHtmlTo(client_id,value);
}

Ajax.prototype.if_exists_set_html_to = function(client_id,value)
{
	ifExistsSetHtmlTo(client_id,value);
}

Ajax.prototype.initialize = function(http_request)
{
	responseText = http_request.responseText;
}

Ajax.prototype.get_input_text_value = function (sID)
{
	return getTextFieldValue(sID);
}

Ajax.prototype.get_dropdown_value = function (sID)
{
	return getDropDownValue(sID);
}

Ajax.prototype.get_drop_down_value_from_combo = function (combo)
{
	return getDropDownValueFromCombo(combo);
}

var iUniqueRequestCounter = 0;

Ajax.prototype.call_server = function()
{
	if (arguments.length < 1)
	{
		alert('No arguments in call_server()')
		return;
	}
	ajax_init_request();
    ajax_set_variable("xx","yy"); // 20 feb

    if (globalSessionPrefix == "")
    {
        globalSessionPrefix = create_global_session_prefix();
    }

	ajax_set_variable("global_session_prefix",globalSessionPrefix); // 20 feb
	for(var i=0;i<arguments.length;i++)
	{
		if (i == 0)
		{
			ajax_set_variable("procedure",arguments[0]);
            // The request HAS to be unique, 
            // if NOT it might not be shipped to the server. 
            // Ohhh, yess we just love caching, don't we ? ;)
			ajax_set_variable("unique",++iUniqueRequestCounter);
        }
		else
		{
            if (arguments[i] != null && arguments[i].length > 0)
            {
    			ajax_set_variable("parameter_" + i,cleanText(arguments[i]));
            }
            else
            {
    			ajax_set_variable("parameter_" + i,arguments[i]);
            }
		}			
	}
	
	ajax_send_request_to_server();
}


 Ajax.prototype.get_current_url = function()
{
  var wm = Components.classes["@mozilla.org/appshell/window-mediator;1"].getService(Components.interfaces.nsIWindowMediator);
  var recentWindow = wm.getMostRecentWindow("navigator:browser");
  return recentWindow ? recentWindow.content.document.location.toString() : null;
}

Ajax.prototype.from_web_to_star_codes = function(inText)
{
  if (inText == null || inText.length == 0)
    return "";
  var newText = '';
  for (var i=0;i<inText.length;++i)
  {
     newText += uniCode(inText.charAt(i));
  }
  return newText;
}

 
function cleanText(inText)
{
  if (inText == null || inText.length == 0)
    return "";
  var newText = '';
  for (var i=0;i<inText.length;++i)
  {
     newText += uniCode(inText.charAt(i));
  }
  return newText;
}

function createNorwegian(inText)
{ // hei dag
  var newText = '';
  for (var i=0;i<inText.length;++i)
  {
     // if (inText.charAt(i) == 'D')
     //    inText[i] = 'd';
  }
  return inText
}

function uniCode(c)
{
    if (c == '\u00E5') return '*00E5*' // Potensial bug, this one must be in the start ...

    if (c == '\u00E6') return '*00E6*' // Dec &#230;
    else if (c == '\u00F8') return '*00F8*' // ø
    else if (c == '\u00E5') return '*00E5*' // å
    else if (c == '\u00C6') return '*00C6*' // Æ
    else if (c == '\u00D8') return '*00D8*' // Ø
    else if (c == '\u00C5') return '*00C5*' // Å

    if (c == '&') return '*7*'
    else if (c == '?') return '*8*'
    else if (c == '<') return '*9*'
    else if (c == '>') return '*10*'

    else if (c == '\u00C0') return 	'*00C0*' // 	À
    else if (c == '\u00C1') return 	'*00C1*' // 	Á
    else if (c == '\u00C2') return 	'*00C2*' // 	Â
    else if (c == '\u00C3') return 	'*00C3*' // 	Ã
    else if (c == '\u00C4') return 	'*00C4*' // 	Ä
    else if (c == '\u00C5') return 	'*00C5*' // 	Å
    else if (c == '\u00C6') return 	'*00C6*' // 	Æ
    else if (c == '\u00C7') return 	'*00C7*' // 	Ç
    else if (c == '\u00C8') return 	'*00C8*' // 	È
    else if (c == '\u00C9') return 	'*00C9*' // 	É
    else if (c == '\u00CA') return 	'*00CA*' // 	Ê
    else if (c == '\u00CB') return 	'*00CB*' // 	Ë
    else if (c == '\u00CC') return 	'*00CC*' // 	Ì
    else if (c == '\u00CD') return 	'*00CD*' // 	Í
    else if (c == '\u00CE') return 	'*00CE*' // 	Î
    else if (c == '\u00CF') return 	'*00CF*' // 	Ï
    else if (c == '\u00D0') return 	'*00D0*' // 	Ð
    else if (c == '\u00D1') return 	'*00D1*' // 	Ñ
    else if (c == '\u00D2') return 	'*00D2*' // 	Ò
    else if (c == '\u00D3') return 	'*00D3*' // 	Ó
    else if (c == '\u00D4') return 	'*00D4*' // 	Ô
    else if (c == '\u00D5') return 	'*00D5*' // 	Õ
    else if (c == '\u00D6') return 	'*00D6*' // 	Ö
    else if (c == '\u00D7') return 	'*00D7*' // 	×
    else if (c == '\u00D8') return 	'*00D8*' // 	Ø
    else if (c == '\u00D9') return 	'*00D9*' // 	Ù
    else if (c == '\u00DA') return 	'*00DA*' // 	Ú
    else if (c == '\u00DB') return 	'*00DB*' // 	Û
    else if (c == '\u00DC') return 	'*00DC*' // 	Ü
    else if (c == '\u00DD') return 	'*00DD*' // 	Ý
    else if (c == '\u00DE') return 	'*00DE*' // 	Þ
    else if (c == '\u00DF') return 	'*00DF*' // 	ß
    else if (c == '\u00E0') return 	'*00E0*' // 	à
    else if (c == '\u00E1') return 	'*00E1*' // 	á
    else if (c == '\u00E2') return 	'*00E2*' // 	â
    else if (c == '\u00E3') return 	'*00E3*' // 	ã
    else if (c == '\u00E4') return 	'*00E4*' // 	ä
    else if (c == '\u00E5') return 	'*00E5*' // 	å
    else if (c == '\u00E6') return 	'*00E6*' // 	æ
    else if (c == '\u00E7') return 	'*00E7*' // 	ç
    else if (c == '\u00E8') return 	'*00E8*' // 	è
    else if (c == '\u00E9') return 	'*00E9*' // 	é
    else if (c == '\u00EA') return 	'*00EA*' // 	ê
    else if (c == '\u00EB') return 	'*00EB*' // 	ë
    else if (c == '\u00EC') return 	'*00EC*' // 	ì
    else if (c == '\u00ED') return 	'*00ED*' // 	í
    else if (c == '\u00EE') return 	'*00EE*' // 	î
    else if (c == '\u00EF') return 	'*00EF*' // 	ï
    else if (c == '\u00F0') return 	'*00F0*' // 	ð
    else if (c == '\u00F1') return 	'*00F1*' // 	ñ
    else if (c == '\u00F2') return 	'*00F2*' // 	ò
    else if (c == '\u00F3') return 	'*00F3*' // 	ó
    else if (c == '\u00F4') return 	'*00F4*' // 	ô
    else if (c == '\u00F5') return 	'*00F5*' // 	õ
    else if (c == '\u00F6') return 	'*00F6*' // 	ö
    else if (c == '\u00F7') return 	'*00F7*' // 	÷
    else if (c == '\u00F8') return 	'*00F8*' // 	ø
    else if (c == '\u00F9') return 	'*00F9*' // 	ù
    else if (c == '\u00FA') return 	'*00FA*' // 	ú
    else if (c == '\u00FB') return 	'*00FB*' // 	û
    else if (c == '\u00FC') return 	'*00FC*' // 	ü
    else if (c == '\u00FD') return 	'*00FD*' // 	ý
    else if (c == '\u00FE') return 	'*00FE*' // 	þ
    else if (c == '\u00FF') return 	'*00FF*' // 	ÿ

    return c;
}

Ajax.prototype.url_encode = function(clearString)
{
  if (clearString == null || clearString == '') return clearString;
  var output = '';
  var x = 0;
  clearString = clearString.toString();
  var regex = /(^[a-zA-Z0-9_.]*)/;
  while (x < clearString.length) {
    var match = regex.exec(clearString.substr(x));
    if (match != null && match.length > 1 && match[1] != '') {
    	output += match[1];
      x += match[1].length;
    } else {
      if (clearString[x] == ' ')
        output += '+';
      else {
        var charCode = clearString.charCodeAt(x);
        var hexVal = charCode.toString(16);
        output += '%' + hexVal.toUpperCase();
      }
      x++;
    }
  }
  return output;
}

Ajax.prototype.log = function(message)
{
    window.status='Status : ' + message;
}


Ajax.prototype.create_cookie = function(name,value,days) 
{
	if (days) 
	{
		var date = new Date();
		date.setTime(date.getTime()+(days*24*60*60*1000));
		var expires = "; expires="+date.toGMTString();
	}
	else var expires = "";
	document.cookie = name+"="+value+expires+"; path=/";
}

Ajax.prototype.delete_cookie = function(name) 
{
	create_cookie(name,"",-1);
}


Ajax.prototype.get_cookie = function(name) 
{
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++) 
	{
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}

Ajax.prototype.render_server_html = function () {

    // Display dialog message if shipped ...
    var sMessageContent = this.get_server_value("message_line");

    if (sMessageContent) {
        this.set_html_to("message_line", sMessageContent);
    }

    var s = this.get_server_value("html_tags_to_render");

    if (!s || s == "") return;

    var html_tab = s.split(";");

    for (var i = 0; i < html_tab.length; i++) {
        this.set_html(html_tab[i]);
    }


    // if (document.getElementById("menu_1")) {

        // 1 - Sett alle til normal font ...
        var x = document.getElementsByClassName("menu_item_twitter");
        var i;
        for (i = 0; i < x.length; i++) {
            x[i].style.fontWeight = "normal";
        }

        // 2 - Sett på menu_1 og menu_2
        var menuItem1 = document.getElementById(this.get_server_value("level_1_click"));
        // alert(this.get_server_value("level_1_click"));

        if (menuItem1) {
            menuItem1.style.fontWeight = "bold";
        }

        var menuItem2 = document.getElementById(this.get_server_value("level_2_click"));
        // alert(this.get_server_value("level_2_click"))
        if (menuItem2) {
            menuItem2.style.fontWeight = "bold";
        }

        // a) Pakk ut alle menu_trykk fra server
        // b) Finn dem, finn siblings og null ut
    //}

}




function ClientCall(parameters)
{
	if (parameters.length == 0) return true;
	if (parameters.length == 1 && parameters[0] == "serverside_call") return false;
	return true;
}

function ServerCall(parameters)
{
	if (parameters.length == 1 && parameters[0] == "serverside_call") return true;
	return false;
}


