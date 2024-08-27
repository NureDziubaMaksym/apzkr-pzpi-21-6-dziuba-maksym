using Microsoft.AspNetCore.Mvc;
using Backend.Core.Interfaces;
using Backend.Infrastructure.Models;
using Backend.Core.Repository;
using Backend.Core.DtoModels;
using Backend.Infrastructure.Data;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Backend.Controllers
{
    [Route("users/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, DataContext context, IMapper mapper)
        {
            _userRepository = userRepository;
            _dataContext = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound();
            }

            var user = _mapper.Map<UserDto>(_userRepository.GetUserById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult DeleteUser(int id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound();
            }

            _userRepository.DeleteUser(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.Save())
            {
                return BadRequest(ModelState);
            }

            return Ok("User has been deleted: " + id);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("bylogin/{login}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult GetUserByLogin(string login)
        {
            var user = _userRepository.GetUserByLogin(login);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }


    }
}
