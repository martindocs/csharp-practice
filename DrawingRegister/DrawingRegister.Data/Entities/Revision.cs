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
        /*
            To know about the migration: EF may treat a rename as drop the old column and add a new one rather than a rename, because it only sees the before and after model, not your intent. 
           Since the data is fake, that’s harmless here. But read the generated migration anyway. If you see DropColumn followed by AddColumn, that’s what happened. 
           In a real database with real data that would lose the column’s contents, and you’d edit the migration to use RenameColumn instead. 
         */
        public string RevisionLetter { get; set; }
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Revision Letter must be 1 to 150 characters length.")]
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DrawingId { get; set; }
        public Drawing Drawing { get; set; }
    }
}

