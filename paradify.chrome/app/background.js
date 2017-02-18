if (!localStorage.getItem('install_time')) {

    var now = new Date().getTime();
    localStorage.setItem('install_time', now);
    chrome.tabs.create({url: "http://www.paradify.com/welcome.html"});

}

chrome.runtime.onMessage.addListener(function (message, sender, sendResponse){

    switch(message.type) {
        case 'setBadgeText':
            setBadgeText(message.text);
            break;
        case 'clearBadge':
            clearBadge();
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


function clearBadge() {
    chrome.browserAction.setBadgeText ( { text: '' } );
}

chrome.webNavigation.onCompleted.addListener(function(details) {
    chrome.tabs.query({active: true, currentWindow: true}, function (tabs) {
        chrome.tabs.sendMessage(tabs[0].id, {type: 'backgroundStarts', details: details}, function (response) {

        });
    });
});