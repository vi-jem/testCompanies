using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myrestful.Infrastructure;
using myrestful.Models;

namespace myrestful.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DBContextEmployees _context;

        public CompanyRepository(DBContextEmployees context)
        {
            _context = context;
        }

        public async Task<IEntity> Create(Company newCompany)
        {
            _context.Companies.Add(newCompany);
            await _context.SaveChangesAsync();
            return new Entity { ID = newCompany.ID };
        }

        public async Task<bool> Delete(int id)
        {
            Company companyToDelete = _context.Companies.SingleOrDefault(item => item.ID == id);
            _context.Companies.Remove(companyToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SearchResult> Filter(SearchQuery parameters)
        {
            IQueryable<Company> result = _context.Companies;
            if (parameters.Keyword != null)
            {
                result = result
                    .Where(item =>
                        item.Name.Contains(parameters.Keyword) ||
                        item.Employees.Any(employee =>
                            employee.FirstName.Contains(parameters.Keyword) ||
                            employee.LastName.Contains(parameters.Keyword)
                        )
                    );
            }
            if (parameters.EmployeeDateOfBirthFrom.HasValue)
            {
                result = result
                    .Where(item =>
                        item.Employees.Any(employee =>
                            employee.DateOfBirth >= parameters.EmployeeDateOfBirthFrom.Value));
            }
            if (parameters.EmployeeDateOfBirthTo.HasValue)
            {
                result = result
                    .Where(item =>
                        item.Employees.Any(employee =>
                            employee.DateOfBirth <= parameters.EmployeeDateOfBirthTo.Value));
            }
            if (parameters.EmployeeJobTitles.Any())
            {
                result = result
                    .Where(item =>
                        item.Employees.Any(employee =>
                            parameters.EmployeeJobTitles.Contains(employee.JobTitle)));
            }
            List<Company> list = await result.Include(item => item.Employees).ToListAsync();
            return new SearchResult { Results = list};
        }

        public async Task<IEntity> GetById(int id)
        {
            Company company = await _context.Companies
                .Include(item => item.Employees)
                .SingleOrDefaultAsync(item => item.ID == id);
            return company;
        }

        public async Task<bool> Update(int id, Company newValues)
        {
            Company companyToUpdate = await _context.Companies
                .Where(company => company.ID == id)
                .Include(item => item.Employees)
                .SingleOrDefaultAsync();


            _context.Entry(companyToUpdate).CurrentValues.SetValues(newValues);

            foreach (Employee existingEmployee in companyToUpdate.Employees.ToList())
            {
                if (!newValues.Employees.Any(item => item.ID == existingEmployee.ID))
                {
                    _context.Employees.Remove(existingEmployee);
                }
            }

            foreach (Employee newEmployeeValues in companyToUpdate.Employees)
            {
                Employee existingEmployee = companyToUpdate.Employees
                    .Where(item => item.ID == newEmployeeValues.ID).SingleOrDefault();
                if (existingEmployee != null)
                {
                    _context.Entry(existingEmployee).CurrentValues.SetValues(newEmployeeValues);
                }
                else
                {
                    Employee newEmployee = new Employee
                    {
                        FirstName = newEmployeeValues.FirstName,
                        LastName = newEmployeeValues.LastName,
                        DateOfBirth = newEmployeeValues.DateOfBirth,
                        JobTitle = newEmployeeValues.JobTitle
                    };
                    companyToUpdate.Employees.Add(newEmployee);
                }
            }
            _context.SaveChanges();

            return true;
        }
    }
}