using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IResultRepository
    {
        ICollection<Result> ResulsBySession(int sessionId);
        ICollection<Result> ResultByContent(int contentId);
    }
}
