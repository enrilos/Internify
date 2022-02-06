﻿namespace Internify.Data.Common
{
    public class DataConstants
    {
        public const int UrlMaxLength = 500;
        public const int DescriptionMaxLength = 2000;
        public const int NameMaxLength = 70;

        public class Company
        {
            public const int IndustryMaxLength = 50;
            public const int CEOMaxLength = 40;
        }

        public class Internship
        {
            public const int RoleMaxLength = 50;
        }

        public class Article
        {
            public const int TitleMaxLength = 50;
            public const int ContentMaxLength = 5000;
        }

        public class Review
        {
            public const int TitleMaxLength = 50;
            public const int ContentMaxLength = 2000;
        }
    }
}