using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Data.Entities
{
    public class Drawing
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string DocumentNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Path { get; set; }
        [Required]
        [StringLength(100)]
        public Type DrawingType { get; set; }
        [Required]
        [StringLength(100)]
        public Status DrawingStatus { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<Revision> Revisions { get; set; } = new List<Revision>(); // init collection so its not null at the start
        public ICollection<Tag> Tags { get; set; } = new List<Tag>(); // init collection so its not null at the start

    }
}
