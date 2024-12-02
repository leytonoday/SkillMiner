using MediatR;

namespace SkillMiner.Application.CQRS.Commands;

public record GenerateDesiredSkillsByJobTitleCommand(string JobTitle) : IRequest;

public class GenerateDesiredSkillsByJobTitleCommandHandler : IRequestHandler<GenerateDesiredSkillsByJobTitleCommand>
{
    public Task Handle(GenerateDesiredSkillsByJobTitleCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}