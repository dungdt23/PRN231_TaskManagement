using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
        Prn231AsmTaskManagementContext _context;
        public MemberDAO(Prn231AsmTaskManagementContext context)
        {
            _context = context;
        }
        public List<Member> GetMembers()
        {
            var members = _context.Members.ToList();
            return members;
        }
        public Member Login(string username,string password)
        {
            var user = _context.Members
                        .FirstOrDefault(x => x.Username.ToLower().Equals(username.ToLower()) && x.Password.ToLower().Equals(password.ToLower()));
            return user;
        }
        public void Create(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }
        public void Update(int id,Member member)
        {
            member.UserId = id;
            _context.Entry<Member>(member).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var existed = _context.Members.FirstOrDefault(x => x.UserId == id);
            if (existed != null)
            {
                _context.Members.Remove(existed);
                _context.SaveChanges();
            } 
        }
        public Member GetUser(string username)
        {
            var user = _context.Members.FirstOrDefault(x => x.Username.Equals(username));
            return user;
        }
        public List<UserTeam> GetMembersOfTeam(int teamId)
        {
            var userTeams = _context.UserTeams.Where(x => x.TeamId == teamId)
                .Include(x => x.User)
                .ToList();
            return userTeams;
        }
    }
}
