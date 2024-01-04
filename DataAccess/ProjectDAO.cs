using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProjectDAO
    {
        Prn231AsmTaskManagementContext _context;
        public ProjectDAO(Prn231AsmTaskManagementContext context)
        {
            _context = context;
        }
        public List<Project> GetProjects()
        {
            return _context.Projects.ToList();
        }
        public void Create(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }
        public void Update(int id, Project project)
        {
            project.ProjectId = id;
            _context.Entry<Project>(project).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var existed = _context.Projects.FirstOrDefault(x => x.ProjectId == id);
            var teams = _context.Teams.Where(x => x.ProjectId == id).ToList();
            TeamDAO teamDAO = new TeamDAO(_context);
            foreach (var item in teams)
            {
                teamDAO.Delete(item.TeamId);
            }
            if (existed != null)
            {
                _context.Projects.Remove(existed);
                _context.SaveChanges();
            }
        }
    }
}
