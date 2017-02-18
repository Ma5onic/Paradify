chrome.runtime.onMessage.addListener(function (message, sender, sendResponse){
    if (message != undefined ) {
        if (message.type == 'backgroundStarts' && message.details != undefined && message.details.url == window.location.href) {
            paradify.pageLoad();
        } else if (message.type == 'getTrackInfo') {
            var trackInfo = paradify.getTrackInfo(message.pageName);
            sendResponse(trackInfo);
        }
    }
});

var paradify = {
    pageLoad : function() {
        chrome.runtime.sendMessage({type: 'clearBadge'});
        var url = window.location.href.toLowerCase();
        var pageName = getPageName(url);
        if (pageName != undefined) {
            var trackInfo = paradify.getTrackInfo(pageName);
            if (trackInfo != undefined && trackInfo.success) {
                chrome.runtime.sendMessage({type: 'setBadgeText', text: ' 1 '});
            }
        }
    },

    getTrackInfo : function(pageName) {

        var response = {};

        try {
            var playingResult = readNowPlayingText(pageName);

            if (playingResult != null) {
                response.track = playingResult.track;
                response.artist = playingResult.artist;
                response.success = true;
            } else {
                response.success = false;
                response.errMessage = "There is no playing song on this page";
            }
        } catch (err) {
            response.success = false;
            response.errMessage = err.message;
        }

        return response;
    }

}


