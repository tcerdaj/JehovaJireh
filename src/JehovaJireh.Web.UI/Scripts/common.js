/*This is a Common file that store all the function and utility that
whe need in the whole UI.*/

var generalError = "An server error occurred and has been logged." +
    "\nPlease contact your administrator. " + "ServerException";

function createCookie(name, value, days) {
    $.cookie(name, value, { expires: days });
    //if fails we try using localStorage
    if (readCookie(name) === null || readCookie(name) === undefined) {
        localStorage.setItem(name, value);
    }
}

function readCookie(name) {
    var _localStorage = localStorage.getItem(name);
    if (_localStorage !== null)
        return _localStorage;
    return $.cookie(name);
}

function eraseCookie(name) {
    if (name !== '' && name !== undefined) {
        $.removeCookie(name);
        localStorage.removeItem(name);
    }
}

function getBaseUrl() {
    //return window.location.host.indexOf('localhost') > -1 ? "http://jehovajireh.web.service/" : "http://jehovajireh.web.service/";
   return window.location.host.indexOf('localhost') > -1 ? "http://localhost:58095/" : "http://jehovajireh.web.service/";
}

var donationStatus = function () {
	return $.ajax({
		url: "/Services/DonationStatus",
		datatype: "json",
		contentType: 'application/json',
		type: "GET",
		success: function (result) {
			console.log("success");
		},
		error: function (xhr, status, error) {
			alert(xhr.responseTest);
		}
	});
};

var donationTypes = function () {
	return $.ajax({
		url: "/Services/DonationTypes",
		datatype: "json",
		contentType: 'application/json',
		type: "GET",
		success: function (result) {
			console.log("success");
		},
		error: function (xhr, status, error) {
			alert(xhr.responseTest);
		}
	});
};


var getLanguages = function () {
    return $.ajax({
        url: getBaseUrl() + "bible/languages",
        datatype: "json",
        contentType: 'application/json',
        type: "GET",
        success: function (result) {
            console.log("success");
        },
        error: function (xhr, status, error) {
            alert(xhr.responseTest);
        }
    });
};


function displayFieldTextByArray(id, array) {
	var result = null;
	if (!(id === undefined || id === "" || id === null || id === "00000000-0000-0000-0000-000000000000") && !(array === undefined || array === "" || array === null)) {
		result = $.grep(array, function (n, i) {
			return n.Value === id.toString();
		});
	}
	return result !== null ? (result.length > 0?  result[0].Text !== undefined ? result[0].Text : "": "") : "";
}

function getItemByArray(uid, array) {
    var result = null;
    if (!(uid === undefined || uid === "" || uid === null || uid === "00000000-0000-0000-0000-000000000000") && !(array === undefined || array === "" || array === null)) {
        result = $.grep(array, function (n, i) {
            return n.uid === uid;
        });
    }
    return result !== null ? (result.length > 0 ? result[0]: null ) : null;
}

function getItemById(id, array) {
    var result = null;
    if (!(id === undefined || id === "" || id === null || id === "00000000-0000-0000-0000-000000000000") && !(array === undefined || array === "" || array === null)) {
        result = $.grep(array, function (n, i) {
            return n.id === id;
        });
    }
    return result !== null ? (result.length > 0 ? result[0] : null) : null;
}
function getArrayByCollectionCode(uid, array) {
    var result = null;
    if (!(uid === undefined || uid === "" || uid === null || uid === "00000000-0000-0000-0000-000000000000") && !(array === undefined || array === "" || array === null)) {
        result = $.grep(array, function (n, i) {
            return n.collection_code === uid;
        });
    }
    return result;
}


function findMemberFromList(id, array) {
    var result = null;
    if (!(id === undefined || id === "" || id === null || id === "00000000-0000-0000-0000-000000000000") && !(array === undefined || array === "" || array === null || array.length === 0)) {
        result = $.grep(array, function (n, i) {
            return n.ConnectionId === id.toString();
        });
    }
    return result !== null  && result.length > 0 ? result[0] : result;
}

function convertDictionaryToObject(dictionary) {
    var result = {};
    $.each(dictionary, function (index, value) {
        result[value["Key"]] = value["Value"];
    });

    return result;
}

var getInitials = function (string) {
    var names = string.split(' '),
        initials = names[0].substring(0, 1).toUpperCase();

    if (names.length > 1) {
        initials += names[names.length - 1].substring(0, 1).toUpperCase();
    }
    return initials;
};

// check if an element exists in array using a comparer function
// comparer : function(currentElement)
Array.prototype.inArray = function (comparer) {
    for (var i = 0; i < this.length; i++) {
        if (comparer(this[i])) return true;
    }
    return false;
}; 

// adds an element to the array if it does not already exist using a comparer 
// function
Array.prototype.pushIfNotExist = function (element, comparer) {
    if (!this.inArray(comparer)) {
        this.push(element);
    }
};

Array.prototype.removeIfExist = function (element, comparer) {
    if (this.inArray(comparer)) {
        this.slice(this.indexOf(element, 1));
    }
};

!function ($) {
    $.extend($.fn, {
        busyIndicator: function (c) {
            b = $(this);
            var d = b.find(".k-loading-mask");
            c ? d.length || (d = $("<div class='k-loading-mask'><span class='k-loading-text'>Loading...</span><div class='k-loading-image'/><div class='k-loading-color'/></div>").width(b.outerWidth()).height(b.outerHeight()).prependTo(b)) : d && d.remove()
        }
    });
}(jQuery);


function openDialog(title, htmlMessage) {

    var applySetup = $('.container #dialog').length === 0;
    var dialog;

    if (applySetup) {
        $('.container').append($('<div id="dialog"> </div >'));
        dialog = $('.container #dialog');

        if (dialog.length === 3) {
            $('.container #dialog:eq(2)').remove();
            $('.container #dialog:eq(1)').remove();
        }

        dialog.kendoDialog({
            width: "400px",
            title: title,
            closable: true,
            modal: false,
            content: htmlMessage,
            actions: [
                { text: 'Ok', primary: true }
            ],
        });
    }
    else {
        dialog = $('.container #dialog');
        dialog.data("kendoDialog").title(title);
        dialog.data("kendoDialog").content(htmlMessage);
    }

    dialog.data("kendoDialog").open();
}


