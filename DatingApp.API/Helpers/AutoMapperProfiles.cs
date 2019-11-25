
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
            #region Response DTOs
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
            CreateMap<Photo, PhotoForReturnDTO>();
            CreateMap<Message, MessageToReturnDTO>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).URL))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).URL));

            #endregion


            #region Request DTOs

            CreateMap<UserForUpdateDTO, User>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<PhotoForCreationDTO, Photo>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();

            #endregion
        }
    }
}
