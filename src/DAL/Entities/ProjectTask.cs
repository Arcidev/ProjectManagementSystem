using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    /// <summary>
    /// Entity for project task (named as ProjectTask because of existing Task class)
    /// </summary>
    public class ProjectTask : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int? ParentTaskId { get; set; }

        public int ProjectId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public TaskState State { get; set; }

        [ForeignKey(nameof(ParentTaskId))]
        public virtual ProjectTask ParentTask { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }

        public virtual ICollection<ProjectTask> SubTasks { get; set; }
    }
}
