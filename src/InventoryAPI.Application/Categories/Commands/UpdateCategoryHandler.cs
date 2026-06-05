using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Categories.Commands
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<bool>>
    {
        private readonly ICategoryReadRepository _readRepository;
        private readonly ICategoryWriteRepository _writeRepository;

        public UpdateCategoryHandler(ICategoryReadRepository readRepository, ICategoryWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public async Task<Result<bool>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _readRepository.GetByIdAsync(command.Id, cancellationToken);
            if (category is null)
                return Result<bool>.Failure("Category not found.");

            category.Update(command.Name, command.Description);
            await _writeRepository.UpdateAsync(category);

            return Result<bool>.Success(true);
        }
    }
}
