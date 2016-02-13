chrome.runtime.onMessage.addListener(function (message, sender, sendResponse){
    var response = {action: "getResult"};
    if (message != null && message.action == "get") {
        try {
            var playingResult = readNowPlayingText(message);

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
        } finally {
        }
    } else {
        response.success = false;
        response.errMessage = "Request is null";
    }
    sendResponse(response);

});

function readNowPlayingText(message) {
    if (message.pageName == 'radioparadise' && window.getSelection().toString().trim() != "") {
        var result = {track: window.getSelection().toString().trim(), artist: ""};
        return result;
    }
    else if (message.pageName == 'radioparadise') {
        return readRadioParadise();
    }
    else if (message.pageName == 'powerapp') {
        return readPowerfm();
    }
    else if (message.pageName == 'youtube') {
        return readYoutube();
    }
    else if (message.pageName == 'karnaval') {
        return readKarnaval();
    }
    else if (message.pageName == 'soundcloud') {
        return readsoundCloud();
    } else if (message.pageName == 'vimeo') {
        return readVimeo();
    } else if (message.pageName == 'dailymotion') {
        return readDailyMotion();
    } else if (message.pageName == 'kralmuzik') {
        return readKralmuzik();
    } else if (message.pageName == 'tunein') {
        return readTunein();
    } else {
        return null;
    }


}

function readRadioParadise() {
    var iframe = document.getElementById('content');
    if (iframe != undefined) {
        var innerDoc = (iframe.contentDocument) ? iframe.contentDocument : iframe.contentWindow.document;
        var data = innerDoc.getElementById("nowplaying_title").innerText;
        var arr = data.split('—');
        var result = {track: arr[0].replace(/^\s*|\s*$/g, ''), artist: arr[1].replace(/^\s*|\s*$/g, '')};
        return result;
    }
    return null;
}

function readPowerfm() {
    var currentSongDiv = document.getElementById('currentSong');
    var track = currentSongDiv.getElementsByTagName('a')[0].innerText;

    var currentSongDiv = document.getElementById('currentArtist');
    var artist = currentSongDiv.getElementsByTagName('a')[0].innerText;

    var result = {track: track, artist: artist};
    return result;
}

function readYoutube() {
    var track = document.getElementById('eow-title').innerHTML.trim();
    var result = {track: track};
    return result;
}

function readKarnaval() {
    var track = document.getElementsByClassName('nowplaying')[0].getElementsByClassName('name')[0].innerHTML;
    var position = track.lastIndexOf(' -');
    track = track.substr(0, position);
    var artist = document.getElementById('current-artist').innerHTML;

    var result = {track: track, artist: artist};
    return result;
}

function readsoundCloud() {
    var track = document.getElementsByClassName('playbackTitle__link')[0].innerHTML.trim();
    var result;
    if (track != '') {
        result = {track: track, artist: ''};
    }
    return result;
}

function readVimeo() {
    var track = document.getElementsByClassName('js-clip_title')[0].innerHTML.trim();
    var result;
    if (track != '') {
        result = {track: track, artist: ''};
    }
    return result;
}
function readDailyMotion() {
    var track = document.getElementById('video_title').innerHTML.trim();
    var result;
    if (track != '') {
        result = {track: track, artist: ''};
    }
    return result;
}

function readKralmuzik() {
    var currentSongDiv = document.getElementsByClassName('live-info')[0];
    var track = currentSongDiv.getElementsByTagName('h2')[0].innerText;

    var artist = currentSongDiv.getElementsByTagName('h1')[0].innerText;

    var result = {track: track, artist: artist};

    return result;
}
function readTunein() {
    var track = document.getElementsByClassName('_navigateNowPlaying')[1].innerHTML;

    var result = {track: track};

    return result;
}