using BusinessObjects.DTOs;
using BusinessObjects.Requests;

namespace Repositories.IRepositories
{
    public interface IProjectRepository
    {
        List<ProjectDTO> GetProjects();
        void Create(ProjectRequest project);
        void Delete(int id);    
        void Update(int id, ProjectRequest project);
    }
}