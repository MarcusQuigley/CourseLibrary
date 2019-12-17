using AutoMapper;
using CourseLibrary.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.MapperProfiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            CreateMap<Entities.Author, Models.AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
                )
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge())
                );

            CreateMap<Models.AuthorForCreationDto, Entities.Author>();
            CreateMap<Models.AuthorForUpdateDto, Entities.Author>();
        }
    }
}
