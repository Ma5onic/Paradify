var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var UserSchema = new Schema({
    id: {type: String, index: { unique: true } },
    display_name: String,
    product: String,
    email: String
});

var User = module.exports = mongoose.model('User', UserSchema);