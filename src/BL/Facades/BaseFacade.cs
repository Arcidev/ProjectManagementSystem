using AutoMapper;
using Riganti.Utils.Infrastructure.Core;
using System;

namespace BL.Facades
{
    public abstract class BaseFacade
    {
        protected readonly IMapper mapper;
        protected readonly Func<IUnitOfWorkProvider> uowProviderFunc;

        protected BaseFacade(IMapper mapper, Func<IUnitOfWorkProvider> uowProviderFunc)
        {
            this.mapper = mapper;
            this.uowProviderFunc = uowProviderFunc;
        }

        protected void IsNotNull(object obj, string errorMessage)
        {
            if (obj == null)
                throw new UIException(errorMessage);
        }
    }
}
