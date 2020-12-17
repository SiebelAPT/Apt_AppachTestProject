using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain.ViewModel;
using WebClient.Abstractions;
using WebClient.Shared.Models;
using WebClient.Components;
using WebClient.Shared.Components;

namespace WebClient.Pages
{
    public class TasksBase: ComponentBase
    {
        #region Protected Variables

        protected bool isFirstRender;
        protected bool showTasks;
        protected string newTask;
        protected bool isLoaded;
        protected bool isShowAllButton { get; set; } = true;
        protected bool isTaskListerShow { get; set; }
        protected TaskVm taskViewModel;
        protected TaskToDoModel[] familyTasksToDo { get; set; }

        #endregion

        #region Define Parameters

        [Parameter]
        public List<TaskVm> LoadedTasks { get; set; }

        [Parameter]
        public MemberVm SelectedMember { get; set; }

        [Parameter]
        public TaskVm SelectedTask { get; set; }

        [Inject]
        public ITaskDataService taskDataService { get; set; }

        [Inject]
        public IMemberDataService memberDataService { get; set; }

        #endregion

        protected override Task OnInitializedAsync()
        {
            familyTasksToDo = new TaskToDoModel[0];
            taskViewModel = taskDataService.SelectedTask ?? new TaskVm();

            UpdateTasks();
            
            taskDataService.TasksChanged += TaskDataService_TasksChanged;

            showTasks = true;
            isLoaded = true;
            isShowAllButton = true;
            isTaskListerShow = true;

            return base.OnInitializedAsync();
        }

        #region Declare EventHandlers

        protected void TaskDataService_TasksChanged(object sender, EventArgs e)
        {
            showTasks = true;
            isLoaded = true;
            isShowAllButton = true;
            isTaskListerShow = true;

            taskViewModel = taskDataService.SelectedTask ?? new TaskVm();
            familyTasksToDo = taskDataService.PopulateLoadedTasks(taskDataService.EnumTasksToDo.ToList(), memberDataService.SelectedMember, memberDataService);
            
            UpdateTasks();
                        
            StateHasChanged();
        }

        #endregion

        #region Protected Methods

        protected async void OnAddTask()
        {
            if (taskViewModel == null) taskViewModel = new TaskVm();

            taskViewModel.Id = Guid.NewGuid();
            taskViewModel.Subject = newTask;
            taskViewModel.IsComplete = false;
            if (memberDataService != null && memberDataService.SelectedMember != null && memberDataService.SelectedMember.Id != Guid.Empty)
            {
                taskViewModel.AssignedToId = memberDataService.SelectedMember.Id;
            }
            else
                taskViewModel.AssignedToId = Guid.Empty;

            newTask = string.Empty;

            await taskDataService.CreateTask(taskViewModel);            
        }

        protected void UpdateTasks()
        {
            var result = taskDataService.EnumTasksToDo;

            if (result.Any())
            {
                LoadedTasks = result.ToList();
            }            
        }

        protected void LeftMenuAllTaskButtonClicked()
        {
            if (LoadedTasks == null || LoadedTasks.Count <= 0) UpdateTasks();

            familyTasksToDo = taskDataService.PopulateLoadedTasks(LoadedTasks, memberDataService.SelectedMember, memberDataService);
        }

        protected async void SaveCompleteTask()
        {
            if (taskDataService == null || taskDataService.SelectedTask == null) return;

            taskViewModel = taskDataService.SelectedTask;            

            await taskDataService.CompleteTask(taskViewModel);            
        }

        #endregion
    }
}
