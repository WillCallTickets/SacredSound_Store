
String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/g, "");
}
String.prototype.ltrim = function() {
    return this.replace(/^\s+/, "");
}
String.prototype.rtrim = function() {
    return this.replace(/\s+$/, "");
}

function showCoords(c) {
    // variables can be accessed here as
    // c.x, c.y, c.x2, c.y2, c.w, c.h
    
    jQuery('#x1').val(c.x);
    jQuery('#y1').val(c.y);
    jQuery('#x2').val(c.x2);
    jQuery('#y2').val(c.y2);
    jQuery('#w1').val(c.w);
    jQuery('#h1').val(c.h);
    
};

function resetAuthControls(btnId, blockId) {

    var btn = document.getElementById(btnId);
    if (btn != undefined)
        btn.style.display = 'inline-block';

    var block = document.getElementById(blockId);
    if (block != undefined)
        block.style.display = 'none';

}
function clickAuth(sender, blockingDivName, confirmText) {

    if (confirmText.length > 0) {

        confirmBox = confirm(confirmText);

        if (confirmBox != true) {

            return false;            

        }
    }

    disableButton(sender, blockingDivName);
}
function disableButton(sender, continueDiv) {

    var cont = document.getElementById(continueDiv);

    try {
        //hide sender and display continueDiv
        sender.style.display = 'none';
        cont.style.display = 'inline';
    }
    catch (e) { }
}

function allowNoMoreThan_N_Selections(sender, maxSelections) {

    var baseName = sender.name.substring(0, sender.name.length-1);

    //find the other checkboxes with the like id
    //regex
    //var baseId = baseName.replace(/\$/g, "_");
    var checks = new Array();
    
    //loop thru the elements and get all of the checkbox elements
    for (var i = 0; i < theForm.length; i++) {

        var theElement = theForm.elements[i];
        if (theElement.type != null && theElement.type == "checkbox" && theElement.name.indexOf(baseName) != -1 && theElement.checked) {
            checks.push(theElement);
        }
    }
    
    //if we are over selected - than this element cannot be selected
    //deselect and show alert?
    if (checks.length > maxSelections)
        sender.checked = false;
}

function showEmailLetterSelection(ddlName) 
{
    var elem = getElementByLikeId(document.forms[0], ddlName);
    
    if(elem != undefined) {
    
        if(elem.value != 0)
            doPagePopup('/Admin/MailerViewer.aspx?mlr=' + elem.value,'true')
    }
}

function FillTextBoxWithText(controlName, txt)
{
    var elem = getElementByLikeId(document.forms[0],controlName);
    
    if(elem != undefined) {
    
        elem.value = txt;
    }
}

function imagePopup(imageId, dimension) {
    
    try {
        var width = 48; //offset - a nice safe number
        width += parseInt(dimension,10);
        
        var URL = "/ImagePopup.aspx?img=" + imageId;
        
	    H = window.open("", 'Information', 'toolbar=0,scrollbars=1,location=0,status=0,menubar=0,resizable=1,width=' + width + ',height=' + dimension + ',screenX=100,screenY=200,top=100,left=200');
	    H.location.href = URL;
	    H.focus(); 
	}
	catch(ex) {}
}

function doPageTab(url) {

    try {
        var H

        H = window.open("", 'newpage');
        H.location.href = url;
        H.focus();
    }
    catch (Exception) { }
}

function doPagePopup(url, employBrowserFeatures) {
    
	try {
		var H
		if(employBrowserFeatures == undefined || employBrowserFeatures == 'false')
		    H = window.open("", 'Information', 'toolbar=1,scrollbars=1,location=0,status=0,menubar=0,resizable=1,width=1000,height=800,screenX=50,screenY=50,top=50,left=50');
		else
		    H = window.open("", 'Information', 'toolbar=1,scrollbars=1,location=1,status=1,menubar=1,resizable=1,width=1000,height=800,screenX=50,screenY=50,top=50,left=50');
		H.location.href = url;
		H.focus(); 
	}
	catch(Exception) {}
}

function doPagePopupWithDimension(url, width, height) {
	try {
		
		var H = window.open("", 'Information', 'toolbar=0,scrollbars=0,location=1,status=0,menubar=0,resizable=1,width=' +  width + ',height=' + height + ',screenX=50,screenY=50,top=50,left=50');
		H.location.href = url;
		H.focus(); 
	}
	catch(Exception) {}
}

function Set_Cookie( name, value, expires, path, domain, secure ) {

	// set time, it's in milliseconds
	var today = new Date();
	today.setTime( today.getTime() );

	/*
	if the expires variable is set, make the correct (in minutes) 1000 * 60
	expires time, the current script below will set 
	it for x number of days, to make it for hours, 
	delete * 24, for minutes, delete * 60 * 24
	*/
	if ( expires ) {
		expires = expires * 1000 * 60;// * 60 * 24;
	}
	var expires_date = new Date( today.getTime() + (expires) );

	document.cookie = name + "=" +escape( value ) + ( ( expires ) ? ";expires=" + expires_date.toGMTString() : "" ) + 
	( ( path ) ? ";path=" + path : "" ) + ( ( domain ) ? ";domain=" + domain : "" ) + ( ( secure ) ? ";secure" : "" );
}

function getSiteCookie(cookieName) {
    if (document.cookie.length > 0) {
        if (cookieName == undefined) {
            return unescape(document.cookie);
        }
        else {
            c_name = cookieName; //this is our site cookie
            c_start = document.cookie.indexOf(c_name + "=")

            if (c_start != -1) {
                c_start = c_start + c_name.length + 1; //allow for equal sign

                //check for other cookies within the cookie
                //if not found then use the end of this cookie
                //if not found then use the end of the entire cookie
                c_end = document.cookie.indexOf("&", c_start);
                if (c_end == -1)
                    c_end = document.cookie.indexOf(";", c_start);
                if (c_end == -1)
                    c_end = document.cookie.length;

                var retVal = unescape(document.cookie.substring(c_start, c_end));

                return retVal;
            }
        }
    }

    return "";
}

// this deletes the cookie when called
function Delete_Cookie( name, path, domain ) {
	
	if ( Get_Cookie( name ) ) 
		document.cookie = name + "=" + ( ( path ) ? ";path=" + path : "") + ( ( domain ) ? ";domain=" + domain : "" ) + 
		";expires=Thu, 01-Jan-1970 00:00:01 GMT";
}


function ValidateRequiredCheckBox(source,args) {

	//source is the custom validator	
	if(source.id.indexOf('CustomTerms') != -1) {
		var elem = getElementByLikeId(document.forms[0],'CheckTerms');
		
		if(elem != undefined) {
			args.IsValid = elem.checked;
		}
		
	}
}

 //this works on controls that have been named with an underscore
function getDOMElement(parentId, controlToFind) {
    
    if(parentId == undefined)
        return "";
    
    var idParts;
    
    if(parentId.indexOf("$") != -1)
        idParts = parentId.split('$');
    else
        idParts = parentId.split(':');
    
    //idParts.pop();//get rid of the last element - the parent id name
    idParts.push(controlToFind);
    //return uniqueID.replace(/\$/g, '_');
    var controlName = idParts.join("_");
    var ret = $get(controlName);
    
    return ret;
}
function getChildElement(parentId, controlToFind) { 
    
    if(parentId == undefined)
        return "";
    
    var idParts;
    
    if(parentId.indexOf("$") != -1)
        idParts = parentId.split('$');
    else
        idParts = parentId.split(':');
    
    idParts.pop();//get rid of the last element - the parent id name
    idParts.push(controlToFind);
    var controlName = idParts.join("_");
    var ret = $get(controlName);
    
    return ret;
}

function redirect(url) {
    window.location = url;
}
    
	
//Use this one		
function getElementByLikeId(form,elementName) {
	
	var theForm = form;
	
	for(var i=0; i<theForm.length; i++) {
		
		var theElement = theForm.elements[i];
		
		if ((theElement.type != null) && ((theElement.id.indexOf(elementName) != -1) || (theElement.name.indexOf(elementName) != -1)))
		{								
			return theElement;
		}
	}
	
	return undefined;
}

function EnsureShipCheck(sender, checkId) { 
    
    var checkbox = getElementByLikeId(document.forms[0], checkId);
    
    if(sender != undefined && checkbox != undefined) { 
        
        var input = sender.value;
        if(input != undefined && input.length > 0)
            checkbox.checked = false;
    }
}


function SetFieldFocus(source, fieldToFocus) {

	getElementByLikeId(source.document.forms[0], fieldToFocus).focus();
	
}


function setFieldFocus(strForm, fieldToFocus) {

	getElementByLikeId(strForm, fieldToFocus).focus();
	
}

function set_FieldFocus(strField) { 

	var frm = document.forms[0];
	getElementByLikeId(frm, strField).focus();
}

function CloseTheWindow() {
	
	try
	{
		netscape.security.PrivelegeManager.enablePrivelege("UniversalBrowserWrite");
		
	}
	catch(Exception){}
	
	self.close();
}
			