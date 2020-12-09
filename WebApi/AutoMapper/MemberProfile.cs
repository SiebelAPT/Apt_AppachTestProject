using AutoMapper;
using Domain.Commands;
using Domain.DataModels;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.AutoMapper
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<CreateMemberCommand, MemberDm>();
            CreateMap<UpdateMemberCommand, MemberDm>();
            CreateMap<MemberDm, MemberVm>();
        }
    }
}
