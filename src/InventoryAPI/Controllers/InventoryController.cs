using InventoryAPI.Application.Movs.Commands;
using InventoryAPI.Controllers.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movements")]
        public async Task<IActionResult> RegisterMovement([FromBody] RegisterMovementRequest request)
        {
            var requestId = HttpContext.Request.Headers["Idempotency-Key"].FirstOrDefault() ?? Guid.NewGuid().ToString();
            var command = new RegisterMovCommand(
                requestId,
                request.ProductId,
                request.Quantity,
                request.MovementType,
                request.Reason);

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(RegisterMovement), new { id = result.Value }, result.Value);
        }
    }
}
