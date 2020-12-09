using Core.Abstractions.Repositories;
using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class MemberRepository : BaseRepository<Guid, MemberDm, MemberRepository>, IMemberRepository
    {
        public MemberRepository(FamilyTaskContext context) : base(context)
        { }
       

        IMemberRepository IBaseRepository<Guid, MemberDm, IMemberRepository>.NoTrack()
        {
            return base.NoTrack();
        }

        IMemberRepository IBaseRepository<Guid, MemberDm, IMemberRepository>.Reset()
        {
            return base.Reset();
        }

       
    }
}
