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
    showLoading();
    window.q = encodeURIComponent(query);
    window.fullJsonUrl = String.format("{0}{1}?q={2}", defaults.url, defaults.searchJsonPath, encodeURIComponent(query));
    $(defaults.query).val(decodeURIComponent(window.q));

    $.ajax({
        type: "GET",
        url: window.fullJsonUrl,
        success: function (htmlResult) {
            $(defaults.waitingId).addClass('hidden');
            $(defaults.resultId).removeClass('hidden');
            $(defaults.resultId).html(htmlResult);
            if (htmlResult.indexOf('Oh snap') == -1) {
                initPlayback();
                initTracksLink();
            }
            $(defaults.formId).removeClass('hidden');
            initDuration();
        },
        error: function (xhr, textStatus, err) {
            console.log(xhr);
            console.log(textStatus);
            console.log(err);
        },
        done: function () {
            hideLoading();
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
        } else if (message.url.indexOf('vimeo.com') > -1) {
            message.pageName = 'vimeo';
        } else if (message.url.indexOf('dailymotion.com') > -1) {
            message.pageName = 'dailymotion';
        } else if (message.url.indexOf('kralmuzik.com.tr') > -1) {
            message.pageName = 'kralmuzik';
        } else if (message.url.indexOf('tunein.com') > -1) {
            message.pageName = 'tunein';
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
                setTimeout(function () {
                    chrome.tabs.create({url: url});
                }, 100);

            });
        }
    });
}

var showLoading = function () {
    $(defaults.waitingId).removeClass('hidden');
    $(defaults.resultId).addClass('hidden');
    //$(defaults.formId).addClass('hidden');
}

var hideLoading = function () {
    $(defaults.waitingId).addClass('hidden');
}

var initDuration = function () {
    $('.duraion').each(function (index, val) {
        $(this).html(millisToMinutesAndSeconds($(this).html()));
    });
}
function millisToMinutesAndSeconds(millis) {

    var minutes = Math.floor(millis / 60000);
    var seconds = ((millis % 60000) / 1000).toFixed(0);
    return minutes + ":" + (seconds < 10 ? '0' : '') + seconds;
}