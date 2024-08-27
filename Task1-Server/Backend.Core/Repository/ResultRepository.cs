using Backend.Core.Interfaces;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repository
{
    public class ResultRepository : IResultRepository
    {
        private readonly DataContext _dataContext;

        public ResultRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public ICollection<Result> ResulsBySession(int sessionId)
        {
            return _dataContext.Results
                .Where(r => r.SessionId == sessionId)
                .OrderBy(r => r.ResultId)
                .ToList();
        }

        public ICollection<Result> ResultByContent(int contentId)
        {
            return _dataContext.Sessions
                .Where(s => s.ContentId == contentId)
                .SelectMany(s => s.Results)
                .OrderBy(r => r.ResultId)
                .ToList();
        }
    }
}
