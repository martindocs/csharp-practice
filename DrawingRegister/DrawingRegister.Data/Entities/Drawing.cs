using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Data.Entities
{
    public class Drawing
    {
        public int Id { get; set; }
        public int DocumentNumber { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be 5 to 100 characters length.")]
        public string Name { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Path must be 5 to 250 characters length.")]
        public string Path { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Drawing Type must be 5 to 100 characters length.")]
        public Type DrawingType { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Drawing Status must be 5 to 100 characters length.")]
        public Status DrawingStatus { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<Revision> Revisions { get; set; } = new List<Revision>(); // init collection so its not null at the start
        public ICollection<Tag> Tags { get; set; } = new List<Tag>(); // init collection so its not null at the start

    }
}
