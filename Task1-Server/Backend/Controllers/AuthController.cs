using Backend.AuthModels;
using Backend.Core.DtoModels;
using Backend.Core.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Controllers
{
    [Route("auth/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordService _passwordService;
        private readonly IdGenService _idGenService;

        public AuthController(DataContext context, IConfiguration configuration, PasswordService passwordService, IdGenService idGenService)
        {
            _context = context;
            _configuration = configuration;
            _passwordService = passwordService;
            _idGenService = idGenService;
        }

        [HttpPost]
        [Route("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Login == model.Login);

            if (user == null || user.Role != "admin" || !_passwordService.VerifyPasswordHash(model.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid credentials or not an admin." });
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("login-user")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Login == model.Login);

            if (user == null || !_passwordService.VerifyPasswordHash(model.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = _context.Users.Any(u => u.Login == model.Login);
            if (userExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                Email = model.Email,
                Login = model.Login,
                Password = _passwordService.CreatePasswordHash(model.Password),
                Role = "User",
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                Race = model.Race,
                Gender = model.Gender
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = _context.Users.Any(u => u.Login == model.Login);
            if (userExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            // Генерация нового UserId с использованием IdGenService
            int newUserId = _idGenService.GenerateNewId<User>();

            User user = new User()
            {
                UserId = newUserId, // Устанавливаем сгенерированный Id
                Email = model.Email,
                Login = model.Login,
                Password = _passwordService.CreatePasswordHash(model.Password),
                Role = "admin",
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                Race = model.Race,
                Gender = model.Gender
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Status = "Success", Message = "Admin created successfully!" });
        }


        [HttpPost]
        [Route("debug-check-password")]
        public IActionResult DebugCheckPassword([FromBody] PasswordCheckModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserId == model.UserId);
            if (user == null)
            {
                return NotFound(new { Status = "Error", Message = "User not found!" });
            }

            // Хешируем введенный пароль
            var enteredPasswordHash = _passwordService.CreatePasswordHash(model.Password);

            // Получаем сохраненный хеш из базы данных
            var storedPasswordHash = user.Password;

            // Возвращаем оба хеша для отладки
            return Ok(new
            {
                EnteredPasswordHash = enteredPasswordHash,
                StoredPasswordHash = storedPasswordHash,
                Match = enteredPasswordHash == storedPasswordHash
            });
        }

        [HttpPost]
        [Route("encrypt-all-passwords")]
        public IActionResult EncryptAllPasswords()
        {
            _passwordService.EncryptAll();
            return Ok(new { Status = "Success", Message = "All passwords have been re-encrypted successfully!" });
        }

        [HttpPost]
        [Route("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var username = User.Identity.Name;
            var user = _context.Users.SingleOrDefault(u => u.Login == username);

            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }

            if (!_passwordService.VerifyPasswordHash(model.CurrentPassword, user.Password))
            {
                return Unauthorized(new { message = "Current password is incorrect." });
            }

            user.Password = _passwordService.CreatePasswordHash(model.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password successfully changed." });
        }


    }
}
