using FluentValidation;
using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.Queue;
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
    ICommandQueueForProducer commandQueueForProducer,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<QueueWebScrapeJobsByTitleCommand, Guid>
{
    public async Task<Guid> Handle(QueueWebScrapeJobsByTitleCommand request, CancellationToken cancellationToken)
    {
        Guid trackingId = await commandQueueForProducer.WriteAsync(new WebScrapeJobsByTitleQueuedCommand(request.JobTitle), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return trackingId;
    }
}