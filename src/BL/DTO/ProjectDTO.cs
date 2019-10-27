using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.DTO
{
    /// <summary>
    /// DTO for creating project
    /// </summary>
    public class ProjectDTO
    {
        public int Id { get; set; }

        public int? ParentProjectId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public ProjectState State
        {
            get
            {
                // All tasks completed => Project completed
                if (Tasks != null && !Tasks.Any() && SubProjects != null && SubProjects.Any() && Tasks.All(x => x.State == TaskState.Completed) && SubProjects.All(x => x.State == ProjectState.Completed))
                    return ProjectState.Completed;

                if (Tasks != null && !Tasks.Any() && Tasks.Any(x => x.State != TaskState.Planned) || SubProjects != null && SubProjects.Any() && SubProjects.Any(x => x.State != ProjectState.Planned))
                    return ProjectState.InProgress;

                return ProjectState.Planned;
            }
        }

        public List<ProjectDTO> SubProjects { get; set; }

        public List<TaskDTO> Tasks { get; set; }
    }
}
