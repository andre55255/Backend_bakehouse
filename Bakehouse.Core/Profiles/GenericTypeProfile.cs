using AutoMapper;
using Bakehouse.Communication.ViewObjects;
using Bakehouse.Core.Entities;

namespace Bakehouse.Core.Profiles
{
    public class GenericTypeProfile : Profile
    {
        public GenericTypeProfile()
        {
            CreateMap<GenericType, GenericType>()
                .ReverseMap();

            CreateMap<GenericTypeVO, GenericType>()
                .ReverseMap();
        }
    }
}