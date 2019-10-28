using BL.Queries;
using BL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace BL.Installers
{
    /// <summary>
    /// Service install helper
    /// </summary>
    public static class ServiceInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained service installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var baseRepository = typeof(BaseRepository<,>);
            var baseQuery = typeof(AppQuery<>);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && (t.BaseType.GetGenericTypeDefinition() == baseRepository || t.BaseType.GetGenericTypeDefinition() == baseQuery)))
            {
                services.AddTransient(type);
            }

            return services.AddSingleton<Func<ProjectRepository>>(provider => () => provider.GetRequiredService<ProjectRepository>())
                .AddSingleton<Func<TaskRepository>>(provider => () => provider.GetRequiredService<TaskRepository>())
                .AddSingleton<Func<ProjectsQuery>>(provider => () => provider.GetRequiredService<ProjectsQuery>())
                .AddSingleton<Func<TasksQuery>>(provider => () => provider.GetRequiredService<TasksQuery>())

                .AddSingleton<IUnitOfWorkProvider, AppUnitOfWorkProvider>()
                .AddSingleton<IUnitOfWorkRegistry, AsyncLocalUnitOfWorkRegistry>()
                .AddSingleton<IDateTimeProvider, UtcDateTimeProvider>()
                .AddSingleton<Func<IUnitOfWorkProvider>>(provider => () => provider.GetService<IUnitOfWorkProvider>())
                .AddTransient(typeof(IRepository<,>), typeof(EntityFrameworkRepository<,>));
        }
    }
}
