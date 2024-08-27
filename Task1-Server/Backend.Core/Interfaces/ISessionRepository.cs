using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface ISessionRepository
    {
        ICollection<Session> GetSessions();
        ICollection<Session> SessionsByGrouptId(int id);
        bool AddSession(Session session);
        bool Save();
    }
}
