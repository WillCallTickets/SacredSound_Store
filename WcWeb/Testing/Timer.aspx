<%@ Page Language="C#" AutoEventWireup="true"
   CodeFile="Timer.aspx.cs" Inherits="WillCallWeb.Timer" Title="- Timer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>


<style type="text/css">

.lcdstyle{ /*Example CSS to create LCD countdown look*/
background-color:black;
color:lime;
font: bold 18px MS Sans Serif;
padding: 3px;
}

.lcdstyle sup{ /*Example CSS to create LCD countdown look*/
font-size: 80%
}

</style>

<script type="text/javascript">
/***********************************************
* Universal Countdown script- © Dynamic Drive (http://www.dynamicdrive.com)
* This notice MUST stay intact for legal use
* Visit http://www.dynamicdrive.com/ for this script and 100s more.
***********************************************/

function cdLocalTime(container, servermode, offsetMinutes, targetdate, debugmode){
    
    //if we cant find the div there is nowhere to display this - quit
    if (!document.getElementById || !document.getElementById(container)) return;
    
    //get the div
    this.container=document.getElementById(container)
    
    //gets the servers time
    var servertimestring=(servermode=="server-php")? 
        '<? print date("F d, Y H:i:s", time())?>' : 
        (servermode=="server-ssi")? 
            '<!--#config timefmt="%B %d, %Y %H:%M:%S"--><!--#echo var="DATE_LOCAL" -->' : 
            '<%= DateTime.Now %>'
    
    //start setting localtime
    this.serverdate=new Date(servertimestring)
    this.localtime= new Date();
    
    this.localtime.setTime(this.serverdate.getTime()+offsetMinutes*60*1000) //add user offset to server time
    alert('local: ' + this.localtime + ' server: ' + this.serverdate);
    
    this.targetdate=new Date(targetdate)
    this.debugmode=(typeof debugmode!="undefined")? 1 : 0
    this.timesup=false
    this.updateTime()
}

cdLocalTime.prototype.updateTime=function(){
    var thisobj=this
    this.localtime.setSeconds(this.localtime.getSeconds()+1)
    setTimeout(function(){thisobj.updateTime()}, 1000) //update time every second
}

cdLocalTime.prototype.displaycountdown=function(baseunit, functionref){
    this.baseunit=baseunit
    this.formatresults=functionref
    this.showresults()
}

cdLocalTime.prototype.showresults=function(){
    
    var thisobj=this
    var debugstring=(this.debugmode)? 
        "<p style=\"background-color: #FCD6D6; color: black; padding: 5px\"><big>Debug Mode on!</big><br /><b>Current Local time:</b> "+
        this.localtime.toLocaleString()+
        "<br />Verify this is the correct current local time, in other words, time zone of count down date.<br /><br /><b>Target Time:</b> "+
        this.targetdate.toLocaleString()+"<br />Verify this is the date/time you wish to count down to (should be a future date).</p>" : 
        ""

    var timediff=(this.targetdate-this.localtime)/1000 //difference btw target date and current date, in seconds
    if (timediff<0){ //if time is up
        this.timesup=true
        this.container.innerHTML=debugstring+this.formatresults()
        return
    }

    var oneMinute=60 //minute unit in seconds
    var oneHour=60*60 //hour unit in seconds
    var oneDay=60*60*24 //day unit in seconds
    var dayfield=Math.floor(timediff/oneDay)
    var hourfield=Math.floor((timediff-dayfield*oneDay)/oneHour)
    var minutefield=Math.floor((timediff-dayfield*oneDay-hourfield*oneHour)/oneMinute)
    var secondfield=Math.floor((timediff-dayfield*oneDay-hourfield*oneHour-minutefield*oneMinute))

    if (this.baseunit=="hours"){ //if base unit is hours, set "hourfield" to be topmost level
        hourfield=dayfield*24+hourfield
        dayfield="n/a"
    }
    else if (this.baseunit=="minutes"){ //if base unit is minutes, set "minutefield" to be topmost level
        minutefield=dayfield*24*60+hourfield*60+minutefield
        dayfield=hourfield="n/a"
    }
    else if (this.baseunit=="seconds"){ //if base unit is seconds, set "secondfield" to be topmost level
        var secondfield=timediff
        dayfield=hourfield=minutefield="n/a"
    }
    
    if(! this.container)
        return;
        
    this.container.innerHTML=debugstring+this.formatresults(dayfield, hourfield, minutefield, secondfield)
    setTimeout(function(){thisobj.showresults()}, 1000) //update results every second
}

/////CUSTOM FORMAT OUTPUT FUNCTIONS BELOW//////////////////////////////

//Create your own custom format function to pass into cdLocalTime.displaycountdown()
//Use arguments[0] to access "Days" left
//Use arguments[1] to access "Hours" left
//Use arguments[2] to access "Minutes" left
//Use arguments[3] to access "Seconds" left

//The values of these arguments may change depending on the "baseunit" parameter of cdLocalTime.displaycountdown()
//For example, if "baseunit" is set to "hours", arguments[0] becomes meaningless and contains "n/a"
//For example, if "baseunit" is set to "minutes", arguments[0] and arguments[1] become meaningless etc

//1) Display countdown using plain text
function formatresults(){
    if (this.timesup==false){//if target date/time not yet met
    var displaystring="<span style='background-color: #CFEAFE'>"+arguments[1]+" hours "+arguments[2]+" minutes "+arguments[3]+" seconds</span> left until launch time"
    }
    else{ //else if target date/time met
        var displaystring="Launch time!"
    }
    return displaystring
}

//2) Display countdown with a stylish LCD look, and display an alert on target date/time
function formatresults2(){
if (this.timesup==false){ //if target date/time not yet met
    var displaystring="<span class='lcdstyle'>"+arguments[0]+" <sup>days</sup> "+arguments[1]+" <sup>hours</sup> "+arguments[2]+" <sup>minutes</sup> "+arguments[3]+" <sup>seconds</sup></span> left until launch time"
    }
    else{ //else if target date/time met
    var displaystring="" //Don't display any text
    alert("Launch time!") //Instead, perform a custom alert
    }
    return displaystring
}

</script>

</head>
<body>
    <form id="form1" runat="server">
    <div>



<table border="1" cellpadding="3" cellspacing="0" width="600px">


<tr>
    <th>ME</th><th>EST</th><th>CST</th><th>MST</th><th>PST</th><th>Server</th><th>JAPAN?</th>
</tr>
<tr>
    <td><div id="tmrclient" class="clock"></div></td>
    <td><div id="tmrest" class="clock"></div></td>
    <td><div id="tmrcst" class="clock"></div></td>
    <td><div id="tmrmst" class="clock"></div></td>
    <td><div id="tmrpst" class="clock"></div></td>
    <td><div id="tmrsrv" class="clock"></div></td>
    <td><div id="tmrjpn" class="clock"></div></td>
</tr>

</table>
<div id="cdcontainer"></div>

<script type="text/javascript">
    //cdLocalTime("ID_of_DIV_container", "server_mode", LocaltimeoffsetMinutes, "target_date", "opt_debug_mode")
    //cdLocalTime.displaycountdown("base_unit", formatfunction_reference)

    //Note: "launchdate" should be an arbitrary but unique variable for each instance of a countdown on your page:

    var launchdate = new cdLocalTime("cdcontainer", "server-asp", 0, "April 23, 2010 15:53:00", "debugmode")
    launchdate.displaycountdown("days", formatresults2)
    
</script>

    </div>
    </form>
</body>
</html>
