using Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BL.DTO
{
    /// <summary>
    /// DTO for project
    /// </summary>
    public class ProjectDTO : SubProjectDTO
    {
        public override ProjectState State
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

        public List<SubProjectDTO> SubProjects { get; set; }
    }
}
