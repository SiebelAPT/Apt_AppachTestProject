using AutoMapper;
using Domain.Commands;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.AutoMapper;

namespace WebApi
{
    public static class Setup
    {
        public static void AddSupportingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(MemberProfile).Assembly);
            services.AddAutoMapper(typeof(TaskTypes).Assembly);

            services.AddMvc().AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            services.AddDbContext<DataLayer.FamilyTaskContext>(options =>
            {
                //var ConnString = configuration.GetSection("ConnectionStrings:SqlDb").Value;
                //options.UseSqlServer(configuration.GetSection("ConnectionStrings:SqlDb").Value);
                var ConnString = config.GetSection("ConnectionStrings")["SqlDb"];                
                options.UseSqlServer(ConnString);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader());
            });
        }
    }
}
