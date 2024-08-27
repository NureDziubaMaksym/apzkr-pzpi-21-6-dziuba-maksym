using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int SessionId { get; set; }
        public string AvrEmotion { get; set; }
        public Session Session { get; set; }
    }
}
