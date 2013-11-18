using System.Data.Entity;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IDbContextFactory
    {
        DbContext CreateContext(string sqlConnectionString);
    }
}
