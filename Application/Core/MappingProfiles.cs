using Application.Activities.DTOs;
using Application.Comments.DTOs;
using Application.Photos.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string? currentUsername = null;

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
                opt.MapFrom(src => src.AppUser.Photos.FirstOrDefault(p => p.IsMain)!.Url))
            .ForMember(dest => dest.FollowersCount, opt =>
                opt.MapFrom(src => src.AppUser.Followers.Count))
            .ForMember(dest => dest.FollowingCount, opt =>
                opt.MapFrom(src => src.AppUser.Followings.Count))
            .ForMember(dest => dest.Following, opt =>
                opt.MapFrom(src => src.AppUser.Followers
                    .Any(u => u.Observer.UserName == currentUsername)));

        CreateMap<Photo, PhotoDto>();

        CreateMap<AppUser, Profiles.DTOs.ProfileDto>()
            .ForMember(dest => dest.Image, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain)!.Url))
            .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
            .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Followings.Count))
            .ForMember(dest => dest.Following, opt =>
                opt.MapFrom(src => src.Followers.Any(u => u.Observer.UserName == currentUsername)));

        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Author.UserName))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Author.DisplayName))
            .ForMember(dest => dest.Image, opt =>
                opt.MapFrom(src => src.Author.Photos.FirstOrDefault(p => p.IsMain)!.Url));

        CreateMap<ActivityAttendee, Profiles.DTOs.UserActivityDto>()
            .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.Activity.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Activity.Title))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Activity.Category))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Activity.Date))
            .ForMember(dest => dest.HostUsername, opt =>
                opt.MapFrom(src => src.Activity.Attendees
                    .FirstOrDefault(aa => aa.IsHost)!.AppUser.UserName));
    }
}
