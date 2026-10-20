using log4net;
using System;
using System.Threading;
using System.Threading.Tasks;

public static class ApiFallbackManager
{
    private static readonly ILog log = LogManager.GetLogger("ApiFallbackLogger");

    /// <summary>
    /// Executes an API function with a specified timeout. If the API fails, times out,
    /// or throws an exception, it seamlessly executes the direct Database fallback function.
    /// </summary>
    /// <typeparam name="T">Result Type</typeparam>
    /// <param name="apiFunc">Asynchronous API call delegate</param>
    /// <param name="dbFallbackFunc">Asynchronous or synchronous DB Fallback delegate</param>
    /// <param name="actionName">Human readable action name for logging</param>
    /// <param name="timeoutSeconds">Timeout in seconds for the API call (Default: 3s)</param>
    /// <param name="isResultSuccessful">Optional predicate to check if API result is valid and not a connection/server failure</param>
    /// <returns>Result from API or fallback DB query</returns>
    public static async Task<T> ExecuteWithFallbackAsync<T>(
        Func<Task<T>> apiFunc,
        Func<Task<T>> dbFallbackFunc,
        string actionName,
        int timeoutSeconds = 5,
        Func<T, bool>? isResultSuccessful = null)
    {
        try
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)))
            {
                var apiTask = apiFunc();
                var completedTask = await Task.WhenAny(apiTask, Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), cts.Token));

                if (completedTask == apiTask)
                {
                    cts.Cancel(); // Cancel delay task
                    T apiResult = await apiTask;

                    // If predicate provided, verify result success
                    if (apiResult != null && (isResultSuccessful == null || isResultSuccessful(apiResult)))
                    {
                        return apiResult;
                    }

                    log.Warn($"[API Fallback] API '{actionName}' returned invalid or error result. Switching to direct DB connection.");
                }
                else
                {
                    log.Warn($"[API Fallback] API '{actionName}' timed out after {timeoutSeconds}s. Switching to direct DB connection.");
                }
            }
        }
        catch (Exception ex)
        {
            log.Warn($"[API Fallback] API '{actionName}' exception: {ex.Message}. Switching to direct DB connection.");
        }

        // Execute direct SQL database fallback
        try
        {
            log.Info($"[API Fallback] Executing direct DB query for '{actionName}'...");
            return await dbFallbackFunc();
        }
        catch (Exception dbEx)
        {
            log.Error($"[API Fallback] Direct DB query for '{actionName}' also failed: {dbEx.Message}");
            throw;
        }
    }
}
