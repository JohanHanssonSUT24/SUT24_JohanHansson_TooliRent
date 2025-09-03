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
            CreateMap<Tool, ToolDto>().ReverseMap();
            CreateMap<CreateToolDto, Tool>();
            CreateMap<UpdateToolDto, Tool>().ReverseMap();
        }
    }
}
