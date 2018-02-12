function select(trackId, trackName, artistId, artistName, mustGetRecommendedSongs) {

    $("#input_trackId").val(trackId);
    $("#input_trackName").val(trackName);
    $("#span-selected-song").html(trackName);

    $('.custom-modal-body-p').html($('.custom-playlistList').html());
    $('.modal-title').html(trackName);
    $('.modal').modal();

    $.notify({
        // options
        message: 'The song "' + trackName + '" selected. Select playlist to add!'
    }, {
            // settings
            type: 'info',
            offset: 3,
            position: null,
            delay: 1500,
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
    if (mustGetRecommendedSongs == true)
        loadRecommendedSongs(trackId, trackName, artistId, artistName);
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
                customNotify.notify('An error occurred while adding to playlist! Please try it again.');
            }
        },
        error: function (xhr, textStatus, err) {
            customNotify.notify('An error occurred while adding to playlist! Please try it again.');
        }
    });

    customModal.close();

}


function loadRecommendedSongs(trackId, trackName, artistId, artistName) {
    $.ajax({
        type: "GET",
        url: "Recommendation/Playlist",
        dataJson: { trackId: trackId, artistId: artistId },
        data: {
            "trackId": trackId,
            "artistId": artistId

        },
        success: function (response) {
            if (response != null) {
                $('.custom-recommendedSongs').html(response);
                $('.custom-recommendedSongs').show();
                $('.custom-title-recommendedSongs').html('Recommended based on ' + trackName + ' - ' + artistName);
                $('.custom-title-recommendedSongs').show();
                 

            }
        },
        error: function (xhr, textStatus, err) {
            console.log(err);
        }
    });
}

var customModal = {
    close: function() {
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
    }
}