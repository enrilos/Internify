namespace Internify.Data.Common.Attributes
{
    using System.ComponentModel.DataAnnotations;

    public class BirthDateAttribute : RangeAttribute
    {
        public BirthDateAttribute()
            : base(typeof(DateTime),
                  DateTime.UtcNow.AddYears(-100).ToShortDateString(),
                  DateTime.UtcNow.AddYears(-16).ToShortDateString())
        {
        }
    }
}
