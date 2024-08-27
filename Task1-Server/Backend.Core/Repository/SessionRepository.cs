using Backend.Core.Interfaces;
using Backend.Core.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataContext _dataContext;
        private readonly IdGenService _idGenService;

        public SessionRepository(DataContext dataContext, IdGenService idGenService)
        {
            _dataContext = dataContext;
            _idGenService = idGenService;
        }

        public bool AddSession(Session session)
        {
            if (session == null)
            {
                return false;
            }

            session.SessionId = _idGenService.GenerateNewId<Session>();

            _dataContext.Sessions.Add(session);
            return Save();
        }

        public ICollection<Session> GetSessions()
        {
            return _dataContext.Sessions.OrderBy(s => s.SessionId).ToList();
        }

        public bool Save()
        {
            return _dataContext.SaveChanges() >= 0;
        }

        public ICollection<Session> SessionsByGrouptId(int id)
        {
            return _dataContext.Sessions
                .Where(s => s.FocusGroupId == id)
                .OrderBy(s => s.SessionId)
                .ToList();
        }
    }
}
