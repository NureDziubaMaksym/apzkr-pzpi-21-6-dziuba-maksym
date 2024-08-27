using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public int FocusGroupId { get; set; }
        public int ContentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FocusGroup FocusGroup { get; set; }
        public Content Content { get; set; }
        public ICollection<Result> Results { get; set; }
    }
}
