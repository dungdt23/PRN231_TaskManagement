using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Requests;

namespace Repositories.IRepositories
{
    public interface ITeamRepository
    {
        List<TeamDTO> GetTeams();
        void Create(TeamRequest teamRequest);
        void Update(int id,TeamRequest teamRequest);
        void Delete(int id);
        List<TeamDTO> GetTeams(int projectID);
        List<UserTeamDTO> GetMembersOfTeam(int teamId);
        void AddMemberToTeam(string email, UserTeamRequest userTeamRequest);
        void RemoveMemberFromTeam(int userTeamId);
        void SetTeamLeader(int userTeamId, int teamId);
        List<TeamDTO> GetTeamsOfUser(int leaderId);
        TeamDTO GetTeamByTaskId(int taskId);
    }
}