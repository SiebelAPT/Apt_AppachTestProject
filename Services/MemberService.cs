using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.DataModels;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MemberService : IMemberService
    {
        #region Declare Variables

        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public MemberService(IMapper mapper, IMemberRepository memberRepository)
        {
            _mapper = mapper;
            _memberRepository = memberRepository;
        }

        #endregion

        #region Public Methods

        public async Task<CreateMemberCommandResult> CreateMemberCommandHandler(CreateMemberCommand command)
        {
            var member = _mapper.Map<MemberDm>(command);
            var persistedMember = await _memberRepository.CreateRecordAsync(member);

            var vm = _mapper.Map<MemberVm>(persistedMember);

            return new CreateMemberCommandResult()
            {
                Payload = vm
            };
        }
        
        public async Task<UpdateMemberCommandResult> UpdateMemberCommandHandler(UpdateMemberCommand command)
        {
            var isSucceed = true;
            var member = await _memberRepository.ByIdAsync(command.Id);

            _mapper.Map<UpdateMemberCommand,MemberDm>(command, member);
            
            var affectedRecordsCount = await _memberRepository.UpdateRecordAsync(member);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new UpdateMemberCommandResult() { 
               Succeed = isSucceed
            };
        }

        public async Task<GetAllMembersQueryResult> GetAllMembersQueryHandler()
        {
            IEnumerable<MemberVm> vm = new List<MemberVm>();

            var members = await _memberRepository.Reset().ToListAsync();

            if (members != null && members.Any())
                vm = _mapper.Map<IEnumerable<MemberVm>>(members);

            return new GetAllMembersQueryResult() { Payload = vm };
        }

        #endregion
    }
}
