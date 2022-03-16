namespace Internify.Data.Common
{
    public class DataConstants
    {
        public const int UrlMaxLength = 500;
        public const int DescriptionMinLength = 30;
        public const int DescriptionMaxLength = 4000;
        public const int NameMaxLength = 70;
        public const int NameMinLength = 2;

        public class Company
        {
            public const int FoundedMinYear = 1645;
            public const int FoundedMaxYear = 2050;
        }

        public class University
        {
            public const int FoundedMinYear = 1088;
            public const int FoundedMaxYear = 2050;
        }

        public class Internship
        {
            public const int RoleMinLength = 2;
            public const int RoleMaxLength = 50;
        }

        public class Application
        {
            public const int CoverLetterMinLength = 30;
            public const int CoverLetterMaxLength = 3000;
        }

        public class Article
        {
            public const int TitleMinLength = 4;
            public const int TitleMaxLength = 50;
            public const int ContentMinLength = 50;
            public const int ContentMaxLength = 5000;
        }

        public class Review
        {
            public const int TitleMaxLength = 50;
            public const int ContentMaxLength = 2000;
        }

        public class Comment
        {
            public const int ContentMaxLength = 600;
        }
    }
}