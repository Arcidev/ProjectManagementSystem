using AutoMapper;
using Riganti.Utils.Infrastructure.Core;
using System;

namespace BL.Facades
{
    /// <summary>
    /// Base facade
    /// </summary>
    public abstract class BaseFacade
    {
        protected readonly IMapper mapper;
        protected readonly Func<IUnitOfWorkProvider> uowProviderFunc;

        protected BaseFacade(IMapper mapper, Func<IUnitOfWorkProvider> uowProviderFunc)
        {
            this.mapper = mapper;
            this.uowProviderFunc = uowProviderFunc;
        }

        /// <summary>
        /// Checks nullability of object
        /// </summary>
        /// <param name="obj">Object to be tested</param>
        /// <param name="errorMessage">Error message</param>
        /// <exception cref="UIException">Thrown when obj is null</exception>
        protected void IsNotNull(object obj, string errorMessage)
        {
            if (obj == null)
                throw new UIException(errorMessage);
        }
    }
}
