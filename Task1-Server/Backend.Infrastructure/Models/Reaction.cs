using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Models
{
    public class Reaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReactionId { get; set; }
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public string Comment { get; set; }
        public int Grade { get; set; }
        public int InterestRate { get; set; }
        public User User { get; set; }
        public Content Content { get; set; }
    }
}
