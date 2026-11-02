using DrawingRegister.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Web.Models.Drawing.Edit
{
    public class EditDrawingViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Document Number.")]
        public int? DocumentNumber { get; set; } 

        [Required(ErrorMessage = "Please enter Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Path.")]
        public string Path { get; set; }

        [Required(ErrorMessage = "Please enter Drawing Type.")]       
        public Data.Entities.Type? DrawingType { get; set; } 

        [Required(ErrorMessage = "Please enter Drawing Status.")]
        public Status? DrawingStatus { get; set; } 

        [Required(ErrorMessage = "Please enter project name.")]
        public int? ProjectId { get; set; }
        
        public List<SelectListItem> Projects { get; set; } = new();
    }
}
