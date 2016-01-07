
// Init ...
var sa_menu_1 = get_menu_item("", "")
var sa_menu_2 = get_menu_item("", "") 


function get_main_menu_item(id, text)
{
    var s = "";
    s = "<div id=" + id + " class=sa_menu_main_menu_item onmouseover=create_drop_list(this.id)>";
    s += text;
    s += "</div>";
    return s;
}

function get_menu_item(menu_text, menu_select)
{
    return "<div onmouseover=menu_item_mouse_over(this); class=sa_menu_sub_menu_item onclick=" + menu_select + " ><div style='padding:5px;'>" + menu_text + "</div></div>";
}

function menu_item_mouse_over(menuElement)
{
    var looper = menuElement.parentElement.firstChild;
    while (looper != null)
    {
        if (menuElement.firstChild.innerHTML != "")
        {
            looper.style.fontWeight = "normal";
            looper.style.color="rgb(240,240,240)";
            looper.style.background="rgb(70,70,70)";
        }
        looper = looper.nextSibling;
    }
    menuElement.parentElement.style.display='';
    if (menuElement.firstChild.innerHTML != "")
    {
        menuElement.style.color="rgb(70,70,70)";
        menuElement.style.background="rgb(240,240,240)";
    }
}

function sa_menu_create_drop_list(id)
{
    var e = document.getElementById(id);
    var menu = document.getElementById("floatdiv");
    menu.style.top = (e.offsetTop + 25) + "px";
    menu.style.left = e.offsetLeft + "px";
    menu.style.display = "";

    if (id == "sa_menu_1") menu.innerHTML = sa_menu_1;
    else if (id == "sa_menu_2") menu.innerHTML = sa_menu_2;
    else menu.innerHTML = "???";
}


