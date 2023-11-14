using NSubstitute;
using Microsoft.Extensions.Logging;

namespace Example;

public class LoggerAssertion
{
    [Test]
    public void Test1()
    {
        var logger = Substitute.For<MockLogger<FooService>>();
        var subject = new FooService(logger);
        
        subject.Bar(123);
        
        logger.Received(1).LogInformation("The value passed in is 123");
    }
}

public class FooService
{
    private readonly ILogger<FooService> _logger;

    public FooService(ILogger<FooService> logger)
    {
        _logger = logger;
    }

    public void Bar(int value)
    {
        _logger.LogInformation("The value passed in is {Value}", value);
    }
}

public abstract class MockLogger<T> : ILogger<T>
{
    void ILogger.Log<TState>(
        LogLevel logLevel, 
        EventId eventId, 
        TState state, 
        Exception exception, 
        Func<TState, Exception, string> formatter) => 
        Log(logLevel, formatter(state, exception));

    public abstract void Log(LogLevel logLevel, string message);

    public virtual bool IsEnabled(LogLevel logLevel) => true;

    public abstract IDisposable BeginScope<TState>(TState state);
}