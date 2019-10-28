using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using Shared.Enums;
using System.Linq;

namespace BL.Queries
{
    public class ProjectsQuery : AppQuery<ProjectDTO>
    {
        public ProjectsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        public ProjectState? ProjectState { get; set; }

        protected override IQueryable<ProjectDTO> GetQueryable()
        {
            var query = Context.Projects.AsQueryable();
            if (ProjectState.HasValue)
                query = query.Where(x => x.State == ProjectState.Value && x.ParentProjectId == null);

            return query.ProjectTo<ProjectDTO>(mapperConfig);
        }
    }
}
