using AutoMapper;
using Bakehouse.Communication.ViewObjects.Account;
using Bakehouse.Communication.ViewObjects.Utils;
using Bakehouse.Core.Entities;
using Microsoft.AspNetCore.Identity;

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

            CreateMap<IdentityRole<int>, SelectObjectVO>()
                .ForMember(x => x.Label, x => x.MapFrom(x => x.Name))
                .ForMember(x => x.Value, x => x.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<ApplicationUser, SaveUserVO>()
                .ForMember(x => x.Password, x => x.MapFrom(x => x.PasswordHash))
                .ForMember(x => x.Contact, x => x.MapFrom(x => x.PhoneNumber))
                .ReverseMap();

            CreateMap<ApplicationUser, UpdateUserVO>()
                .ForMember(x => x.Contact, x => x.MapFrom(x => x.PhoneNumber))
                .ReverseMap();
        }
    }
}