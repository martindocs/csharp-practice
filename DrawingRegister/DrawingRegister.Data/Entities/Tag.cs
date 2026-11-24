using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingRegister.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<Drawing> Drawings { get; set; } = new List<Drawing>(); // init collection so its not null at the start
    }
}
