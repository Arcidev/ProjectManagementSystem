using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BL.DTO
{
    /// <summary>
    /// DTO for subproject (to prevent nesting) as we will support only first lvl nesting
    /// </summary>
    public class SubProjectDTO
    {
        public int Id { get; set; }

        public int? ParentProjectId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public virtual ProjectState State
        {
            get
            {
                // All tasks completed => Project completed
                if (Tasks != null && Tasks.Any() && Tasks.All(x => x.State == TaskState.Completed))
                    return ProjectState.Completed;

                if (Tasks != null && Tasks.Any() && Tasks.Any(x => x.State != TaskState.Planned))
                    return ProjectState.InProgress;

                return ProjectState.Planned;
            }
        }

        public List<TaskDTO> Tasks { get; set; }
    }
}
