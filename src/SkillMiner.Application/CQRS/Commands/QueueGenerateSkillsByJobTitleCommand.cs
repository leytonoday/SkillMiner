using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.Queue;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Commands;

public class QueueGenerateSkillsByJobTitleCommand : IRequest<Guid>;

public class QueueGenerateSkillsByJobTitleCommandHandler(
    ICommandQueueForProducer commandQueueForProducer,
    IUnitOfWork unitOfWork) : IRequestHandler<QueueGenerateSkillsByJobTitleCommand, Guid>
{
    public async Task<Guid> Handle(QueueGenerateSkillsByJobTitleCommand request, CancellationToken cancellationToken)
    {
        Guid trackingId = await commandQueueForProducer.WriteAsync(new GenerateSkillsByJobTitleQueuedCommand(), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return trackingId;
    }
}