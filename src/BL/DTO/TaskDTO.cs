using System.Collections.Generic;

namespace BL.DTO
{
    /// <summary>
    /// DTO for project task
    /// </summary>
    public class TaskDTO : SubTaskDTO
    {
        public List<SubTaskDTO> SubTasks { get; set; }
    }
}
