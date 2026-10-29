using DrawingRegister.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Web.Models.Drawing.Add
{
    public class AddDrawingViewModel
    {
        [Required(ErrorMessage = "Please enter Document Number.")]
        public int? DocumentNumber { get; set; } // for default int is 0, to remove def value it need to be nullable

        [Required(ErrorMessage = "Please enter Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Path.")]
        public string Path { get; set; }

        [Required(ErrorMessage = "Please enter Drawing Type.")]

        /*
            A new ViewModel has DrawingType and DrawingStatus at their default, which is the enum member with value 0 (your first member). asp-for sees that value and marks that option as selected, which overrides your selected placeholder.         
         */
        public Data.Entities.Type? DrawingType { get; set; } // by default without nullable is first value from enum

        [Required(ErrorMessage = "Please enter Drawing Status.")]
        public Status? DrawingStatus { get; set; } // by default without nullable is first value from enum
        [Required(ErrorMessage = "Please enter project name.")]
        public int? ProjectId { get; set; }

        // standard approach is to put the options on the ViewModel and let the server render them
        public List<SelectListItem> Projects { get; set; } = new();
    }
}
