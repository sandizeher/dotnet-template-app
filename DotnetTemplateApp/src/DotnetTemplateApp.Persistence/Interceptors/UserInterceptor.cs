using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DotnetTemplateApp.Persistence.Interceptors
{
    public class UserInterceptor(ILogger<UserInterceptor> logger) : SaveChangesInterceptor
    {
        public override Task SaveChangesFailedAsync(
            DbContextErrorEventData eventData,
            CancellationToken cancellationToken = default)
        {
            logger.LogError(eventData.Exception, "Error while saving changes to database");
            UserChangeFailed(eventData.Context);

            return Task.CompletedTask;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UserChangeSuccess(eventData.Context);

            return ValueTask.FromResult(result);
        }

        private void UserChangeSuccess(DbContext? context)
        {
            if (context == null) return;

            try
            {
                logger.LogInformation("Successfully applied changes");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error happened in interceptor while saving.");
            }
        }

        private void UserChangeFailed(DbContext? context)
        {
            if (context == null) return;

            try
            {
                logger.LogInformation("Change failed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error happened in interceptor while saving.");
            }
        }
    }
}
