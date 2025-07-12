using BusinessLayer.IService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class SlotAutoGenerationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SlotAutoGenerationService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromDays(7); // Chạy mỗi tuần

        public SlotAutoGenerationService(
            IServiceProvider serviceProvider,
            ILogger<SlotAutoGenerationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Slot Auto Generation Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await GenerateSlotsForAllConsultants();
                    _logger.LogInformation("Auto-generated slots for all consultants");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while auto-generating slots");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }

        private async Task GenerateSlotsForAllConsultants()
        {
            using var scope = _serviceProvider.CreateScope();
            var consultantService = scope.ServiceProvider.GetRequiredService<IConsultantService>();

            var slotsGenerated = await consultantService.AutoGenerateSlotsForAllConsultantsAsync(weeksAhead: 4, slotDurationMinutes: 60);
            _logger.LogInformation("Generated {SlotsGenerated} slots for all consultants", slotsGenerated);
        }
    }
} 