using FluentValidation;
using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.Queue;
using SkillMiner.Domain.Entities.BackgroundTaskEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Commands;

public record QueueWebScrapeJobsByTitleCommand(string JobTitle) : IRequest<Guid>;

public class QueueWebScrapeJobsByTitleCommandValidator : AbstractValidator<QueueWebScrapeJobsByTitleCommand>
{
    public QueueWebScrapeJobsByTitleCommandValidator()
    {
        RuleFor(x => x.JobTitle)
            .NotNull()
            .NotEmpty()
            .WithMessage("JobTitle cannot be null or empty.");
    }
}

public class QueueWebScrapeByJobTitleCommandHandler(
    ICommandQueueWriter commandQueueWriter,
    IBackgroundTaskRepository backgroundTaskRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<QueueWebScrapeJobsByTitleCommand, Guid>
{
    public async Task<Guid> Handle(QueueWebScrapeJobsByTitleCommand request, CancellationToken cancellationToken)
    {
        var backgroundTask = BackgroundTask.CreateNew();
        await backgroundTaskRepository.AddAsync(backgroundTask, cancellationToken);

        await commandQueueWriter.WriteAsync(new WebScrapeJobsByTitleQueuedCommand(request.JobTitle, backgroundTask.Id), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return backgroundTask.Id.Value;
    }
}