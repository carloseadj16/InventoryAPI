using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Domain.Entities
{
    public class IdempotencyKey
    {
        public Guid Id { get; private set; }
        public string RequestId { get; private set; } = string.Empty;
        public string Response { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        private IdempotencyKey() { }

        public static IdempotencyKey Create(string requestId, string response)
        {
            return new IdempotencyKey
            {
                Id = Guid.NewGuid(),
                RequestId = requestId,
                Response = response,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
