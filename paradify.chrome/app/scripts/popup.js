String.format = function () {
    var s = arguments[0];

    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}

var defaults = {
    url: "http://search.paradify.com/",
    searchJsonPath: "searchJson",
    searchPath: "search",
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
        searchQuery($(defaults.searchBoxClass).val());
    });
    $(defaults.query).keypress(function (event) {
        if (event.which == defaults.events.ENTER) {
            searchQuery($(defaults.searchBoxClass).val());
        }
    });
});


function searchQuery(query) {
    window.q = encodeURIComponent(query);
    window.fullJsonUrl = String.format("{0}{1}?q={2}", defaults.url, defaults.searchJsonPath, encodeURIComponent(query));

    $.ajax({
        type: "GET",
        url: window.fullJsonUrl,
        success: function (htmlResult) {
            $(defaults.waitingId).addClass('hidden');
            $(defaults.resultId).removeClass('hidden');
            $(defaults.resultId).html(htmlResult);
            if (htmlResult.indexOf('Oh snap') > -1) {
                $(defaults.formId).removeClass('hidden');
            } else {
                initPlayback();
                initTracksLink();
            }

        },
        error: function (xhr, textStatus, err) {
            console.log(xhr);
            console.log(textStatus);
            console.log(err);
        }
    });
    return;
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
                searchQuery(q);
            } else {
                $(defaults.resultId).addClass('hidden');
                $(defaults.formId).removeClass('hidden');
                $(defaults.waitingId).addClass('hidden');

            }
        });
    });
}

var initTracksLink = function () {
    $(".trackId[trackId]").each(function () {
        var trackId = $(this).attr("trackId");
        if (trackId != undefined && trackId != '') {
            $(this).click(function () {
                var url = String.format("{0}{1}?q={2}&t={3}", defaults.url, defaults.searchPath, window.q, trackId);
                setTimeout(function(){
                    chrome.tabs.create({url: url});
                }, 100);

            });
        }
    });
}