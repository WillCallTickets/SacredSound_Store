
/***********************************************
* Universal Countdown script- © Dynamic Drive (http://www.dynamicdrive.com)
* This notice MUST stay intact for legal use
* Visit http://www.dynamicdrive.com/ for this script and 100s more.
***********************************************/
function cdLocalTime(spanName, servertimestring, offsetMinutes, targetdate, debugmode){

    //if we cant find the div there is nowhere to display this - quit
    var container = "tmr" + spanName;
    if (!document.getElementById || !document.getElementById(container)) return;
    this.spanName = spanName;
    this.container = document.getElementById(container)

    this.includeClientSleepTolerance = 1;
    this.sleeptolerancesecs = 30;
    this.debugmode = (typeof debugmode!="undefined")? 1 : 0
    this.alertstring = "Please be aware that your device fell asleep and we need to update the time on your cart item(s). Items may be removed from your cart.";
    
    //start setting localtime
    this.initialtime = new Date();
    this.serverdate = new Date(servertimestring)
    this.localtime= new Date();    
    this.localtime.setTime(this.serverdate.getTime()+offsetMinutes*60*1000) //add user offset to server time    
    this.targetdate=new Date(targetdate)
    
    this.timesup=false
    this.updateTime()
}

cdLocalTime.prototype.updateTime = function () {
    var thisobj = this
    this.localtime.setSeconds(this.localtime.getSeconds() + 1)
    this.initialtime.setSeconds(this.initialtime.getSeconds() + 1)

    setTimeout(function () { thisobj.updateTime() }, 1000) //update time every second
}

cdLocalTime.prototype.displaycountdown=function(baseunit, functionref){
    this.baseunit=baseunit
    this.formatresults=functionref
    this.showresults()
}

cdLocalTime.prototype.showresults = function () {

    if (!this.container) return;

    var thisobj = this
    var devtime = new Date()

    /*****CHECK TOLERANCE*****/
    //compare current time with incremented initialtime
    //if these times are out of tolerance - update localtime
    if (this.includeClientSleepTolerance) {

        var basediff = (devtime - this.initialtime)
        if (basediff == undefined)
            basediff = (this.initialtime - devtime)

        if (basediff != undefined && basediff > (this.sleeptolerancesecs * 1000)) {

            alert(this.alertstring);

            this.localtime.setTime(this.localtime.getTime() + basediff);
            this.initialtime = new Date();

            setTimeout(function () { thisobj.showresults() }, 1000);
          
        }
    }
    /*****END CHECK TOLERANCE*****/

    var debugstring = (this.debugmode) ?
        "<p style=\"background-color: #FCD6D6; color: black; padding: 5px\"><big>Debug Mode on!</big><br /><b>Current Local time:</b> " +
        this.localtime.toLocaleString() +
        "<br />Verify this is the correct current local time, in other words, time zone of count down date.<br /><br />" +
        "<b>Target Time:</b> " +
        this.targetdate.toLocaleString() + "<br />Verify this is the date/time you wish to count down to (should be a future date)." : ""

    if (this.debugmode && this.includeClientSleepTolerance) {
        debugstring += "<br/><br/><b>Initial Time:</b>" + this.initialtime.toLocaleString() + "<br /><br />" +
            "<b>Loop Time:</b>" + devtime.toLocaleString() + "<br /><br/>"
    }

    if (debugstring.length > 0) {
        debugstring += "</p>";
    }


    var timediff = (this.targetdate - this.localtime) / 1000 //difference btw target date and current date, in seconds

    if (timediff < 0) { //if time is up

        this.timesup = true
        this.container.innerHTML = debugstring + this.formatresults();

        var functioncontainer = "tmrfnc" + this.spanName;

        //if we CAN find the div - display
        if (document.getElementById(functioncontainer)) {

            this.functioncontainer = document.getElementById(functioncontainer);
            this.functioncontainer.style.display = "none";
        }

        //show alert and postback
        alert("Items have expired in your cart!");
        window.location.replace(location);

        return;
    }

    var oneMinute = 60 //minute unit in seconds
    var oneHour = 60 * 60 //hour unit in seconds
    var oneDay = 60 * 60 * 24 //day unit in seconds
    var dayfield = Math.floor(timediff / oneDay)
    var hourfield = Math.floor((timediff - dayfield * oneDay) / oneHour)
    var minutefield = Math.floor((timediff - dayfield * oneDay - hourfield * oneHour) / oneMinute)
    var secondfield = Math.floor((timediff - dayfield * oneDay - hourfield * oneHour - minutefield * oneMinute))

    if (this.baseunit == "hours") { //if base unit is hours, set "hourfield" to be topmost level
        hourfield = dayfield * 24 + hourfield
        dayfield = "n/a"
    }
    else if (this.baseunit == "minutes") { //if base unit is minutes, set "minutefield" to be topmost level
        minutefield = dayfield * 24 * 60 + hourfield * 60 + minutefield
        dayfield = hourfield = "n/a"
    }
    else if (this.baseunit == "seconds") { //if base unit is seconds, set "secondfield" to be topmost level
        var secondfield = timediff
        dayfield = hourfield = minutefield = "n/a"
    }

    this.container.innerHTML = debugstring + this.formatresults(dayfield, hourfield, minutefield, secondfield)
    setTimeout(function () { thisobj.showresults() }, 1000) //update results every second


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
        var mins = arguments[2];
        var mindisplayleadingzero = "";
        if(mins < 10)
            mindisplayleadingzero = "0";
        
        var secs = arguments[3];
        var secdisplayleadingzero = "";
        if(secs < 10)
            secdisplayleadingzero = "0";
                
        var displaystring = "item reserved for <span class='clock_wonb'>" + mindisplayleadingzero + mins + "m " + secdisplayleadingzero + secs + "s </span>";     
    }
    else{ //else if target date/time met
        var displaystring="<span class='expired'>these items have expired in your cart.</span>"
    }
    return displaystring
}

//called from the page 
//gets cookie and finds cart items
//displays counters for each item
function displayItemCountdowns(servertime) {

    items_cookie = getSiteCookie("crtitms");//get the cartitems cookie
            
	if(items_cookie.length > 0)
	{
		//for every item display a countdown
		items = items_cookie.split('~');
	    items.pop();//remove the last item which is empty due to the trailing separator

	    if (items.length > 0) {
	        
		    //so, for each ticket in the cart.....
		    for(var i=0;i<items.length;i++)
		    {
			    //now take the individual ticket and split it into its values
			    itemInfo = items[i].split(',');
			    
			    spanName    = itemInfo[0];
				expiry      = itemInfo[1];
				
				var launchtimer = new cdLocalTime(spanName, servertime, 0, expiry)
                launchtimer.displaycountdown("minutes", formatresults)
			}
	    }
	}
}

