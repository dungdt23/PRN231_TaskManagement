using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TeamDAO
    {
        Prn231AsmTaskManagementContext _context;
        public TeamDAO(Prn231AsmTaskManagementContext context)
        {
            _context = context;
        }
        public List<Team> GetTeams()
        {

            var teams = _context.Teams.Include(x => x.Project).ToList();
            return teams;
        }
        public List<Team> GetTeams(int projectID)
        {

            var teams = _context.Teams.Where(x => x.ProjectId == projectID)
                                      .Include(x => x.Project).ToList();
            return teams;
        }
        public List<Team> GetTeamsOfUser(int userId)
        {
            var userTeams = _context.UserTeams.Include(x => x.Team)
                                              .ThenInclude(a => a.Project)
                                              .Where(x => x.UserId == userId).ToList();
            List<Team> teams = new List<Team>();
            foreach (var item in userTeams)
            {
                teams.Add(item.Team);
            }
            return teams;
        }
        public void Create(Team team)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();
        }
        public void Update(int id,Team team)
        {
            team.TeamId = id;
            _context.Entry<Team>(team).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            //remove constraint related to team
            var userteams = _context.UserTeams.Where(x => x.TeamId == id);
            var tasks = _context.Tasks.Where(x => x.TeamId == id);
            _context.UserTeams.RemoveRange(userteams);
            foreach (var item in tasks)
            {
                var lst = _context.TaskAssigns.Where(x => x.TaskId == item.TaskId);
                _context.TaskAssigns.RemoveRange(lst);  
            }
            _context.Tasks.RemoveRange(tasks);
            var existed = _context.Teams.FirstOrDefault(x => x.TeamId == id);
            if (existed != null)
            {
                _context.Teams.Remove(existed);
                _context.SaveChanges();
            }
        }
        public List<UserTeam> GetMembersOfTeam(int teamId)
        {
            var userteams = _context.UserTeams.Where(x => x.TeamId == teamId)
                .Include(x => x.Team)
                .Include(x => x.User)
                .ToList();
            return userteams;
        }
        public void AddMemberToTeam(string email,UserTeam userTeam)
        {
            var existedUser = _context.Members.First(x => x.Email.Equals(email));
            var existedUserTeam = _context.UserTeams.FirstOrDefault(x => x.UserId == existedUser.UserId && x.TeamId == userTeam.TeamId);
            if (existedUserTeam != null)
            {
                throw new Exception("Member is already in this team!");
            }
            if (existedUser != null)
            {
                userTeam.UserId = existedUser.UserId;
                _context.UserTeams.Add(userTeam);
                _context.SaveChanges();
            }
        }
        public void RemoveMemberFromTeam(int userTeamId)
        {
            var existed = _context.UserTeams.FirstOrDefault(x => x.UserTeamId == userTeamId);
            _context.UserTeams.Remove(existed);
            _context.SaveChanges();
        }
        public void SetTeamLeader(int userTeamId,int teamId)
        {
            var recentLeader = _context.UserTeams.FirstOrDefault(x => x.IsLeader == true && x.TeamId == teamId);
            if (recentLeader != null)
            {
                recentLeader.IsLeader = false;
            }
            var newLeader = _context.UserTeams.FirstOrDefault(x => x.UserTeamId == userTeamId);
            if (newLeader != null)
            {
                newLeader.IsLeader = true;
            }
            _context.SaveChanges();
        }
        public Team GetTeamByTaskId(int taskId)
        {
            var team = _context.Tasks.Include(x => x.Team).FirstOrDefault(x => x.TaskId == taskId).Team;
            return team;
        }
    }
}
