
attr = DS.attr;
App.Blogg = DS.Model.extend({
    title: attr('string'),
    body: attr('string'),
    posted_at: attr('date'),
    author: attr('string')
});