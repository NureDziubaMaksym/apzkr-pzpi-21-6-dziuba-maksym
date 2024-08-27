using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Backend.Infrastructure.Models
{
    public class FocusGroup
    {
        public int FocGrId { get; set; }
        public string Race { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<FillingGroup> FillingGroups { get; set; }
    }
}
