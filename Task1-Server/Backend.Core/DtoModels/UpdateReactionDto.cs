using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.DtoModels
{
    public class UpdateReactionDto
    {
        public string Comment { get; set; }
        public int? Grade { get; set; }
        public int? InterestRate { get; set; }
    }
}
