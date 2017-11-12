var gaTrack = {

    addToPlaylistUrl: function () {
        if (ga != undefined) {
            ga('set', 'page', '/addToPlaylist' + location.search);
            ga('send', 'pageview');
        }
    },

    signout: function (userNane) {
        if (ga != undefined) {
            ga('set', 'page', '/signout/' + userNane);
            ga('send', 'pageview');
        }
    },


    paypalClick: function () {
        if (ga != undefined) {
            ga('set', 'page', '/paypal');
                ga('send', 'pageview');
        }
    }
}