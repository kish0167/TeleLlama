using TeleLlama;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(services => services.AddHostedService<Worker>());
builder.UseWindowsService();

var host = builder.Build();
host.Run();