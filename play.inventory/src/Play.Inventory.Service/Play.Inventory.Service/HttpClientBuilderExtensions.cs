using Play.Inventory.Service.Clients;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Play.Inventory.Service;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddPolicyHandlersWithLogging(this IHttpClientBuilder builder, ILogger<CaterlogClient> logger)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                5,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning(
                        "Delaying for {delay}ms, then making retry {retry}.",
                        timespan.TotalMilliseconds,
                        retryAttempt
                    );
                });

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(1));

        return builder
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);
    }
}
