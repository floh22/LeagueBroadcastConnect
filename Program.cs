// League Broadcast Connect - League Broadcast (Essence) addon for game information in files
//     Copyright (C) 2021 Lars Eble
//     This file, Program.cs, is part of League Broadcast Connect
//
//     League Broadcast Connect is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     League Broadcast (Essence) is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY, without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with League Broadcast Connect.  If not, see <https://www.gnu.org/licenses/>.

using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System.Threading.Tasks;

namespace LeagueBroadcastConnect
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (CommandLineOptions opts) =>
                {
                    await CreateHostBuilder(args, opts).Build().RunAsync();
                    return 0;
                },
                errs => Task.FromResult(-1)); // Invalid arguments
        }

        public static IHostBuilder CreateHostBuilder(string[] args, CommandLineOptions opts) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(configureLogging => configureLogging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(opts);
                    services.AddHostedService<ConnectWorker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "LeagueBroadcast Connect Service";
                            config.SourceName = "LeagueBroadcast Connect Service";
                        });
                }).UseWindowsService();
    }
}

