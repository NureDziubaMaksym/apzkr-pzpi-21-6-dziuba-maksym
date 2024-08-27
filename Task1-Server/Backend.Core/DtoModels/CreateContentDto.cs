using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.DtoModels
{
    public class CreateContentDto
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }
}
