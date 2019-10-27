using BL.DTO;
using BL.Facades;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BL.Tests
{
    public class TaskTests : TestBase
    {
        private static TaskFacade TaskFacade => services.GetRequiredService<TaskFacade>();
        private static ProjectFacade ProjectFacade => services.GetRequiredService<ProjectFacade>();

        [Fact]
        public async Task TestCRUDOperations()
        {
            var project = new ProjectDTO()
            {
                Code = "SomeCode",
                Name = "Name",
                StartDate = DateTime.Now
            };
            await ProjectFacade.AddProject(project);

            var currentDate = DateTime.Now.Date;
            var taskFacade = TaskFacade;
            var task = new TaskDTO()
            {
                ProjectId = project.Id,
                Name = "Task",
                Description = "Description",
                State = TaskState.InProgress,
                StartDate = currentDate,
                FinishDate = currentDate.AddDays(1),
                SubTasks = new List<SubTaskDTO>()
                {
                    new SubTaskDTO { Name = "SubTask", Description = "Description", State = TaskState.InProgress, StartDate = currentDate, FinishDate = currentDate }
                }
            };

            await taskFacade.AddTask(task);
            var dto = await taskFacade.GetTask(task.Id);
            Assert.NotNull(dto);
            CompareTask(task, dto);

            Assert.NotEmpty(await taskFacade.GetTasksByProject(project.Id));

            task.Name = "Task1";
            task.Description = "Description1";

            await taskFacade.UpdateTask(task);
            dto = await taskFacade.GetTask(dto.Id);
            Assert.NotNull(dto);
            CompareTask(task, dto);

            await taskFacade.DeleteTask(dto.Id);
            Assert.Null(await taskFacade.GetTask(dto.Id));
        }

        [Fact]
        public async Task TestNotExistingTaskGet()
        {
            Assert.Null(await TaskFacade.GetTask(-1));
            Assert.Empty(await TaskFacade.GetTasksByProject(-1));
        }

        [Fact]
        public async Task TestUpdateNotExistingProjectGet()
        {
            await Assert.ThrowsAsync<UIException>(() => TaskFacade.UpdateTask(new TaskDTO() { Id = -1 }));
        }

        private void CompareTask(TaskDTO expected, TaskDTO actual)
        {
            CompareSubTask(expected, actual);
            for (int i = 0; i < expected.SubTasks.Count; i++)
                CompareSubTask(expected.SubTasks[i], actual.SubTasks[i]);

            Assert.All(actual.SubTasks, x => Assert.Equal(expected.ProjectId, x.ProjectId));
        }

        private void CompareSubTask(SubTaskDTO expected, SubTaskDTO actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.State, actual.State);
            Assert.Equal(expected.StartDate, actual.StartDate);
            Assert.Equal(expected.FinishDate, actual.FinishDate);
        }
    }
}
