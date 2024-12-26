using MediatR;
using SkillMiner.Domain.Entities.BackgroundTaskEntity;

namespace SkillMiner.Application.CQRS.Queries;

public record GetBackgroundJobStatusQuery(BackgroundTaskId BackgroundTaskId) : IRequest<BackgroundTaskStatus>;

public class GetBackgroundJobStatusQueryHandler
    (IBackgroundTaskRepository backgroundTaskRepository)
    : IRequestHandler<GetBackgroundJobStatusQuery, BackgroundTaskStatus>
{
    public async Task<BackgroundTaskStatus> Handle(GetBackgroundJobStatusQuery request, CancellationToken cancellationToken)
    {
        BackgroundTask? backgroundTask = await backgroundTaskRepository.GetByIdAsync(request.BackgroundTaskId, cancellationToken)
            ?? throw new Exception($"BackgroundTask with Id {request.BackgroundTaskId} not found.");

        return backgroundTask.Status;
    }
}
