using BusinessObjects.DTOs;
using BusinessObjects.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IMemberRepository
    {
        List<MemberDTO> GetMembers();
        void Create(MemberRequest member);
        void Update(int id,MemberRequest member);  
        void Delete(int id);    
        MemberDTO Login(LoginRequest loginRequest);
        MemberDTO GetUser(string username);
        List<UserTeamDTO> GetMembersOfTeam(int teamId); 
    }
}
