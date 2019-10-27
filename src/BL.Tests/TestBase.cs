using BL.Installers;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BL.Tests
{
    public abstract class TestBase
    {
        protected static readonly IServiceProvider services;

        static TestBase()
        {
            services = new ServiceCollection()
                .AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDB"), ServiceLifetime.Transient, ServiceLifetime.Transient)
                .AddTransient<Func<DbContext>>(provider => () => provider.GetService<AppDbContext>())
                .ConfigureAutoMapper()
                .ConfigureServices()
                .ConfigureFacades()
                .BuildServiceProvider();

        }
    }
}
