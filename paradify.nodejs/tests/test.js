var should = require('chai').should();
var supertest = require('supertest');
var api = supertest('http://localhost:5000');
var routes = require("../routes.js")


describe('paradify-service-test', function () {
    it('errors if wrong basic auth', function(done) {
        api.get('/')
            .expect(401, done)

        routes
    });

    //describe('mode sanitize', function () {
    //    var routeModule = rewire("../routes.js");
    //
    //    it('dddddddd', function (done) {
    //        testModule.__set__("SpotifyWebApi", {
    //            getMe: function () {
    //
    //            }
    //        });
    //        testModule.__set__("SpotifyWebApi", {hfe: 'dfsfd'});
    //        testModule.getMe();
    //
    //    })
    //
    //});


});