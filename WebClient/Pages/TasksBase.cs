using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
        protected TaskVm taskModel;
        protected TaskToDoModel[] paramTasksToDo { get; set; }

        #endregion

        #region Define Parameters

        [Parameter]
        public List<TaskVm> LoadedTasks { get; set; }

        [Parameter]
        public MemberVm SelectedMember { get; set; }

        [Inject]
        public ITaskDataService taskDataService { get; set; }

        [Inject]
        public IMemberDataService memberDataService { get; set; }

        #endregion

        protected override Task OnInitializedAsync()
        {
            paramTasksToDo = new TaskToDoModel[0];
            taskModel = taskDataService.SelectedTask ?? new TaskVm();

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
            taskModel = taskDataService.SelectedTask ?? new TaskVm();
            paramTasksToDo = taskDataService.PopulateLoadedTasks(taskDataService.EnumTasksToDo.ToList(), memberDataService.SelectedMember, memberDataService);
            
            UpdateTasks();
                        
            showTasks = true;
            isLoaded = true;
            isShowAllButton = true;
            isTaskListerShow = true;

            StateHasChanged();
        }

        #endregion

        #region Protected Methods

        protected async void OnAddTask()
        {
            if (taskModel == null) taskModel = new TaskVm();

            taskModel.Id = Guid.NewGuid();
            taskModel.Subject = newTask;
            taskModel.IsComplete = false;
            if (memberDataService != null && memberDataService.SelectedMember != null && memberDataService.SelectedMember.Id != Guid.Empty)
            {
                taskModel.AssignedToId = memberDataService.SelectedMember.Id;
            }
            else
                taskModel.AssignedToId = Guid.Empty;

            newTask = string.Empty;

            await taskDataService.CreateTask(taskModel);            
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

            paramTasksToDo = taskDataService.PopulateLoadedTasks(LoadedTasks, memberDataService.SelectedMember, memberDataService);
        }

        #endregion
    }
}
