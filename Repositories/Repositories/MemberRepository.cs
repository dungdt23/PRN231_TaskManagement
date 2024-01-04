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

namespace Repositories.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        Prn231AsmTaskManagementContext _context;
        IMapper _mapper;
        MemberDAO _memberDAO;
        public MemberRepository(Prn231AsmTaskManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(MemberRequest member)
        {
            _memberDAO = new MemberDAO(_context);
            var m = _mapper.Map<Member>(member);
            _memberDAO.Create(m);
        }

        public void Delete(int id)
        {
            _memberDAO = new MemberDAO(_context);
            _memberDAO.Delete(id);
        }

        public List<MemberDTO> GetMembers()
        {
            _memberDAO = new MemberDAO(_context);
            var mems = _memberDAO.GetMembers();
            var memDTOs = _mapper.Map<List<MemberDTO>>(mems);
            return memDTOs;
        }

        public List<UserTeamDTO> GetMembersOfTeam(int teamId)
        {
            _memberDAO = new MemberDAO(_context);
            var userTeams = _memberDAO.GetMembersOfTeam(teamId);
            var userTeamDTOs = _mapper.Map<List<UserTeamDTO>>(userTeams);
            return userTeamDTOs;
        }

        public MemberDTO GetUser(string username)
        {
            _memberDAO = new MemberDAO(_context);
            var user = _memberDAO.GetUser(username);
            var userDTO = _mapper.Map<MemberDTO>(user);
            return userDTO;
        }

        public MemberDTO Login(LoginRequest loginRequest)
        {
            _memberDAO = new MemberDAO(_context);
            var user = _memberDAO.Login(loginRequest.Username, loginRequest.Password);
            var userDTOs = _mapper.Map<MemberDTO>(user);
            return userDTOs;
        }

        public void Update(int id, MemberRequest member)
        {
            _memberDAO = new MemberDAO(_context);
            var m = _mapper.Map<Member>(member);
            _memberDAO.Update(id, m);
        }
    }
}
