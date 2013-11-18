using System;
using System.IO;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Blobs
{
    public interface IAsynchronousBlockBlobRepository
    {
        Task<IBlob> UploadAsync(string name, Stream stream);
        void Upload(string name, Stream stream);
        void Upload(string name, Stream stream, Action<string> success, Action<string, Exception> failure);
        IBlob Get(string name);
    }
}
