using Microsoft.EntityFrameworkCore;
using PasteIt.Data;

namespace PasteIt.Services
{
    public class ExpiredSnippetsCleanupService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

        public ExpiredSnippetsCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var expiredSnippets = await dbContext.Snippets
                        .Where(s => s.ExpiresAt <= DateTime.Now)
                        .ToListAsync(stoppingToken);

                    if (expiredSnippets.Any())
                    {
                        dbContext.Snippets.RemoveRange(expiredSnippets);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                await Task.Delay(_interval, stoppingToken);
            }
        }

    }
}
