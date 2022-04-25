using System.Diagnostics;

namespace APIClient
{
    public class Worker : BackgroundService
    {
        private readonly ApiClient apiClient;
        private readonly ILogger<Worker> _logger;

        public Worker(
            ApiClient apiClient,
            ILogger<Worker> logger
            )
        {
            this.apiClient = apiClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<string> jobNumbers = new List<string>()
                {
                    "9900004817"
                };

                Task[] resultTasks = new Task[jobNumbers.Count];

                Stopwatch sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < jobNumbers.Count; i++)
                {
                    Task resultTask = apiClient.ExecuteRequest(jobNumbers.ElementAt(i), stoppingToken);

                    resultTasks[i] = resultTask;
                }

                Task.WaitAll(resultTasks);

                sw.Stop();

                _logger.LogInformation($"Completed in {sw.ElapsedMilliseconds / 1000}s ");

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}