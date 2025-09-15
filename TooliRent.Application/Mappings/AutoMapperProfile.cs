using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TooliRent.Application.DTOs;
using TooliRent.Domain.Entities;

namespace TooliRent.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Tool, ToolDto>()
                .ForMember(dest => dest.ToolCategoryName, opt => opt.MapFrom(src => src.ToolCategory.Name))
                .ReverseMap();

            CreateMap<CreateToolDto, Tool>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<UpdateToolDto, Tool>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<ToolCategory, ToolCategoryDto>().ReverseMap();
            CreateMap<CreateToolCategoryDto, ToolCategory>();
            CreateMap<UpdateToolCategoryDto, ToolCategory>();
        }
    }
}
