using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;

namespace WebClient.Abstractions
{
    public interface IMemberDataService
    {
        IEnumerable<MemberVm> Members { get; }
        MemberVm SelectedMember { get; }

        event EventHandler MembersChanged;
        event EventHandler SelectedMemberChanged;
        event EventHandler<string> UpdateMemberFailed;
        event EventHandler<string> CreateMemberFailed;


        Task UpdateMember(MemberVm model);
        Task CreateMember(MemberVm model);
        void SelectMember(Guid id);
        void SelectNullMember();

    }
}
