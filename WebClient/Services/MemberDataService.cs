using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebClient.Abstractions;
using Microsoft.AspNetCore.Components;
using Domain.ViewModel;
using Core.Extensions.ModelConversion;

namespace WebClient.Services
{
    public class MemberDataService : IMemberDataService
    {
        #region Define Private Variables

        private readonly HttpClient httpClient;
        private IEnumerable<MemberVm> members;

        #endregion

        #region Define Public Variables

        public IEnumerable<MemberVm> Members => members;

        public MemberVm SelectedMember { get; set; }

        public event EventHandler MembersChanged;
        public event EventHandler<string> UpdateMemberFailed;
        public event EventHandler<string> CreateMemberFailed;
        public event EventHandler SelectedMemberChanged;

        #endregion

        #region Constructor

        public MemberDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            members = new List<MemberVm>();
            LoadMembers();
        }

        #endregion

        #region Private Methods

        private async void LoadMembers()
        {
            members = (await GetAllMembers()).Payload;
            MembersChanged?.Invoke(this, null);
        }

        private async Task<GetAllMembersQueryResult> GetAllMembers()
        {
            return await httpClient.GetJsonAsync<GetAllMembersQueryResult>("members");
        }

        private async Task<CreateMemberCommandResult> Create(CreateMemberCommand command)
        {
            return await httpClient.PostJsonAsync<CreateMemberCommandResult>("members", command);
        }

        private async Task<UpdateMemberCommandResult> Update(UpdateMemberCommand command)
        {
            return await httpClient.PutJsonAsync<UpdateMemberCommandResult>($"members/{command.Id}", command);
        }

        #endregion

        #region Public Methods

        public async Task UpdateMember(MemberVm model)
        {
            var result = await Update(model.ToUpdateMemberCommand());

            Console.WriteLine(JsonSerializer.Serialize(result));

            if(result != null)
            {
                var updatedList = (await GetAllMembers()).Payload;

                if(updatedList != null)
                {
                    members = updatedList;
                    MembersChanged?.Invoke(this, null);
                    return;
                }
                UpdateMemberFailed?.Invoke(this, "The save was successful, but we can no longer get an updated list of members from the server.");
            }

            UpdateMemberFailed?.Invoke(this, "Unable to save changes.");
        }

        public async Task CreateMember(MemberVm model)
        {
            var result = await Create(model.ToCreateMemberCommand());

            if (result != null)
            {
                var updatedList = (await GetAllMembers()).Payload;

                if (updatedList != null)
                {
                    members = updatedList;
                    MembersChanged?.Invoke(this, null);
                    return;
                }
                UpdateMemberFailed?.Invoke(this, "The creation was successful, but we can no longer get an updated list of members from the server.");
            }

            UpdateMemberFailed?.Invoke(this, "Unable to create record.");
        }

        public void SelectMember(Guid id)
        {
            if (members.All(memberVm => memberVm.Id != id)) return;
            {
                SelectedMember = members.SingleOrDefault(memberVm => memberVm.Id == id);
                SelectedMemberChanged?.Invoke(this, null);
            }
        }

        public void SelectNullMember()
        {
            SelectedMember = null;
            SelectedMemberChanged?.Invoke(this, null);
        }

        #endregion
    }
}
