using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.Queue;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Commands;

public class QueueGenerateSkillsByProfessionCommand : IRequest<Guid>;

public class QueueGenerateSkillsByProfessionCommandHandler(
    ICommandQueueForProducer commandQueueForProducer,
    IUnitOfWork unitOfWork) : IRequestHandler<QueueGenerateSkillsByProfessionCommand, Guid>
{
    public async Task<Guid> Handle(QueueGenerateSkillsByProfessionCommand request, CancellationToken cancellationToken)
    {
        Guid trackingId = await commandQueueForProducer.WriteAsync(new GenerateSkillsByProfessionQueuedCommand(), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return trackingId;
    }
}