using FluentValidation;
using MediatR;

namespace Revisio.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) :
        IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
               return await next();
            var context = new ValidationContext<TRequest>(request);
            var result =await Task.WhenAll( validators.Select(x => x.ValidateAsync(context, cancellationToken)));
            var failure = result.SelectMany(x => x.Errors).Where(x => x is not null).ToList();
            if (failure != null)
                throw new ValidationException(failure);
           return await next();
        }
    }
}
