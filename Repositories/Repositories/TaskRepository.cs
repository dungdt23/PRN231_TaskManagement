using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Requests;
using DataAccess;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = BusinessObjects.Models.Task;

namespace Repositories.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        Prn231AsmTaskManagementContext _context;
        IMapper _mapper;
        TaskDAO _taskDAO;
        public TaskRepository(Prn231AsmTaskManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(TaskRequest taskRequest)
        {
            _taskDAO = new TaskDAO(_context);
            var task = _mapper.Map<Task>(taskRequest);
            _taskDAO.Create(task);
        }

        public void Delete(int id)
        {
            _taskDAO = new TaskDAO(_context);
            _taskDAO.Delete(id);
        }

        public List<TaskDTO> GetTasks()
        {
            _taskDAO = new TaskDAO(_context);
            var tasks = _taskDAO.GetTasks();
            var taskDTOs = _mapper.Map<List<TaskDTO>>(tasks);
            return taskDTOs;
        }

        public void Update(int id, TaskRequest taskRequest)
        {
            _taskDAO = new TaskDAO(_context);
            var task = _mapper.Map<Task>(taskRequest);
            _taskDAO.Update(id, task);
        }
        public List<TaskDTO> GetTeamTasks(int leaderId)
        {
            _taskDAO = new TaskDAO(_context);
            var teamTasks = _taskDAO.GetTeamTasks(leaderId);
            var teamTaskDTOs = _mapper.Map<List<TaskDTO>>(teamTasks);
            return teamTaskDTOs;
        }
        public void AssignTask(TaskAssignRequest taskAssignDTO)
        {
            _taskDAO = new TaskDAO(_context);
            var taskAssign = _mapper.Map<TaskAssign>(taskAssignDTO);
            _taskDAO.AssignTask(taskAssign);
        }

        public List<TaskDTO> GetTasksOfLeader(int leaderId, string taskName, int teamId, int categoryId, int pageIndex, int pageSize)
        {
            _taskDAO = new TaskDAO(_context);
            var teamTasks = _taskDAO.GetTasksOfLeader(leaderId, taskName, teamId, categoryId, pageIndex, pageSize);
            var teamTaskDTOs = _mapper.Map<List<TaskDTO>>(teamTasks);
            return teamTaskDTOs;
        }
        public List<TaskAssignDTO> GetTaskAssigns(int taskId)
        {
            _taskDAO = new TaskDAO(_context);
            var taskAssigns = _taskDAO.GetTaskAssigns(taskId);
            var taskAssinDTOs = _mapper.Map<List<TaskAssignDTO>>(taskAssigns);
            return taskAssinDTOs;
        }
        public void DeleteTaskAssign(int taskAssignId)
        {
            _taskDAO = new TaskDAO(_context);
            _taskDAO.DeleteTaskAssign(taskAssignId);
        }
        public void UpdateTaskAssign(int taskAssignId, TaskAssignRequest taskAssignRequest)
        {
            _taskDAO = new TaskDAO(_context);
            var taskAssign = _mapper.Map<TaskAssign>(taskAssignRequest);
            _taskDAO.UpdateTaskAssign(taskAssignId, taskAssign);
        }
        public List<TaskAssignDTO> GetTasksOfUser(int userId, string taskName, int teamId, DateTime assignDate, DateTime startDate, DateTime endDate, string status, int pageIndex, int pageSize)
        {
            _taskDAO = new TaskDAO(_context);
            var taskAssigns = _taskDAO.GetTasksOfUser(userId,taskName,teamId,assignDate,startDate,endDate,status,pageIndex,pageSize);
            var taskAssignDTOs = _mapper.Map<List<TaskAssignDTO>>(taskAssigns);
            return taskAssignDTOs;
        }
        public void CheckTask(Dictionary<Int32, Boolean> dics)
        {
            _taskDAO = new TaskDAO(_context);
            _taskDAO.CheckTask(dics);
        }
    }
}
