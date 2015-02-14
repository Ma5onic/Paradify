var defaults = {
    searchUrl: "http://search.paradify.com/Search",
    searchBoxClass: ".searchBox",
    clickButtonClass: ".clickButton",
    query: "#q",
    result: "result",
    resultId: "#result",
    formId: "#form",
    waitingId: "#waiting",
    events: {ENTER: 13}
}
var message = {action: "get"};

$(document).ready(function () {
    initQuery();

    $(defaults.clickButtonClass).click(function () {
        showIframe($(defaults.searchBoxClass).val());
    });
    $(defaults.query).keypress(function (event) {
        if (event.which == defaults.events.ENTER) {
            showIframe($(defaults.searchBoxClass).val());
        }
    });
});


function showIframe(query) {
    var fullUrl = String.format("{0}?q={1}", defaults.searchUrl, encodeURIComponent(query));
    chrome.tabs.create({url: fullUrl});
    return;
}

String.format = function () {
    var s = arguments[0];

    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}


var initQuery = function () {
    chrome.tabs.query({active: true, currentWindow: true}, function (tabs) {
        message.url = tabs[0].url.toLowerCase();
        if (message.url.indexOf('radioparadise') > -1) {
            message.pageName = 'radioparadise';
        } else if (message.url.indexOf('powerapp') > -1) {
            message.pageName = 'powerapp';
        } else if (message.url.indexOf('youtube.com') > -1) {
            message.pageName = 'youtube';
        } else if (message.url.indexOf('karnaval.com') > -1) {
            message.pageName = 'karnaval';
        } else if (message.url.indexOf('soundcloud.com') > -1) {
            message.pageName = 'soundcloud';
        }
        chrome.tabs.sendMessage(tabs[0].id, message, function (response) {
            if (response != undefined && response.success) {

                if (response.artist == undefined) {
                    response.artist = '';
                }
                var q = String.format("{0} {1}", response.track, response.artist);
                showIframe(q);
            } else {
                $(defaults.resultId).addClass('hidden');
                $(defaults.formId).removeClass('hidden');
                $(defaults.waitingId).addClass('hidden');

            }
        });
    });
}