using InventoryAPI.Application.Products.Commands;
using InventoryAPI.Application.Products.Queries;
using InventoryAPI.Controllers.Request;
using InventoryAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize=20, [FromQuery]int? categoryId = null)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(page, pageSize));

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var requestId = HttpContext.Request.Headers["Idempotency-Key"].FirstOrDefault() ?? Guid.NewGuid().ToString();
            var command = new CreateProductCommand(
                requestId,
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId
            );

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            var commandWithId = command with { Id = id };
            var result = await _mediator.Send(commandWithId);

            return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
        }
    }
}
