using Application.Activities.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, ActivityDto>();

        CreateMap<CreateActivityDto, Activity>();

        CreateMap<EditActivityDto, Activity>();
    }
}
