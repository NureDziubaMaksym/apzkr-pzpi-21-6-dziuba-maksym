using Backend.Core.Interfaces;
using Backend.Core.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Backend.Core.Repository
{
    public class ContentRepository : IContentRepository
    {
        private readonly DataContext _dataContext;
        private readonly IdGenService _idGenService;

        public ContentRepository(DataContext dataContext, IdGenService idGenService)
        {
            _dataContext = dataContext;
            _idGenService = idGenService;
        }

        public bool AddContent(Content content)
        {
            if (content == null)
            {
                return false;
            }

            content.ContentId = _idGenService.GenerateNewId<Content>();

            _dataContext.Add(content);
            return Save();
        }

        public Content GetContentByTitle(string title)
        {
            return _dataContext.Contents.FirstOrDefault(c => c.Title.ToLower().Trim() == title.ToLower().Trim());
        }

        public ICollection<Content> GetContents()
        {
            return _dataContext.Contents.OrderBy(p => p.ContentId).ToList();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
