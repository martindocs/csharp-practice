using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DrawingRegister.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be 5 to 100 characters length.")]
        public string Name { get; set; }
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must to be 10 to 200 characters length")]
        public string? Description { get; set; }
        public ICollection<Drawing> Drawings { get; set; } = new List<Drawing>(); // init collection so its not null at the start
    }
}