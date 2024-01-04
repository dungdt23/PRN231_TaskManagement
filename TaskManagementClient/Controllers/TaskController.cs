using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementClient.Controllers
{
    public class TaskController : Controller
    {
        private readonly HttpClient _httpClient = null;
        private string TaskApiUrl = null;
        private string TeamApiUrl = null;
        private string CategoryApiUrl = null;
        private string MemberApiUrl = null;

        public TaskController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            TaskApiUrl = "http://localhost:5245/api/Tasks";
            TeamApiUrl = "http://localhost:5245/api/Teams";
            CategoryApiUrl = "http://localhost:5245/api/Categories";
            MemberApiUrl = "http://localhost:5245/api/Members";
        }
        public async Task<IActionResult> IndexAsync()
        {
            await GetDataAsync();
            return View();
        }
        public async Task<IActionResult> DetailAsync(int? taskId)
        {
            if (taskId.HasValue)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{TaskApiUrl}/GetTaskAssigns/{taskId.Value}");
                string strData = await response.Content.ReadAsStringAsync();
                var taskAssigns = JsonConvert.DeserializeObject<List<TaskAssignDTO>>(strData);
                int teamId = 0;
                if (taskAssigns.Count == 0)
                {
                    var team = await GetTeamByTask(taskId.Value);
                    teamId = team.TeamId;
                }
                else
                {
                    teamId = taskAssigns.FirstOrDefault().Task.TeamId;

                }
                var users = await GetMembersOfTeam(teamId);
                ViewData["taskAssigns"] = taskAssigns;
                ViewData["users"] = users;
                ViewData["taskId"] = taskId.Value;
                return View();
            }
            else
            {
                return View();

            }
        }
        public async Task<IActionResult> MyTaskAsync(string? taskName, int? teamId, DateTime? assignDate, DateTime? startDate, DateTime? endDate, string? status, int p = 1, int s = 5)
        {
            //get information of user
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            ViewData["role"] = role;
            HttpResponseMessage responseUser = await _httpClient.GetAsync($"{MemberApiUrl}/{username}");
            string strDataUser = await responseUser.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<MemberDTO>(strDataUser);
            //handle filter
            string inputTaskName = "null";
            if (!String.IsNullOrEmpty(taskName))
            {
                inputTaskName = taskName;
            }
            int inputTeamId = 0;
            if (teamId.HasValue)
            {
                inputTeamId = teamId.Value;
            }
            DateTime inputAssignDate = DateTime.MinValue;
            if (assignDate.HasValue)
            {
                inputAssignDate = assignDate.Value;
            }
            DateTime inputStartDate = DateTime.MinValue;
            if (startDate.HasValue)
            {
                inputStartDate = startDate.Value;
            }
            DateTime inputEndDate = DateTime.MinValue;
            if (endDate.HasValue)
            {
                inputEndDate = endDate.Value;
            }
            string inputStatus = "null";
            if (!String.IsNullOrEmpty(status))
            {
                inputStatus = status;
            }
            List<TaskAssignDTO> taskAssigns = await GetTasksOfUser(user.UserId, inputTaskName, inputTeamId, inputAssignDate, inputStartDate, inputEndDate, inputStatus, p, s);
            List<TeamDTO> teams = await GetTeamsOfUser();
            ViewData["taskAssigns"] = taskAssigns;
            ViewData["teams"] = teams;
            if (inputTaskName.Equals("null"))
            {
                ViewData["taskNameFiltered"] = "";
            }
            else
            {
                ViewData["taskNameFiltered"] = inputTaskName;
            }
            ViewData["teamIdFiltered"] = inputTeamId;
            ViewData["statusFiltered"] = inputStatus;
            ViewData["assignDateFiltered"] = inputAssignDate;
            ViewData["startDateFiltered"] = inputStartDate;
            ViewData["endDateFiltered"] = inputEndDate;

            List<TaskAssignDTO> totals = await GetTasksOfUser(user.UserId, inputTaskName, inputTeamId, inputAssignDate, inputStartDate, inputEndDate, inputStatus, 0, 0);
            ViewData["total"] = totals.Count();
            ViewData["pageIndex"] = p;
            ViewData["pageSize"] = s;
            return View();
        }
        public async Task<IActionResult> TaskAssignAsync(string? taskName, int? teamId, int? categoryId, int p = 1, int s = 3)
        {
            //get information of user
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            ViewData["role"] = role;
            HttpResponseMessage responseUser = await _httpClient.GetAsync($"{MemberApiUrl}/{username}");
            string strDataUser = await responseUser.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<MemberDTO>(strDataUser);
            //handle filter
            string inputTaskName = "null";
            int inputTeamId = 0;
            int inputCategoryId = 0;
            if (!String.IsNullOrEmpty(taskName))
            {
                inputTaskName = taskName;
            }
            if (teamId.HasValue)
            {
                inputTeamId = teamId.Value;
            }
            if (categoryId.HasValue)
            {
                inputCategoryId = categoryId.Value;
            }
            if (inputTaskName.Equals("null"))
            {
                ViewData["taskNameFiltered"] = "";
            }
            else
            {
                ViewData["taskNameFiltered"] = inputTaskName;
            }
            ViewData["teamIdFiltered"] = inputTeamId;
            ViewData["categoryIdFiltered"] = inputCategoryId;
            List<TaskDTO> tasks = await GetTasksOfTeams(token, user.UserId, inputTaskName, inputTeamId, inputCategoryId, p, s);
            ViewData["tasks"] = tasks;
            List<TeamDTO> teams = await GetTeams();
            ViewData["teams"] = teams;
            List<CategoryDTO> cates = await GetCategories();
            ViewData["cates"] = cates;
            List<TaskDTO> totals = await GetTasksOfTeams(token, user.UserId, inputTaskName, inputTeamId, inputCategoryId, 0, 0);
            ViewData["total"] = totals.Count();
            ViewData["pageIndex"] = p;
            ViewData["pageSize"] = s;
            return View();
        }
        public async Task GetDataAsync()
        {
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            ViewData["role"] = role;
            if (role.Equals("Admin"))
            {
                List<TaskDTO> tasks = await GetTasks();
                ViewData["tasks"] = tasks;
            }
            else
            {
                //HttpResponseMessage responseUser = await _httpClient.GetAsync($"{MemberApiUrl}/{username}");
                //string strDataUser = await responseUser.Content.ReadAsStringAsync();
                //var user = JsonConvert.DeserializeObject<MemberDTO>(strDataUser);

                //List<TaskDTO> tasks = await GetTasksOfTeams(token, user.UserId, "null", 0, 0);
                //ViewData["tasks"] = tasks;
            }
            List<TeamDTO> teams = await GetTeams();
            ViewData["teams"] = teams;
            List<CategoryDTO> cates = await GetCategories();
            ViewData["cates"] = cates;
        }
        private async Task<List<TaskDTO>> GetTasks()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(TaskApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskDTO>>(strData);
            return tasks;
        }
        private async Task<List<TaskDTO>> GetTasksOfTeams(string token, int leaderId, string taskName, int teamId, int categoryId, int pageIndex, int pageSize)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync($"{TaskApiUrl}/GetTasksOfLeader/{leaderId}/{taskName}/{teamId}/{categoryId}/{pageIndex}/{pageSize}");
            string strData = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskDTO>>(strData);
            return tasks;
        }
        private async Task<List<TaskAssignDTO>> GetTasksOfUser(int userId, string taskName, int teamId, DateTime assignDate, DateTime startDate, DateTime endDate, string status, int pageIndex, int pageSize)


        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{TaskApiUrl}/GetTasksOfUser/{userId}/{taskName}/{teamId}/{status}/{pageIndex}/{pageSize}?assignDate={assignDate}&startDate={startDate}&endDate={endDate}");
            string strData = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskAssignDTO>>(strData);
            return tasks;
        }
        private async Task<List<TeamDTO>> GetTeams()
        {

            HttpResponseMessage response = await _httpClient.GetAsync(TeamApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<List<TeamDTO>>(strData);
            return teams;
        }
        private async Task<List<CategoryDTO>> GetCategories()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(CategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var cates = JsonConvert.DeserializeObject<List<CategoryDTO>>(strData);
            return cates;
        }
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            TaskRequest taskRequest = new TaskRequest();
            taskRequest.Taskname = HttpContext.Request.Form["name"];
            taskRequest.Des = HttpContext.Request.Form["des"];
            taskRequest.TeamId = Int32.Parse(HttpContext.Request.Form["team"]);
            taskRequest.CategoryId = Int32.Parse(HttpContext.Request.Form["cate"]);
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(taskRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(TaskApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Add fail";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int taskId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{TaskApiUrl}/{taskId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Delete fail";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            TaskRequest taskRequest = new TaskRequest();
            taskRequest.Taskname = HttpContext.Request.Form["name"];
            taskRequest.Des = HttpContext.Request.Form["des"];
            taskRequest.TeamId = Int32.Parse(HttpContext.Request.Form["team"]);
            taskRequest.CategoryId = Int32.Parse(HttpContext.Request.Form["cate"]);
            int taskId = Int32.Parse(HttpContext.Request.Form["taskId"]);
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(taskRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{TaskApiUrl}/{taskId}", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Update fail";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AssignTask()
        {
            TaskAssignRequest taskAssign = new TaskAssignRequest();
            int taskId = Int32.Parse(HttpContext.Request.Form["taskId"]);
            taskAssign.TaskId = taskId;
            taskAssign.UserId = Int32.Parse(HttpContext.Request.Form["user"]);
            taskAssign.Des = HttpContext.Request.Form["des"];
            taskAssign.AssignDate = DateTime.Now;
            taskAssign.StartDate = DateTime.Parse(HttpContext.Request.Form["startDate"]);
            taskAssign.EndDate = DateTime.Parse(HttpContext.Request.Form["dueDate"]);
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(taskAssign);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{TaskApiUrl}/AssignTask", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Add fail";
            }
            return RedirectToAction("Detail", new { taskId });
        }
        public async Task<List<UserTeamDTO>> GetMembersOfTeam(int teamId)
        {
            HttpResponseMessage responseUser = await _httpClient.GetAsync($"{MemberApiUrl}/GetMembersOfTeam/{teamId}");
            string strDataUser = await responseUser.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserTeamDTO>>(strDataUser);
            return users;
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTaskAssign()
        {
            TaskAssignRequest taskAssign = new TaskAssignRequest();
            int taskId = Int32.Parse(HttpContext.Request.Form["taskId"]);
            int taskAssignId = Int32.Parse(HttpContext.Request.Form["taskAssignId"]);
            taskAssign.TaskId = taskId;
            taskAssign.UserId = Int32.Parse(HttpContext.Request.Form["user"]);
            taskAssign.Des = HttpContext.Request.Form["des"];
            taskAssign.AssignDate = DateTime.Now;
            taskAssign.StartDate = DateTime.Parse(HttpContext.Request.Form["startDate"]);
            taskAssign.EndDate = DateTime.Parse(HttpContext.Request.Form["dueDate"]);
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(taskAssign);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{TaskApiUrl}/UpdateTaskAssign/{taskAssignId}", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Update fail";
            }
            return RedirectToAction("Detail", new { taskId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTaskAssign(int taskAssignId)
        {
            int taskId = Int32.Parse(HttpContext.Request.Form["taskId"]);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{TaskApiUrl}/DeleteTaskAssign/{taskAssignId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Delete fail";
            }
            return RedirectToAction("Detail", new { taskId });
        }
        [HttpPost]
        public async Task<IActionResult> CheckTask()
        {   //get information of user
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            ViewData["role"] = role;
            HttpResponseMessage responseUser = await _httpClient.GetAsync($"{MemberApiUrl}/{username}");
            string strDataUser = await responseUser.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<MemberDTO>(strDataUser);

            List<TaskAssignDTO> taskAssigns = await GetTasksOfUser(user.UserId, "null", 0, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "null", 0, 0);
            //dict to store key and status of taskassign belong to user
            Dictionary<Int32, Boolean> dics = new Dictionary<int, bool>();
            foreach (var item in taskAssigns)
            {
                if (!String.IsNullOrEmpty(HttpContext.Request.Form["checkbox[" + @item.TaskAssignId + "]"]) || HttpContext.Request.Form["checkbox[" + @item.TaskAssignId + "]"].Count == 2)
                {
                    string str = HttpContext.Request.Form["checkbox[" + @item.TaskAssignId + "]"];
                    dics.Add(item.TaskAssignId, true);
                }
                else
                {
                    dics.Add(item.TaskAssignId, false);
                }
            }
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(dics);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{TaskApiUrl}/CheckTask", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Update process failed";
            }
            return RedirectToAction("MyTask");
        }
        private async Task<List<TeamDTO>> GetTeamsOfUser()
        {
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            //get username from api, based on username
            HttpResponseMessage responseUser = await _httpClient.GetAsync($"{MemberApiUrl}/{username}");
            string strDataUser = await responseUser.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<MemberDTO>(strDataUser);
            HttpResponseMessage response = await _httpClient.GetAsync($"{TeamApiUrl}/TeamsOfUser/{user.UserId}");
            string strData = await response.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<List<TeamDTO>>(strData);
            return teams;

        }
        private async Task<TeamDTO> GetTeamByTask(int taskId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{TeamApiUrl}/GetTeamByTask/{taskId}");
            string strData = await response.Content.ReadAsStringAsync();
            var team = JsonConvert.DeserializeObject<TeamDTO>(strData);
            return team;
        }
    }
}

