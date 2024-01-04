using BusinessObjects.DTOs;
using BusinessObjects.Requests;
using BusinessObjects.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TaskManagementClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient = null;
        private string MemberApiUrl = null;
        public LoginController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "http://localhost:5245/api/Members";
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            LoginRequest request = new LoginRequest();
            request.Username = HttpContext.Request.Form["username"];
            request.Password = HttpContext.Request.Form["password"];
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{MemberApiUrl}/Login", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Login failed! Please check your username or password!";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                // Deserialize the response content into a LoginResponse object
                string strData = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(strData);
                var token = loginResponse.Token;
                HttpContext.Session.SetString("token", token);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public  async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Login");
        }
    }
}
