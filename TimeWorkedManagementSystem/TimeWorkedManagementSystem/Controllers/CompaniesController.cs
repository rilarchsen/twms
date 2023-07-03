using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.DTOs;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    public class CompaniesController : ApiControllerBase
    {
        private readonly UserDbContext _dbContext;
        private readonly IUserService _userService;

        public CompaniesController(UserDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }
        
        // GET: Companies
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(Company[]))]
        public async Task<IActionResult> GetAllCompanies()
        {
            return Ok(await _dbContext.Companies.ToListAsync());
        }
        
        // GET: Companies/{id}
        [HttpGet("{id}")]
        [SwaggerResponse(200, "OK", typeof(Company))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetCompanyDetails(Guid id)
        {
            Company? company;
            if ((company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id)) is null)
            {
                return NotFound(id);
            }

            return Ok(company);
        }
        
        // PUT: Companies
        [HttpPut]
        [SwaggerResponse(200, "OK", typeof(Company))]
        public async Task<IActionResult> AddCompany(CreateCompanyRequest request)
        {
            var company = _dbContext.Companies.Add(new Company
            {
                Name = request.Name,
                Email = request.Email
            });
            await _dbContext.SaveChangesAsync();
            return Ok(company.Entity);
        }
        
        // PUT: Companies/Edit
        [HttpPut("Edit")]
        [SwaggerResponse(200, "OK", typeof(Company))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> EditCompany(EditCompanyRequest request)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == request.CompanyId);
            if (company is null)
            {
                return NotFound(request.CompanyId);
            }

            company.Name = request.Name;
            company.Email = request.Email;
            await _dbContext.SaveChangesAsync();
            return Ok(company);
        }
        
        // DELETE: Companies/{id}
        [HttpDelete("{id}")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company is null)
            {
                return NotFound(id);
            }

            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
