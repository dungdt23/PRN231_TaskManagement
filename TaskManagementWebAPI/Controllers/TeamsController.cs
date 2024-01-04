using BusinessObjects.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using Repositories.Repositories;

namespace TaskManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private ITeamRepository _teamRepository;
        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        [HttpGet("Detail/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var teams = _teamRepository.GetMembersOfTeam(id);
                return Ok(teams);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("Project/{id}")]
        public IActionResult GetByProjectID(int id)
        {
            try
            {
                var teams = _teamRepository.GetTeams(id);
                return Ok(teams);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var teams = _teamRepository.GetTeams();
                return Ok(teams);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public IActionResult Post(TeamRequest teamRequest)
        {
            try
            {
                _teamRepository.Create(teamRequest);
                return Ok(teamRequest);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("AddMemberToTeam/{email}")]
        public IActionResult AddMemberToTeam(string email, UserTeamRequest userTeamRequest)
        {
            try
            {
                _teamRepository.AddMemberToTeam(email, userTeamRequest);
                return Ok(userTeamRequest);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("RemoveMemberFromTeam/{userTeamId}")]
        public IActionResult RemoveMemberFromTeam(int userTeamId)
        {
            try
            {
                _teamRepository.RemoveMemberFromTeam(userTeamId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                _teamRepository.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Put(int id, TeamRequest teamRequest)
        {
            try
            {
                _teamRepository.Update(id, teamRequest);
                return Ok(teamRequest);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("SetTeamLeader/{id}/{teamId}"), Authorize(Roles = "Admin")]
        public IActionResult SetTeamLeader(int id, int teamId)
        {
            try
            {
                _teamRepository.SetTeamLeader(id, teamId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("TeamsOfUser/{userId}")]
        public IActionResult GetTeamsOfUser(int userId)
        {
            try
            {
                var teams = _teamRepository.GetTeamsOfUser(userId);
                return Ok(teams);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("GetTeamByTask/{taskId}")]
        public IActionResult GetTeamByTask(int taskId)
        {
            try
            {
                var team = _teamRepository.GetTeamByTaskId(taskId);
                return Ok(team);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
