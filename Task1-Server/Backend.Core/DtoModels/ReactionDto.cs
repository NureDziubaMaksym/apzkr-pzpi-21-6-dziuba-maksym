using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.DtoModels
{
    public class ReactionDto
    {
        public int ReactionId { get; set; }
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public string Comment { get; set; }
        public int Grade { get; set; }
        public int InterestRate { get; set; }
    }
}
