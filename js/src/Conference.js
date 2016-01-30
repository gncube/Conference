/** @jsx React.DOM */
var TimesheetEditor = require('./TimesheetEditor'),
    Comments = require('./Comments'),
    Tags = require('./Tags'),
    Speakers = require('./Speakers'),
    TagVotes = require('./TagVotes'),
    SessionVotes = require('./SessionVotes'),
    Scheduler = require('./Scheduler/Scheduler'),
    Schedule = require('./Schedule/Schedule'),
    Resources = require('./Resources/Resources'),
    BulkAddUsers = require('./BulkAddUsers/BulkAddUsers');

(function($, window, document, undefined) {

  $(document).ready(function() {
    ConnectConference.loadData();
  });

  window.ConnectConference = {
    modules: {},

    loadData: function() {
      $('.connectConference').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var resources = $(el).data('resources');
        var security = $(el).data('security');
        ConnectConference.modules[moduleId] = {
          service: new ConferenceService($, moduleId),
          resources: resources,
          security: security
        };
      });
      $('.timesheetEditor').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var slots = $(el).data('slots');
        var conferenceId = $(el).data('conference');
        var nrDays = $(el).data('nrdays');
        React.render(<TimesheetEditor moduleId={moduleId} slots={slots} 
           conferenceId={conferenceId} nrDays={nrDays} />, el);
      });
      $('.commentsComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var conferenceId = $(el).data('conference');
        var sessionId = $(el).data('session');
        var visibility = $(el).data('visibility');
        var pageSize = $(el).data('pagesize');
        var comments = $(el).data('comments');
        var appPath = $(el).data('apppath');
        var totalComments = $(el).data('totalcomments');
        var loggedIn = $(el).data('loggedin');
        var title = $(el).data('title');
        var help = $(el).data('help');
        React.render(<Comments moduleId={moduleId} 
           conferenceId={conferenceId} sessionId={sessionId} appPath={appPath}
           totalComments={totalComments} loggedIn={loggedIn} title={title} help={help}
           visibility={visibility} pageSize={pageSize} comments={comments} />, el);
      });
      $('.tagsComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var conferenceId = $(el).data('conference');
        var fieldName = $(el).data('name');
        var placeholder = $(el).data('placeholder');
        var tags = $(el).data('tags');
        React.render(<Tags moduleId={moduleId} name={fieldName} tags={tags} placeholder={placeholder}
           conferenceId={conferenceId} />, el);
      });
      $('.speakersComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var conferenceId = $(el).data('conference');
        var sessionId = $(el).data('session');
        var fieldName = $(el).data('name');
        var speakers = $(el).data('speakers');
        React.render(<Speakers moduleId={moduleId} name={fieldName} speakers={speakers} sessionId={sessionId}
           conferenceId={conferenceId} />, el);
      });
      $('.tagVoteComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var conferenceId = $(el).data('conference');
        var voteList = $(el).data('votelist');
        var allowVote = $(el).data('allowvote');
        var allowAdd = $(el).data('allowadd');
        React.render(<TagVotes moduleId={moduleId} voteList={voteList} allowVote={allowVote} allowAdd={allowAdd}
           conferenceId={conferenceId} />, el);
      });
      $('.sessionVoteComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var conferenceId = $(el).data('conference');
        var voteList = $(el).data('votelist');
        var allowVote = $(el).data('allowvote');
        React.render(<SessionVotes moduleId={moduleId} voteList={voteList} allowVote={allowVote}
           conferenceId={conferenceId} />, el);
      });
      $('.schedulerComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var conference = $(el).data('conference');
        var nrDays = $(el).data('nrdays');
        var slots = $(el).data('slots');
        var sessions = $(el).data('sessions');
        var gridHeight = $(el).data('gridheight');
        var locations = $(el).data('locations');
        React.render(<Scheduler moduleId={moduleId} conference={conference} locations={locations}
                      nrDays={nrDays} slots={slots} sessions={sessions} gridHeight={gridHeight} />, el);
      });
      $('.scheduleComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var conference = $(el).data('conference');
        var nrDays = $(el).data('nrdays');
        var slots = $(el).data('slots');
        var sessions = $(el).data('sessions');
        var gridHeight = $(el).data('gridheight');
        var locations = $(el).data('locations');
        React.render(<Schedule moduleId={moduleId} conference={conference} locations={locations}
                      nrDays={nrDays} slots={slots} sessions={sessions} gridHeight={gridHeight} />, el);
      });
      $('.resourcesComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var tabId = $(el).data('tabid');
        var conferenceId = $(el).data('conferenceid');
        var sessionId = $(el).data('sessionid');
        var resources = $(el).data('resources');
        var canAdd = $(el).data('canadd');
        var resourceDir = $(el).data('resourcedir');
        React.render(<Resources moduleId={moduleId} conferenceId={conferenceId} resources={resources}
                      canAdd={canAdd} tabId={tabId} sessionId={sessionId} resourceDir={resourceDir} />, el);
      });
      $('.bulkAddUsersComponent').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var tabId = $(el).data('tabid');
        var conferenceId = $(el).data('conferenceid');
        var type = $(el).data('type');
        React.render(<BulkAddUsers moduleId={moduleId} conferenceId={conferenceId} type={type} />, el);
      });
    },

    formatString: function(format) {
      var args = Array.prototype.slice.call(arguments, 1);
      return format.replace(/{(\d+)}/g, function(match, number) {
        return typeof args[number] != 'undefined' ? args[number] : match;
      });
    }


  }


})(jQuery, window, document);
