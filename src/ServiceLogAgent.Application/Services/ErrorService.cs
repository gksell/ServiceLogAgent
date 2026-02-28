using ServiceLogAgent.Application.Abstractions;

namespace ServiceLogAgent.Application.Services;

public class ErrorService : IErrorService
{
    public Task ThrowForDemoAsync()
    {
        throw new InvalidOperationException("Intentional exception for log testing.");
    }
}
