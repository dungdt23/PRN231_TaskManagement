using BusinessObjects.Models;
using BusinessObjects.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = BusinessObjects.Models.Task;

namespace DataAccess
{
    public class TaskDAO
    {
        Prn231AsmTaskManagementContext _context;
        public TaskDAO(Prn231AsmTaskManagementContext context)
        {
            _context = context;
        }
        public List<Task> GetTasks()
        {
            return _context.Tasks.Include(x => x.Team).Include(x => x.Category).ToList();
        }

        public void Create(Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }
        public void Update(int id, Task task)
        {
            task.TaskId = id;
            _context.Entry<Task>(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var assigns = _context.TaskAssigns.Where(x => x.TaskId == id).ToList();
            var comments = _context.TaskComments.Where(x => x.TaskId != id).ToList();
            _context.TaskAssigns.RemoveRange(assigns);
            _context.TaskComments.RemoveRange(comments);
            var existed = _context.Tasks.FirstOrDefault(x => x.TaskId == id);
            if (existed != null)
            {
                _context.Tasks.Remove(existed);
                _context.SaveChanges();
            }
        }
        public List<Task> GetTeamTasks(int leaderId)
        {
            var userTeams = _context.UserTeams.Where(x => x.UserId == leaderId && x.IsLeader == true).ToList();
            List<Task> teamTasks = new List<Task>();
            foreach (var item in userTeams)
            {
                var tasks = _context.Tasks.Include(x => x.Category)
                    .Include(x => x.Team)
                    .Where(x => x.TeamId == item.TeamId).ToList();
                teamTasks.AddRange(tasks);
            }
            return teamTasks;
        }
        public List<Task> GetTasksOfLeader(int leaderId, string taskName, int teamId, int categoryId, int pageIndex, int pageSize)
        {
            var userTeams = _context.UserTeams.Where(x => x.UserId == leaderId && x.IsLeader == true).ToList();
            List<Task> teamTasks = new List<Task>();
            foreach (var item in userTeams)
            {
                IQueryable<Task> query = _context.Tasks.Include(x => x.Category)
                    .Include(x => x.Team)
                    .Where(x => x.TeamId == item.TeamId);
                if (!taskName.Equals("null"))
                {
                    query = query.Where(x => x.Taskname.Contains(taskName));
                }
                if (teamId != 0)
                {
                    query = query.Where(x => x.TeamId == teamId);
                }
                if (categoryId != 0)
                {
                    query = query.Where(x => x.CategoryId == categoryId);
                }
                if (pageIndex != 0 && pageSize != 0)
                {
                    query = query
                            .Skip((int)((pageIndex - 1) * pageSize))
                            .Take((int)pageSize);
                }
                var tasks = query.ToList();
                teamTasks.AddRange(tasks);
            }
            return teamTasks;
        }
        public void AssignTask(TaskAssign taskAssign)
        {
            _context.TaskAssigns.Add(taskAssign);
            _context.SaveChanges();
        }
        public List<TaskAssign> GetTaskAssigns(int taskId)
        {
            var taskAssigns = _context.TaskAssigns.Include(x => x.User)
                .Include(x => x.Task)
                .Where(x => x.TaskId == taskId).ToList();
            return taskAssigns;
        }
        public void DeleteTaskAssign(int taskAssignId)
        {
            var existed = _context.TaskAssigns.FirstOrDefault(x => x.TaskAssignId == taskAssignId);
            if (existed != null)
            {
                _context.TaskAssigns.Remove(existed);
                _context.SaveChanges();

            }
        }
        public void UpdateTaskAssign(int taskAssignId, TaskAssign taskAssign)
        {
            taskAssign.TaskAssignId = taskAssignId;
            _context.Entry<TaskAssign>(taskAssign).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
        public List<TaskAssign> GetTasksOfUser(int userId, string taskName, int teamId, DateTime assignDate, DateTime startDate, DateTime endDate, string status, int pageIndex, int pageSize)
        {
            List<TaskAssign> taskAssigns = new List<TaskAssign>();
            IQueryable<TaskAssign> query = _context.TaskAssigns
                .Include(x => x.Task)
                .ThenInclude(x => x.Team)
                .Include(x => x.User)
                .Where(x => x.User.UserId == userId);
            if (!taskName.Equals("null"))
            {
                query = query.Where(x => x.Task.Taskname.Contains(taskName));
            }
            if (teamId != 0)
            {
                query = query.Where(x => x.Task.TeamId == teamId);
            }
            if (assignDate != DateTime.MinValue)
            {
                query = query.Where(x => x.AssignDate == assignDate);
            }
            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.StartDate >= startDate);
            }
            if (endDate != DateTime.MinValue)
            {
                query = query.Where(x => x.EndDate <= endDate);
            }
            if (!status.Equals("null"))
            {
                if (status.Equals("Done"))
                {
                    query = query.Where(x => x.Status == true);
                }
                else
                {
                    query = query.Where(x => x.Status == false);
                }
            }
            if (pageIndex != 0 && pageSize != 0)
            {
                query = query
                        .Skip((int)((pageIndex - 1) * pageSize))
                        .Take((int)pageSize);
            }
            taskAssigns = query.ToList();
            return taskAssigns;
        }
        public void CheckTask(Dictionary<Int32, Boolean> dics)
        {
            foreach (var item in dics)
            {
                var taskAssign = _context.TaskAssigns.FirstOrDefault(x => x.TaskAssignId == item.Key);
                bool status = item.Value;
                taskAssign.Status = status;
            }
            _context.SaveChanges();
        }


    }
}
