namespace TeleLlama;

public class Worker : BackgroundService
{
    public Worker() { }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TeleLlamaService.Initialize(); 
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}