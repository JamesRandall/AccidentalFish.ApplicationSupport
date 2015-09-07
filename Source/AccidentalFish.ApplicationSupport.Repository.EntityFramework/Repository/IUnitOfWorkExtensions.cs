using System;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    // ReSharper disable once InconsistentNaming
    public static class IUnitOfWorkExtensions
    {
        public static ISqlExecuterAsync GetSqlExecuter(this IUnitOfWorkAsync unitOfWorkAsync)
        {
            EntityFrameworkUnitOfWorkAsync internalUnitOfWork = unitOfWorkAsync as EntityFrameworkUnitOfWorkAsync;
            if (internalUnitOfWork == null)
            {
                throw new InvalidOperationException("Sql execution only supported with an Entity Framework based repository");
            }
            return new SqlExecuterAsync(internalUnitOfWork.Context);
        }

        public static ISqlExecuter GetSqlExecuter(this IUnitOfWork unitOfWork)
        {
            EntityFrameworkUnitOfWork internalUnitOfWork = unitOfWork as EntityFrameworkUnitOfWork;
            if (internalUnitOfWork == null)
            {
                throw new InvalidOperationException("Sql execution only supported with an Entity Framework based repository");
            }
            return new SqlExecuter(internalUnitOfWork.Context);
        }
    }
}
