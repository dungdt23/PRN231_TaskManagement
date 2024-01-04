using BusinessObjects.DTOs;
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
    public class ProjectsController : ControllerBase
    {
        private IProjectRepository _projectRepository;
        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        //[HttpGet]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var projects = _projectRepository.GetProjects();
                return Ok(projects);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public IActionResult Post(ProjectRequest projectDTO)
        {
            try
            {
                _projectRepository.Create(projectDTO);
                return Ok(projectDTO);
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
                _projectRepository.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Put(int id, ProjectRequest projectDTO)
        {
            try
            {
                _projectRepository.Update(id,projectDTO);
                return Ok(projectDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
