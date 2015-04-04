using System.Data.Entity;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    public interface IDbContextFactory
    {
        DbContext CreateContext(string sqlConnectionString);
    }
}
