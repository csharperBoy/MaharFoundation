using AutoMapper;
using Mahar.Common.DTOs;
using Mahar.Core.Models;

namespace Mahar.Identity.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
        }
    }
}