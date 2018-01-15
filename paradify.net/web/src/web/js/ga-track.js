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

var gaEvent = {
    track: {
        
        searched: function (search) {
            ga('send', 'event', 'Track', 'Searched', decodeURIComponent(search));
        },

        notFound: function (search) {
            ga('send', 'event', 'Track', 'NotFound', decodeURIComponent(search));
        },

        play: function (trackName) {
            ga('send', 'event', 'Track', 'Play', decodeURIComponent(trackName));
        },

        addToPlaylist: function (trackName) {
            ga('send', 'event', 'Track', 'AddToPlaylist', decodeURIComponent(trackName));
        },

        
      
    }
}