using BL.DTO;
using BL.Facades;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BL.Tests
{
    public class ProjectTests : TestBase
    {
        private static ProjectFacade ProjectFacade => services.GetRequiredService<ProjectFacade>();

        [Fact]
        public async Task TestCRUDOperations()
        {
            var currentDate = DateTime.Now.Date;
            var projectFacade = ProjectFacade;
            var project = new ProjectDTO()
            {
                Code = "Code",
                StartDate = currentDate,
                Name = "Name",
                Tasks = new List<TaskDTO>()
                {
                    new TaskDTO() { Name = "Task1", Description = "Description1", State = TaskState.Planned, StartDate = currentDate, SubTasks = new List<TaskDTO>() },
                    new TaskDTO() { Name = "Task2", Description = "Description2", State = TaskState.InProgress, StartDate = currentDate, FinishDate = currentDate.AddDays(1), SubTasks = new List<TaskDTO>()
                    {
                        new TaskDTO { Name = "SubTask", Description = "Description", State = TaskState.Completed, StartDate = currentDate, FinishDate = currentDate }
                    } }
                }
            };

            await projectFacade.AddProject(project);
            var dto = await projectFacade.GetProject("Code");
            Assert.NotNull(dto);
            CompareProject(project, dto);

            dto = await projectFacade.GetProject(dto.Id);
            Assert.NotNull(dto);
            CompareProject(project, dto);

            await projectFacade.DeleteProject(dto.Id);
            Assert.Null(await projectFacade.GetProject("Code"));
            Assert.Null(await projectFacade.GetProject(dto.Id));
        }

        [Fact]
        public async Task TestNotExistingProjectGet()
        {
            Assert.Null(await ProjectFacade.GetProject("SomeRandomCode"));
        }

        [Fact]
        public async Task TestUpdateNotExistingProjectGet()
        {
            await Assert.ThrowsAsync<UIException>(() => ProjectFacade.UpdateProject(new ProjectDTO() { Id = -1 }));
        }

        private void CompareProject(ProjectDTO expected, ProjectDTO actual)
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.StartDate, actual.StartDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Tasks.Count, actual.Tasks.Count);

            foreach (var (task, taskDto) in actual.Tasks.Join(expected.Tasks, x => x.Name, y => y.Name, (task, taskDto) => (task, taskDto)))
            {
                Assert.Equal(task.Name, taskDto.Name);
                Assert.Equal(task.Description, taskDto.Description);
                Assert.Equal(task.State, taskDto.State);
                Assert.Equal(task.StartDate, taskDto.StartDate);
                Assert.Equal(task.FinishDate, taskDto.FinishDate);
                Assert.Equal(task.SubTasks.Count, taskDto.SubTasks.Count);
            }
        }
    }
}
