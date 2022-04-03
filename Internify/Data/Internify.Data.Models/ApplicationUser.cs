namespace Internify.Data.Models
{
    using Common;
    using Microsoft.AspNetCore.Identity;
    using System;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
