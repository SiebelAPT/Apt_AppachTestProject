using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain.ViewModel;
using Domain.ClientSideModels;
using WebClient.Abstractions;
using WebClient.Shared.Models;
using Core.Extensions.ModelConversion;

namespace WebClient.Shared.Components
{
    public class LeftMenuBase: ComponentBase
    {
        #region Inject Services Interfaces

        [Inject]
        public ITaskDataService taskDataService { get; set; }

        [Inject]
        public IMemberDataService memberDataService { get; set; }

        #endregion

        #region Declare Parameters

        [Parameter]
        public Domain.ClientSideModels.MenuItem[] menuItems { get; set; }

        [Parameter]
        public Boolean showAllTaskButton { get; set; } = true;

        [Parameter]
        public EventCallback<MouseEventArgs> OnShowAllTaskClicked { get; set; }

        #endregion

        protected override Task OnInitializedAsync()
        {
            menuItems = memberDataService.Members.ToMenuItems();

            memberDataService.MembersChanged += MemberService_MembersChanged;
            memberDataService.SelectedMemberChanged += MemberService_SelectedMemberChanged;

            if (memberDataService.SelectedMember != null)
            {
                SetActiveItem(memberDataService.SelectedMember.Id);
            }

            return base.OnInitializedAsync();
        }

        #region Declaration of EvendHandlers

        protected async Task ShowAllTaskClicked(MouseEventArgs e)
        {
            await OnShowAllTaskClicked.InvokeAsync(e);
        }

        private void MemberService_SelectedMemberChanged(object sender, EventArgs e)
        {
            InactivateAllItems();
            if (memberDataService.SelectedMember != null) SetActiveItem(memberDataService.SelectedMember.Id);
            StateHasChanged();
        }

        private void MemberService_MembersChanged(object sender, EventArgs e)
        {
            menuItems = memberDataService.Members.ToMenuItems();

            /*"ShowAllTaskClicked" is Called here Forcefully to enrure showing of Relationship (Avatars) of the Tasks with the 
             Members. As the members are getting loaded later than the loading of the Tasks due, to async Calls*/
            MouseEventArgs evnt = new MouseEventArgs();
            evnt.Button = 0;
            ShowAllTaskClicked(evnt);

            StateHasChanged();
        }

        #endregion

        #region Private Methods

        private void InactivateAllItems()
        {
            foreach (var menuItem in menuItems)
            {
                menuItem.isActive = false;
            }
        }

        private void SetActiveItem(Guid id)
        {
            foreach (var menuItem in menuItems)
            {
                if (menuItem.referenceId == id)
                {
                    menuItem.isActive = true;
                }
            }
        }

        #endregion

        #region Protected Methods

        protected void SelectMember(Guid id)
        {
            memberDataService.SelectMember(id);
        }

        protected void OnAddItem()
        {
            memberDataService.SelectNullMember();
        }

        #endregion
    }
}
