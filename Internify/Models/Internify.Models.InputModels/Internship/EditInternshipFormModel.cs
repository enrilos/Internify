namespace Internify.Models.InputModels.Internship
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants;

    public class EditInternshipFormModel
    {
        [Required]
        public string Id { get; set; }

        public bool IsPaid { get; set; }

        [Range(typeof(decimal), "1", "9999999")]
        public decimal? SalaryUSD { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }
    }
}