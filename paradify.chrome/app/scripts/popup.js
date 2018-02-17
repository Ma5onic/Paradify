chrome.runtime.sendMessage({type: 'clearBadge'});

function searchQueryResult(htmlResult, query) {
    $(defaults.waitingId).addClass('hidden');
    $(defaults.resultId).removeClass('hidden');
    $(defaults.resultId).html(htmlResult);
    if (htmlResult.indexOf('Oh snap') == -1) {
        initPlayback();
        initTracksLink(query);
    }
    $(defaults.formId).removeClass('hidden');
    initDuration();
}

function searchQuery(query, pageName) {
    if (pageName == null || pageName == undefined) {
        pageName = '';
    }

    var url = String.format("{0}{1}?q={2}&p={3}", defaults.url, defaults.searchPath, query, pageName);

    setTimeout(function () {
        chrome.tabs.create({url: url});
    }, 100);
}

var initTracksLink = function (query) {
    $(".trackId[trackId]").each(function () {
        var trackId = $(this).attr("trackId");
        if (trackId != undefined && trackId != '') {
            $(this).click(function () {
                var url = String.format("{0}{1}?q={2}&t={3}", defaults.url, defaults.searchPath, encodeURIComponent(query), trackId);
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
        searchQuery(encodeURIComponent($(defaults.searchBoxClass).val()));
    });

    $(defaults.query).keypress(function (event) {
        if (event.which == defaults.events.ENTER) {
            searchQuery(encodeURIComponent($(defaults.searchBoxClass).val()));
        }
    });

    initButtonsLink();
});

var initQuery = function () {
    chrome.tabs.query({active: true, currentWindow: true}, function (tabs) {

        var url = tabs[0].url.toLowerCase();
                var pageName = getPageName(url);
                if (pageName != undefined) {
                    chrome.tabs.sendMessage(tabs[0].id, {type: 'getTrackInfo', pageName: pageName}, function (trackInfo) {
        
                        if (trackInfo != undefined && trackInfo.success && trackInfo.track != '') {

                            if (trackInfo.artist == undefined) {
                                trackInfo.artist = '';
                            }

                            try{
                                saveTrackToStorage(trackInfo, function() {
                                    getTrackFromStorageAndShowHtml();
                                });
                            } catch (err){
                             var query = String.format("{0} {1}", trackInfo.track, trackInfo.artist);
                             $('#q').val(query);
                             searchQuery(encodeURIComponent(query), searchQueryResult, pageName);
                            }
                            
                        } else {
                            $(defaults.resultId).addClass('hidden');
                            $(defaults.formId).removeClass('hidden');
                            $(defaults.waitingId).addClass('hidden');
                            getTrackFromStorageAndShowHtml();
                        }
                        
                    });
                } else {
                    getTrackFromStorageAndShowHtml();
                }

        
    });
}
function getTrackFromStorageAndShowHtml() {
    chrome.storage.sync.get({
        foundTracks: 'foundTracks'
        }, function(responseGet) {

        if (responseGet.foundTracks != 'foundTracks') {

            var htmlFoundHistory = '';
            if (responseGet.foundTracks.length > 0) {
                htmlFoundHistory += "<ul class='history-ul'>";

                for (i = 0; i < responseGet.foundTracks.length; i++) { 
                    if (responseGet.foundTracks[i].track == undefined || responseGet.foundTracks[i].track == '')
                        continue;

                    var query = String.format("{0} {1}", 
                    responseGet.foundTracks[i].track, 
                    responseGet.foundTracks[i].artist == undefined ? "" : responseGet.foundTracks[i].artist
                    );

                    var queryForPopup = (query.length > 33 ? query.substring(0, 33) + '...' : query) + 
                    (responseGet.foundTracks[i].pageName == undefined ? "" : " - " + responseGet.foundTracks[i].pageName);

                    htmlFoundHistory += 
                    "<li>"
                    + String.format("<button class=\"searchButton btn btn-success\" searchValue=\"{0}\" pageName=\"{1}\"> + </button>",  encodeURIComponent(query), responseGet.foundTracks[i].pageName)
                    + queryForPopup
                    + "</li>";
                }
                htmlFoundHistory += "</ul>";

                document.getElementById('foundHistory').innerHTML = htmlFoundHistory;

                $("#foundHistory").show();
                $("#notFoundHistory").hide();

                $(".searchButton").click(function () {
                    searchQuery($(this).attr("searchValue"), $(this).attr("pageName"));
                });

            } else {
                $("#foundHistory").hide();
                $("#search-history-head").hide();
                $("#notFoundHistory").show();
            }

            

        } else {  $("#foundHistory").hide();
        $("#search-history-head").hide();
        $("#notFoundHistory").show();
        }
    });
}

function saveTrackToStorage(foundTrack, callback) {
    var tempfoundTracks;
    
    chrome.storage.sync.get({
        foundTracks: 'foundTracks'
        }, function(responseGet) {
            if (responseGet.foundTracks == 'foundTracks') {
                tempfoundTracks = [];
            } else {
                tempfoundTracks = responseGet.foundTracks;
            }
            
            var found = false;
            var foundIndex = 0;
            for(var i = 0; i < tempfoundTracks.length; i++) {
                if (tempfoundTracks[i].track == foundTrack.track) {// && tempfoundTracks[i].artist == foundTrack.artist
                    found = true;
                    foundIndex = i;
                    break;
                }
            }

            if (found) {
                if (foundIndex !== -1) {
                    tempfoundTracks.splice(foundIndex, 1);
                }
            }

            tempfoundTracks.unshift(foundTrack);

            if (tempfoundTracks.length > 50) {
                tempfoundTracks = tempfoundTracks.splice(0, 50);
            }

            chrome.storage.sync.set({
                foundTracks: tempfoundTracks
                }, function(responseSet) {
                    callback();
            });
    });
}

function initButtonsLink() {
    $('.supported-links .btn').click(function () {
        var url = $(this).attr('url');
        chrome.tabs.create({url: url});        
    });

    
    $('.paypal a').click(function () {
        chrome.tabs.create({url: 'https://www.paypal.com/paypalme/volkanakinpasa/5'});        
    });
}