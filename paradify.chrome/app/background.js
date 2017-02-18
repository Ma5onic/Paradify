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
        case 'clearBadget':
            clearBadget();
            break;
    }
});

function setBadgeText(text) {
    chrome.browserAction.setBadgeText ( { text: text } );
    chrome.browserAction.setBadgeBackgroundColor({ color: "#FF0000" });
}


function clearBadget() {
    chrome.browserAction.setBadgeText ( { text: '' } );
}

chrome.webNavigation.onCompleted.addListener(function(details) {
    chrome.tabs.query({active: true, currentWindow: true}, function (tabs) {
        chrome.tabs.sendMessage(tabs[0].id, {type: 'backgroundStarts', details: details}, function (response) {

        });
    });
});