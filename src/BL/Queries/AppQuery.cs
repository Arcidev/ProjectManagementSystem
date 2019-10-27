using AutoMapper;
using DAL.Context;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BL.Queries
{
    public abstract class AppQuery<T> : EntityFrameworkQuery<T>
    {
        protected IConfigurationProvider mapperConfig;

        public new AppDbContext Context => (AppDbContext)base.Context;

        public AppQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider)
        {
            mapperConfig = config;
        }
    }
}
