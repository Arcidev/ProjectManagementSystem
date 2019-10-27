using AutoMapper;
using BL.DTO;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace BL.Tests
{
    public class MappingTests : TestBase
    {
        private static IMapper Mapper => services.GetRequiredService<IMapper>();

        [Fact]
        public void TestProjectMapping()
        {
            var project = new ProjectDTO()
            {
                SubProjects = new List<SubProjectDTO>()
                {
                    new ProjectDTO() { SubProjects = new List<SubProjectDTO>() { new SubProjectDTO() { Code = "123" } } },
                    new ProjectDTO() { SubProjects = new List<SubProjectDTO>() },
                    new SubProjectDTO()
                }
            };

            var mapped = Mapper.Map<Project>(project);
            Assert.All(mapped.SubProjects, x => Assert.Null(x.SubProjects)); // We do not want to have this mapped when using child class
        }

        [Fact]
        public void TestTaskMapping()
        {
            var task = new TaskDTO()
            {
                SubTasks = new List<SubTaskDTO>()
                {
                    new TaskDTO() { SubTasks = new List<SubTaskDTO>() { new SubTaskDTO() { Name = "123" } } },
                    new TaskDTO() { SubTasks = new List<SubTaskDTO>() },
                    new SubTaskDTO()
                }
            };

            var mapped = Mapper.Map<ProjectTask>(task);
            Assert.All(mapped.SubTasks, x => Assert.Null(x.SubTasks)); // We do not want to have this mapped when using child class
        }
    }
}
