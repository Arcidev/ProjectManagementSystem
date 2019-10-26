using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Project : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int? ParentProjectId { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public ProjectState State { get; set; }

        [ForeignKey(nameof(ParentProjectId))]
        public virtual Project ParentProject { get; set; }

        public virtual ICollection<Project> SubProjects { get; set; }

        public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
