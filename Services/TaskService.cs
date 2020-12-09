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
    public class TaskService : ITaskService
    {
        #region Declare Variables

        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _taskMapper;
        private const Boolean taskIsCompleteValue = true;

        #endregion

        #region Constructor

        public TaskService(IMapper mapper, ITaskRepository taskRepository)
        {
            _taskMapper = mapper;
            _taskRepository = taskRepository;
        }

        #endregion

        #region Public Methods

        public async Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command)
        {
            var task = _taskMapper.Map<TaskDm>(command);
            var createdTask = await _taskRepository.CreateRecordAsync(task);

            var vm = _taskMapper.Map<TaskVm>(createdTask);

            return new CreateTaskCommandResult()
            {
                Payload = vm
            };
        }
        
        public async Task<UpdateTaskCommandResult> UpdateTaskCommandHandler(UpdateTaskCommand command)
        {
            var isSucceed = true;
            var createdTask = await _taskRepository.ByIdAsync(command.Id);

            _taskMapper.Map<UpdateTaskCommand, TaskDm>(command, createdTask);
            
            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(createdTask);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new UpdateTaskCommandResult() { 
               Succeed = isSucceed
            };
        }

        public async Task<AssignTaskCommandResult> AssignTaskCommandHandler(AssignTaskCommand command)
        {
            var isSucceed = true;

            var task = await _taskRepository.ByIdAsync(command.Id);
            task.Id = command.Id;
            task.AssignedToId = command.AssignedToId;

            _taskMapper.Map<AssignTaskCommand, TaskDm>(command, task);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task);

            if (affectedRecordsCount < 1) isSucceed = false;

            return new AssignTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }

        public async Task<CompleteTaskCommandResult> CompleteTaskCommandHandler(CompleteTaskCommand command)
        {
            var isSucceed = true;

            var complettedTask = await _taskRepository.ByIdAsync(command.Id);
            complettedTask.IsComplete = taskIsCompleteValue;

            _taskMapper.Map<CompleteTaskCommand, TaskDm>(command, complettedTask);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(complettedTask);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new CompleteTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }

        public async Task<CompleteMemberTaskCommandResult> CompleteMemberTaskCommandHandler(CompleteMemberTaskCommand command)
        {
            var isSucceed = true;

            var complettedTask = await _taskRepository.ByIdAsync(command.Id);
            complettedTask.Id = command.Id;
            complettedTask.AssignedToId = command.AssignedToId;
            complettedTask.IsComplete = taskIsCompleteValue;

            //var comlettedTask = await _taskRepository.UpdateAsync(task);

            _taskMapper.Map<CompleteMemberTaskCommand, TaskDm>(command, complettedTask);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(complettedTask);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new CompleteMemberTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            IEnumerable<TaskVm> vm = new List<TaskVm>();

            var tasks = await _taskRepository.Reset().ToListAsync();

            if (tasks != null && tasks.Any())
                vm = _taskMapper.Map<IEnumerable<TaskVm>>(tasks);

            return new GetAllTasksQueryResult() { Payload = vm };
        }        

        #endregion        
    }
}
