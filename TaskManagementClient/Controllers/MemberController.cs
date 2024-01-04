using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementClient.Controllers
{
    public class MemberController : Controller
    {
        private readonly HttpClient _httpClient = null;
        private string MemberApiUrl = null;
        public MemberController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "http://localhost:5245/api/Members";
        }
        public async Task<IActionResult> IndexAsync()
        {
            await GetDataAsync();
            return View();
        }
        public async Task GetDataAsync()
        {
            List<MemberDTO> members = await GetMembers();
            ViewData["members"] = members;
        }
        private async Task<List<MemberDTO>> GetMembers()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(MemberApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var mems = JsonConvert.DeserializeObject<List<MemberDTO>>(strData);
            return mems;
        }
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            MemberRequest memberRequest = new MemberRequest();
            memberRequest.Username = HttpContext.Request.Form["username"];
            memberRequest.Firstname = HttpContext.Request.Form["firstname"];
            memberRequest.Middlename = HttpContext.Request.Form["middlename"];
            memberRequest.Lastname = HttpContext.Request.Form["lastname"];
            memberRequest.Age = Int32.Parse(HttpContext.Request.Form["age"]);
            memberRequest.Address = HttpContext.Request.Form["address"];
            memberRequest.Email = HttpContext.Request.Form["email"];
            memberRequest.Password = HttpContext.Request.Form["password"];
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(memberRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(MemberApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Add fail";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            int userId = Int32.Parse(HttpContext.Request.Form["userId"]);
            MemberRequest memberRequest = new MemberRequest();
            memberRequest.Username = HttpContext.Request.Form["username"];
            memberRequest.Firstname = HttpContext.Request.Form["firstname"];
            memberRequest.Middlename = HttpContext.Request.Form["middlename"];
            memberRequest.Lastname = HttpContext.Request.Form["lastname"];
            memberRequest.Age = Int32.Parse(HttpContext.Request.Form["age"]);
            memberRequest.Address = HttpContext.Request.Form["address"];
            memberRequest.Email = HttpContext.Request.Form["email"];
            memberRequest.Password = HttpContext.Request.Form["password"];
            var jsonContent = JsonSerializer.Serialize(memberRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{MemberApiUrl}/{userId}", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Update fail";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{MemberApiUrl}/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["messageResponse"] = "Delete fail";
            }
            return RedirectToAction("Index");
        }


    }
}
