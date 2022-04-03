namespace Internify.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.Message;

    public class Message
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string SenderId { get; init; }

        public ApplicationUser Sender { get; set; }

        public DateTime SentOn { get; init; } = DateTime.UtcNow;
    }
}