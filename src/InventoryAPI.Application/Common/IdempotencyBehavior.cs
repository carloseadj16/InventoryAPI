using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Common
{
    public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentCommand
    {
        private readonly IIdempotencyRepository _idempotencyRepository;

        public IdempotencyBehavior(IIdempotencyRepository idempotencyRepository)
        {
            _idempotencyRepository = idempotencyRepository;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var existing = await _idempotencyRepository.GetByRequestIdAsync(request.RequestId, cancellationToken);
            if (existing is not null)
                return JsonSerializer.Deserialize<TResponse>(existing.Response)!;

            var response = await next();

            var idempotencyKey = IdempotencyKey.Create(request.RequestId, JsonSerializer.Serialize(response));
            await _idempotencyRepository.SaveAsync(idempotencyKey, cancellationToken);

            return response;
        }
    }
}
