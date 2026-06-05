using InventoryAPI.Application.Common;
using InventoryAPI.Domain.Entities;
using InventoryAPI.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAPI.Application.Categories.Commands
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<int>>
    {
        private readonly ICategoryWriteRepository _writeRepository;

        public CreateCategoryHandler(ICategoryWriteRepository writeRepository)
        {
            _writeRepository = writeRepository;
        }

        public async Task<Result<int>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = Category.Create(command.Name,command.Description);
            var Id = await _writeRepository.AddAsync(category);

            return Result<int>.Success(Id);
        }
    }
}
