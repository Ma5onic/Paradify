var key = {
    dbStore: {
        db: process.env.ParadifyDbName,
        host: process.env.ParadifyDbHost,
        port: process.env.ParadifyDbPort,
        username: process.env.ParadifyDbUserName,
        password: process.env.ParadifyDbPassword,
        collection: 'session',
        autoReconnect: true
    }
    , SENDGRID_PASSWORD: ''
    , SENDGRID_USERNAME: ''
};

key.dbUrl = 'mongodb://' + key.dbStore.username + ':' + key.dbStore.password + '@' + key.dbStore.host + ':' + key.dbStore.port + '/' + key.dbStore.db;
module.exports = key;