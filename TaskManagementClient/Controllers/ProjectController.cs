using BusinessObjects.DTOs;
using BusinessObjects.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace TaskManagementClient.Controllers
{
    public class ProjectController : Controller
    {
        private string ProjectApiUrl = null;
        private readonly HttpClient _httpClient = null;
        public ProjectController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            ProjectApiUrl = "http://localhost:5245/api/Projects";
        }
        public async Task<IActionResult> IndexAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(ProjectApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<List<ProjectDTO>>(strData);
            ViewData["projects"] = projects;
            //await GetDataAsync();
            return View();
        }
        public async Task GetDataAsync()
        {
            List<ProjectDTO> projects = await GetProjects();
            ViewData["projects"] = projects;
        }
        private async Task<List<ProjectDTO>> GetProjects()
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(ProjectApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Add fail";
            }
            string strData = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<List<ProjectDTO>>(strData);
            return projects;
        }
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            string token = HttpContext.Session.GetString("token");
            ProjectRequest project = new ProjectRequest();
            project.Projectname = HttpContext.Request.Form["name"];
            project.Status = Int32.Parse(HttpContext.Request.Form["status"]);
            project.Des = HttpContext.Request.Form["des"];
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(project);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.PostAsync(ProjectApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Add fail";
            }
            await GetDataAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int projectId)
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{ProjectApiUrl}/{projectId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Delete fail";
            }
            await GetDataAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            string token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ProjectRequest project = new ProjectRequest();
            project.Projectname = HttpContext.Request.Form["name"];
            project.Status = Int32.Parse(HttpContext.Request.Form["status"]);
            int projectId = Int32.Parse(HttpContext.Request.Form["projectId"]);
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(project);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{ProjectApiUrl}/{projectId}", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Update fail";
            }
            await GetDataAsync();
            return RedirectToAction("Index");
        }
    }
}
