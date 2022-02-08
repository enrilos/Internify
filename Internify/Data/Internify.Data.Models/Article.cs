namespace Internify.Data.Models
{
    using Common;
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants;
    using static Common.DataConstants.Article;

    public class Article : IAuditInfo, IDeletableEntity
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        // The default should be company image url
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}