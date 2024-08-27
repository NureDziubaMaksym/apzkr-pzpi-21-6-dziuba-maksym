using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.DtoModels
{
    public class AddMemberDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}
