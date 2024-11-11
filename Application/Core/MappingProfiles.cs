using Application.Activities.DTOs;
using Application.Photos.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.HostUsername, opt =>
                opt.MapFrom(src => src.Attendees.FirstOrDefault(aa => aa.IsHost)!.AppUser.UserName));

        CreateMap<CreateActivityDto, Activity>();

        CreateMap<EditActivityDto, Activity>();

        CreateMap<ActivityAttendee, AttendeeDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.AppUser.UserName))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.AppUser.DisplayName))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.AppUser.Bio))
            .ForMember(dest => dest.Image, opt =>
                opt.MapFrom(src => src.AppUser.Photos.FirstOrDefault(p => p.IsMain)!.Url));

        CreateMap<Photo, PhotoDto>();

        CreateMap<AppUser, Profiles.Profile>()
            .ForMember(dest => dest.Image, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain)!.Url));
    }
}
