using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = BusinessObjects.Models.Task;

namespace BusinessObjects.Mapping
{
    public class MappingProfile : AutoMapper.Profile 
    {
        public MappingProfile()
        {
            //mapping for Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            //mapping for Project
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<Project, ProjectRequest>().ReverseMap();
            //mapping for Team
            CreateMap<Team, TeamDTO>().ReverseMap();
            CreateMap<Team, TeamRequest>().ReverseMap();
            //mapping for User-Team
            CreateMap<UserTeam, UserTeamDTO>().ReverseMap();
            CreateMap<UserTeam, UserTeamRequest>().ReverseMap();
            //mapping for Member
            CreateMap<Member, MemberRequest>().ReverseMap();
            CreateMap<Member, MemberDTO>().ReverseMap();
            CreateMap<Member, RegisterRequest>().ReverseMap();
            //mapping for Task
            CreateMap<Task, TaskDTO>().ReverseMap();
            CreateMap<Task, TaskRequest>().ReverseMap();
            //mapping for TaskAssign
            CreateMap<TaskAssign, TaskAssignDTO>().ReverseMap();
            CreateMap<TaskAssign, TaskAssignRequest>().ReverseMap();

        }
    }
}
