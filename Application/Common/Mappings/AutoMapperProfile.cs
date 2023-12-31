using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ProductExample, ProductExampleDto>();
    }
}