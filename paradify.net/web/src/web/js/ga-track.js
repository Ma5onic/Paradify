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

}

var gaEvent = {
    track: {

        searched: function (search) {
            this.ga('send', 'event', 'Track', 'Searched', decodeURIComponent(search));
        },

        notFound: function (search) {
            this.ga('send', 'event', 'Track', 'NotFound', decodeURIComponent(search));
        },

        play: function (trackName) {
            this.ga('send', 'event', 'Track', 'Play', decodeURIComponent(trackName));
        },

        addToPlaylist: function (trackName) {
            this.ga('send', 'event', 'Track', 'AddToPlaylist', decodeURIComponent(trackName));
        },

        selectedToAddPlaylist: function (trackName) {
            this.ga('send', 'event', 'Track', 'SelectedToAddPlaylist', decodeURIComponent(trackName));
        },

        selectedToAddPlaylistForRecommendation: function (trackName) {
            this.ga('send', 'event', 'Track', 'SelectedToAddPlaylistForRecommendation', decodeURIComponent(trackName));
        },
        selectedToAddPlaylistForRecentlyPlayedTracksClick: function (trackName) {
            this.ga('send', 'event', 'Track', 'SelectedToAddPlaylistForRecentlyPlayedTracksClick', decodeURIComponent(trackName));
        },
        selectedSaveClick: function (trackName) {
            this.ga('send', 'event', 'Track', 'SelectedSaveClick', decodeURIComponent(trackName));
        },
        recommend: function (search) {
            this.ga('send', 'event', 'Track', 'Recommend', decodeURIComponent(search));
        },
        countryChange: function (code) {
            this.ga('send', 'event', 'Track', 'NewReleaseCountryChange', code);
        },
        canceledForAddPlaylist: function (trackName) {
            this.ga('send', 'event', 'Track', 'canceledForAddPlaylist', decodeURIComponent(trackName));
        },

        ga: function (del, del2, eventName, categoryName, label) {
            gtag('event', eventName, {
                'event_category': categoryName,
                'event_label': label
            });
        }
    }
}