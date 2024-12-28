using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Application.CQRS.Queries;

public record GetPendingAndProcessingCommandQueueMessagesQuery : IRequest<IDictionary<Guid, ProcessingStatus>>;

public class GetPendingAndProcessingCommandQueueMessagesQueryHandler
    (ICommandQueueForProducer commandQueueForProducer)
    : IRequestHandler<GetPendingAndProcessingCommandQueueMessagesQuery, IDictionary<Guid, ProcessingStatus>>
{
    public Task<IDictionary<Guid, ProcessingStatus>> Handle(GetPendingAndProcessingCommandQueueMessagesQuery request, CancellationToken cancellationToken)
    {
        return commandQueueForProducer.GetPendingAndProcessingAsync(cancellationToken);
    }
}