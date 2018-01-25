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

function searchQuery(query, searchResult) {
    var fullJsonUrl = String.format("{0}{1}?q={2}", defaults.url, defaults.searchJsonPath, query);

    var url = String.format("{0}{1}?q={2}", defaults.url, defaults.searchPath, query);

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
        searchQuery(encodeURIComponent($(defaults.searchBoxClass).val()),searchQueryResult);
    });

    $(defaults.query).keypress(function (event) {
        if (event.which == defaults.events.ENTER) {
            searchQuery(encodeURIComponent($(defaults.searchBoxClass).val()), searchQueryResult);
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

                            try{
                                saveTrackToStorage(trackInfo, function() {
                                    getTrackFromStorageAndShowHtml();
                                });
                            } catch (err){
                             var query = String.format("{0} {1}", trackInfo.track, trackInfo.artist);
                             $('#q').val(query);
                             searchQuery(encodeURIComponent(query), searchQueryResult);
                            }
                            
                        } else {
                            $(defaults.resultId).addClass('hidden');
                            $(defaults.formId).removeClass('hidden');
                            $(defaults.waitingId).addClass('hidden');
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
            htmlFoundHistory += "<ul class='history-ul'>";
            for (i = 0; i < responseGet.foundTracks.length; i++) { 
                var query = String.format("{0} {1}", responseGet.foundTracks[i].track, responseGet.foundTracks[i].artist == undefined ? "" : responseGet.foundTracks[i].artist);
                htmlFoundHistory += 
                "<li>"
                + String.format("<button class=\"searchButton btn btn-warning\" searchValue=\"{0}\">add</button>",  encodeURIComponent(query))
                + query
                + "</li>";
            }
            htmlFoundHistory += "</ul>";

            document.getElementById('foundHistory').innerHTML = htmlFoundHistory;
            $('.found-history').show();
            
            $(".searchButton").click(function () {
                searchQuery($(this).attr("searchValue"));
            });

        } else {
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
            for(var i = 0; i < tempfoundTracks.length; i++) {
                if (tempfoundTracks[i].track == foundTrack.track) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                tempfoundTracks.unshift(foundTrack);
            }
            
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