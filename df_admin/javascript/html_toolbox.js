

function html_toolbox_text_on_lost_focus(e) 
{
    var current_text = getTextAreaValue(e.id);
    var default_text_color = e.getAttribute("data-default-text-color")
    var text_color = e.getAttribute("data-text-color")
    if (current_text == "") {
        var default_text = e.getAttribute("data-default-text")
        var data_default_text_size = e.getAttribute("data-default-text-size")
        if (data_default_text_size) e.style.fontSize = data_default_text_size + "px";
        e.value = default_text;
        e.style.color = default_text_color;
    }
    else {
        var normal_text_size = e.getAttribute("data-normal-text-size")
        if (normal_text_size) e.style.fontSize = normal_text_size + "px";
        // alert(normal_text_size + "px");
        e.style.color = text_color;
    }
}

 

function html_toolbox_text_on_focus(e) 
{
    var default_text = e.getAttribute("data-default-text")
    var default_text_color = e.getAttribute("data-default-text-color")
    var text_color = e.getAttribute("data-text-color")

    var current_text = getTextAreaValue(e.id);
    if (current_text == default_text) 
    {
        e.value = "";
        e.style.color = text_color;
    }
    else 
    {
        e.style.color = text_color;
    }
}

function html_toolbox_enter_key_hit(ev,function_call) {
    if (ev && ev.which) {
        charkey = ev.which
    }
    else {
        charkey = ev.keyCode
    }

    if (charkey == 13) {
        eval(function_call + "()");
        return true;
    }
    return false;
}


function html_toolbox_mouseover(e)
{    
    e.style.background = e.getAttribute("data-mouseover-background"); 
}

function html_toolbox_mouseout(e) {
    e.style.background = e.getAttribute("data-mouseout-background");
}


function html_toolbox_mouseover_link(e) {
    e.style.color = e.getAttribute("data-mouseover-color");
    e.style.textDecoration = "underline";
}

function html_toolbox_mouseout_link(e) {
    e.style.color = e.getAttribute("data-mouseout-color");
    e.style.textDecoration = "";
}


function html_toolbox_get_text_field_value(id) {
    var e = document.getElementById(id);
    if (!e) return "";
    var default_text = e.getAttribute("data-default-text")
    if (default_text && default_text == e.value) return "";
    if (!e.value) return "";
    return e.value;
}

function html_toolbox_get_text_area_value(id) {
    return getTextAreaValue(id);
}


function html_toolbox_goto_link(sUrl) {
    // window.location = sUrl;
    location.href = sUrl;
}




