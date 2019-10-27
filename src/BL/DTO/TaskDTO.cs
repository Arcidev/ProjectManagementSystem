using Shared.Enums;
using System;
using System.Collections.Generic;

namespace BL.DTO
{
    /// <summary>
    /// DTO for project task
    /// </summary>
    public class TaskDTO
    {
        public int Id { get; set; }

        public int? ParentTaskId { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public TaskState State { get; set; }

        public List<TaskDTO> SubTasks { get; set; }
    }
}
