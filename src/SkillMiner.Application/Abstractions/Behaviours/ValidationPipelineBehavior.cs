using FluentValidation;
using MediatR;
using SkillMiner.Domain.Shared.Errors;

namespace SkillMiner.Application.Abstractions.Behaviours;

/// <summary>
/// Pipeline behavior responsible for validating the specified <typeparamref name="TRequest"/> using registered FluentValidation validators
/// and returning a <typeparamref name="TResponse"/> with validation errors if any.
/// </summary>
public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest
{
    /// <summary>
    /// Handles the validation of the request and executes the next step in the pipeline if validation succeeds.
    /// </summary>
    /// <param name="request">The request to be validated.</param>
    /// <param name="next">The next step in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token for handling asynchronous operations.</param>
    /// <returns>A <typeparamref name="TResponse"/> with validation errors if any, or the result of the next step in the pipeline.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Validate the request and collect errors
        var validationErrors = new List<Error>();
        foreach (var validator in validators)
        {
            var results = await validator.ValidateAsync(request, cancellationToken);
            if (results.Errors.Count > 0)
            {
                validationErrors.AddRange(results.Errors.Select(f => new Error($"Validation.{typeof(TRequest).Name}.{f.PropertyName}", f.ErrorMessage)));
            }
        }

        // If there are any errors, throw error
        if (validationErrors.Count > 0)
        {
            throw new ErrorException(validationErrors);
        }

        // Otherwise, continue with the pipeline
        return await next();
    }
}