using Core.Abstractions.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataLayer
{
    public static class DataLayerStartup
    {
        public static void AddDatalayer(this IServiceCollection services)
        {
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
        }
    }
}
