using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.Queue;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Commands;

public class QueueGenerateSkillsByProfessionCommand : IRequest<Guid>;

public class QueueGenerateSkillsByProfessionCommandHandler(
    ICommandQueueForProducer commandQueueForProducer,
    IMicrosoftJobListingRepository microsoftJobListingRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<QueueGenerateSkillsByProfessionCommand, Guid>
{
    public async Task<Guid> Handle(QueueGenerateSkillsByProfessionCommand request, CancellationToken cancellationToken)
    {
        bool hasUnprocessedJobListings = await microsoftJobListingRepository.HasUnprocessedJobListingsAsync(cancellationToken);

        if (!hasUnprocessedJobListings)
        {
            throw new Exception("No Job Listings to analyze. Invoke the job web scaper.");
        }

        Guid trackingId = await commandQueueForProducer.WriteAsync(new GenerateSkillsByProfessionQueuedCommand(), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return trackingId;
    }
}