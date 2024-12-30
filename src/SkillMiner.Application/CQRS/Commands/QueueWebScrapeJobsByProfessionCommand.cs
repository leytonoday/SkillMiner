using FluentValidation;
using MediatR;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.CQRS.Queue;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Commands;

public record QueueWebScrapeJobsByProfessionCommand(string Profession) : IRequest<Guid>;

public class QueueWebScrapeJobsByProfessionCommandValidator : AbstractValidator<QueueWebScrapeJobsByProfessionCommand>
{
    public QueueWebScrapeJobsByProfessionCommandValidator()
    {
        RuleFor(x => x.Profession)
            .NotNull()
            .NotEmpty()
            .WithMessage("Profession cannot be null or empty.");
    }
}

public class QueueWebScrapeByProfessionCommandHandler(
    ICommandQueueForProducer commandQueueForProducer,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<QueueWebScrapeJobsByProfessionCommand, Guid>
{
    public async Task<Guid> Handle(QueueWebScrapeJobsByProfessionCommand request, CancellationToken cancellationToken)
    {
        Guid trackingId = await commandQueueForProducer.WriteAsync(new WebScrapeJobsByProfessionQueuedCommand(request.Profession), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return trackingId;
    }
}