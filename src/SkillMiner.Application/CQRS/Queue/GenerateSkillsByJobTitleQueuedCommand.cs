using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Application.CQRS.Queue;

public record GenerateSkillsByJobTitleQueuedCommand : QueuedCommand;

public class GenerateSkillsByJobTitleQueuedCommandHandler : IQueuedCommandHandler<GenerateSkillsByJobTitleQueuedCommand>
{
    public Task Handle(GenerateSkillsByJobTitleQueuedCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}