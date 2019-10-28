using Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public TaskState State { get; set; }
    }
}
