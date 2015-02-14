var mongoose = require( 'mongoose' );
var key = require('./../configs/key');



//mongoose.connect(dbURI);

mongoose.connection.on('connected', function () {
    console.log('Mongoose default connection open to ' +  key.dbUrl);
});

mongoose.connection.on('error',function (err) {
    console.log('Mongoose default connection error: ' + err);
});

mongoose.connection.on('disconnected', function () {
    console.log('Mongoose default connection disconnected');
});

process.on('SIGINT', function() {
    mongoose.connection.close(function () {
        console.log('Mongoose default connection disconnected through app termination');
        process.exit(0);
    });
});


var db = {

    connect : function(){
        mongoose.connect(key.dbUrl);
    },
    disconnect: function(){
        mongoose.connection.disconnect();
    },
    mongoose : mongoose
}
require('./../model/user');
module.exports = db ;//{mongoose :mongoose , dbURI : key.dbUrl };