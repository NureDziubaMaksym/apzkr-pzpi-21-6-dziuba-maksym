using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IContentRepository
    {
        ICollection<Content> GetContents();
        bool AddContent(Content content);
        Content GetContentByTitle(string title);
        bool Save();
    }
}
