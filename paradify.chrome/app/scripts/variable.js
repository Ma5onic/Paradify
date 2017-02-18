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

String.format = function () {
    var s = arguments[0];

    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}


function getPageName(url) {
    var pageName;
    if (url.indexOf('radioparadise') > -1) {
        pageName = 'radioparadise';
    } else if (url.indexOf('powerapp') > -1) {
        pageName = 'powerapp';
    } else if (url.indexOf('youtube.com') > -1) {
        pageName = 'youtube';
    } else if (url.indexOf('karnaval.com') > -1) {
        pageName = 'karnaval';
    } else if (url.indexOf('soundcloud.com') > -1) {
        pageName = 'soundcloud';
    } else if (url.indexOf('vimeo.com') > -1) {
        pageName = 'vimeo';
    } else if (url.indexOf('dailymotion.com') > -1) {
        pageName = 'dailymotion';
    } else if (url.indexOf('kralmuzik.com.tr') > -1) {
        pageName = 'kralmuzik';
    } else if (url.indexOf('tunein.com') > -1) {
        pageName = 'tunein';
    }
    return pageName;
}




function readNowPlayingText(pageName) {
    if (pageName == 'radioparadise' && window.getSelection().toString().trim() != "") {
        var result = {track: window.getSelection().toString().trim(), artist: ""};
        return result;
    }
    else if (pageName == 'radioparadise') {
        return readRadioParadise();
    }
    else if (pageName == 'powerapp') {
        return readPowerfm();
    }
    else if (pageName == 'youtube') {
        return readYoutube();
    }
    else if (pageName == 'karnaval') {
        return readKarnaval();
    }
    else if (pageName == 'soundcloud') {
        return readsoundCloud();
    } else if (pageName == 'vimeo') {
        return readVimeo();
    } else if (pageName == 'dailymotion') {
        return readDailyMotion();
    } else if (pageName == 'kralmuzik') {
        return readKralmuzik();
    } else if (pageName == 'tunein') {
        return readTunein();
    } else {
        return null;
    }
}

function readRadioParadise() {
    var iframe = document.getElementById('content');
    if (iframe != undefined) {
        var innerDoc = (iframe.contentDocument) ? iframe.contentDocument : iframe.contentWindow.document;
        var c = innerDoc.getElementsByClassName("song_title");
        var data = c[3].innerText;
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
    var track = document.getElementsByClassName('playbackSoundBadge__title sc-truncate')[0].getAttribute("title");
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