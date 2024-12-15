using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.JobListingEntity.Queue;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.JobListingEntity.Commands;

public record QueueGenerateSkillsByJobTitleCommand(string JobTitle) : IRequest;

public class QueueWebScrapeByJobTitleCommandHandler(
    ICommandQueueWriter commandQueueWriter,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<QueueGenerateSkillsByJobTitleCommand>
{
    public async Task Handle(QueueGenerateSkillsByJobTitleCommand request, CancellationToken cancellationToken)
    {
        await commandQueueWriter.WriteAsync(new GenerateSkillsByJobTitleQueuedCommand(request.JobTitle), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}