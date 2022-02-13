namespace Internify.Web.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Models.ViewModels.Candidate;
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

            // int age = (int) ((DateTime.Now - bday).TotalDays/365.242199); (accurate up to 107 years of age.)
            CreateMap<Candidate, CandidateListingViewModel>()
                .ForMember(x => x.Age, y => y.MapFrom(s => (int)((DateTime.Now - s.BirthDate).TotalDays / 365.242199)))
                .ForMember(x => x.Specialization, y => y.MapFrom(s => s.Specialization.Name))
                .ForMember(x => x.Country, y => y.MapFrom(s => s.Country.Name));

            CreateMap<Candidate, CandidateDetailsViewModel>()
                .ForMember(x => x.BirthDate, y => y.MapFrom(s => s.BirthDate.ToShortDateString()))
                .ForMember(x => x.Specialization, y => y.MapFrom(s => s.Specialization.Name))
                .ForMember(x => x.Country, y => y.MapFrom(s => s.Country.Name));
        }
    }
}
