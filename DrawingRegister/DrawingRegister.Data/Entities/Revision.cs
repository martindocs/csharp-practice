using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingRegister.Data.Entities
{
    public class Revision
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Revision Letter must be 1 to 10 characters length.")]
        public string Letter { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DrawingId { get; set; }
        public Drawing Drawing { get; set; }
    }
}
