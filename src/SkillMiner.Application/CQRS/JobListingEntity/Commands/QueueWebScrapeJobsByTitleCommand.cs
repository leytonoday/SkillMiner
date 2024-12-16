using FluentValidation;
using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.JobListingEntity.Queue;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.JobListingEntity.Commands;

public record QueueWebScrapeJobsByTitleCommand(string JobTitle) : IRequest;

public class QueueWebScrapeJobsByTitleCommandValidator : AbstractValidator<QueueWebScrapeJobsByTitleCommand>
{
    public QueueWebScrapeJobsByTitleCommandValidator()
    {
        RuleFor(x => x.JobTitle)
            .NotEmpty()
            .WithMessage("JobTitle cannot be null or empty.");
    }
}

public class QueueWebScrapeByJobTitleCommandHandler(
    ICommandQueueWriter commandQueueWriter,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<QueueWebScrapeJobsByTitleCommand>
{
    public async Task Handle(QueueWebScrapeJobsByTitleCommand request, CancellationToken cancellationToken)
    {
        await commandQueueWriter.WriteAsync(new WebScrapeJobsByTitleCommandQueuedCommand(request.JobTitle), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}