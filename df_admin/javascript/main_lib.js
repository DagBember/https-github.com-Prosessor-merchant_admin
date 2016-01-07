

var superaktiv_javascripts_included = false;

function include_javascript_libraries(sDirectory)
{
    if (superaktiv_javascripts_included == true)
    {
        return;
    }

    superaktiv_javascripts_included = true;
    // document.write("<link href='http://fonts.googleapis.com/css?family=Varela+Round' rel='stylesheet' type='text/css'>");
    // document.write("<link href='http://fonts.googleapis.com/css?family=Gabriela' rel='stylesheet' type='text/css'>");

    document.write("<link href='http://fonts.googleapis.com/css?family=Gabriela|Varela+Round|Russo+One|Alegreya+Sans+SC|Istok+Web|Share+Tech|Nixie+One|Nova+Square|Emblema+One|Hanalei|Montserrat+Alternates|Capriola|Belleza' rel='stylesheet' type='text/css'>");


    document.write("<script language='javascript' src='" + sDirectory + "javascript/ajax_library.js' type='text/javascript'></script>");
    document.write("<script language='javascript' src='" + sDirectory + "javascript/ajax.js' type='text/javascript'></script>");    
}


