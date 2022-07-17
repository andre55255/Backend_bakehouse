using AutoMapper;
using Bakehouse.Communication.ViewObjects.Category;
using Bakehouse.Core.Entities;

namespace Bakehouse.Core.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryVO>()
                .ReverseMap();
        }
    }
}