using Shared.Enums;
using System;

namespace BL.DTO
{
    /// <summary>
    /// DTO for project subtask (to prevent nesting)
    /// </summary>
    public class SubTaskDTO
    {
        public int Id { get; set; }

        public int? ParentTaskId { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public TaskState State { get; set; }
    }
}
