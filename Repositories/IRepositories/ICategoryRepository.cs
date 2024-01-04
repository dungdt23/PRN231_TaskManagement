using BusinessObjects.DTOs;

namespace Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        List<CategoryDTO> GetCategories();
    }
}