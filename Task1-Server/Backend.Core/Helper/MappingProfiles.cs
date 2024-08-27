using AutoMapper;
using Backend.Core.DtoModels;
using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<FocusGroup, FocusGroupDto>();
            CreateMap<FocusGroupDto, FocusGroup>();
            CreateMap<CreateFocusGroupDto, FocusGroup>();
            CreateMap<Session, SessionDto>();
            CreateMap<CreateSessionDto, Session>();
            CreateMap<Reaction, ReactionDto>();
            CreateMap<ReactionDto, Reaction>();
            CreateMap<CreateReactionDto, Reaction>();
            CreateMap<Content, ContentDto>();
            CreateMap<ContentDto, Content>();
            CreateMap<CreateContentDto, Content>();
            CreateMap<Result, ResultDto>();
        }
    }
}
