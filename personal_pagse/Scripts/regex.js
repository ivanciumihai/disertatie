function ValidateNumber(event) {

    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /^([0-9,]|10|)/;

    if (!regex.test(key)) {
        theEvent.preventDefault ? theEvent.preventDefault() : (theEvent.returnValue = false);
    }
}


function ValidateGroup(event) {

    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /^([0-9]|9999)/;

    if (!regex.test(key)) {
        theEvent.preventDefault ? theEvent.preventDefault() : (theEvent.returnValue = false);
    }
}


function ValidateCredits(event) {

    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /^([0-9]|9999)/;

    if (!regex.test(key)) {
        theEvent.preventDefault ? theEvent.preventDefault() : (theEvent.returnValue = false);
    }
}