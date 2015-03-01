var porNumber = process.env.PORT;
var key = require('./configs/key');
var config = require('./config');
var express = require('express');
var bodyParser = require('body-parser');
var session = require('express-session');
var paradifyService = require('./paradify-service');
var MongoStore = require('connect-mongo')(session);
var app = express();

app.use(session({
    cookie: {maxAge: 3600 * 1000, saveUninitialized: false, reSave: false},
    secret: "1234234656",
    store: new MongoStore(key.dbStore)
}));

app.use(express.static(__dirname + '/src'));
app.use("/src", express.static(__dirname + '/src'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended: true}));

var swig = require('swig');

app.get('/', function (req, res, next) {
    swig.renderFile('src/web/html/index.html', {
        url: config.virtualUrl
    }, function (err, output) {
        res.send(output);
    });
});

app.get('/search', function (req, res) {
    var q = req.query.q;
    setQuery(req, q);
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

function search(req, res, q, accessToken, again) {

    var service = new paradifyService(accessToken);
    service.search(q, function (data, err) {
        if (err != undefined) {
            showSearchResult(res, {}, err, q);
        }
        else {
            showSearchResult(res, data, undefined, q);
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

function showSearchResult(res, model, err, q) {
    swig.renderFile('src/web/html/searchresult.html', {
        dataMe: model.dataMe,
        dataTracks: model.dataTracks,
        dataPlaylist: model.dataPlaylist,
        err: err,
        q: q,
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
porNumber  = porNumber || 80;

console.log('Listening on ' +porNumber );
app.listen(porNumber);