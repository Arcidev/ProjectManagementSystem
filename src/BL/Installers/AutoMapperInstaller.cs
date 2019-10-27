using AutoMapper;
using BL.DTO;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BL.Installers
{
    /// <summary>
    /// AutoMapper install helper
    /// </summary>
    public static class AutoMapperInstaller
    {
        /// <summary>
        /// Extends <see cref="IServiceCollection"/> to allow chained automapper installation
        /// </summary>
        /// <param name="services">DI container</param>
        /// <returns>Passed DI container to allow chaining</returns>
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var autoMapperConfig = new MapperConfiguration(config => {
                config.CreateMap<ProjectDTO, Project>()
                    .ForMember(x => x.Id, action => action.Ignore());
                config.CreateMap<Project, ProjectDTO>();

                config.CreateMap<ProjectTask, TaskDTO>();
                config.CreateMap<TaskDTO, ProjectTask>();
            });

            return services.AddSingleton<IConfigurationProvider>(autoMapperConfig)
                .AddSingleton<IMapper, Mapper>();
        }
    }
}
