using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskToDoController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskToDoController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTaskCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _taskService.CreateTaskCommandHandler(command);

            return Created($"/api/tasktodo/{result.Payload.Id}", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateTaskCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _taskService.UpdateTaskCommandHandler(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }            
        }

        [HttpPut("{taskId:Guid}/{assignToId:Guid}")]
        //[Route("AssignTask")]
        [ProducesResponseType(typeof(AssignTaskCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AssignTask(Guid taskId, Guid assignToId, AssignTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _taskService.AssignTaskCommandHandler(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }

        [HttpPut("{completeTaskId:Guid}/{isComplete:bool}")]
        //[Route("CompleteTask")]
        [ProducesResponseType(typeof(CompleteTaskCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompleteTask(Guid completeTaskId, bool isComplete, CompleteTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _taskService.CompleteTaskCommandHandler(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }

        [HttpPut("{memberTaskId}/{isComplete}/{memberId}")]
        [ProducesResponseType(typeof(CompleteMemberTaskCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompleteFamMemberTask(Guid memberTaskId, bool isComplete, Guid memberId, CompleteMemberTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _taskService.CompleteMemberTaskCommandHandler(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllTasksQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTasks()
        {            
            var result = await _taskService.GetAllTasksQueryHandler();

            return Ok(result);
        }        
    }
}
