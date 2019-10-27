using AutoMapper;
using BL.DTO;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace BL.Facades
{
    public class ProjectFacade : BaseFacade
    {
        private readonly Func<ProjectRepository> projectRepository;

        public ProjectFacade(Func<ProjectRepository> projectRepository,
            IMapper mapper,
            Func<IUnitOfWorkProvider> uowProvider) : base(mapper, uowProvider)
        {
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// Add project
        /// </summary>
        /// <param name="project">Project to be added</param>
        /// <returns>Asynchronous task</returns>
        public async Task AddProject(ProjectDTO project)
        {
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<Project>(project);

            var repo = projectRepository();
            repo.Insert(entity);

            await uow.CommitAsync();
            project.Id = entity.Id;
        }

        /// <summary>
        /// Update existing project
        /// </summary>
        /// <param name="project">Project to be updated</param>
        /// <returns>Asynchronous task</returns>
        /// <exception cref="UIException">Thrown when project to be updated not found</exception>
        public async Task UpdateProject(ProjectDTO project)
        {
            using var uow = uowProviderFunc().Create();
            var repo = projectRepository();

            var entity = await repo.GetByIdAsync(project.Id);
            IsNotNull(entity, "Error");
            mapper.Map(project, entity);
        }

        /// <summary>
        /// Gets existing project
        /// </summary>
        /// <param name="id">Id of project</param>
        /// <returns>Existing project</returns>
        public async Task<ProjectDTO> GetProject(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = projectRepository();

            return mapper.Map<ProjectDTO>(await repo.GetProject(id));
        }

        /// <summary>
        /// Gets existing project
        /// </summary>
        /// <param name="id">Code of project</param>
        /// <returns>Existing project</returns>
        public async Task<ProjectDTO> GetProject(string code)
        {
            using var uow = uowProviderFunc().Create();
            var repo = projectRepository();

            return mapper.Map<ProjectDTO>(await repo.GetProject(code));
        }

        /// <summary>
        /// Deletes existing project
        /// </summary>
        /// <param name="id">Id of project to be deleted</param>
        /// <returns>Asynchronous task</returns>
        public async Task DeleteProject(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = projectRepository();

            repo.Delete(id);
            await uow.CommitAsync();
        }
    }
}
