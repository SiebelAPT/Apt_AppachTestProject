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
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateMemberCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateMemberCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _memberService.CreateMemberCommandHandler(command);

            return Created($"/api/members/{result.Payload.Id}", result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateMemberCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateMemberCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _memberService.UpdateMemberCommandHandler(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }            
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllMembersQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {            
            var result = await _memberService.GetAllMembersQueryHandler();

            return Ok(result);
        }
    }
}
