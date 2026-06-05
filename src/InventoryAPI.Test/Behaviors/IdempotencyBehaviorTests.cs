using FluentAssertions;
using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Test.Behaviors
{
    public record TestIdempotentCommand(string RequestId) : IRequest<Result<int>>, IIdempotentCommand;
    public class IdempotencyBehaviorTests
    {
        private readonly Mock<IIdempotencyRepository> _repositoryMock;
        private readonly IdempotencyBehavior<TestIdempotentCommand, Result<int>> _behavior;

        public IdempotencyBehaviorTests()
        {
            _repositoryMock = new Mock<IIdempotencyRepository>();
            _behavior = new IdempotencyBehavior<TestIdempotentCommand, Result<int>>(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCachedResponse_WhenRequestIdAlreadyExists()
        {
            var requestId = Guid.NewGuid().ToString();
            var cachedResponse = Result<int>.Success(42);
            var stored = IdempotencyKey.Create(requestId, System.Text.Json.JsonSerializer.Serialize(cachedResponse));
            _repositoryMock
                .Setup(r => r.GetByRequestIdAsync(requestId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(stored);

            var command = new TestIdempotentCommand(requestId);
            var result = await _behavior.Handle(command, () => Task.FromResult(Result<int>.Success(99)), CancellationToken.None);

            result.Value.Should().Be(42);
            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<IdempotencyKey>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldSaveAndReturnNewResponse_WhenRequestIdIsNew()
        {
            var requestId = Guid.NewGuid().ToString();
            _repositoryMock
                .Setup(r => r.GetByRequestIdAsync(requestId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IdempotencyKey?)null);
            _repositoryMock
                .Setup(r => r.SaveAsync(It.IsAny<IdempotencyKey>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new TestIdempotentCommand(requestId);
            var result = await _behavior.Handle(command, () => Task.FromResult(Result<int>.Success(99)), CancellationToken.None);

            result.Value.Should().Be(99);
            _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<IdempotencyKey>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
