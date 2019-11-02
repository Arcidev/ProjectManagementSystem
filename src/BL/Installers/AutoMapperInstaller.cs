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
                config.CreateMap<Project, ProjectDTO>();
                config.CreateMap<Project, SubProjectDTO>();
                config.CreateMap<SubProjectDTO, Project>()
                    .ForMember(x => x.Tasks, action => action.PreCondition(x => x.Tasks != null))
                    .ForMember(x => x.Id, action => action.Ignore());
                config.CreateMap<ProjectDTO, Project>()
                    .ForMember(x => x.SubProjects, action => action.PreCondition(x => x.SubProjects != null))
                    .ForMember(x => x.Id, action => action.Ignore());

                config.CreateMap<ProjectTask, TaskDTO>();
                config.CreateMap<ProjectTask, SubTaskDTO>();
                config.CreateMap<SubTaskDTO, ProjectTask>()
                    .ForMember(x => x.Id, action => action.Ignore());
                config.CreateMap<TaskDTO, ProjectTask>()
                    .ForMember(x => x.SubTasks, action => action.PreCondition(x => x.SubTasks != null))
                    .ForMember(x => x.Id, action => action.Ignore());
            });

            return services.AddSingleton<IConfigurationProvider>(autoMapperConfig)
                .AddSingleton<IMapper, Mapper>();
        }
    }
}
