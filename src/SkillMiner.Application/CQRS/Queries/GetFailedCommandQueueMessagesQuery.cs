using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Application.CQRS.Queries;

public record GetFailedCommandQueueMessagesQuery : IRequest<IEnumerable<Guid>>;

public class GetFailedCommandQueueMessagesQueryHandler
    (ICommandQueueForProducer commandQueueForProducer)
    : IRequestHandler<GetFailedCommandQueueMessagesQuery, IEnumerable<Guid>>
{
    public async Task<IEnumerable<Guid>> Handle(GetFailedCommandQueueMessagesQuery request, CancellationToken cancellationToken)
    {
        return await commandQueueForProducer.GetFailedAsync(cancellationToken);
    }
}