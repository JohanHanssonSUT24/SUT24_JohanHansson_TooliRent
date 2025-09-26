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
    public class AutoMapperProfile : Profile//Use AutoMapper to define mappings between entities and DTOs
    {
        public AutoMapperProfile() 
        {
            CreateMap<Tool, ToolDto>()//Custom mapping for Tool to ToolDto. Maps the ToolCategoryName from the related ToolCategory entity.
                .ForMember(dest => dest.ToolCategoryName, opt => opt.MapFrom(src => src.ToolCategory.Name))
                .ReverseMap();

            CreateMap<CreateToolDto, Tool>() // Map CreateToolDto to Tool entity
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<UpdateToolDto, Tool>() // Map UpdateToolDto to Tool entity
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<ToolCategory, ToolCategoryDto>().ReverseMap();
            CreateMap<CreateToolCategoryDto, ToolCategory>();
            CreateMap<UpdateToolCategoryDto, ToolCategory>();

            CreateMap<CreateBookingDto, Booking>();
            CreateMap<Booking, BookingDto>() // Custom mapping for Booking to BookingDto. Maps ToolName and UserName from related entities.
                .ForMember(dest => dest.ToolName, opt => opt.MapFrom(src => src.Tool.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<Tool, ToolStatisticsDto>();
               
        }
    }
}
