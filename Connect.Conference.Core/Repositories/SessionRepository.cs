using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Collections;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.Conference.Core.Data;
using Connect.Conference.Core.Models.Sessions;

namespace Connect.Conference.Core.Repositories
{

	public class SessionRepository : ServiceLocator<ISessionRepository, SessionRepository>, ISessionRepository
 {
        protected override Func<ISessionRepository> GetFactory()
        {
            return () => new SessionRepository();
        }
        public IEnumerable<Session> GetSessions(int conferenceId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Session>();
                return rep.Get(conferenceId);
            }
        }
        public Session GetSession(int conferenceId, int sessionId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Session>();
                return rep.GetById(sessionId, conferenceId);
            }
        }
        public int AddSession(SessionBase session, int userId)
        {
            Requires.NotNull(session);
            Requires.PropertyNotNegative(session, "ConferenceId");
            session.CreatedByUserID = userId;
            session.CreatedOnDate = DateTime.Now;
            session.LastModifiedByUserID = userId;
            session.LastModifiedOnDate = DateTime.Now;
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<SessionBase>();
                rep.Insert(session);
            }
            return session.SessionId;
        }
        public void DeleteSession(SessionBase session)
        {
            Requires.NotNull(session);
            Requires.PropertyNotNegative(session, "SessionId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<SessionBase>();
                rep.Delete(session);
            }
        }
        public void UpdateSession(SessionBase session, int userId)
        {
            Requires.NotNull(session);
            Requires.PropertyNotNegative(session, "SessionId");
            session.LastModifiedByUserID = userId;
            session.LastModifiedOnDate = DateTime.Now;
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<SessionBase>();
                rep.Update(session);
            }
        } 
 }

    public interface ISessionRepository
    {
        IEnumerable<Session> GetSessions(int conferenceId);
        Session GetSession(int conferenceId, int sessionId);
        int AddSession(SessionBase session, int userId);
        void DeleteSession(SessionBase session);
        void UpdateSession(SessionBase session, int userId);
    }
}

