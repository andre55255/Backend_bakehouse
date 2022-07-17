using AutoMapper;
using Bakehouse.Communication.ViewObjects.UnitOfMeasurement;
using Bakehouse.Core.Entities;

namespace Bakehouse.Core.Profiles
{
    public class UnitOfMeasurementProfile : Profile
    {
        public UnitOfMeasurementProfile()
        {
            CreateMap<UnitOfMeasurement, UnitOfMeasurementVO>()
                .ReverseMap();
        }
    }
}