chrome.runtime.sendMessage({type: 'clearBadge'});

function searchQueryResult(htmlResult) {
        $(defaults.waitingId).addClass('hidden');
        $(defaults.resultId).removeClass('hidden');
        $(defaults.resultId).html(htmlResult);
        if (htmlResult.indexOf('Oh snap') == -1) {
            initPlayback();
            initTracksLink();
        }
        $(defaults.formId).removeClass('hidden');
        initDuration();
}

function searchQuery(query, searchResult) {
    var fullJsonUrl = String.format("{0}{1}?q={2}", defaults.url, defaults.searchJsonPath, encodeURIComponent(query));
    showLoading();
    $.ajax({
        type: "GET",
        url: fullJsonUrl,
        success: function (htmlResult) {
            return searchResult(htmlResult);
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


$(document).ready(function () {
    initQuery();

    $(defaults.clickButtonClass).click(function () {
        searchQuery($(defaults.searchBoxClass).val(),searchQueryResult);
    });

    $(defaults.query).keypress(function (event) {
        if (event.which == defaults.events.ENTER) {
            searchQuery($(defaults.searchBoxClass).val(), searchQueryResult);
        }
    });
});

var initQuery = function () {
    chrome.tabs.query({active: true, currentWindow: true}, function (tabs) {

        var url = tabs[0].url.toLowerCase();
        var pageName = getPageName(url);
        if (pageName != undefined) {
            chrome.tabs.sendMessage(tabs[0].id, {type: 'getTrackInfo', pageName: pageName}, function (trackInfo) {
                if (trackInfo != undefined && trackInfo.success) {

                    if (trackInfo.artist == undefined) {
                        trackInfo.artist = '';
                    }

                    var query = String.format("{0} {1}", trackInfo.track, trackInfo.artist);
                    searchQuery(query, searchQueryResult);
                } else {
                    $(defaults.resultId).addClass('hidden');
                    $(defaults.formId).removeClass('hidden');
                    $(defaults.waitingId).addClass('hidden');
                }
            });
        }
    });
}