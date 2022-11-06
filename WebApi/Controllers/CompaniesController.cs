using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth.Model;
using WebApi.Data.Dtos;
using WebApi.Data.Entities;
using WebApi.Data.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesRepository _repo;
        private readonly IAuthorizationService _authorizationService;
        public CompaniesController(ICompaniesRepository repo, IAuthorizationService authorizationService)
        {
            this._repo = repo;
            this._authorizationService = authorizationService;
        }
        [HttpGet]
        [Route("{companyId}/GetAllWorks")]
        public async Task<IEnumerable<WorksDto>> GetAllCompanyWorks(int companyId)
        {
            var works = await _repo.GetAllCompanyWorks(companyId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, companyId, PolicyNames.CompanyEmployee);

            if (!authorizationResult.Succeeded)
                return null;

            return works.Select(e => new WorksDto(e.Id, e.Type, e.Description, e.CreationDate,
                e.ModifiedDate, e.StartDateTime, e.EndDateTime, e.IsPaused, e.ProductionOrder.Id));
        }
        [HttpGet]
        [Authorize(Roles = ERPRoles.Admin)]
        public async Task<IEnumerable<CompanyDto>> GetMany()
        {
            var companies = await _repo.GetManyAsync();
            return companies.Select(e => new CompanyDto(e.Id, e.Name, e.CreatedDate));
        }

        [HttpGet]
        [Route("{companyId}")]
        [Authorize(Roles = ERPRoles.Admin)]
        public async Task<ActionResult<CompanyDto>> Get(int companyId)
        {
            var company = await _repo.GetAsync(companyId);

            if (company is null)
                return NotFound();

            return new CompanyDto(company.Id, company.Name, company.CreatedDate);
        }

        [HttpPost]
        [Authorize(Roles = ERPRoles.Admin)]
        public async Task<ActionResult<CompanyDto>> Create(CreateCompanyDto createCompanyDto)
        {
            var company = new Company
            {
                Name = createCompanyDto.name,
                CreatedDate = DateTime.UtcNow
            };

            await _repo.CreateAsync(company);

            //201
            return Created("", new CompanyDto(company.Id, company.Name, company.CreatedDate));
        }

        [HttpPut]
        [Route("{companyId}")]
        [Authorize(Roles = ERPRoles.Admin)]
        public async Task<ActionResult<CompanyDto>> Update(int companyId, UpdateCompanyDto updateCompanyDto)
        {
            var company = await _repo.GetAsync(companyId);

            if (company is null)
                return NotFound();

            company.Name = updateCompanyDto.name;
            await _repo.UpdateAsync(company);

            return Ok(new CompanyDto(company.Id, company.Name, company.CreatedDate));
        }

        [Route("{companyId}")]
        [HttpDelete]
        [Authorize(Roles = ERPRoles.Admin)]
        public async Task<ActionResult> Delete(int companyId)
        {
            var company = await _repo.GetAsync(companyId);

            if(company is null)
                return NotFound();

            await _repo.DeleteAsync(company);

            //204
            return NoContent();
        }
    }
}
