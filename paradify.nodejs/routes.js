var swig = require('swig');
var config = require('./config');
var paradifyService = require('./paradify-service');

module.exports = function(app){
    app.get('/', function (req, res, next) {
        swig.renderFile('src/web/html/index.html', {
            url: config.virtualUrl
        }, function (err, output) {
            res.send(output);
        });
    });
    app.get('/searchJson', function (req, res) {
        var q = req.query.q;

        if (q == null || q == undefined || q == "") {
            return res.json({});
        }

        var service = new paradifyService();
        service.search(q, function (data, err) {
            var re =  data;
            swig.renderFile('src/web/html/searchresultChrome.html', {
                dataTracks: data.dataTracks,
                err: err,
                q: q,
                url: config.virtualUrl
            }, function (err, output) {
                res.send(output);
            });
        });
    });
    app.get('/search', function (req, res) {
        var q = req.query.q;
        var t = req.query.t;
        setQuery(req, q);
        setTrackId(req, t);
        setReturnUrl(req);

        var token = getToken(req);
        if (q == null || q == undefined || q == "") {
            res.redirect("/");
            return;
        }
        search(req, res, q, token.accessToken);
    });
    app.post('/playlist', function (req, res) {
        addToPlaylist(req, res);
    });
    app.get('/authorize', function (req, res, next) {
        var service = new paradifyService();
        var authorizeURL = service.getAuthorizeUrl();
        res.redirect(authorizeURL);
    });
    app.get('/callback', function (req, res) {
        var returnUrl = req.session.returnUrl;
        var code = req.query.code || null;

        var service = new paradifyService();
        service.callBack(code, function (data, err) {
            if (err != undefined) {
                res.redirect("/");
            }

            setTokens(data, req, res);
            setRefreshToken(data, req, res);

            addUser(req);

            if (returnUrl == undefined) {
                res.redirect("/");
            }

            res.redirect(returnUrl);
        });
    });


    function addToPlaylist(req, res, calledTwice) {
        var token = getToken(req);
        var service = new paradifyService(token.accessToken);
        service.addToPlaylist(req.body.playlistId, req.body.track, function (data, err) {
            if (err != undefined) {
                if (err.message == "No token provided" || err.message == "The access token expired") {
                    refreshToken(req, res, function (dataToken) {
                        if (calledTwice == undefined) {
                            addToPlaylist(req, res, true);
                        }
                        res.json('Error', {errorMessage: err.message});
                    });
                } else {
                    res.json('Error', {errorMessage: err.message});
                }
            } else {
                res.json(data);
            }

        });
    }
    function search(req, res, q, accessToken, again) {

        var service = new paradifyService(accessToken);
        service.search(q, function (data, err) {
            if (err != undefined) {
                showSearchResult(req,res, {}, err, q);
            }
            else {
                showSearchResult(req,res, data, undefined, q);
            }

        });


    }
    function setTokens(data, req, res) {
        req.session.accessToken = data.access_token;
    }
    function setRefreshToken(data, req, res) {
        req.session.refreshToken = data.refresh_token;
    }
    function getToken(req) {
        return {accessToken: req.session.accessToken, refreshToken: req.session.refreshToken};
    }
    function getUrl(req) {
        return req.protocol + '://' + req.get('host') + req.originalUrl;
    }
    function showSearchResult(req, res, model, err, q) {
        swig.renderFile('src/web/html/searchresult.html', {
            dataMe: model.dataMe,
            dataTracks: model.dataTracks,
            dataPlaylist: model.dataPlaylist,
            err: err,
            q: q,
            t:req.session.t,
            url: config.virtualUrl,
            newCreatedPlaylist: model.newCreatedPlaylist
        }, function (err, output) {
            res.send(output);
        });
    }
    function setReturnUrl(req) {
        req.session.returnUrl = getUrl(req);
    }
    function setQuery(req, query) {
        req.session.q = query;
    }
    function setTrackId(req, trackId) {
        req.session.t = trackId;
    }
    function refreshToken(req, res, callback) {
        var token = getToken(req);
        var service = new paradifyService(token.accessToken);
        service.refreshToken(token.refreshToken, function (data, err) {
            setTokens(data, req, res);
            callback(data, err);
        })
    }
    function addUser(req) {
        var token = getToken(req);
        service = new paradifyService(token.accessToken);
        service.addUser();
    }
};