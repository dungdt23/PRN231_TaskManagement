using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Requests;
using DataAccess;
using Repositories.IRepositories;

namespace Repositories.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        Prn231AsmTaskManagementContext _context;
        IMapper _mapper;
        TeamDAO _teamDAO;
        public TeamRepository(Prn231AsmTaskManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(TeamRequest teamRequest)
        {
            _teamDAO = new TeamDAO(_context);
            var team = _mapper.Map<Team>(teamRequest);
            _teamDAO.Create(team);
        }

        public void Delete(int id)
        {
            _teamDAO = new TeamDAO(_context);
            _teamDAO.Delete(id);    
        }

        public List<TeamDTO> GetTeams()
        {
            _teamDAO = new TeamDAO(_context);
            var teamDTOlist = _mapper.Map<List<TeamDTO>>(_teamDAO.GetTeams());
            return teamDTOlist;
        }
        public List<TeamDTO> GetTeams(int projectID)
        {
            _teamDAO = new TeamDAO(_context);
            var teamDTOlist = _mapper.Map<List<TeamDTO>>(_teamDAO.GetTeams(projectID));
            return teamDTOlist;
        }
        public void Update(int id, TeamRequest teamRequest)
        {
            _teamDAO = new TeamDAO(_context);
            var team = _mapper.Map<Team>(teamRequest);
            _teamDAO.Update(id, team);
        }
        public List<UserTeamDTO> GetMembersOfTeam(int teamId)
        {
            _teamDAO = new TeamDAO(_context);
            var userteam = _teamDAO.GetMembersOfTeam(teamId);    
            var userteamDto = _mapper.Map<List<UserTeamDTO>>(userteam);
            return userteamDto;
        }
        public void AddMemberToTeam(string email,UserTeamRequest userTeamRequest)
        {
            var userTeam = _mapper.Map<UserTeam>(userTeamRequest);
            _teamDAO = new TeamDAO(_context);
            _teamDAO.AddMemberToTeam(email,userTeam);
        }
        public void RemoveMemberFromTeam(int userTeamId)
        {
            _teamDAO = new TeamDAO(_context);
            _teamDAO.RemoveMemberFromTeam(userTeamId);
        }

        public void SetTeamLeader(int userTeamId, int teamId)
        {
            _teamDAO = new TeamDAO(_context);
            _teamDAO.SetTeamLeader(userTeamId,teamId);
        }

        public List<TeamDTO> GetTeamsOfUser(int leaderId)
        {
            _teamDAO = new TeamDAO(_context);
            var teams = _teamDAO.GetTeamsOfUser(leaderId);
            var teamDTOs = _mapper.Map<List<TeamDTO>>(teams);
            return teamDTOs;
        }

        public TeamDTO GetTeamByTaskId(int taskId)
        {
            _teamDAO = new TeamDAO(_context);
            var team = _teamDAO.GetTeamByTaskId(taskId);
            var teamDTO = _mapper.Map<TeamDTO>(team);
            return teamDTO;
        }
    }
}
