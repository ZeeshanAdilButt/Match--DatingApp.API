using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {

        public AuthController(IAuthRepository authRepository, IMapper mapper, IConfiguration configuration)
        {
            _authRepo = authRepository;
            _mapper = mapper;
            _config = configuration;
        }

        public IAuthRepository _authRepo { get; }
        public IMapper _mapper { get; }
        public IConfiguration _config { get; }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            //validate request
            userForRegisterDTO.userName = userForRegisterDTO.userName.ToLower();

            //check if user exits
            if (await _authRepo.UserExists(userForRegisterDTO.userName))
                return BadRequest("UserName already exists");

            //create user
            var userToCreate = _mapper.Map<User>(userForRegisterDTO);

            var createdUser = await _authRepo.Register(userToCreate, userForRegisterDTO.password);

            var userToReturn = _mapper.Map<UserForDetailDTO>(createdUser);

            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id}, userToReturn);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {

            var userFromRepo = await _authRepo.Login(userForLoginDTO.userName.ToLower(), userForLoginDTO.password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDTO>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });

        }
    }
}