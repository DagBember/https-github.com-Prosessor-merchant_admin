<%@ Page Language="C#" AutoEventWireup="true" CodeFile="shop_commander_menu.aspx.cs" Inherits="shop_commander_shop_commander_menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

<link href="~/css/live_table.css" rel="stylesheet" type="text/css" />
<link href="~/css/bember_layout.css" rel="stylesheet" type="text/css" />


</head>
<body style='background-color:rgb(247,247,247);' ">

<script language='javascript' src="../javascript/main_lib.js" type='text/javascript'></script>
<script language='javascript' src='../javascript/global_din_fordel.js' type='text/javascript'></script>

<script language='javascript' src='../javascript/sa_menu.js' type='text/javascript'></script>
<script language='javascript' src='../javascript/html_toolbox.js' type='text/javascript'></script>
<script language='javascript' src='../Scripts/jquery-1.4.1.js' type='text/javascript'></script>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>




<script>
    include_javascript_libraries("../");

    // Load the Visualization API and the piechart package.
    google.load('visualization', '1.0', { 'packages': ['corechart'] });
    google.load('visualization', '1', { packages: ['gauge'] });


    

</script>


<!-- ******************************************************************* -->
<!-- ************************ START PAGE ******************************* -->
<!-- ******************************************************************* -->




<script>
    
    // If OK password we return the menu ...
    paint_password();

    paint_menu_1_and_work_page();
    
    init_ajax_web_form();

    window.onload = function () {
        // init_ajax_web_form();
        var e = document.getElementById("user_name");
        if (e) e.focus();
    }


    function level_1_shop_menu() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_1_shop_menu()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }
    


    function level_2_update_show_all_shops()
    {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_update_show_all_shops()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function level_2_report_show_all_shops() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_report_show_all_shops()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function level_1_report() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_1_report()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function menu_2_report_1() {
        if (ClientCall(arguments)) {
            ajax.call_server("menu_2_report_1()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function level_2_report_2() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_report_2()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function level_2_report_3() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_report_3()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
            // 14 sept
            drawChart("0", null, false);
            // drawChart("1", null, false);
        }
    }

    function level_2_report_3() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_report_3()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
            // 14 sept
            drawChart("0", null, false);
            // drawChart("1", null, false);
        }
    }



    function level_1_analyze() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_1_analyze()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function level_2_analyze_1() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_analyze_1()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function level_2_analyze_2() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_analyze_2()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function level_2_analyze_3() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_analyze_3()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function level_2_analyze_4() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_analyze_4()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function level_2_shop_report_show_chain_invoice() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_shop_report_show_chain_invoice()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function level_1_campaign() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_1_campaign()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function level_2_report_verifone_to_webservice() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_report_verifone_to_webservice()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function level_2_report_azure_deployment() {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_report_azure_deployment()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }



    function shop_update_show_shop(shop_id) 
    {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_show_shop()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_update_enrollment_click(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_enrollment_click()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_update_enrollment_save(shop_id, sCheckboxId) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_enrollment_save()", shop_id, getCheckBoxValue(sCheckboxId));
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_update_enrollment_cancel(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_enrollment_cancel()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function shop_update_enrollment_sms_click(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_enrollment_sms_click()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_update_enrollment_sms_save(shop_id, sSmsEnrollmentTextId) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_enrollment_sms_save()", shop_id, getTextFieldValue(sSmsEnrollmentTextId));
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_update_enrollment_sms_cancel(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_enrollment_sms_cancel()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }



    function shop_update_merchant_id_click(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_merchant_id_click()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_update_merchant_id_save(shop_id, input_field_id)
    {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_merchant_id_save()", shop_id, getTextFieldValue(input_field_id));
        }
        else if (ServerCall(arguments)) 
        {
            ajax.render_server_html();
        }
    }

    function shop_update_merchant_id_cancel(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_merchant_id_cancel()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_update_close_shop(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_update_close_shop()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    /* **************** REPORT **************************************************** */
    function shop_report_show_shop(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_report_show_shop()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_report_close_shop(shop_id) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_report_close_shop()", shop_id);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function level_2_live_start_show() {
        if (ClientCall(arguments)) {

            // var x = document.getElementById("menu_1");
            // x.style.display = "none";
            ajax.call_server("level_2_live_start_show()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }

    function shop_live_card_inserted_event(bax, token, event_type, hour_minute_second) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_live_card_inserted_event()", bax, token, event_type, hour_minute_second);
        }
        else if (ServerCall(arguments)) {
        
            ajax.render_server_html();
        }
    }

    function shop_live_basket_event(basket_id, token, event_type, hour_minute_second) {        
        if (ClientCall(arguments)) {
            ajax.call_server("shop_live_basket_event()", basket_id, token, event_type, hour_minute_second);
        }
        else if (ServerCall(arguments)) {

            ajax.render_server_html();
        }
    }

    function shop_live_accepted_membership_event(basket_id, token, event_type, hour_minute_second) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_live_accepted_membership_event()", basket_id, token, event_type, hour_minute_second);
        }
        else if (ServerCall(arguments)) {

            ajax.render_server_html();
        }
    }


    function move_around() 
    {
        var x = document.getElementById('dag_id');
        x.setAttribute("x", 10 + "px")
        x.setAttribute("y", 10 + "px")

        var x = document.getElementById('dag_id2');
        x.setAttribute("x", 100 + "px")
        x.setAttribute("y", 100 + "px")


        // x.setAttribute("x", 300);

    }


    function shop_live_date_clicked() {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_live_date_clicked()");
        }
        else if (ServerCall(arguments)) {

            var e = document.getElementById("shop_live_date_modify");
            e.style.display = "";
            ajax.render_server_html();
        }
    }


    function shop_live_date_clicked_cancel() {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_live_date_clicked_cancel()");
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
        }
    }


    function shop_live_refresh(year,month,day,hours,to_hour) {
        if (ClientCall(arguments)) {
            ajax.call_server("shop_live_refresh()",year,month,day,hours,to_hour);
        }
        else if (ServerCall(arguments)) {

            ajax.render_server_html();
        }
    }

    function shop_live_refresh_hours_and_to_hour()
    {
        if (ClientCall(arguments)) {
            var sHours = getSelectedComboValue("nof_hours");
            var sToHour = getSelectedComboValue("to_hour");
            ajax.call_server("shop_live_refresh_hours_and_to_hour()", sHours, sToHour);
        }
        else if (ServerCall(arguments)) {

            ajax.render_server_html();
            // setInterval(function () { alert("Hello") }, 3000);
        }
    }

    var globalTimer = 0;

    function shop_live_start_timer()
    {
        globalTimer = setInterval(function () { shop_live_start_timer_job() }, 1000);
    }

    function shop_live_stop_timer_job() {
        clearInterval(globalTimer);

        if (ClientCall(arguments)) {
            ajax.call_server("shop_live_stop_timer_job()");
        }
        else if (ServerCall(arguments)) {
            lastTimerMinutes = -1;
            ajax.render_server_html();
        }

    }


    var lastTimerMinutes = -1;

    function shop_live_start_timer_job()
    {
        if (ClientCall(arguments)) {

            var d = new Date();
            var h = d.getHours();
            var m = d.getMinutes();
            var s = d.getSeconds();
            if (h < 10) h = "0" + h;
            if (m < 10) m = "0" + m;
            if (s < 10) s = "0" + s;

            ajax.set_html_to("visible_time", h + ":" + m + ":" + s);

            if (lastTimerMinutes != m) 
            {
                lastTimerMinutes = m;
                ajax.call_server("shop_live_start_timer_job()");
            }
        }
        else if (ServerCall(arguments)) {

            ajax.render_server_html();
        }
    }


    var focused_index = -1;

    function chain_get_dashboard_data(index,bMaximized) 
    {
        if (ClientCall(arguments)) {
            ajax.call_server("chain_get_dashboard_data()", index, bMaximized);
        }
        else if (ServerCall(arguments)) {
            var index = ajax.get_server_value("dashboard_index");

            var json_chart_data = null;
            
            try {
                json_chart_data = JSON.parse(ajax.get_server_value("json_chart_data"));
            }
           catch (error) {
               alert("Error:" + error);
            }
            
            try {
                drawChart(index, json_chart_data, ajax.get_server_value("maximized"));

                

            }
            catch (error) {
            }

            // Resize 11. sept.
            for (i = 0; i <= 8; ++i) {

                var eChartData = document.getElementById("chart_div_" + index);

                // alert(eChartData.attributes.data);
                // lert(eChartData.attributes.data[0]);

                if (i != index) {
                    size_div_to("outer_chart_id_" + i, 150, 250, 0.2)
                    eChartData.attributes.dataset.maxi_view = "false";
                    // alert(eChartData.attributes.dataset.maximized);
                }
                else {
                    if (focused_index != index) {
                        focused_index = index;
                        size_div_to("outer_chart_id_" + index, 300, 500, 0.2)
                        eChartData.attributes.dataset.maximized = "true";
                    }
                    else {
                        focused_index = -1;
                        size_div_to("outer_chart_id_" + i, 150, 250, 0.2)
                        eChartData.attributes.dataset.maxi_view = "false";
                        // alert(eChartData.attributes.dataset.maximized);
                    }
                }
            }
        }
    }

    function size_div_to(id, h, w, seconds) {
        var e = document.getElementById(id);
        if (!e) return;
        e.style.transition = "height " + seconds + "s," + "width " + seconds + "s";
        e.style.height = h + "px";
        e.style.width = w + "px";
    }

    var dashTab = []

    function chain_dashboard_member_development(index,json_list,bMaximized) {
        var data = new google.visualization.DataTable();

        var options = { 'title': 'Medlemsutvikling 2015',
            'width': 250,
            'height': 100
        };

        data.addColumn('string', 'Task');
        data.addColumn('number', 'Antall medlemmer');

        for (var i = 0; i < json_list.graph_data.length; i++) {
            var mName = json_list.graph_data[i].name;
            var iCount = json_list.graph_data[i].value;
            data.addRow([mName, parseInt(iCount)]);
        }

        // Init and draw our chart, passing in some options.
        dashTab[index] = new google.visualization.ColumnChart(document.getElementById('chart_div_' + index));        
        dashTab[index].draw(data, options);
    }

    function chain_dashboard_member_enrollment_state(index,json_list) {
        var gaugeOptions = { min: 0, max: 280, yellowFrom: 200, yellowTo: 250,
            redFrom: 250, redTo: 280, minorTicks: 5
        };

        var options = { 'title': '',
            'width': 250,
            'height': 100
        };

        var data = new google.visualization.DataTable();
        data.addColumn('number', '');
        // data.addColumn('number', 'Torpedo');
        data.addRows(1);
        data.setCell(0, 0, 100);
        // data.setCell(0, 1, 80);

        dashTab[index] = new google.visualization.Gauge(document.getElementById('chart_div_' + index));

        dashTab[index].draw(data, options);
    }


    function drawChart(id,json_list,bMaximized) {
    
        var chartIndex = parseInt(id);

        if (chartIndex == 0) 
        {
            if (bMaximized)
                chain_dashboard_member_development(id, json_list, true);
            else {
                ajax.set_html_to("chart_div_" + chartIndex, "Minimized");
                // var eDiv = document.getElementById("chart_div_" + chartIndex);
            }
        }
        else if (chartIndex == 1) 
        {
            if (bMaximized)
                ajax.set_html_to("chart_div_" + chartIndex, "Maximized");
            else {
                ajax.set_html_to("chart_div_" + chartIndex, "Minimized");
            }
            chain_dashboard_member_enrollment_state(id, json_list);
        }        
        google.visualization.events.addListener(chartIndex, 'select', selectHandler);        
    }


    function selectHandler(e) {
        
        var selectedItem = dashTab[0].getSelection()[0];
        if (selectedItem) {
            var topping = data.getValue(selectedItem.row, 0);
            alert('The user selected ' + topping);
        }
        else
            alert('nothing' + e);
    }


    function move_to_center(id)
    {
        var eMoving = document.getElementById(id);
        var new_x = parseInt(eMoving.dataset.center_x) - eMoving.offsetWidth / 2;
        eMoving.style.left = new_x + "px";
    }

    function level_2_report_4(period) {
        if (ClientCall(arguments)) {
            ajax.call_server("level_2_report_4()",period);
        }
        else if (ServerCall(arguments)) {
            ajax.render_server_html();
            
            document.getElementById('menu_1').style.display = "none"; 
            document.getElementById('menu_2').style.display = "none"; 
            
            move_to_center("frame_1_heading_1");
            move_to_center("frame_1_heading_2");
            move_to_center("frame_1_heading_3");
            
            move_to_center("frame_2_heading_1");
            move_to_center("frame_2_heading_2");
            move_to_center("frame_2_heading_3");

            move_to_center("frame_3_heading_1");
            move_to_center("frame_3_heading_2");
            move_to_center("frame_3_heading_3");

            display_chart_index();


            // display_chart_index("1"); // 24 sept
            // display_chart_index("3"); // 24 sept
        }
    }

    var global_chart_count = 0;

    function display_chart_index() 
    {
        // This one shows all charts ... at startup
        var cgi = JSON.parse(ajax.get_server_value("json_chart_global_info"));

        global_chart_count = parseInt(cgi.chart_global_info.dashboard_count);

        for (chart_index = 0; chart_index < global_chart_count; ++chart_index) 
        {            
            if (chart_index == 9) // 24 sept
            {
                alert('Her er vi. Skal tegne 4 linjer'); // 24 sept
            }
            else
            {
                var json_chart_data = JSON.parse(ajax.get_server_value("json_chart_data_" + chart_index));
                var json_chart_graphic = JSON.parse(ajax.get_server_value("json_chart_graphic_" + chart_index));

                if (chart_index == 1)
                {
                    var json_dim_and_values = JSON.parse(ajax.get_server_value("json_dim_and_values_" + chart_index));
                    draw_google_chart_lines_3("dashboard_cell_" + chart_index, json_chart_data, json_chart_graphic,json_dim_and_values); // 24 sept
                }
                else if (chart_index == 3)
                {
                    var json_dim_and_values = JSON.parse(ajax.get_server_value("json_dim_and_values_" + chart_index));
                    draw_google_chart_lines_4("dashboard_cell_" + chart_index, json_chart_data, json_chart_graphic,json_dim_and_values); // 24 sept
                }
                else if (chart_index == 4)
                {
                     draw_google_chart_sample_01("dashboard_cell_" + chart_index, json_chart_data, json_chart_graphic);
                }
                else
                {
                    draw_google_chart_sample_01("dashboard_cell_" + chart_index, json_chart_data, json_chart_graphic);
                }
            }
        }
    }

    function set_to_org_wh(e,index) {
        size_div_to(e.id, e.dataset.orgheight.replace("px", ""), e.dataset.orgwidth.replace("px", ""), 1)
        redisplay_chart(index, e.dataset.orgheight.replace("px", ""), e.dataset.orgwidth.replace("px", ""));
    }

    function is_org_wh(e) {
        if (e.dataset.orgwidth == e.style.width) return true;
        return false;
    }

    function redisplay_chart(index,h,w) {
        var json_chart_data = JSON.parse(ajax.get_server_value("json_chart_data_" + index));
        var json_chart_graphic = JSON.parse(ajax.get_server_value("json_chart_graphic_" + index));
        json_chart_graphic.chart_graphic.w = w;
        json_chart_graphic.chart_graphic.h = h;
        draw_google_chart_sample_01('dashboard_cell_' + index, json_chart_data, json_chart_graphic);        
    }

    // 16 sept 2015
    function dashboard_cell_clicked(pre_id, index) {
        // test_chart();

        return;
        var eThisElement = document.getElementById(pre_id + index);

        var x = eThisElement.style.width;
        x = x.replace("px", "");

        if (is_org_wh(eThisElement)) {
            for (i = 0; i < parseInt(global_chart_count); ++i) {
                var e = document.getElementById(pre_id + i);

                if (i != index) {
                    if (e) {
                        size_div_to(pre_id + i, 0, 0, 1)
                        redisplay_chart(i, 0, 0);
                    }
                }
                else {
                    size_div_to(pre_id + i, 500, 980, 1)
                    // Funker ikke ... setTimeout("redisplay_chart(i, 500, 980);", 1000);
                    redisplay_chart(i, 500, 980);
                }
            }
        }
        else {
            if (x.length <= 1) // Small
            {
                for (i = 0; i < parseInt(global_chart_count); ++i) {
                    var e = document.getElementById(pre_id + i);

                    if (e) set_to_org_wh(e,i);
                }
            }
            else // Big
            {
                for (i = 0; i < parseInt(global_chart_count); ++i) {
                    var e = document.getElementById(pre_id + i);

                    if (e) set_to_org_wh(e,i);
                }
            }
        }
    }

    function draw_google_chart_sample_01(target_id, json_data,json_graphic) {

        var chartType = json_graphic.chart_graphic.type;
        if (chartType == "bember_heading_and_percent") {
            // ajax.set_html_to(target_id, "89 % enrollment totalt");            
            return;
        }

        var data = new google.visualization.DataTable();
        var w = json_graphic.chart_graphic.w;
        var h = json_graphic.chart_graphic.h;

        var chartTitle = json_graphic.chart_graphic.title;
        var hAxis = json_graphic.chart_graphic.h_axis;
        var vAxis = json_graphic.chart_graphic.v_axis;

        var options = null;

        if (chartType == "pie_donut") 
        {
            options = { 
                title: chartTitle,
                width: w,
                height: h,  
                legend: { position: 'top', maxLines: 3  },
                pieHole: 0.4
            };
        }
        else {
         options = {
                title: chartTitle,
                width: w,
                height: h,        
                legend: { position: 'none' },
              };
      }

      data.addColumn('string', 'Måned');
      data.addColumn('number', 'Antall medlemmer');

      for (var i = 0; i < json_data.graph_data.length; i++) {
          var mName = json_data.graph_data[i].name;
          var iCount = json_data.graph_data[i].value;
          data.addRow([mName, parseInt(iCount)]);
      }

      // Init and draw our chart, passing in some options.
      var eSample = null;

      if (chartType == "bar_vertical") eSample = new google.visualization.ColumnChart(document.getElementById(target_id));
      else if (chartType == "line") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "line_3_lines") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "line_4_lines") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "pie_donut") eSample = new google.visualization.PieChart(document.getElementById(target_id));
      else alert('Unknown:' + chartType);

      // alert(data.toJSON());

      eSample.draw(data, options);
    }





    function test_chart() {    
     
        var cgi = ajax.get_server_value("json_two_dim_one_value");

        var cgiData = JSON.parse(cgi);
        
        var options = {
          chart: {
            title: 'Company Performance',
            subtitle: 'Sales, Expenses, and Profit: 2014-2017',
          }
        };

        var data2 = new google.visualization.DataTable();

        for (i=0;i<cgiData.graph_data_dim_1.length;++i)
        {
            data2.addColumn(cgiData.graph_data_dim_1[i].type, cgiData.graph_data_dim_1[i].name);
        }

        /*
        data2.addColumn('string', 'Year');
        data2.addColumn('number', 'Sales');
        data2.addColumn('number', 'Expences');
        data2.addColumn('number', 'Profit');
        */
        data2.addRows(cgiData.graph_data_dim_2.length);
       
        for (i=0;i<cgiData.graph_data_dim_2.length;++i)
        {
            data2.setCell(i, 0, cgiData.graph_data_dim_2[i].name);

            for (v=0;v<cgiData.graph_data_dim_2[i].values.length;++v)
            {
                data2.setCell(i, v+1, cgiData.graph_data_dim_2[i].values[v].value);
            }
        }
        var chart = new google.visualization.ColumnChart(document.getElementById("test_chart"));

        chart.draw(data2, options);
    }


    function draw_google_chart_lines_3(target_id, json_data,json_graphic, json_columns) {

        // alert('3_lines');
        var chartType = json_graphic.chart_graphic.type;
        if (chartType == "bember_heading_and_percent") {
            // ajax.set_html_to(target_id, "89 % enrollment totalt");            
            return;
        }

        var data = new google.visualization.DataTable();
        var w = json_graphic.chart_graphic.w;
        var h = json_graphic.chart_graphic.h;

        // w = 1200;

        var chartTitle = json_graphic.chart_graphic.title;
        var hAxis = json_graphic.chart_graphic.h_axis;
        var vAxis = json_graphic.chart_graphic.v_axis;

        var options = null;

        if (chartType == "pie_donut") 
        {
            options = { 
                title: chartTitle,
                width: w,
                height: h,  
                legend: { position: 'top', maxLines: 3  },
                pieHole: 0.4
            };
        }
        else {


         options = {

           pointSize: 6,
            series: {
                0: { pointShape: 'circle' },
                1: { pointShape: 'circle' },
                2: { pointShape: 'circle' }
            },
 
         series: {
            0: { color: 'rgb(69,144,121)' },
            1: { color: 'rgb(244,166,40)' },
            2: { color: 'rgb(59,135,224)' }
            },

            title: chartTitle,
            backgroundColor: 'rgb(247,247,247)',
            vAxis:{
                baselineColor: 'rgb(234,234,234)',
                gridlineColor: 'rgb(234,234,234)',
                textPosition: 'none'
            },            
           
            width: w,
            height: h,        
            legend: { position: 'none' },
         };
      }

      data.addColumn('string', json_columns.columns.dim_1);
      data.addColumn('number', json_columns.columns.value_1);
      data.addColumn('number', json_columns.columns.value_2);
      data.addColumn('number', json_columns.columns.value_3);

      for (var i = 0; i < json_data.graph_data.length; i++) {
          var mName = json_data.graph_data[i].name;
          var iCount = json_data.graph_data[i].value;
          var iCount2 = json_data.graph_data[i].value2;
          var iCount3 = json_data.graph_data[i].value3;
          data.addRow([mName, parseInt(iCount),parseInt(iCount2),parseInt(iCount3)]);
      }

      // Init and draw our chart, passing in some options.
      var eSample = null;

      if (chartType == "bar_vertical") eSample = new google.visualization.ColumnChart(document.getElementById(target_id));
      else if (chartType == "line") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "line_3_lines") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "line_4_lines") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "pie_donut") eSample = new google.visualization.PieChart(document.getElementById(target_id));
      else alert('Unknown:' + chartType);

      // alert(data.toJSON());

      eSample.draw(data, options);
    }



    function draw_google_chart_lines_4(target_id, json_data,json_graphic, json_columns) {

        // alert('3_lines');
        var chartType = json_graphic.chart_graphic.type;
        if (chartType == "bember_heading_and_percent") {
            // ajax.set_html_to(target_id, "89 % enrollment totalt");            
            return;
        }

        var data = new google.visualization.DataTable();
        var w = json_graphic.chart_graphic.w;
        var h = json_graphic.chart_graphic.h;

        var chartTitle = json_graphic.chart_graphic.title;
        var hAxis = json_graphic.chart_graphic.h_axis;
        var vAxis = json_graphic.chart_graphic.v_axis;

        var options = null;

        if (chartType == "pie_donut") 
        {
            options = { 
                title: chartTitle,
                width: w,
                height: h,  
                legend: { position: 'top', maxLines: 3  },
                pieHole: 0.4
            };
        }
        else {
         options = {
                title: chartTitle,
                width: w,
                height: h,        
                legend: { position: 'none' },
              };
      }

      data.addColumn('string', json_columns.columns.dim_1);
      data.addColumn('number', json_columns.columns.value_1);
      data.addColumn('number', json_columns.columns.value_2);
      data.addColumn('number', json_columns.columns.value_3);
      data.addColumn('number', json_columns.columns.value_4);

      for (var i = 0; i < json_data.graph_data.length; i++) {
          var mName = json_data.graph_data[i].name;
          var iCount = json_data.graph_data[i].value;
          var iCount2 = json_data.graph_data[i].value2;
          var iCount3 = json_data.graph_data[i].value3;
          var iCount4 = json_data.graph_data[i].value4;
          data.addRow([mName, parseInt(iCount),parseInt(iCount2),parseInt(iCount3),parseInt(iCount4)]);
      }

      // Init and draw our chart, passing in some options.
      var eSample = null;

      if (chartType == "bar_vertical") eSample = new google.visualization.ColumnChart(document.getElementById(target_id));
      else if (chartType == "line") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "line_3_lines") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "line_4_lines") eSample = new google.visualization.LineChart(document.getElementById(target_id));
      else if (chartType == "pie_donut") eSample = new google.visualization.PieChart(document.getElementById(target_id));
      else alert('Unknown:' + chartType);

      // alert(data.toJSON());

      eSample.draw(data, options);
    }





</script>

<div id=chain_start_date></div>
<div id=chain_end_date></div>


</body>
</html>
