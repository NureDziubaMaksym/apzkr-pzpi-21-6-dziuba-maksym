using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.DtoModels
{
    public class ResultDto
    {
        public int ResultId { get; set; }
        public int SessionId { get; set; }
        public string AvrEmotion { get; set; }
    }
}
