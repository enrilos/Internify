namespace Internify.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants;

    public class Specialization
    {
        [Key]
        public string SpecializationId { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
