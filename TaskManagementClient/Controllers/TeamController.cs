using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementClient.Controllers
{
    public class TeamController : Controller
    {
        private readonly HttpClient _httpClient = null;
        private string TeamApiUrl = null;
        private string ProjectApiUrl = null;
        private string MemberApiUrl = null;
        public TeamController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            TeamApiUrl = "http://localhost:5245/api/Teams";
            ProjectApiUrl = "http://localhost:5245/api/Projects";
            MemberApiUrl = "http://localhost:5245/api/Members";
        }
        public async Task<IActionResult> IndexAsync(int? projectID)
        {
            string token = HttpContext.Session.GetString("token");
            if (String.IsNullOrEmpty(token))
            {
                return View();

            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
                ViewData["role"] = role;

            }
            if (projectID.HasValue)
            {
                await GetDataAsync(projectID.Value);
            }
            else
            {
                await GetDataAsync(null);
            }
            return View();
        }
        public async Task<IActionResult> DetailAsync(int teamId)
        {
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            ViewData["role"] = role;
            List<UserTeamDTO> userteams = await GetDetailsOfTeam(teamId);
            ViewData["userteams"] = userteams;
            ViewData["teamId"] = teamId;
            return View();
        }
        public async Task GetDataAsync(int? projectID)
        {
            if (projectID.HasValue)
            {
                List<TeamDTO> teams = await GetTeams(projectID.Value);
                ViewData["teams"] = teams;
            }
            else
            {
                List<TeamDTO> teams = await GetTeams();
                ViewData["teams"] = teams;
            }
            List<ProjectDTO> projects = await GetProjects();
            ViewData["projects"] = projects;
        }
        private async Task<List<TeamDTO>> GetTeams()
        {
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            ViewData["role"] = role;
            if (role.Equals("Admin"))
            {
                HttpResponseMessage response = await _httpClient.GetAsync(TeamApiUrl);
                string strData = await response.Content.ReadAsStringAsync();
                var teams = JsonConvert.DeserializeObject<List<TeamDTO>>(strData);
                return teams;
            }
            else
            {
                //get username from api, based on username
                HttpResponseMessage responseUser = await _httpClient.GetAsync($"{MemberApiUrl}/{username}");
                string strDataUser = await responseUser.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<MemberDTO>(strDataUser);
                HttpResponseMessage response = await _httpClient.GetAsync($"{TeamApiUrl}/TeamsOfUser/{user.UserId}");
                string strData = await response.Content.ReadAsStringAsync();
                var teams = JsonConvert.DeserializeObject<List<TeamDTO>>(strData);
                return teams;
            }
        }
        //get team by projectID
        private async Task<List<TeamDTO>> GetTeams(int projectID)
        {
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            ViewData["role"] = role;
            HttpResponseMessage response = await _httpClient.GetAsync($"{TeamApiUrl}/Project/{projectID}");
            string strData = await response.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<List<TeamDTO>>(strData);
            return teams;
        }
        private async Task<List<ProjectDTO>> GetProjects()
        {
            string token = HttpContext.Session.GetString("token");
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(ProjectApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<List<ProjectDTO>>(strData);
            return projects;
        }
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            TeamRequest team = new TeamRequest();
            string name = HttpContext.Request.Form["name"];
            string des = HttpContext.Request.Form["des"];
            int projectId = Int32.Parse(HttpContext.Request.Form["projectId"]);
            team.Teamname = name;
            team.Des = des;
            team.ProjectId = projectId;
            var jsonContent = JsonSerializer.Serialize(team);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(TeamApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Add fail";
            }
            await GetDataAsync(null);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int teamId)
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{TeamApiUrl}/{teamId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Delete fail";
            }
            await GetDataAsync(null);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            TeamRequest team = new TeamRequest();
            string name = HttpContext.Request.Form["name"];
            string des = HttpContext.Request.Form["des"];
            int projectId = Int32.Parse(HttpContext.Request.Form["projectId"]);
            int teamId = Int32.Parse(HttpContext.Request.Form["teamId"]);
            team.Teamname = name;
            team.Des = des;
            team.ProjectId = projectId;
            var jsonContent = JsonSerializer.Serialize(team);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{TeamApiUrl}/{teamId}", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Update fail";
            }
            await GetDataAsync(null);
            return RedirectToAction("Index");
        }
        //get details of team with teamId
        private async Task<List<UserTeamDTO>> GetDetailsOfTeam(int teamId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{TeamApiUrl}/Detail/{teamId}");
            string strData = await response.Content.ReadAsStringAsync();
            var userteams = JsonConvert.DeserializeObject<List<UserTeamDTO>>(strData);
            return userteams;
        }
        [HttpPost]
        public async Task<IActionResult> AddMemberToTeam()
        {
            UserTeamRequest userTeamRequest = new UserTeamRequest();
            int teamId = Int32.Parse(HttpContext.Request.Form["teamId"]);
            userTeamRequest.TeamId = teamId;
            string email = HttpContext.Request.Form["email"];
            string role = HttpContext.Request.Form["role"];
            userTeamRequest.Role = role;
            userTeamRequest.UserId = 0;
            var jsonContent = JsonSerializer.Serialize(userTeamRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{TeamApiUrl}/AddMemberToTeam/{email}", content);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(errorMessage))
                {
                    TempData["messageResponse"] = "Add fail";
                }
                else
                {
                    TempData["messageResponse"] = errorMessage;
                }
            }
            return RedirectToAction("detail", new { teamId = teamId });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveMemberFromTeam(int userTeamId)
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            int teamId = Int32.Parse(HttpContext.Request.Form["teamId"]);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{TeamApiUrl}/RemoveMemberFromTeam/{userTeamId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Delete fail";
            }
            await GetDataAsync(null);
            return RedirectToAction("detail", new { teamId = teamId });
        }
        [HttpPost]
        public async Task<IActionResult> SetTeamLeader(int userTeamId, int teamId)
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PutAsync($"{TeamApiUrl}/SetTeamLeader/{userTeamId}/{teamId}", null);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Set new leader fail";
            }
            await GetDataAsync(null);
            return RedirectToAction("Index");
        }

    }
}
