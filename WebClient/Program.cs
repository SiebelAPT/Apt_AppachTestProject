using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using WebClient.Abstractions;
using WebClient.Services;

namespace WebClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services
                .AddBlazorise(options =>
               {
                   options.ChangeTextOnKeyPress = true;
               })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient("FamilyTaskAPI", client => client.BaseAddress = new Uri("https://localhost:5001/api/"));
            builder.Services.AddSingleton<IMemberDataService, MemberDataService>();
            builder.Services.AddSingleton<ITaskDataService, TaskDataService>();

            var host = builder.Build();

            host.Services
            .UseBootstrapProviders()
            .UseFontAwesomeIcons();

            await host.RunAsync();
        }
    }
}