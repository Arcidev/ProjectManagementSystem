﻿using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DAL.Installers
{
    /// <summary>
    /// Db install helper
    /// </summary>
    public static class DbInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained database installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (connectionString == null)
                throw new ArgumentException(nameof(connectionString));

            return services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient)
                .AddSingleton<Func<DbContext>>(provider => () => provider.GetService<AppDbContext>());
        }
    }
}
