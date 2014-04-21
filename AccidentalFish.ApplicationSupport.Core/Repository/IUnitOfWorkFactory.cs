namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        IUnitOfWorkAsync CreateAsync();
    }
}
