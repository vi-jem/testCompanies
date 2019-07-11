using System.Collections.Generic;
using System.Threading.Tasks;
using myrestful.Models;

namespace myrestful.Repositories
{
    public interface ICompanyRepository
    {
        Task<IEntity> GetById(int id);
        Task<IEntity> Create(Company newCompany);
        Task<SearchResult> Filter(SearchQuery parameters);
        Task<bool> Update(int id, Company newValues);
        Task<bool> Delete(int id);
    }
}