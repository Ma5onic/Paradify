var porNumber = process.env.PORT;
var key = require('./configs/key');
var express = require('express');
var bodyParser = require('body-parser');
var session = require('express-session');
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

var routes = require('./routes')(app);

porNumber = porNumber || 80;
console.log('Listening on ' + porNumber);
app.listen(porNumber);