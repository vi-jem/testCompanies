using System.Collections.Generic;
using System.Linq;
using myrestful.Infrastructure;
using myrestful.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace myrestful.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly DBContextEmployees _context;
        public CompanyController(DBContextEmployees context)
        {
            _context = context;
        }

        // GET /company
        public ActionResult<IEnumerable<IEntity>> Get()
        {
            List<Company> companies = _context.Companies
                .Include(item => item.Employees)
                .ToList();

            return companies;
        }

        // GET /company/id
        [Route("{id}")]
        public ActionResult<IEntity> Get(int id)
        {
            Company company = _context.Companies
                .Include(item => item.Employees)
                .SingleOrDefault(item => item.ID == id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // POST /company/create
        [Route("create")]
        [HttpPost]
        public ActionResult<IEntity> Create(Company newCompany)
        {
            _context.Companies.Add(newCompany);
            _context.SaveChanges();
            IEntity created = new Entity { ID = newCompany.ID };

            return Created(Url.Action("Get", created.ID), created);
        }

        // POST /company/search
        [Route("search")]
        [HttpPost]
        public ActionResult<SearchResult> Search(SearchQuery parameters)
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

            return new SearchResult { Results = result.Include(item => item.Employees).ToList() };
        }

        // PUT /company/update/id
        [Route("update/{id}")]
        [HttpPut]
        public ActionResult Update(int id, Company newValues)
        {
            Company companyToUpdate = _context.Companies
                .Where(company => company.ID == id)
                .Include(item => item.Employees)
                .SingleOrDefault();

            if (companyToUpdate == null)
            {
                return NotFound();
            }

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

            return Ok();
        }

        // DELETE /company/delete/id
        [Route("delete/{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Company companyToDelete = _context.Companies.SingleOrDefault(item => item.ID == id);
            if (companyToDelete == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(companyToDelete);
            _context.SaveChanges();

            return NoContent();
        }
    }
}