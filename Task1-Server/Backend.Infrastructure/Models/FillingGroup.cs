using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Models
{
    public class FillingGroup
    {
        public int AddGrId { get; set; }
        public int FocusId { get; set; }
        public int UserId { get; set; }

        public FocusGroup FocusGroup { get; set; }
        public User User { get; set; }
    }
}
