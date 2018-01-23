if (!localStorage.getItem('install_time')) {

    var now = new Date().getTime();
    localStorage.setItem('install_time', now);
    chrome.tabs.create({url: "http://www.spotifydiscovery.com/welcome.html"});

}

chrome.runtime.onMessage.addListener(function (message, sender, sendResponse){

    switch(message.type) {
        case 'setBadgeText':
            setBadgeText(message.text);
            break;
        case 'clearBadge':
            clearBadge();
            break;
        case 'saveTrackToStorage':
            saveTrackToStorage(message.foundTrack);
            break;    
    }
});

function setBadgeText(text) {
    chrome.browserAction.setBadgeText ( { text: 'Hey' } );
    chrome.browserAction.setBadgeBackgroundColor({ color: "#FF55A9" });

    var intervalID = setInterval(function () {
        chrome.browserAction.setBadgeText ( { text: text } );
        window.clearInterval(intervalID);
    }, 2000);
}

function saveTrackToStorage(foundTrack) {
    var tempfoundTracks;

    chrome.storage.sync.get({
        foundTracks: 'foundTracks'
        }, function(responseGet) {
            if (responseGet.foundTracks == 'foundTracks') {
                tempfoundTracks = [];
            } else {
                tempfoundTracks = responseGet.foundTracks;
            }
            
            tempfoundTracks.unshift(foundTrack);
            
            if (tempfoundTracks.length > 50) {
                tempfoundTracks = tempfoundTracks.splice(0, 50);
            }

            chrome.storage.sync.set({
                foundTracks: tempfoundTracks
                }, function(responseSet) {
            });
    });
}

function clearBadge() {
    chrome.browserAction.setBadgeText ( { text: '' } );
}

chrome.webNavigation.onCompleted.addListener(function(details) {
    

    chrome.tabs.query({active: true, currentWindow: true}, function (tabs) {
        if (tabs.length == 0)
        return;

        chrome.tabs.sendMessage(tabs[0].id, {type: 'backgroundStarts', details: details}, function (response) {
            
        });
        
    });
    
});

function contextMenuClicked(details)
{
    chrome.tabs.query({active: true, currentWindow: true}, function (tabs) {
        if (tabs.length == 0)
            return;

        chrome.tabs.sendMessage(tabs[0].id, {type: 'contextMenuClicked', details: details}, function (returnUrl) {
            chrome.tabs.create({url: returnUrl});
        });
    });
}

chrome.contextMenus.create({title: "Add to Spotify Playlist", contexts:["selection"], onclick: contextMenuClicked});