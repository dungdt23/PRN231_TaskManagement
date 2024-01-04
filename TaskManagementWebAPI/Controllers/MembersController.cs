using BusinessObjects.DTOs;
using BusinessObjects.Requests;
using BusinessObjects.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories.IRepositories;
using Repositories.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace TaskManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private IMemberRepository _memberRepository;
        private JWTSettings _jwtSetting;
        private IConfiguration _configuration;
        public MembersController(IMemberRepository memberRepository, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _jwtSetting = configuration.GetSection("Jwt").Get<JWTSettings>();
            _configuration = configuration;
        }
        [HttpGet] 
        public IActionResult Get()
        {
            try
            {
                var mems = _memberRepository.GetMembers();
                return Ok(mems); 
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("{username}")]
        public IActionResult GetUser(string username)
        {
            try
            {
                var mem = _memberRepository.GetUser(username);
                return Ok(mem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Post(MemberRequest memberRequest)
        {
            try
            {
                _memberRepository.Create(memberRequest);
                return Ok(memberRequest);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, MemberRequest memberRequest)
        {
            try
            {
                _memberRepository.Update(id, memberRequest);
                return Ok(memberRequest);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _memberRepository.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var adminAcc = _configuration.GetSection("Admin").Get<LoginRequest>();
            var userLogin = _memberRepository.Login(request);
            if (userLogin != null || (string.Equals(request.Username, adminAcc.Username, StringComparison.CurrentCultureIgnoreCase) && request.Password == adminAcc.Password))
            {
                var token = GenerateJwtToken(userLogin);
                LoginResponse loginResponse = new LoginResponse();
                loginResponse.Token = token;
                return Ok(loginResponse);
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }
        private string GenerateJwtToken(MemberDTO? userLogin)
        {
            var adminAcc = _configuration.GetSection("Admin").Get<LoginRequest>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", userLogin == null ? adminAcc.Username : userLogin.Username) }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.DurationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = "dungdt",
                Issuer = "dungdt"
            };
            if (userLogin == null)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            else
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Leader"));
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpGet("GetMembersOfTeam/{teamId}")]
        public async Task<IActionResult> GetMembersOfTeam(int teamId)
        {
            try
            {
                var mems = _memberRepository.GetMembersOfTeam(teamId);
                return Ok(mems);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
