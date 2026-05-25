using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Interfaces;
using MediatR;

namespace InventoryAPI.Application.Categories.Commands
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<bool>>
    {
        private readonly ICategoryReadRepository _readRepository;
        private readonly ICategoryWriteRepository _writeRepository;

        public DeleteCategoryHandler(ICategoryReadRepository readRepository, ICategoryWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public async Task<Result<bool>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var exists = await _readRepository.ExistsAsync(command.Id);
            if (!exists)
                return Result<bool>.Failure("Category not found.");

            await _writeRepository.DeleteAsync(command.Id);

            return Result<bool>.Success(true);
        }
    }
}
