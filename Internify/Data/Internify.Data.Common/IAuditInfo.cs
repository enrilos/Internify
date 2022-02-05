namespace Internify.Data.Common
{
    using System;

    public interface IAuditInfo
    {
        DateTime CreatedOn { get; init; }

        DateTime? ModifiedOn { get; set; }
    }
}