using AutoMapper;
using Backend.Core.DtoModels;
using Backend.Core.Interfaces;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Backend.Controllers
{
    
    [Route("session/[controller]")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public SessionController(ISessionRepository sessionRepository, DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Session>))]
        public IActionResult GetSessions()
        {
            var sessions = _mapper.Map<List<SessionDto>>(_sessionRepository.GetSessions());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(sessions);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("group/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Session>))]
        [ProducesResponseType(400)]
        public IActionResult GetSessionsByGroupId(int id)
        {
            var sessions = _mapper.Map<List<SessionDto>>(_sessionRepository.SessionsByGrouptId(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(sessions);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        [ProducesResponseType(201, Type = typeof(CreateSessionDto))]
        [ProducesResponseType(400)]
        public IActionResult AddSession([FromBody] CreateSessionDto sessionDto)
        {
            if (sessionDto == null)
            {
                return BadRequest(ModelState);
            }

            var session = _mapper.Map<Session>(sessionDto);

            // Конвертируем время в UTC перед сохранением
            if (session.StartTime.Kind == DateTimeKind.Unspecified)
            {
                session.StartTime = DateTime.SpecifyKind(session.StartTime, DateTimeKind.Utc);
            }
            else
            {
                session.StartTime = session.StartTime.ToUniversalTime();
            }

            if (session.EndTime.Kind == DateTimeKind.Unspecified)
            {
                session.EndTime = DateTime.SpecifyKind(session.EndTime, DateTimeKind.Utc);
            }
            else
            {
                session.EndTime = session.EndTime.ToUniversalTime();
            }

            if (!_sessionRepository.AddSession(session))
            {
                ModelState.AddModelError("", "Failed to add the session.");
                return StatusCode(500, ModelState);
            }

            return Ok("Session has been created");
        }

    }
}
