$(document).ready(function () {
    populate();

    installButton.installNow();

    $('#navigation > li > a').click(function (e) {
        var nameWithHash = $(this).attr('href');
        var name = nameWithHash.replace('#', '');
        showPage(name);
        navigateActiveMark(nameWithHash);
    });

});

function populate() {
    if (window.location.hash != '') {
        var name = window.location.hash.replace('#', '');
        showPage(name);
    } else {
        showPage('home')
    }
    navigateActiveMark(window.location.hash);
}

function navigateActiveMark(nameWithHash) {
    $('#navigation > li > a[href="' + nameWithHash + '"]').parent().addClass('active').siblings().removeClass('active');
}

var installButton = {
    fadeOut: function () {
        $(defaults.installButton).fadeTo("slow", 0.5, function () {
        });
    },
    fadeIn: function () {
        $(defaults.installButton).fadeTo("slow", 1, function () {
        });
    },
    installing: function () {
        $(defaults.installButton).html("installing");
    },
    installed: function () {
        $(defaults.installButton).html("thank you!");
    },
    installNow: function () {
        $(defaults.installButton).html("Install extension");
    }
}

var defaults = {
    installButton: '#install-button',
    mainPageClass: '.main-page',
    chromeExtensionUrl: 'https://chrome.google.com/webstore/detail/bocdilfmhiggklhdifohjfghbdncgele'
}

function install() {
    installButton.fadeOut();
    installButton.installing();
    chrome.webstore.install(defaults.chromeExtensionUrl, function () {
        installButton.installed();
        installButton.fadeIn();
    }, function (err) {
        installButton.fadeIn();
        installButton.installNow();
    });
}

function showPage(className) {
    $(defaults.mainPageClass).hide();
    $('.' + className).show();
}