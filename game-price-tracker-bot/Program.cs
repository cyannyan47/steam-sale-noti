using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace game_price_tracker_bot
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await Startup.RunAsync(args);
    }
        
}
