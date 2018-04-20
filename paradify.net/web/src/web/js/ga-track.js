var gaTrack = {

    addToPlaylistUrl: function () {
        //if (ga != undefined) {
        //    ga('set', 'page', '/addToPlaylist' + location.search);
        //    ga('send', 'pageview');
        //}
    },

    signout: function (userNane) {
        //if (ga != undefined) {
        //    ga('set', 'page', '/signout/' + userNane);  
        //    ga('send', 'pageview');
        //}
    },

    chromeStoreClick: function () {
        //if (ga != undefined) {
        //    ga('set', 'page', '/chromeStore');
        //    ga('send', 'pageview');
        //}
    }
}

var gaEvent = {
    track: {
        
        searched: function (search) {
            //ga('send', 'event', 'Track', 'Searched', decodeURIComponent(search));
        },

        notFound: function (search) {
           // ga('send', 'event', 'Track', 'NotFound', decodeURIComponent(search));
        },

        play: function (trackName) {
            //ga('send', 'event', 'Track', 'Play', decodeURIComponent(trackName));
        },

        addToPlaylist: function (trackName) {
            //ga('send', 'event', 'Track', 'AddToPlaylist', decodeURIComponent(trackName));
        },

        selectedToAddPlaylist: function (trackName) {
            //ga('send', 'event', 'Track', 'SelectedToAddPlaylist', decodeURIComponent(trackName));
        },

        selectedToAddPlaylistForRecommendation: function (trackName) {
            //ga('send', 'event', 'Track', 'SelectedToAddPlaylistForRecommendation', decodeURIComponent(trackName));
        },
        selectedToAddPlaylistForRecentlyPlayedTracksClick: function (trackName) {
            //ga('send', 'event', 'Track', 'SelectedToAddPlaylistForRecentlyPlayedTracksClick', decodeURIComponent(trackName));
        },
        selectedSaveClick: function (trackName) {
            //ga('send', 'event', 'Track', 'SelectedSaveClick', decodeURIComponent(trackName));
        },
        recommend: function (search) {
           // ga('send', 'event', 'Track', 'Recommend', decodeURIComponent(search));
        },
        countryChange: function (code) {
           // ga('send', 'event', 'Track', 'NewReleaseCountryChange', code);
        },
        canceledForAddPlaylist: function (trackName) {
            //ga('send', 'event', 'Track', 'canceledForAddPlaylist', decodeURIComponent(trackName));
        }
    }
}