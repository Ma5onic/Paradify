function select(trackId, trackName, artistId, artistName, fromSonglistClick, fromRecommendationListClick, fromRecentlyPlayedTracksClick) {

    $("#input_trackId").val(trackId);
    $("#input_trackName").val(trackName);
    $("#span-selected-song").html(trackName);

    loadPlaylist(trackName);

    if (fromSonglistClick) {
        gaEvent.track.selectedToAddPlaylist(trackName);
        
        $("#input_from").val('fromSonglistClick');

        loadRecommendedSongs(trackId, trackName, artistId, artistName);
    }

    if (fromRecommendationListClick) {
        gaEvent.track.selectedToAddPlaylistForRecommendation(trackName);
        $("#input_from").val('fromRecommendationListClick');
    }

    
    if (fromRecentlyPlayedTracksClick) {
        gaEvent.track.selectedToAddPlaylistForRecentlyPlayedTracksClickn(trackName);

        $("#input_from").val('fromRecentlyPlayedTracksClick');

        loadRecommendedSongs(trackId, trackName, artistId, artistName);
    }
}


function addToPlaylist(playlistId) {
    var trackId = $("#input_trackId").val();
    var trackName = $("#input_trackName").val();

    var dataJson = {};
    dataJson.playlistId = playlistId;
    dataJson.trackId = 'spotify:track:' + trackId;

    customNotify.notify('Adding...');
    $.ajax({
        type: "POST",
        url: "api/playlist",
        data: dataJson,
        success: function (errorResponse) {
            if (errorResponse.error == null) {
                customNotify.notify('Added to playlist!');
                gaTrack.addToPlaylistUrl();
                gaEvent.track.addToPlaylist(trackName);
            } else {
                customNotify.error('An error occurred while adding to playlist! Please try it again.');
            }
        },
        error: function (xhr, textStatus, err) {
            customNotify.error('An error occurred while adding to playlist! Please try it again.');
        }
    });

    customModal.close();
    animateByClass('custom-title-recommendedSongs');

}

function loadPlaylist(trackName) {
    $.ajax({
        type: "GET",
        url: "Home/GetPlaylists",
        
        success: function (response) {
            if (response != null && response != '') {
                $('.custom-modal-body-p').html(response);
                $('.modal-title').html(trackName);
                $('.modal').modal();
            } else {
                customNotify.error('An error occurred while loading your playlists! Please try it again.');
            }
        },
        error: function (xhr, textStatus, err) {
            customNotify.error('An error occurred while loading your playlists! Please try it again.');
        }
    });
}

function loadRecommendedSongs(trackId, trackName, artistId, artistName) {
    $.ajax({
        type: "GET",
        url: "Recommendation/Index",
        dataJson: { trackId: trackId, artistId: artistId },
        data: {
            "trackId": trackId,
            "artistId": artistId

        },
        success: function (response) {
            if (response != null && response != '') {
                $('.custom-recommendedSongs').html(response);
                $('.custom-recommendedSongs').show();
                $('.custom-title-recommendedSongs').html('Recommended tracks based on \'' + trackName + ' - ' + artistName + '\'');
                $('.custom-title-recommendedSongs').show();
                initPlayback();

            }
        },
        error: function (xhr, textStatus, err) {

        }
    });
}

var customModal = {
    close: function () {
        $('.modal').modal('hide');
    }
}

var customNotify = {
    notify: function (message) {
        $.notify({
            // options
            message: message
        }, {
                // settings
                type: 'success',
                offset: 3,
                position: null,
                delay: 1000,
                placement: {
                    from: "top",
                    align: "center"
                },
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                allow_dismiss: false,
            });
    },

    error: function (message) {
        $.notify({
            // options
            message: message
        }, {
                // settings
                type: 'success',
                offset: 3,
                position: null,
                delay: 1000,
                placement: {
                    from: "top",
                    align: "center"
                },
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                allow_dismiss: false,
            });
    }
}

function animateByClass(className) {
    $("html, body").animate({ scrollTop: $("." + className).offset().top }, 1000);
}

function paypal() {
    gaTrack.paypalClick();
}