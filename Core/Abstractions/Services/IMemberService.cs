using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface IMemberService
    {
        Task<CreateMemberCommandResult> CreateMemberCommandHandler(CreateMemberCommand command);
        Task<UpdateMemberCommandResult> UpdateMemberCommandHandler(UpdateMemberCommand command);
        Task<GetAllMembersQueryResult> GetAllMembersQueryHandler();
    }
}
