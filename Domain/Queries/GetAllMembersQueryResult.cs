using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Queries
{
    public class GetAllMembersQueryResult
    {
        public IEnumerable<MemberVm> Payload { get; set; }        
    }

}
