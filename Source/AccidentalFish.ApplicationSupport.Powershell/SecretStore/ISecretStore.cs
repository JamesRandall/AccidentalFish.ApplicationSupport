using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Powershell.SecretStore
{
    internal interface ISecretStore
    {
        Task Save(string key, string value);
        string EncodeKey(string key);
    }
}
