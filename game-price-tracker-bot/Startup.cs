
using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using game_price_tracker_bot.Services;

namespace game_price_tracker_bot
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(string[] args)
        {
            // Building the configuration and save it as a yml file in the current directory
            var builder = new ConfigurationBuilder()    // From Extension.Configuration
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("_config.yml");
            Configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<CommandHandler>();

            await provider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Setup a collection of Discord related services
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {                                           // Add Discord to the collection
                LogLevel = Discord.LogSeverity.Verbose,
                MessageCacheSize = 1000                 // how many messages should be kept in cache
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig {
                LogLevel = Discord.LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,             // Discord's command service will wait for every
                                                            // function to finish before doing something else
                CaseSensitiveCommands = false
            }))
            .AddSingleton<CommandHandler>()   // Add CommandHandler to the service collection
            .AddSingleton<StartupService>()   // Add StartupService to the service collection
            .AddSingleton(Configuration);
        }
    }
}