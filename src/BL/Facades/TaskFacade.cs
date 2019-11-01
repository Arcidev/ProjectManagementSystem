using AutoMapper;
using BL.DTO;
using BL.Queries;
using BL.Repositories;
using BL.Resources;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Facades
{
    /// <summary>
    /// Facade to handle task based operations
    /// </summary>
    public class TaskFacade : BaseFacade
    {
        private readonly Func<TaskRepository> taskRepository;
        private readonly Func<ProjectRepository> projectRepository;
        private readonly Func<TasksQuery> tasksQuery;

        public TaskFacade(Func<TaskRepository> taskRepository,
            Func<ProjectRepository> projectRepository,
            Func<TasksQuery> tasksQuery,
            IMapper mapper,
            Func<IUnitOfWorkProvider> uowProvider) : base(mapper, uowProvider)
        {
            this.taskRepository = taskRepository;
            this.projectRepository = projectRepository;
            this.tasksQuery = tasksQuery;
        }

        /// <summary>
        /// Add task
        /// </summary>
        /// <param name="task">Task to be added</param>
        /// <returns>Asynchronous task</returns>
        public async Task AddTask(TaskDTO task)
        {
            CheckandNormalizeProjectId(task);
            using var uow = uowProviderFunc().Create();
            var entity = mapper.Map<ProjectTask>(task);

            var repo = taskRepository();
            repo.Insert(entity);

            await uow.CommitAsync();
            task.Id = entity.Id;
        }

        /// <summary>
        /// Update existing task
        /// </summary>
        /// <param name="task">Task to be updated</param>
        /// <returns>Asynchronous task</returns>
        /// <exception cref="UIException">Thrown when project to be updated not found or subtask contains another subtask</exception>
        public async Task UpdateTask(TaskDTO task)
        {
            if (task.ParentTaskId.HasValue && task.SubTasks != null && task.SubTasks.Any())
                throw new UIException(ErrorMessages.SubProjectContaingProjects);

            CheckandNormalizeProjectId(task);
            using var uow = uowProviderFunc().Create();
            var repo = taskRepository();

            var entity = await repo.GetByIdAsync(task.Id);
            IsNotNull(entity, ErrorMessages.TaskNotFound);

            mapper.Map(task, entity);

            await uow.CommitAsync();
        }

        /// <summary>
        /// Gets existing task
        /// </summary>
        /// <param name="id">Id of task</param>
        /// <returns>Existing task</returns>
        public async Task<TaskDTO> GetTask(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = taskRepository();

            return mapper.Map<TaskDTO>(await repo.GetTask(id));
        }

        /// <summary>
        /// Gets tasks of project
        /// </summary>
        /// <param name="projectId">Id of project</param>
        /// <returns>Enumerable of project tasks</returns>
        public async Task<IEnumerable<TaskDTO>> GetTasksByProject(int projectId)
        {
            using var uow = uowProviderFunc().Create();
            var query = tasksQuery();
            query.ProjectId = projectId;

            return await query.ExecuteAsync();
        }

        /// <summary>
        /// Get projects by state
        /// </summary>
        /// <param name="state">Project state</param>
        /// <returns>Projects by state</returns>
        public async Task<IEnumerable<TaskDTO>> GetTasks(TaskState state)
        {
            using var uow = uowProviderFunc().Create();
            var query = tasksQuery();
            query.TaskState = state;

            return await query.ExecuteAsync();
        }

        /// <summary>
        /// Deletes existing tasks
        /// </summary>
        /// <param name="id">Id of task to be deleted</param>
        /// <returns>Asynchronous task</returns>
        public async Task DeleteTask(int id)
        {
            using var uow = uowProviderFunc().Create();
            var repo = taskRepository();
            await repo.DeleteProject(id);

            await uow.CommitAsync();
        }

        /// <summary>
        /// Sets subtasks projectId to the same as parent task has
        /// </summary>
        /// <param name="tasks">Tasks to be set</param>
        public static void NormalizeTaskProjectId(IEnumerable<TaskDTO> tasks)
        {
            // Nothing to set
            if (tasks == null)
                return;

            foreach (var task in tasks.Where(x => x.SubTasks != null))
                NormalizeTaskProjectId(task);
        }

        private void CheckandNormalizeProjectId(TaskDTO task)
        {
            using var uow = uowProviderFunc().Create();
            var repo = projectRepository();
            IsNotNull(repo.GetById(task.ProjectId), ErrorMessages.ProjectNotFound);

            NormalizeTaskProjectId(task);
        }

        private static void NormalizeTaskProjectId(TaskDTO task)
        {
            if (task.SubTasks == null)
                return;

            foreach (var subtask in task.SubTasks)
                subtask.ProjectId = task.ProjectId;
        }
    }
}
