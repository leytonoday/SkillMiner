using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Application.CQRS.Queries;

public record GetCommandQueueMessageProcessingStatusQuery(Guid TrackingId) : IRequest<ProcessingStatus>;

public class GetBackgroundJobStatusQueryHandler
    (ICommandQueueForProducer commandQueueForProducer)
    : IRequestHandler<GetCommandQueueMessageProcessingStatusQuery, ProcessingStatus>
{
    public async Task<ProcessingStatus> Handle(GetCommandQueueMessageProcessingStatusQuery request, CancellationToken cancellationToken)
    {
        var processingStatus = await commandQueueForProducer.GetCommandQueueMessageProcessingStatusAsync(request.TrackingId, cancellationToken)
            ?? throw new Exception($"CommandQueueMessage not found with tracking Id {request.TrackingId}");

        return processingStatus;
    }
}
