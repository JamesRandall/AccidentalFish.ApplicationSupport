using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using CuttingEdge.Conditions;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlockBlob : IBlob
    {
        private readonly CloudBlockBlob _blockBlob;

        public BlockBlob(CloudBlockBlob blockBlob)
        {
            Condition.Requires(blockBlob).IsNotNull();
            _blockBlob = blockBlob;
        }

        public Task<string> DownloadStringAsync()
        {
            return DownloadStringAsync(Encoding.UTF8);
        }

        public async Task<string> DownloadStringAsync(Encoding encoding)
        {
            string result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                await _blockBlob.DownloadToStreamAsync(stream);
                stream.Position = 0;
                StreamReader streamReader = new StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public async Task<byte[]> DownloadBytesAsync()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                await _blockBlob.DownloadToStreamAsync(stream);
                return stream.ToArray();
            }
        }

        public string DownloadString()
        {
            return DownloadString(Encoding.UTF8);
        }

        public string DownloadString(Encoding encoding)
        {
            string result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                _blockBlob.DownloadToStream(stream);
                stream.Position = 0;
                StreamReader streamReader = new StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public async Task<Stream> DownloadAsync()
        {
            MemoryStream stream = new MemoryStream();
            await _blockBlob.DownloadToStreamAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public Stream GetUploadStream()
        {
            return _blockBlob.OpenWrite();
        }

        public async Task<bool> Exists()
        {
            bool doesExist = await _blockBlob.ExistsAsync();
            return doesExist;
        }

        public async Task UploadBytesAsync(byte[] bytes)
        {
            await _blockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
        }

        public async Task UploadStreamAsync(Stream stream)
        {
            await _blockBlob.UploadFromStreamAsync(stream);
        }

        public async Task UploadImageAsync(Image image, ImageFormat imageFormat)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                ms.Position = 0;
                await _blockBlob.UploadFromStreamAsync(ms);
            }
        }
        
        public Uri Url { get { return _blockBlob.Uri; }}
    }
}
