using System.Collections.Generic;
using myrestful.Models;
using Microsoft.AspNetCore.Mvc;
using myrestful.Services;
using System.Threading.Tasks;

namespace myrestful.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;
        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        // GET /company/id
        [Route("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            IEntity company = await _service.GetById(id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        // POST /company/create
        [Route("create")]
        [HttpPost]
        public async Task<ActionResult> Create(Company newCompany)
        {
            IEntity created = await _service.Create(newCompany);
            string path = Url.Action("Get", created.ID) ?? $"/company/{created.ID}";
            return Created(path, created);
        }

        // POST /company/search
        [Route("search")]
        [HttpPost]
        public async Task<ActionResult> Search(SearchQuery parameters)
        {
            SearchResult result = await _service.Filter(parameters);
            return Ok(result);
        }

        // PUT /company/update/id
        [Route("update/{id}")]
        [HttpPut]
        public async Task<ActionResult> Update(int id, Company newValues)
        {
            IEntity companyToUpdate = await _service.GetById(id);
            if (companyToUpdate == null)
            {
                return NotFound();
            }
            bool result = await _service.Update(id, newValues);
            return Ok();
        }

        // DELETE /company/delete/id
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            IEntity companyToDelete = await _service.GetById(id);
            if (companyToDelete == null)
            {
                return NotFound();
            }
            bool result = await _service.Delete(id);
            return NoContent();
        }
    }
}