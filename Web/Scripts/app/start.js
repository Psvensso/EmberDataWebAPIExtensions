App = Ember.Application.create({
    rootElement: '#ember-app'
});

App.NewpostController = Ember.Controller.extend({
    init: function () {
        debugger;
        return this._super();
    }
});
App.IndexRoute = Ember.Route.extend({
    //model: function () {
    //    return this.store.find('blogg');
    //}
});
DS.RESTAdapter.reopen({
    namespace: 'api'
});
