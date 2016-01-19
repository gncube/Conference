/** @jsx React.DOM */
var SchedulerDay = require('./SchedulerDay'),
  SchedulerUnscheduledSession = require('./SchedulerUnscheduledSession');

var Scheduler = React.createClass({

  resources: null,
  service: null,

  getInitialState: function() {
    this.resources = ConnectConference.modules[this.props.moduleId].resources;
    this.service = ConnectConference.modules[this.props.moduleId].service;
    var locationList = {};
    for (i=0;i<this.props.locations.length;i++)
    {
      locationList[this.props.locations[i].LocationId] = i;
    }
    return {
      sessionList: this.props.sessions,
      locationList: locationList
    }
  },

  render: function() {
    var unscheduledSessions = this.state.sessionList.map(function(item) {
      if (item.SlotId == 0) {
        return <SchedulerUnscheduledSession {...this.props} session={item} key={item.SessionId} />
      }
      else
      {
        return null;        
      }
    });
    var scheduleDays = [];
    for (i = 1; i <= this.props.nrDays; i++) {
      var daySlots = [];
      for (j=0;j<this.props.slots.length;j++)
      {
        var slot = this.props.slots[j];
        if (slot.DayNr == undefined | slot.DayNr == i)
        {
          daySlots.push(slot);
        }
      }
      scheduleDays.push(
        <SchedulerDay day={i} slots={daySlots} 
           start={Math.floor(daySlots[0].StartMinutes/60) * 60 - 60}
           finish={120 + Math.floor(daySlots[daySlots.length - 1].StartMinutes / 60) * 60}
           locationList={this.state.locationList} 
           leftMargin={50}
           sessionList={this.state.sessionList}
           locations={this.props.locations} />
        );
    }
    return (
      <div className="row Scheduler">
        <div className="col-xs-12 col-md-2">
          {unscheduledSessions}
        </div>
        <div className="col-xs-12 col-md-10">
          {scheduleDays}
        </div>
      </div>
    );
  },

  componentDidMount: function() {
    $(document).ready(function() {
      var hasReset = true;
      interact('.session')
        .draggable({
          inertia: false,
          restrict: {
            endOnly: true
          },
          autoScroll: false,
          onend: function(event) {}
        })
        .on('dragmove', function(event) {
          moveObject(event.target, event.dx, event.dy);
        });
      interact('.canDrop')
        .dropzone({
          accept: '.session',
          overlap: 0.5,
          ondropactivate: function(event) {
            hasReset = false;
          },
          ondragenter: function(event) {
            var dropzoneElement = event.target;
            dropzoneElement.classList.add('drop-target');
          },
          ondragleave: function(event) {
            event.target.classList.remove('drop-target');
          },
          ondrop: function(event) {
            var sessionId = event.relatedTarget.getAttribute('data-sessionid');
            var slotId = event.target.getAttribute('data-slotid');
            var locationId = event.target.getAttribute('data-locationid');
            var day = event.target.getAttribute('data-day');
            var isPlenary = event.relatedTarget.getAttribute('data-plenary');
            if (event.relatedTarget.getAttribute('data-slotkey') != '') {
              $('[data-reactid="' + event.relatedTarget.getAttribute('data-slotkey') + '"]').attr('class', 'sessionSlot canDrop');
            }
            hasReset = true;
            var session = $(event.relatedTarget);
            var dropBox = event.target.getBoundingClientRect();
            var sessionBox = event.relatedTarget.getBoundingClientRect();
            session.width(dropBox.width - 12);
            session.height(dropBox.height - 12);
            moveObject(event.relatedTarget, 
                       dropBox.left - sessionBox.left + 4,
                       dropBox.top - sessionBox.top + 4);
            session.data('orig-x', session.data('x'));
            session.data('orig-y', session.data('y'));
            event.relatedTarget.setAttribute('data-slotkey', event.target.getAttribute('data-reactid'));
            event.target.classList.remove('canDrop');
          },
          ondropdeactivate: function(event) {
            if (!hasReset)
            {
              var s = event.relatedTarget;
              var session = $(s);
              var x = session.data('orig-x');
              var y = session.data('orig-y');
              s.style.webkitTransform =
                s.style.transform =
                'translate(' + x + 'px, ' + y + 'px)';
              s.setAttribute('data-x', x);
              s.setAttribute('data-y', y);
              hasReset = true;
            }
            event.target.classList.remove('drop-target');
          }
        });
    });
  }

});

module.exports = Scheduler;