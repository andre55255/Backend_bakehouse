using AutoMapper;
using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Core.Entities;

namespace Bakehouse.Core.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, UserVO>()
                .ForMember(x => x.FullName, x => x.MapFrom(x => x.Name + " " + x.LastName))
                .ForMember(x => x.Contact, x => x.MapFrom(x => x.PhoneNumber))
                .ReverseMap();
        }
    }
}