using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity>
         where SaveViewModel : class
         where ViewModel : class
         where Entity : class

    {
        Task<SaveViewModel> CreateAsync(SaveViewModel viewModel);
        Task<SaveViewModel> UpdateAsync(SaveViewModel viewModel, int id);
        Task DeleteAsync(int id);
        Task<ViewModel> GetByIdAsync(int id);
        Task<List<ViewModel>> GetAllAsync();
        Task<SaveViewModel> GetByIdSaveViewModelAsync(int id);
    }
}
