using AutoMapper;
using Backend.Core.DtoModels;
using Backend.Core.Interfaces;
using Backend.Core.Repository;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace Backend.Controllers
{
    [Route("groups/[controller]")]
    [ApiController]
    public class FocusGroupController : Controller
    {
        private readonly IFocusGroupRepository _focusGroupRepository;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public FocusGroupController(IFocusGroupRepository focusGroupRepository, DataContext context, IMapper mapper)
        {
            _focusGroupRepository = focusGroupRepository;
            _dataContext = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FocusGroup>))]
        public IActionResult GetFocusGroups()
        {
            var focusGroups = _mapper.Map<List<FocusGroupDto>>(_focusGroupRepository.GetFocusGroups());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(focusGroups);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FocusGroup))]
        [ProducesResponseType(400)]
        public IActionResult GetFocusGroup(int id)
        {
            if (!_focusGroupRepository.FocusGroupExists(id))
            {
                return NotFound();
            }

            var focusGroup = _mapper.Map<FocusGroupDto>(_focusGroupRepository.GetFocusGroupById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(focusGroup);
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}/users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetGroupUsers(int id)
        {
            if (!_focusGroupRepository.FocusGroupExists(id))
            {
                return NotFound();
            }

            var users = _focusGroupRepository.GetGroupUsers(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult DeleteFocusGroup(int id)
        {
            if (!_focusGroupRepository.FocusGroupExists(id))
            {
                return NotFound();
            }

            _focusGroupRepository.DeleteFocusGroup(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_focusGroupRepository.Save())
            {
                return BadRequest(ModelState);
            }

            return Ok("Focus Group has been deleted: " + id);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("addmember")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddMemberToGroup([FromBody] AddMemberDto addMemberDto)
        {
            if (addMemberDto == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _focusGroupRepository.AddMemberToGroup(addMemberDto.UserId, addMemberDto.GroupId);

                if (!result)
                {
                    ModelState.AddModelError("Error:", "User already in group or does not meet the criteria.");
                    return StatusCode(422, ModelState);
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok("User successfully added to group.");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("removeuser/{groupId}/{userId}")]
        public IActionResult DeleteFromGroup(int groupId, int userId)
        {
            if (!_focusGroupRepository.DeleteFromGroup(groupId, userId))
            {
                return NotFound($"User with ID {userId} not found in group with ID {groupId}.");
            }

            return Ok($"User with ID {userId} was removed from group with ID {groupId}.");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        [ProducesResponseType(201, Type = typeof(CreateFocusGroupDto))]
        [ProducesResponseType(400)]
        public IActionResult AddFocusGroup([FromBody] CreateFocusGroupDto focusGroupDto)
        {
            if (focusGroupDto == null)
            {
                return BadRequest(ModelState);
            }

            var focusGroup = _mapper.Map<FocusGroup>(focusGroupDto);

            var result = _focusGroupRepository.AddFocusGroup(focusGroup);

            if (!result)
            {
                ModelState.AddModelError("", "Failed to add the focus group.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetFocusGroup", new { id = focusGroup.FocGrId }, focusGroup);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("edit/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult FocusGroupEdit(int id, [FromBody] UpdateFocusGroupDto updateFocusGroupDto)
        {
            if (updateFocusGroupDto == null)
            {
                return BadRequest(ModelState);
            }

            var result = _focusGroupRepository.UpdateFocusGroup(id, updateFocusGroupDto);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Focus Group has been updated");
        }
    }
}
