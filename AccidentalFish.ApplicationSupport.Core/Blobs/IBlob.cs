using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Blobs
{
    public interface IBlob
    {
        Task<Stream> DownloadAsync();
        Task<string> DownloadStringAsync();
        Task<string> DownloadStringAsync(Encoding encoding);
        Task<byte[]> DownloadBytesAsync();

        Stream GetUploadStream();

        Task UploadBytesAsync(byte[] bytes);

        Task UploadStreamAsync(Stream stream);

        Task UploadImageAsync(Image image, ImageFormat imageFormat);

        string DownloadString();
        string DownloadString(Encoding encoding);

        Uri Url { get; }
    }
}
