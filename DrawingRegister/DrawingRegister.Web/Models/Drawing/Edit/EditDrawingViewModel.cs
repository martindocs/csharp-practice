using DrawingRegister.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Web.Models.Drawing.Edit
{
    public class EditDrawingViewModel: IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Document Number.")]
        [RegularExpression(@"^[A-Z]{3}-[0-9]{4}$", ErrorMessage = "Use the format DRW-1234")]
        public string? DocumentNumber { get; set; } 

        [Required(ErrorMessage = "Please enter Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Path.")]
        public string Path { get; set; }

        [Required(ErrorMessage = "Please enter Drawing Type.")]       
        public Data.Entities.Type? DrawingType { get; set; } 

        [Required(ErrorMessage = "Please enter Drawing Status.")]
        public Status? DrawingStatus { get; set; }
        public DateTime? ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please enter project name.")]
        public int? ProjectId { get; set; }
        
        public List<SelectListItem> Projects { get; set; } = new();

        // Validate only runs if the attribute checks pass first. So an empty required field shows its message first, and the cross-field message appears on the next submit.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Status is Released and ReleaseDate is empty

            if (DrawingStatus == Status.Released && !ReleaseDate.HasValue) // release needs revision 
            {               
                yield return new ValidationResult("Please choose the release date", new[] { nameof(ReleaseDate) });
            }

            // Status is not Released and ReleaseDate has a value
            if (DrawingStatus.Value != Status.Released && ReleaseDate.HasValue)
            {
                // error on ReleaseDate
                yield return new ValidationResult("Please set the status to Released", new[] { nameof(DrawingStatus) });
            }

            // If the object is valid (no IF fires), the Validate method should return an empty collection, which means "valid"            
        }
    }
}
