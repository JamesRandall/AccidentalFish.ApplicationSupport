using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    public interface IResourceCreator
    {
        Task CreateIfNotExists();
    }
}
