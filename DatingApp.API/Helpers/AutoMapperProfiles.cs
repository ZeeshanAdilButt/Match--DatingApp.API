
using AutoMapper;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src=> src.Photos.FirstOrDefault(p=> p.IsMain).URL))
                .ForMember(dest=> dest.Age, opt =>
                opt.MapFrom(src=> src.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailDTO>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).URL))
                .ForMember(dest => dest.Age, opt =>
                 opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotosForDetailDTO>();
        }
    }
}
