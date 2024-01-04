using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using DataAccess;
using Repositories.IRepositories;

namespace Repositories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        Prn231AsmTaskManagementContext _context;
        IMapper _mapper;
        CategoryDAO categoryDAO;
        public CategoryRepository(Prn231AsmTaskManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<CategoryDTO> GetCategories()
        {
            categoryDAO = new CategoryDAO(_context);
            var categories = categoryDAO.GetCategories();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }
    }
}
