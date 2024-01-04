using BusinessObjects.DTOs;
using BusinessObjects.Requests;

namespace Repositories.IRepositories
{
    public interface ITaskRepository
    {
        List<TaskDTO> GetTasks();
        void Create(TaskRequest taskRequest);
        void Update(int id, TaskRequest taskRequest);
        void Delete(int id);
        List<TaskDTO> GetTeamTasks(int leaderId);
        List<TaskDTO> GetTasksOfLeader(int leaderId, string taskName, int teamId, int categoryId, int pageIndex, int pageSize);
        void AssignTask(TaskAssignRequest taskAssignDTO);
        List<TaskAssignDTO> GetTaskAssigns(int taskId);
        void DeleteTaskAssign(int taskAssignId);
        void UpdateTaskAssign(int taskAssignId, TaskAssignRequest taskAssignRequest);
        List<TaskAssignDTO> GetTasksOfUser(int userId, string taskName, int teamId, DateTime assignDate, DateTime startDate, DateTime endDate, string status, int pageIndex, int pageSize);
        void CheckTask(Dictionary<Int32, Boolean> dics);
    }
}