using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.AddEnvironmentVariables();
        configHost.AddCommandLine(args);
        configHost.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
    })
    .Build();

host.Run();
