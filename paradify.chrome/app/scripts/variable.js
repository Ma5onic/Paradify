var defaults = {
    url: "http://www.spotifydiscovery.com/Search/",
    searchJsonPath: "searchJson",
    searchPath: "searchp",
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
    else if (url.indexOf('jango.com') > -1) {
        pageName = 'jango';
    }
    else if (url.indexOf('qmusic.nl') > -1) {
        pageName = 'qmusic';
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
    } else if (pageName == 'jango') {
        return readJango();
    } else if (pageName == 'qmusic') {
        return readQMusic();
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
    var currentSongDiv = document.getElementsByClassName('artistSongTitle')[0].innerText;
    var track = currentSongDiv;

    var currentSongDiv = document.getElementsByClassName('artistTitle')[0].innerText;
    var artist = currentSongDiv;

    var result = {track: track, artist: artist};
    return result;
}

function readYoutube() {
    var track = document.title.trim().replace(' - YouTube', '');
    var result = { track: track };
    return result;
}

function readKarnaval() {
    var title = document.getElementsByClassName('title')[0];
    var track = title.firstChild.firstChild.innerText;

    var artist = document.getElementsByClassName('sub_title')[0].firstChild.firstChild.innerText;

    var result = {track: track, artist: artist};

    return result;
}

function readsoundCloud() {
    var track = document.getElementsByClassName('playbackSoundBadge__titleLink sc-truncate')[0].getAttribute("title");
    var result;
    if (track != '') {
        result = {track: track, artist: ''};
    }
    return result;
}

function readVimeo() {
    var track = document.title.trim().replace('on Vimeo', '');
    var result;
    if (track != '') {
        result = {track: track, artist: ''};
    }
    return result;
}
function readDailyMotion() {
    var track = document.title.trim().replace(' - Video Dailymotion', '');
    var result;
    if (track != '') {
        result = { track: track, artist: '' };
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

function readJango() {
    var track = document.getElementById('current-song').innerHTML;
    var artist = document.getElementById('player_current_artist').getElementsByTagName('a')[0].innerText;
    var result = {track: track, artist: artist};
    return result;
}

function readQMusic() {
    var track = document.getElementsByClassName('current-track')[0].getElementsByClassName('title')[0].innerHTML;
    
    var artist = document.getElementsByClassName('current-track')[0].getElementsByClassName('artist')[0].innerHTML;

    var result = {track: track, artist: artist};

    return result;
}
