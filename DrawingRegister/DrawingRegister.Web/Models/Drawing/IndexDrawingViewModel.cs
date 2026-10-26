using DrawingRegister.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Web.Models.Drawing
{
    public class IndexDrawingViewModel
    {
        public int Id { get; set; }
        public int DocumentNumber { get; set; }
        public string Name { get; set; }

        /*
            Entity-typed properties are a smell in ViewModels. The enums (Type, Status) are fine for now, they’re simple values. Entities with navigation properties are what you want to keep out.
        */
        public Data.Entities.Type DrawingType { get; set; }
        public Status DrawingStatus { get; set; }
        public DateTime? ReleaseDate { get; set; }
        //public Project Project { get; set; } // No need to load whole Project just need name
        public string ProjectName { get; set; }

    }
}
