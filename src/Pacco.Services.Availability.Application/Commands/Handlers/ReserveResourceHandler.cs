using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public sealed class ReserveResourceHandler : ICommandHandler<ReserveResource>
    {
        private readonly IResourcesRepository _repository;
        private readonly IEventProcessor _processor;

        public ReserveResourceHandler(IResourcesRepository repository, IEventProcessor processor)
        {
            _repository = repository;
            _processor = processor;
        }

        public async Task HandleAsync(ReserveResource command)
        {
            var resource = await _repository.GetAsync(command.ResourceId);
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }
            
            var reservation = new Reservation(command.DateTime, command.Priority);
            resource.AddReservation(reservation);
            await _repository.UpdateAsync(resource);
            await _processor.ProcessAsync(resource.Events);
        }
    }
}