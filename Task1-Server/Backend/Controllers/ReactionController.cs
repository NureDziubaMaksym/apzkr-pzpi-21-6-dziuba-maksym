using AutoMapper;
using Backend.Core.DtoModels;
using Backend.Core.Interfaces;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace Backend.Controllers
{
    [Route("reactions/[controller]")]
    [ApiController]
    public class ReactionController : Controller
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ReactionController(IReactionRepository reactionRepository, DataContext context, IMapper mapper)
        {
            _dataContext = context;
            _reactionRepository = reactionRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reaction>))]
        public IActionResult GetReactions()
        {
            var reactions = _mapper.Map<List<ReactionDto>>(_reactionRepository.GetReactions());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reactions == null || reactions.Count == 0)
            {
                return NotFound("No reactions found.");
            }

            return Ok(reactions);
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}")]
        public IActionResult GetReaction(int id)
        {
            var reaction = _reactionRepository.GetReactionById(id);

            if (reaction == null)
            {
                return NotFound();
            }

            var reactionDto = _mapper.Map<ReactionDto>(reaction);
            return Ok(reactionDto);
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("user/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reaction>))]
        [ProducesResponseType(400)]
        public IActionResult GetReactionsByUserId(int id)
        {
            var reactions = _mapper.Map<List<ReactionDto>>(_reactionRepository.ReactionsByUserId(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reactions == null || reactions.Count == 0)
            {
                return NotFound($"No reactions found for user with ID {id}.");
            }

            return Ok(reactions);
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("content/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reaction>))]
        [ProducesResponseType(400)]
        public IActionResult GetReactionsByContentId(int id)
        {
            var reactions = _mapper.Map<List<ReactionDto>>(_reactionRepository.ReactionsByContentId(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reactions == null || reactions.Count == 0)
            {
                return NotFound($"No reactions found for content with ID {id}.");
            }

            return Ok(reactions);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("group/{groupId}/content/{contentId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reaction>))]
        [ProducesResponseType(400)]
        public IActionResult GetReactionsByGroup(int groupId, int contentId)
        {
            var reactions = _mapper.Map<List<ReactionDto>>(_reactionRepository.ReactionsByGroup(groupId, contentId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reactions == null || reactions.Count == 0)
            {
                return NotFound($"No reactions found for group with ID {groupId} and content with ID {contentId}.");
            }

            return Ok(reactions);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteReaction(int id)
        {
            if (!_reactionRepository.ReactionExsists(id))
            {
                return NotFound();
            }

            _reactionRepository.ReactionDelete(id);

            if (!_reactionRepository.Save())
            {
                return BadRequest();
            }

            return Ok("Reaction has been deleted: " + id);
        }

        [Authorize(Roles = "user,admin")]
        [HttpPost("add")]
        [ProducesResponseType(201, Type = typeof(CreateReactionDto))]
        [ProducesResponseType(400)]
        public IActionResult AddReaction([FromBody] CreateReactionDto createReactionDto)
        {
            if (createReactionDto == null)
            {
                return BadRequest(ModelState);
            }

            var reaction = _mapper.Map<Reaction>(createReactionDto);

            if (!_reactionRepository.AddReaction(reaction))
            {
                ModelState.AddModelError("", "Failed to add the reaction.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetReaction", new { id = reaction.ReactionId }, reaction);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("update/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PatchReaction(int id, [FromBody] UpdateReactionDto updateReactionDto)
        {
            if (updateReactionDto == null)
            {
                return BadRequest(ModelState);
            }

            var result = _reactionRepository.UpdateReaction(id, updateReactionDto);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Content has been added");
        }

    }
}
