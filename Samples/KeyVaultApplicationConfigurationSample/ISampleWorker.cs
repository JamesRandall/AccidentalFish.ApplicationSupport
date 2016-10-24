using System.Threading.Tasks;

namespace KeyVaultApplicationConfigurationSample
{
    internal interface ISampleWorker
    {
        Task Post();
        Task Read();
    }
}