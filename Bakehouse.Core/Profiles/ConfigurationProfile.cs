using AutoMapper;
using Bakehouse.Communication.ViewObjects.Configuration;
using Bakehouse.Core.Entities;

namespace Bakehouse.Core.Profiles
{
    public class ConfigurationProfile : Profile
    {
        public ConfigurationProfile()
        {
            CreateMap<Configuration, Configuration>()
                .ReverseMap();
            CreateMap<ConfigurationVO, Configuration>()
                .ReverseMap();
        }
    }
}