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
    var i = 0;
    chrome.browserAction.setBadgeText ( { text: text } );
    var first = setTimeout(function () {
        if (i <= 5) {
            chrome.browserAction.setBadgeBackgroundColor({ color: "#FF0000" });

            var second = setTimeout(function () {
                chrome.browserAction.setBadgeBackgroundColor({ color: "#0000ff" });
            },500);

            clearInterval(second);

        }
        clearInterval(first);
    }, 1000);

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