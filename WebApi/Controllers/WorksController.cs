using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth.Model;
using WebApi.Data.Dtos;
using WebApi.Data.Entities;
using WebApi.Data.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/productionorders/{orderId}/works")]
    public class WorksController : ControllerBase
    {
        private readonly IWorksRepository _repo;
        private readonly IProductionOrdersRepository _productionOrderRepository;
        private readonly IAuthorizationService _authorizationService;

        public WorksController(IWorksRepository repo, IProductionOrdersRepository _productionOrderRepository, IAuthorizationService authorizationService)
        {
            this._repo = repo;
            this._productionOrderRepository = _productionOrderRepository;
            this._authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<WorksDto>> GetMany(int? companyId, int? orderId)
        {
            var works = await _repo.GetManyAsync(companyId, orderId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, companyId, PolicyNames.CompanyEmployee);

            if (!authorizationResult.Succeeded)
                return null;

            return works.Select(e => new WorksDto(e.Id, e.Type, e.Description, e.CreationDate,
                e.ModifiedDate, e.StartDateTime, e.EndDateTime, e.IsPaused, e.ProductionOrder.Id, e.UserId));
        }

        [HttpGet]
        [Route("{workId}")]
        public async Task<ActionResult<WorksDto>> Get(int? companyId, int? orderId, int? workId)
        {
            if (companyId is null || orderId is null || workId is null)
                return BadRequest("Company id, order id or workId is null");

            var work = await _repo.GetAsync(workId.Value, orderId.Value, companyId.Value);

            if (work is null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, companyId, PolicyNames.CompanyEmployee);

            if (!authorizationResult.Succeeded)
                return Forbid();

            return new WorksDto(work.Id, work.Type, work.Description, work.CreationDate,
                work.ModifiedDate, work.StartDateTime, work.EndDateTime, work.IsPaused, work.ProductionOrder.Id, work.UserId);
        }

        [HttpPost]
        [Authorize(Roles = ERPRoles.Representative)]
        public async Task<ActionResult<WorksDto>> Create(int companyId, int orderId, CreateWorksDto createWorkDto)
        {
            var order = await _productionOrderRepository.GetAsync(orderId, companyId);

            if (order is null)
                return NotFound($"Couldnt find an order with id {orderId}");
            
            var work = new Work
            {
                Type = createWorkDto.type,
                Description = createWorkDto.description,
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                StartDateTime = null,
                EndDateTime = null,
                IsPaused = false,
                ProductionOrder = order,
                UserId = createWorkDto.userId
            };

            await _repo.CreateAsync(work);

            //201
            return Created("", new WorksDto(work.Id, work.Type, work.Description, work.CreationDate,
                work.ModifiedDate, work.StartDateTime, work.EndDateTime, work.IsPaused, work.ProductionOrder.Id, work.UserId));
        }

        [HttpPut]
        [Route("{workId}")]
        [Authorize(Roles = ERPRoles.Worker + "," + ERPRoles.Representative)]
        public async Task<ActionResult<WorksDto>> Update(int? companyId, int? orderId, int? workId,
            UpdateWorksDto updateWorksDto)
        {
            if (companyId is null || orderId is null || workId is null)
                return BadRequest("Company id, order id or workId is null");

            var work = await _repo.GetAsync(workId.Value, orderId.Value, companyId.Value);

            if (work is null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, work, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid();

            work.Type = updateWorksDto.type is null ? work.Type : updateWorksDto.type;
            work.Description = updateWorksDto.description is null ? work.Description : updateWorksDto.description;
            work.ModifiedDate = DateTime.UtcNow;
            work.StartDateTime = updateWorksDto.startDateTime;
            work.EndDateTime = updateWorksDto.endDatetime;
            work.IsPaused = updateWorksDto.isPaused is null ? work.IsPaused : updateWorksDto.isPaused.Value;

            await _repo.UpdateAsync(work);

            return Ok(new WorksDto(work.Id, work.Type, work.Description, work.CreationDate,
                work.ModifiedDate, work.StartDateTime, work.EndDateTime, work.IsPaused, work.ProductionOrder.Id, work.UserId));
        }

        [Route("{workId}")]
        [HttpDelete]
        [Authorize(Roles = ERPRoles.Representative)]
        public async Task<ActionResult> Delete(int? companyId, int? orderId, int? workId)
        {
            if (companyId is null || orderId is null || workId is null)
                return BadRequest("Company id, order id or workId is null");

            var work = await _repo.GetAsync(workId.Value, orderId.Value, companyId.Value);

            if (work is null)
                return NotFound();

            await _repo.DeleteAsync(work);

            //204
            return NoContent();
        }
    }
}
