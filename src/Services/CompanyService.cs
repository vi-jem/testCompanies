using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myrestful.Models;
using myrestful.Repositories;

namespace myrestful.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepo;

        public CompanyService(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        public async Task<IEntity> Create(Company newCompany)
        {
            return await _companyRepo.Create(newCompany);
        }

        public async Task<bool> Delete(int id)
        {
            return await _companyRepo.Delete(id);
        }

        public async Task<SearchResult> Filter(SearchQuery parameters)
        {
            if(!parameters.EmployeeJobTitles.Any())
            {
                parameters.EmployeeJobTitles = new List<EJobTitle>();
            }

            return await _companyRepo.Filter(parameters);
        }

        public async Task<IEnumerable<IEntity>> GetAll()
        {
            return await _companyRepo.GetAll();
        }

        public async Task<IEntity> GetById(int id)
        {
            return await _companyRepo.GetById(id);
        }

        public async Task<bool> Update(int id, Company newValues)
        {
            return await _companyRepo.Update(id, newValues);
        }
    }
}