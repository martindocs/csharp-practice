using DrawingRegister.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Web.Models.Drawing.Add
{
    public class AddDrawingViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Please enter Document Number.")]
        // @ - verbatim string literals
        [RegularExpression(@"^[A-Z]{3}-[0-9]{4}$", ErrorMessage = "Use the format DRW-1234")]
        public string? DocumentNumber { get; set; } // for default int is 0, to remove def value it need to be nullable

        [Required(ErrorMessage = "Please enter Name.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be 5 to 100 characters length.")]
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
      
        public DateTime? ReleaseDate { get; set; }
        [Required(ErrorMessage = "Please enter project name.")]
        public int? ProjectId { get; set; }

        // standard approach is to put the options on the ViewModel and let the server render them
        public List<SelectListItem> Projects { get; set; } = new();

        /*
            Cross-filed rule. ReleasedDate can't be before set Status to Released. 

            * If Status is Released, ReleaseDate is required. If it isn't Released, ReleaseDate should be empty.
            * MVC runs it automatically with the attributes, so ModelState.IsValid already includes it. Your controller doesn't change
            * Errors show next to the field, the same way your other messages do.
            * It runs on the server only
         */

        // Validate only runs if the attribute checks pass first. So an empty required field shows its message first, and the cross-field message appears on the next submit.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Status is Released and ReleaseDate is empty
           
            if (DrawingStatus == Status.Released && !ReleaseDate.HasValue) // release needs revision 
            {
                // error on release date

                /*
                    yield -  lets a method return items one at a time in a sequence, pausing its execution between each item instead of building an entire collection in memory upfront.
                 
                    ValidationResult(..) - creates one error object. It takes two things: the message and the list of fields the error belongs to.
                                
                    "message" - error message  

                    new[] { ... }: the second parameter wants a collection of field names, not a single name.

                    nameof(ReleaseDate): produces the text "ReleaseDate"
                 
                 */

                // MVC loops over the results, and each time it asks for the next one, your method runs until it hits a yield return, hands that one back, and waits. When it runs off the end, the loop is finished. If no yield return ran, the loop got nothing, which means "valid"
                yield return new ValidationResult("Please choose the release date", new[]{nameof(ReleaseDate)});
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
