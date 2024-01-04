using BusinessObjects.DTOs;
using BusinessObjects.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using Repositories.Repositories;

namespace TaskManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private ITaskRepository _taskRepository;
        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var tasks = _taskRepository.GetTasks();
                return Ok(tasks);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Post(TaskRequest taskRequest)
        {
            try
            {
                _taskRepository.Create(taskRequest);
                return Ok(taskRequest);
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
                _taskRepository.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, TaskRequest taskRequest)
        {
            try
            {
                _taskRepository.Update(id, taskRequest);
                return Ok(taskRequest);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("GetTeamTasks/{leaderId}")]
        public IActionResult GetTeamTasks(int leaderId)
        {
            try
            {
                var tasks = _taskRepository.GetTeamTasks(leaderId);
                return Ok(tasks);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("GetTasksOfLeader/{leaderId}/{taskName}/{teamId}/{categoryId}/{pageIndex}/{pageSize}")]
        public IActionResult GetTasksOfLeader(int leaderId, string taskName, int teamId, int categoryId, int pageIndex, int pageSize)
        {
            try
            {

                var tasks = _taskRepository.GetTasksOfLeader(leaderId, taskName, teamId, categoryId, pageIndex, pageSize);
                return Ok(tasks);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("AssignTask")]
        public IActionResult AssignTask(TaskAssignRequest taskAssignDTO)
        {
            try
            {
                if (taskAssignDTO.StartDate > taskAssignDTO.EndDate)
                {
                    return BadRequest();
                }
                _taskRepository.AssignTask(taskAssignDTO);
                return Ok(taskAssignDTO);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("GetTaskAssigns/{taskId}")]
        public IActionResult GetTaskAssigns(int taskId)
        {
            try
            {
                var tasks = _taskRepository.GetTaskAssigns(taskId);
                return Ok(tasks);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("DeleteTaskAssign/{taskAssignId}")]
        public IActionResult DeleteTaskAssign(int taskAssignId)
        {
            try
            {
                _taskRepository.DeleteTaskAssign(taskAssignId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateTaskAssign/{taskAssignId}")]
        public IActionResult UpdateTaskAssign(int taskAssignId, TaskAssignRequest taskAssignRequest)
        {
            try
            {
                _taskRepository.UpdateTaskAssign(taskAssignId, taskAssignRequest);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("GetTasksOfUser/{userId}/{taskName}/{teamId}/{status}/{pageIndex}/{pageSize}")]
        public IActionResult GetTasksOfUser(int userId, string taskName, int teamId, DateTime assignDate, DateTime startDate, DateTime endDate, string status, int pageIndex, int pageSize)
        {
            try
            {
                var taskAssings = _taskRepository.GetTasksOfUser(userId, taskName, teamId, assignDate, startDate, endDate, status, pageIndex, pageSize);
                return Ok(taskAssings);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpPut("CheckTask")]
        public IActionResult CheckTask(Dictionary<Int32, Boolean> dics)
        {
            try
            {
                _taskRepository.CheckTask(dics);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
