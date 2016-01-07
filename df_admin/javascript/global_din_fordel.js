

var sFontFamily = " style='font-family:arial;' ";

function login_form_detectEnterKey(e)
{        
    if (enterKeyHit(e))
    {
        send_password();
    }    
}

function send_password()
{
    if (ClientCall(arguments))
    {   
        ajax.call_server("send_password()",getTextFieldValue('user_name'), getTextFieldValue('pword'));
    }
    else if (ServerCall(arguments))
    {
        var e = document.getElementById("password_div");
        e.style.display = "none";     
        ajax.render_server_html();   
    }
}

function paint_password() 
{
    document.write("<div id=password_div style='clear:both;border:2px rgb(200,200,200);border-radius:6px;padding:20px;background-color:rgb(240,240,240);font-family:arial;font-size:12px;'>");

    /* 1 */

    document.write("<table cellpadding=5>");
    document.write("  <tr>");
    document.write("    <td>");
    document.write("      <div style='float:left;font-family:arial;font-size:12px;margin-top:5px;margin-right:20px;' >Brukernavn</div>");
    document.write("    </td>");

    document.write("    <td>");
    document.write("       <div style='float:left;margin-right:20px;margin-top:3px;'>");
    document.write("         <input id=user_name onkeypress=login_form_detectEnterKey(event) type=text maxchar=40 size=30 value=''>"); // deploy-fix
    document.write("       </div>");
    document.write("    </td>");
    document.write("  </tr>");

    /* 2 */
    document.write("  <tr>");
    document.write("    <td>");
    document.write("      <div style='float:left;font-family:arial;font-size:12px;margin-top:5px;margin-right:20px;' >Passord</div>");

    document.write("    </td>");

    document.write("    <td>");

    document.write("      <div style='float:left;margin-right:20px;margin-top:3px;'>");


    document.write("        <input id=pword onkeypress=login_form_detectEnterKey(event)  type=password maxchar=10 size=10 value=''>"); // deploy-fix

    document.write("      </div>");
    document.write("    </td>");

    document.write("  </tr>");

    document.write("  <tr>");
    document.write("    <td>");
    document.write("      <div style='float:left;'>");
    document.write("        <input type=button onclick=send_password() value='OK'>");
    document.write("      </div>");
    document.write("      <div style='clear:both;>&nbsp;</div>");
    document.write("      <div style='margin-top:50px;clear:both;font-family:arial;font-size:12px;' id=server_message>&nbsp;</div>");
    document.write("    </td>");

    document.write("  </tr>");
    document.write("  </table>");
    
    document.write("</div>");
}


function paint_menu_1_menu_2()
{
    // Start content ...
    document.write("<div style='clear:both;'>");

    document.write("<table id=table_1 " + sFontFamily + " >");
    
    document.write("<tr>");
    document.write("<td>");
    document.write("<div id=menu_1>");
    document.write("</td>");
    document.write("</tr>");

    document.write("<tr>");
    document.write("<td>");
    document.write("</div>");
    document.write("<div id=menu_2>");
    document.write("</div>");
    document.write("</td>");
    document.write("</tr>");

    document.write("</table>");
    document.write("</div>");
}

function paint_menu_1_and_work_page() {
    // Start content ...
    var sBackGroundColor = "rgb(247,247,247)";

    document.write("<div style='clear:both;' id=menu_1></div>"); // 1 sept
    document.write("<div  style='clear:both;'  id=menu_2></div>"); // 1 sept
    
    document.write("<table cellpadding=0 cellspacing=0 width=100%>");
    document.write("<tr>");
    document.write("<td>");
    document.write("<div style='clear:both;width:100%;'  id=work_page >");
    document.write("</div>");
    document.write("</td>");
    document.write("</tr>");
    document.write("</table>");
    
    // document.write("</div>");
}


function paint_menu_1_menu_2_work_page()
{
    // Start content ...
    document.write("<div style='clear:both;'>");

    document.write("<table id=table_2 " + sFontFamily + " cellspacing=0 cellpadding=0>");
    document.write("<tr>");
    document.write("<td>");
    document.write("<div id=menu_1>");
    document.write("</td>");
    document.write("</tr>");

    document.write("<tr>");
    document.write("<td>");
    document.write("</div>");
    document.write("<div id=menu_2>");
    document.write("</div>");
    document.write("</td>");
    document.write("</tr>");

    document.write("<tr>");
    document.write("<td>");
    document.write("</div>");
    document.write("<div id=work_page>");
    
    document.write("</div>");
    document.write("</td>");
    document.write("</tr>");

    document.write("</table>");
    document.write("</div>");

}


function admin_terminal()
{
    if (ClientCall(arguments))
    {
        ajax.call_server("admin_terminal()");
    }
    else if (ServerCall(arguments))
    {
        var action = ajax.get_server_value("action");
        if (action && action == "call_aspx")
            location.href="takk.aspx";
        else
            ajax.render_server_html();
    }
}



function admin_general()
{
    if (ClientCall(arguments))
    {
        ajax.call_server("admin_general()");
    }
    else if (ServerCall(arguments))
    {
        var action = ajax.get_server_value("action");
        if (action && action == "call_aspx")
            location.href="takk.aspx";
        else
            ajax.render_server_html();
    }
}


function admin_shop()
{
    if (ClientCall(arguments))
    {
        ajax.call_server("admin_shop()");
    }
    else if (ServerCall(arguments))
    {
        var action = ajax.get_server_value("action");
        if (action && action == "call_aspx")
            location.href="takk.aspx";
        else
            ajax.render_server_html();
    }
}

function admin_user_login_email()
{
    location.href="admin_user_login_email.aspx";
}

function admin_user_coupon_view()
{
    location.href="admin_user_coupon_view.aspx";
}


function admin_user_login_facebook()
{
    location.href="admin_user_login_facebook.aspx";
}

function admin_user_login_phone()
{
    location.href="admin_user_login_phone.aspx";
}

function admin_user_demo_card()
{
    location.href="admin_user_demo_card.aspx";
}

function admin_user_create_consumer_from_email()
{
    location.href="admin_user_create_consumer_from_email.aspx";
}

function admin_user_create_consumer_from_facebook()
{
    location.href="admin_user_create_consumer_from_facebook.aspx";
}

function admin_user_verify_consumer_from_pincode()
{
    location.href="admin_user_verify_consumer_from_pincode.aspx";
}


function admin_user_consumer_get_unique_link_from_email()
{
    location.href="admin_user_consumer_get_unique_link_from_email.aspx";
}

function admin_user_consumer_email_set_new_password()
{
    location.href="admin_user_consumer_email_set_new_password.aspx";
}

function admin_user_consumer_get_unique_link_from_phone()
{
    location.href="admin_user_consumer_get_unique_link_from_phone.aspx";
}





function admin_user_create()
{
    location.href="admin_user_create.aspx";
}

function admin_user_view()
{
    location.href="admin_user_view.aspx";
}

function admin_user_shop_connect()
{
    location.href="admin_user_shop_connect.aspx";
}

function admin_coupon_approve()
{
    location.href="admin_coupon_approve.aspx";
}

function admin_shop_view()
{
    location.href="admin_shop_view.aspx";
}

function admin_consumer_view()
{
    location.href="admin_consumer_view.aspx";
}


function admin_show_consumer_coupon()
{
    location.href="admin_show_consumer_coupon.aspx";
}

function admin_single_consumer_coupon()
{
    location.href="admin_single_consumer_coupon.aspx";
}

function admin_set_consumer_coupon_subscription()
{
    location.href="admin_set_consumer_coupon_subscription.aspx";
}

function admin_get_consumer_progress()
{
    location.href="admin_get_consumer_progress.aspx";
}

function admin_consumer_interests()
{
    location.href="admin_consumer_interests.aspx";
}

function admin_veriphone_pilot() {
    location.href = "admin_veriphone_pilot.aspx";
}

function admin_pos_pilot() {
    location.href = "admin_pos_pilot.aspx";
}



function admin_create_consumer_coupon()
{
    location.href="admin_create_consumer_coupon.aspx";
}

function admin_shop_create()
{
    location.href="admin_shop_create.aspx";
}

function admin_chain_create()
{
    location.href="admin_chain_create.aspx";
}


function admin_coupon_view()
{
    location.href="admin_coupon_view.aspx";
}

function admin_coupon_create()
{
    location.href="admin_coupon_create.aspx";
}


function terminal_get_shop_consumer_token_coupons()
{
    location.href="terminal_get_shop_consumer_token_coupons.aspx";
}

function terminal_ping_din_fordel_string()
{
    location.href="terminal_ping_din_fordel_string.aspx";
}

function terminal_ping_din_fordel_string_in_string_out()
{
    location.href="terminal_ping_din_fordel_string_in_string_out.aspx";
}

function point_interface_form()
{
    location.href="point_interface_form.aspx";
}

function admin_user_show_webservices()
{
    location.href="admin_user_webservices.aspx";
}

