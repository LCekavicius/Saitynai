using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Dtos;
using WebApi.Data.Entities;
using WebApi.Data.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/productionorders")]
    public class ProductionOrdersController : ControllerBase
    {
        private readonly IProductionOrdersRepository _repo;
        private readonly ICompaniesRepository _companiesRepo;
        public ProductionOrdersController(IProductionOrdersRepository repo, ICompaniesRepository _companiesRepo)
        {
            this._repo = repo;
            this._companiesRepo = _companiesRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductionOrderDto>> GetMany(int? companyId)
        {
            var productionOrders = await _repo.GetManyAsync(companyId);
            return productionOrders.Select(e => new ProductionOrderDto(e.Id, e.ProductName, e.CreationDate,
                    e.ModifiedDate,e .StartDateTime, e.EndDateTime, e.Company.Id));
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<ActionResult<ProductionOrderDto>> Get(int? companyId, int? orderId)
        {
            if (companyId is null || orderId is null)
                return BadRequest("Company id or order id is null");

            var productionOrder = await _repo.GetAsync(orderId.Value, companyId.Value);

            if (productionOrder is null)
                return NotFound();

            return new ProductionOrderDto(productionOrder.Id, productionOrder.ProductName,
                productionOrder.CreationDate, productionOrder.ModifiedDate,
                productionOrder.StartDateTime, productionOrder.EndDateTime, productionOrder.Company.Id);
        }

        [HttpPost]
        public async Task<ActionResult<ProductionOrderDto>> Create(int companyId, CreateProductionOrderDto createProductionOrderDto)
        {
            var company = await _companiesRepo.GetAsync(companyId);

            if (company is null)
                return NotFound($"Couldnt find a company with id {companyId}");

            var productionOrder = new ProductionOrder
            {
                ProductName = createProductionOrderDto.productName,
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                StartDateTime = null,
                EndDateTime = null,
                Company = company
            };

            await _repo.CreateAsync(productionOrder);

            //201
            return Created("", new ProductionOrderDto(productionOrder.Id, productionOrder.ProductName,
                productionOrder.CreationDate, productionOrder.ModifiedDate,
                productionOrder.StartDateTime, productionOrder.EndDateTime, productionOrder.Company.Id));
        }

        [HttpPut]
        [Route("{orderId}")]
        public async Task<ActionResult<ProductionOrderDto>> Update(int? companyId, int? orderId,
            UpdateProductionOrderDto updateProductionOrderDto)
        {
            if (companyId is null || orderId is null)
                return BadRequest("Company id or order id is null");

            var productionOrder = await _repo.GetAsync(orderId.Value, companyId.Value);

            if (productionOrder is null)
                return NotFound();

            productionOrder.ProductName = updateProductionOrderDto.productName;
            productionOrder.ModifiedDate = DateTime.UtcNow;
            productionOrder.StartDateTime = updateProductionOrderDto.startDateTime;
            productionOrder.EndDateTime = updateProductionOrderDto.endDatetime;
            await _repo.UpdateAsync(productionOrder);

            return Ok(new ProductionOrderDto(productionOrder.Id, productionOrder.ProductName,
                productionOrder.CreationDate, productionOrder.ModifiedDate,
                productionOrder.StartDateTime, productionOrder.EndDateTime, productionOrder.Company.Id));
        }

        [Route("{orderId}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int? companyId, int? orderId)
        {
            if (companyId is null || orderId is null)
                return BadRequest("Company id or order id is null");

            var order = await _repo.GetAsync(orderId.Value, companyId.Value);

            if(order is null)
                return NotFound();

            await _repo.DeleteAsync(order);

            //204
            return NoContent();
        }
    }
}
