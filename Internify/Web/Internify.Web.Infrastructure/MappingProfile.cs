namespace Internify.Web.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Models.InputModels.Candidate;
    using Models.ViewModels.Country;
    using Models.ViewModels.Specialization;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Specialization, SpecializationListingViewModel>();
            CreateMap<Country, CountryListingViewModel>();

            CreateMap<BecomeCandidateFormModel, Candidate>();
        }
    }
}
