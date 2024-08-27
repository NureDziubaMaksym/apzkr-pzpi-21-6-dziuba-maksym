using AutoMapper;
using Backend.Core.DtoModels;
using Backend.Core.Interfaces;
using Backend.Core.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

namespace Backend.Controllers
{
    [Route("result/[controller]")]
    [ApiController]
    public class ResultController : Controller
    {
        private readonly IResultRepository _resultRepository;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ApiConnectionService _connectionService;

        public ResultController(IResultRepository resultRepository, DataContext context, IMapper mapper, ApiConnectionService apiConnectionService)
        {
            _resultRepository = resultRepository;
            _dataContext = context;
            _mapper = mapper;
            _connectionService = apiConnectionService;

        }

        [Authorize(Roles = "admin")]
        [HttpGet("session/{sessionId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Result>))]
        [ProducesResponseType(400)]
        public IActionResult GetResultsBySession(int sessionId)
        {
            var results = _mapper.Map<List<ResultDto>>(_resultRepository.ResulsBySession(sessionId));

            if (results == null || results.Count == 0)
            {
                return NotFound($"No results found for session with ID {sessionId}.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(results);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("content/{contentId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Result>))]
        [ProducesResponseType(400)]
        public IActionResult GetResultsByContent(int contentId)
        {
            var results = _mapper.Map<List<ResultDto>>(_resultRepository.ResultByContent(contentId));

            if (results == null || results.Count == 0)
            {
                return NotFound($"No results found for content with ID {contentId}.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(results);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("connectAndSave/{sessionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConnectAndSaveEmotion(int sessionId)
        {
            var sessionExists = _dataContext.Sessions.Any(s => s.SessionId == sessionId);

            if (!sessionExists)
            {
                return NotFound($"Session with ID {sessionId} not found.");
            }

            await _connectionService.ConnectAndSaveEmotionAsync(sessionId);

            return Ok($"Started listening to MQTT broker for session ID {sessionId}.");
        }


    }
}
