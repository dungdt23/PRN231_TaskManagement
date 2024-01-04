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
    public class ProjectRepository : IProjectRepository
    {
        Prn231AsmTaskManagementContext _context;
        IMapper _mapper;
        ProjectDAO _projectDAO;
        public ProjectRepository(Prn231AsmTaskManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(ProjectRequest projectDTO)
        {
            _projectDAO = new ProjectDAO(_context);
            var project = _mapper.Map<Project>(projectDTO);
            _projectDAO.Create(project);
        }

        public void Delete(int id)
        {
            _projectDAO = new ProjectDAO(_context);
            _projectDAO.Delete(id);

        }

        public List<ProjectDTO> GetProjects()
        {
            _projectDAO = new ProjectDAO(_context);
            var prjList = _projectDAO.GetProjects();
            var prjListDTO = _mapper.Map<List<ProjectDTO>>(prjList);
            return prjListDTO;
        }

        public void Update(int id, ProjectRequest projectDTO)
        {
            _projectDAO = new ProjectDAO(_context);
            var project = _mapper.Map<Project>(projectDTO);
            _projectDAO.Update(id, project);
        }
    }
}
