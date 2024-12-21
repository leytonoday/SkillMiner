using FluentValidation;
using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.JobListingEntity.Queue;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.JobListingEntity.Commands;

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
    IWebScrapingTaskRepository webScrapingTaskRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<QueueWebScrapeJobsByTitleCommand, Guid>
{
    public async Task<Guid> Handle(QueueWebScrapeJobsByTitleCommand request, CancellationToken cancellationToken)
    {
        var webScrapingTask = WebScrapingTask.CreateNew();
        await webScrapingTaskRepository.AddAsync(webScrapingTask, cancellationToken);

        await commandQueueWriter.WriteAsync(new WebScrapeJobsByTitleQueuedCommand(request.JobTitle, webScrapingTask.Id), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return webScrapingTask.Id.Value;
    }
}