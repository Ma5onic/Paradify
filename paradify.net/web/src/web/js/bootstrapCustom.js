var variable = {
    playlist: null,
    recentlyPlayedTracks: null,
    savedTracks: null
};

function select(trackId, trackName, artistId, artistName, fromSonglistClick,
    fromRecommendationListClick, fromRecentlyPlayedTracksClick, fromSavedClick) {

    $("#input_trackId").val(trackId);
    $("#input_trackName").val(trackName);
    $("#span-selected-song").html(trackName);

    openPlaylistPopup(trackName);

    if (fromSonglistClick) {
        gaEvent.track.selectedToAddPlaylist(trackName);

        $("#input_from").val('fromSonglistClick');

    }

    if (fromRecommendationListClick) {
        gaEvent.track.selectedToAddPlaylistForRecommendation(trackName);
        $("#input_from").val('fromRecommendationListClick');
    }


    if (fromRecentlyPlayedTracksClick) {
        gaEvent.track.selectedToAddPlaylistForRecentlyPlayedTracksClick(trackName);

        $("#input_from").val('fromRecentlyPlayedTracksClick');
       
    }


    if (fromSavedClick) {
        gaEvent.track.selectedSaveClick(trackName);
    }

    if (fromSonglistClick || fromRecentlyPlayedTracksClick || fromSonglistClick || fromSavedClick) {
        loadRecommendedSongs(trackId, artistId, function (response) {
            $('.custom-recommendedSongs').html(response);
            $('.custom-recommendedSongs').show();
            $('.custom-title-recommendedSongs').html('Recommended based on ' + trackName);
            $('.custom-title-recommendedSongs').show();
        });
    }
    
}

function recommend(trackId, trackName, artistId) {

    loadRecommendedSongs(trackId, artistId, function (response) {
        $('.custom-recommendedSongs').html(response);
        $('.custom-recommendedSongs').show();
        $('.custom-title-recommendedSongs').html('Recommended based on ' + trackName);
        $('.custom-title-recommendedSongs').show();
    });

    gaEvent.track.recommend(trackName);
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
        url: "/Search/Async/Playlists",
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

var openPlaylistPopupCount = 0;

function openPlaylistPopup(trackName) {
   
    if (openPlaylistPopupCount >= 5) {
        openPlaylistPopupCount = 0;
        return;
    }
    

    if (variable.playlist == null) {
        setTimeout(function () {
            openPlaylistPopupCount++;
            console.log(openPlaylistPopupCount);
            openPlaylistPopup(trackName);
        }, 500);
    }

    $('.custom-modal-body-p').html(variable.playlist);
    $('.modal-title').html(trackName);
    $('.modal').modal();
}

function loadPlaylist() {
    $.ajax({
        type: "GET",
        url: "/Search/Async/Playlists",

        success: function (response) {
            if (response != null && response != '') {
                variable.playlist = response;
            }
        },
        error: function (xhr, textStatus, err) { }
    });
}

function loadSavedTracks(callback) {
    $.ajax({
        type: "GET",
        url: "/Search/Async/SavedTracks",

        success: function (response) {
            if (response != null && response != '') {
                variable.savedTracks = response;
                callback();
            }
        },
        error: function (xhr, textStatus, err) { }
    });
}


function loadRecentlyPlayedTracksShort(callback) {
    $.ajax({
        type: "GET",
        url: "/Search/Async/RecentlyPlayedTracksShort",

        success: function (response) {
            if (response != null && response != '') {
                variable.recentlyPlayedTracks = response;
                callback();
            }
        },
        error: function (xhr, textStatus, err) { }
    });
}

function loadRecentlyPlayedTracks(callback) {
    $.ajax({
        type: "GET",
        url: "/Search/Async/RecentlyPlayedTracks",

        success: function (response) {
            if (response != null && response != '') {
                variable.recentlyPlayedTracks = response;
                callback();
            }
        },
        error: function (xhr, textStatus, err) { }
    });
}

function loadRecommendedSongs(trackId, artistId, callback) {
    $.ajax({
        type: "GET",
        url: "/Search/Async/Recommendations",
        data: {
            "trackId": trackId,
            "artistId": artistId

        },
        success: function (response) {
            if (response != null && response != '') {
                callback(response);
               
                

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

 


function loadSearch() {
    $(document).ready(function () {
        console.log('document ready');

        loadPlaylist();

        loadRecentlyPlayedTracksShort(function () {
            $('.custom-title-recentlyPlayedTracks').show();
            $('.custom-recentlyPlayedTracks').show();
            $('.custom-recentlyPlayedTracks').html(variable.recentlyPlayedTracks);
        });

        loadSavedTracks(function () {
            $('.custom-title-savedTracks').show();
            $('.custom-savedTracks').show();
            $('.custom-savedTracks').html(variable.savedTracks);
        });

    });
}

function loadHome() {
    
    $(document).ready(function () {
       
        loadPlaylist();

        loadRecentlyPlayedTracks(function () {
            $('.custom-title-recentlyPlayedTracks').show();
            $('.custom-recentlyPlayedTracks').show();
            $('.custom-recentlyPlayedTracks').html(variable.recentlyPlayedTracks);
            initPlayback();
        });

        loadSavedTracks(function () {
            $('.custom-title-savedTracks').show();
            $('.custom-savedTracks').show();
            $('.custom-savedTracks').html(variable.savedTracks);
        });

    });
}