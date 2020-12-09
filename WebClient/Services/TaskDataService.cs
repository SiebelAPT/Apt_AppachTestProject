using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Extensions.ModelConversion;
using Microsoft.AspNetCore.Components;
using WebClient.Abstractions;
using WebClient.Shared.Models;
using WebClient.Services;
using Domain.ViewModel;
using Domain.Commands;
using Domain.Queries;

namespace WebClient.Services
{
    public class TaskDataService: ITaskDataService
    {
        #region Define Private Variables

        private readonly HttpClient httpClient;
        private IEnumerable<TaskVm> tasksToDo;
        private Boolean memberTaskCompleteValue = true;

        #endregion

        #region Define Public Variables

        public IEnumerable<TaskVm> EnumTasksToDo => tasksToDo;
        public TaskVm SelectedTask { get; set; }

        #endregion

        #region Public Eventhandlers

        public event EventHandler TasksChanged;
        public event EventHandler TaskSelected;
        public event EventHandler SelectedTaskChanged;
        public event EventHandler ShowAllTaskClicked;
        public event EventHandler<string> UpdateTaskFailed;
        public event EventHandler<string> CreateTaskFailed;
        public event EventHandler<string> GetTasksFailed;

        #endregion

        #region Constructor

        public TaskDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            tasksToDo = new List<TaskVm>();
            LoadTasks();
        }

        #endregion

        #region Private Methods

        private async void LoadTasks()
        {
            tasksToDo = (await GetAllTasks()).Payload;            
            TasksChanged?.Invoke(this, null);
        }

        private async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasktodo");
        }

        private async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await httpClient.PostJsonAsync<CreateTaskCommandResult>("tasktodo", command);
        }

        private async Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command)
        {
            return await httpClient.PutJsonAsync<UpdateTaskCommandResult>($"tasktodo/{command.Id}", command);
        }

        private async Task<AssignTaskCommandResult> Assign(AssignTaskCommand command)
        {
            return await httpClient.PutJsonAsync<AssignTaskCommandResult>($"tasktodo/{command.Id}&{command.AssignedToId}", command);
        }

        private async Task<CompleteTaskCommandResult> Complete(CompleteTaskCommand command)
        {
            return await httpClient.PutJsonAsync<CompleteTaskCommandResult>($"tasktodo/{command.Id}&{"true"}", command);
        }

        private async Task<CompleteMemberTaskCommandResult> CompleteMemTask(CompleteMemberTaskCommand command)
        {
            memberTaskCompleteValue = true;
            return await httpClient.PutJsonAsync<CompleteMemberTaskCommandResult>($"tasktodo/{command.Id}&{memberTaskCompleteValue}&{command.AssignedToId}", command);
        }        

        #endregion

        #region Public Methods

        public void SelectTask(Guid id)
        {
            if (tasksToDo.All(taskVm => taskVm.Id != id)) return;
            {
                SelectedTask = tasksToDo.SingleOrDefault(taskVm => taskVm.Id == id);
                SelectedTaskChanged?.Invoke(this, null);
            }
        }

        public void ToggleTask(Guid id)
        {
            foreach (var taskToDo in tasksToDo)
            {
                if (taskToDo.Id == id)
                {
                    taskToDo.IsComplete = !taskToDo.IsComplete;
                }
            }

            TasksChanged?.Invoke(this, null);
        }

        public void AddTask(TaskVm model)
        {
            tasksToDo.Append(model);
            TasksChanged?.Invoke(this, null);
        }

        public async Task CreateTask(TaskVm model)
        {
            var result = await Create(model.ToCreateTaskCommand());

            if (result != null)
            {
                var updatedTasks = (await GetAllTasks()).Payload;

                if (updatedTasks != null)
                {
                    tasksToDo = updatedTasks;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                
                UpdateTaskFailed?.Invoke(this, "The creation was successful, but we can no longer get an updated list of tasks from the server.");
            }

            UpdateTaskFailed?.Invoke(this, "Unable to create record.");
        }
        
        public async Task UpdateTask(TaskVm model)
        {
            var result = await Update(model.ToUpdateTaskCommand());

            Console.WriteLine(JsonSerializer.Serialize(result));

            if (result != null)
            {
                var updatedTasks = (await GetAllTasks()).Payload;

                if (updatedTasks != null)
                {
                    tasksToDo = updatedTasks;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                
                UpdateTaskFailed?.Invoke(this, "The save was successful, but we can no longer get an updated list of tasks from the server.");
            }
            
            UpdateTaskFailed?.Invoke(this, "Unable to save changes.");
        }

        public async Task AssignTask(Guid id, Guid assignToMemberId, TaskVm model)
        {
            model.Id = id;
            model.AssignedToId = assignToMemberId;

            var result = await Assign(model.ToAssignTaskCommand());

            if (result != null)
            {
                var updatedTasks = (await GetAllTasks()).Payload;

                if (updatedTasks != null)
                {
                    tasksToDo = updatedTasks;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                
                UpdateTaskFailed?.Invoke(this, "The task assignment was successful, but we can no longer get an updated list of member wise tasks from the server.");
            }
            
            UpdateTaskFailed?.Invoke(this, "Unable to assign task(s).");
        }

        public async Task CompleteTask(Guid id, bool isComplete, TaskVm model)
        {
            model.Id = id;
            model.IsComplete = isComplete;

            var result = await Complete(model.ToCompleteTaskCommand());

            Console.WriteLine(JsonSerializer.Serialize(result));

            if (result != null)
            {
                var updatedTasks = (await GetAllTasks()).Payload;

                if (updatedTasks != null)
                {
                    tasksToDo = updatedTasks;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                
                UpdateTaskFailed?.Invoke(this, "The save was successful, but we can no longer get an updated list of tasks from the server.");
            }
            
            UpdateTaskFailed?.Invoke(this, "Unable to save changes.");
        }

        public async Task CompleteMemberTask(Guid id, bool isComplete, Guid assignToMemberId, TaskVm model)
        {
            model.Id = id;
            model.IsComplete = isComplete;
            model.AssignedToId = assignToMemberId;

            var result = await CompleteMemTask(model.ToCompleteMemberTaskCommand());

            Console.WriteLine(JsonSerializer.Serialize(result));

            if (result != null)
            {
                var updatedTasks = (await GetAllTasks()).Payload;

                if (updatedTasks != null)
                {
                    tasksToDo = updatedTasks;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                
                UpdateTaskFailed?.Invoke(this, "The save was successful, but we can no longer get an updated list of tasks from the server.");
            }
            
            UpdateTaskFailed?.Invoke(this, "Unable to save changes.");
        }

        public async Task GetTasks()
        {
            var result = await GetAllTasks();

            Console.WriteLine(JsonSerializer.Serialize(result));

            if (result != null)
            {
                var updatedList = result.Payload;

                if (updatedList != null)
                {
                    tasksToDo = updatedList;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                
                GetTasksFailed?.Invoke(this, "Can no longer get an updated list of tasks from the server.");
            }
            
            GetTasksFailed?.Invoke(this, "Unable to get the tasks.");            
        }

        public TaskToDoModel[] PopulateLoadedTasks(List<TaskVm> loadedTasks, MemberVm selectedMember, IMemberDataService loadedMembers)
        {
            string emptyAvatar = "White";
            TaskToDoModel[] currTaskArray = new TaskToDoModel[0];

            if (loadedTasks != null && loadedTasks.Count > 0)
            {
                currTaskArray = new TaskToDoModel[loadedTasks.Count];
                int taskPosition = 0;

                foreach (TaskVm famTask in loadedTasks)
                {
                    TaskToDoModel currTask = new TaskToDoModel();

                    currTask.Id = famTask.Id;
                    currTask.Subject = famTask.Subject;
                    currTask.IsComplete = famTask.IsComplete;
                    currTask.AssignedToId = famTask.AssignedToId;

                    if (selectedMember != null && selectedMember.Id == famTask.AssignedToId)
                    {
                        currTask.Avatar = (!string.IsNullOrEmpty(selectedMember.Avatar)) ? selectedMember.Avatar : emptyAvatar;
                    }
                    else
                        currTask.Avatar = (famTask.AssignedToId != Guid.Empty) ? GetMemberAvatar(famTask.AssignedToId, loadedMembers) : emptyAvatar;

                    currTaskArray[taskPosition] = currTask;

                    taskPosition++;

                }
            }

            return currTaskArray;
        }

        private string GetMemberAvatar(Guid memberId, IMemberDataService memDataService)
        {
            string avtr = "White";

            if (memDataService != null && memDataService.Members.Count() > 0)
            {
                foreach (MemberVm mem in memDataService.Members)
                {
                    if (mem.Id == memberId)
                    {
                        avtr = mem.Avatar;
                        break;
                    }
                }
            }

            return avtr;
        }

        public void SelectNullTask()
        {
            SelectedTask = null;
            SelectedTaskChanged?.Invoke(this, null);
        }

        #endregion
    }
}