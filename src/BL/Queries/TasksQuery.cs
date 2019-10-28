using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System.Linq;

namespace BL.Queries
{
    public class TasksQuery : AppQuery<TaskDTO>
    {
        public TasksQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        public int? ProjectId { get; set; }

        public TaskState? TaskState { get; set; }

        protected override IQueryable<TaskDTO> GetQueryable()
        {
            var query = Context.Tasks.AsQueryable();
            if (ProjectId.HasValue)
                query = query.Where(x => x.ProjectId == ProjectId.Value);

            if (TaskState.HasValue)
                query = query.Where(x => x.State == TaskState.Value && x.ParentTaskId == null);

            return query.ProjectTo<TaskDTO>(mapperConfig);
        }
    }
}
