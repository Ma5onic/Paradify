var SpotifyWebApi = require("./src/spotify-web-api");
db = require('./model/db');
var config = require('./config');

var scopes = ['playlist-read-private', 'playlist-modify-public', 'playlist-modify-private', 'user-library-modify', 'user-read-private'];
var client_id = 'aba4782305d0480f9dbe2b63a7a77b42';
var client_secret = '4f2585881e99474f92b0e67ea69b22d0';
var redirect_uri = config.virtualUrl + '/callback';
var stateKey = 'benimstatesanane';
function paradifyService(token) {
    var token = token;

    var call_back_function = function (model) {
        model.callback(model.data, model.err);
    };

    var getMe = function (callback) {
        spotifyApi.getMe().then(function (user) {
            call_back_function({callback: callback, data: user})
        }, function (err) {
            call_back_function({callback: callback, err: err})
        });
    };

    var addTracksToPlaylist = function (userId, playlistId, track, callback) {
        spotifyApi.addTracksToPlaylist(userId, playlistId, track).then(function (dataResult) {
            call_back_function({callback: callback, data: dataResult})
        }, function (err) {
            call_back_function({callback: callback, err: err})
        });
    };

    var getUserPlaylists = function (userId, option, callback) {
        spotifyApi.getUserPlaylists(userId, option).then(function (dataPlaylist) {
            call_back_function({callback: callback, data: dataPlaylist});
        }, function (err) {
            call_back_function({callback: callback, err: err});
        });
    };

    var createPlaylist = function (dataMe, callback) {
        spotifyApi.setAccessToken(token);
        spotifyApi.createPlaylist(dataMe.id, "I love Paradify :)").then(function (result) {
            call_back_function({callback: callback, data: result});
        }, function (err) {
            call_back_function({callback: callback, err: err});
        });
    };

    var spotifyApi = new SpotifyWebApi({
        clientId: client_id,
        clientSecret: client_secret,
        redirectUri: redirect_uri
    });

    this.search = function (q, callback) {
        spotifyApi.searchTracksArtists(q).then(function (dataTracks) {
            //return is token not exist
            if (token == undefined) {
                call_back_function({
                    callback: callback,
                    data: {dataTracks: dataTracks}
                });
                return;
            }

            spotifyApi.setAccessToken(token);

            spotifyApi.getMe().then(function (dataMe) {
                var option = {limit: 50};
                getUserPlaylists(dataMe.id, option, function (dataPlaylist, errPlaylist) {
                    if (errPlaylist != undefined) {
                        call_back_function({callback: callback, err: errPlaylist});
                        return;
                    }

                    if (dataPlaylist.items.length > 0) {
                        call_back_function({
                            callback: callback,
                            data: {dataMe: dataMe, dataTracks: dataTracks, dataPlaylist: dataPlaylist}
                        });
                    } else {
                        createPlaylist(dataMe, function (dataCreate, errorCreate) {
                            if (errorCreate != undefined) {
                                call_back_function({
                                    callback: callback,
                                    err: "You have not playlist and we cannot add a new temp playlist to your account"
                                });
                                return;
                            }
                            //again get
                            getUserPlaylists(dataMe.id, option, function (dataPlaylistAgain, errPlaylistAgain) {
                                if (dataPlaylistAgain.items.length > 0) {
                                    call_back_function({
                                        callback: callback,
                                        data: {
                                            dataMe: dataMe,
                                            dataTracks: dataTracks,
                                            dataPlaylist: dataPlaylistAgain,
                                            newCreatedPlaylist: true
                                        }
                                    });
                                } else {
                                    call_back_function({
                                        callback: callback,
                                        err: "You have not playlist and we cannot add a new temp playlist to your account"
                                    });
                                }
                            });

                        });
                    }

                });
            }, function (err) {
                call_back_function({callback: callback, err: err});
            });

        }, function (err) {
            call_back_function({callback: callback, err: err});
        });
    };


    this.getAuthorizeUrl = function () {
        var authorizeURL = spotifyApi.createAuthorizeURL(scopes, stateKey);
        return authorizeURL;
    };

    this.addToPlaylist = function (playlistId, track, callback) {
        spotifyApi.setAccessToken(token);
        var userId;
        getMe(function (data, err) {

            if (err != undefined) {
                call_back_function({callback: callback, err: err});
            }
            userId = data.id;

            addTracksToPlaylist(userId, playlistId, track, function (data, err) {
                if (err != undefined) {
                    call_back_function({callback: callback, err: err});
                }
                call_back_function({callback: callback, data: data});
            });
        });
    };


    this.callBack = function (code, callback) {
        spotifyApi.authorizationCodeGrant(code).then(function (data) {
            call_back_function({callback: callback, data: data});
        }, function (err) {
            call_back_function({callback: callback, err: err});
        });
    };

    this.refreshToken = function (refresh_token, callback) {
        spotifyApi.setRefreshToken(refresh_token);
        spotifyApi.refreshAccessToken().then(function (data) {
            call_back_function({callback: callback, data: data});
        }, function (err) {
            call_back_function({callback: callback, err: err});
        });
    }

    this.addUser = function () {


        spotifyApi.setAccessToken(token);

        spotifyApi.getMe().then(function (me) {
            var userModel = {
                id: me.id,
                display_name: me.display_name,
                product: me.product,
                email: me.email
            }
            db.connect();
            User = db.mongoose.model('User');
            User.findOneAndUpdate({id: userModel.id}, userModel, {upsert: true}, function (err, doc) {
            });
            db.disconnect();
        }, function (err) {
        });
    };

    this.getMe = function(){
        spotifyApi.getMe().then(function (me) {
            var result = me;
        }, function (err) {
        });
    }
};
module.exports = paradifyService;