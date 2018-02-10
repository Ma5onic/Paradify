function select(trackId, trackName) {
    $(".custom-link-playlist").click();
    $("#input_trackId").val(trackId);
    $("#input_trackName").val(trackName);
    $("#span-selected-song").html(trackName);
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
}


function addToPlaylist(playlistId) {
    var trackId = $("#input_trackId").val();
    var trackName = $("#input_trackName").val();

    var dataJson = {};
    dataJson.playlistId = playlistId;
    dataJson.trackId = 'spotify:track:' + trackId;
    $.ajax({
        type: "POST",
        url: "api/playlist",
        data: dataJson,
        success: function (errorResponse) {
            if (errorResponse.error == null) {
                $.notify({
                    // options
                    message: 'Added to your Spotify playlist!'
                }, {
                        // settings
                        type: 'success',
                        offset: 3,
                        position: null,
                        delay: 500,
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
                gaTrack.addToPlaylistUrl();
                gaEvent.track.addToPlaylist(trackName);
            } else {

            }
        },
        error: function (xhr, textStatus, err) {
        }
    });

}
