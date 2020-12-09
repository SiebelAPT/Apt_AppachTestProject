using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Domain.ViewModel;
using WebClient.Abstractions;
using WebClient.Components;
using WebClient.Shared.Components;
using Domain.ClientSideModels;
using Core.Extensions.ModelConversion;

namespace WebClient.Pages
{
    public class MembersBase: ComponentBase
    {       
        protected List<MemberVm> members = new List<MemberVm>();
        protected List<MenuItem> leftMenuItem = new List<MenuItem>();

        protected bool showCreator;
        protected bool isLoaded;
        protected bool isShowAlTaskBtn;

        [Inject]
        protected IMemberDataService MemberDataService { get; set; }

        protected override Task OnInitializedAsync()
        {
            showCreator = true;
            isLoaded = true;
            isShowAlTaskBtn = false;


            UpdateMembers();
            ReloadMenu();

            MemberDataService.MembersChanged += MemberDataService_MembersChanged;
            
            return base.OnInitializedAsync();
        }

        private void MemberDataService_MembersChanged(object sender, EventArgs e)
        {
            showCreator = true;
            isLoaded = true;

            UpdateMembers();
            ReloadMenu();

            StateHasChanged();
        }

        void UpdateMembers()
        {
            var result = MemberDataService.Members;

            if (result.Any())
            {
                members = result.ToList();
            }
        }

        void ReloadMenu()
        {
            for (int i = 0; i < members.Count; i++)
            {
                leftMenuItem.Add(new MenuItem
                {
                    iconColor = members[i].Avatar,
                    label = members[i].FirstName,
                    referenceId = members[i].Id
                });
            }
        }
       
        protected void onAddItem()
        {
            showCreator = true;
            isLoaded = true;

            StateHasChanged();
        }

        protected void onMemberAdd(MemberVm familyMember)
        {
            MemberDataService.CreateMember(familyMember);            
        }

    }
}
