using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlockBlob : IBlob
    {
        private readonly CloudBlockBlob _blockBlob;
        private readonly string _name;
        private readonly IAzureAssemblyLogger _logger;

        public BlockBlob(CloudBlockBlob blockBlob, string name, IAzureAssemblyLogger logger)
        {
            if (blockBlob == null) throw new ArgumentNullException(nameof(blockBlob));

            _blockBlob = blockBlob;
            _name = name;
            _logger = logger;
        }

        public async Task<string> DownloadStringAsync()
        {
            _logger?.Verbose("BlockBlob: DownloadStringAsync - attempting download of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            string result = await DownloadStringAsync(Encoding.UTF8);
            sw.Stop();
            _logger?.Verbose("BlockBlob: DownloadStringAsync - download of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
            return result;
        }

        public async Task<string> DownloadStringAsync(Encoding encoding)
        {
            _logger?.Verbose("BlockBlob: DownloadStringAsync - attempting download of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            string result;
            using (MemoryStream stream = new MemoryStream())
            {
                await _blockBlob.DownloadToStreamAsync(stream);
                stream.Position = 0;
                StreamReader streamReader = new StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
            }
            sw.Stop();
            _logger?.Verbose("BlockBlob: DownloadStringAsync - download of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
            return result;
        }

        public async Task<byte[]> DownloadBytesAsync()
        {
            _logger?.Verbose("BlockBlob: DownloadBytesAsync - attempting download of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            byte[] result;
            using (MemoryStream stream = new MemoryStream())
            {
                await _blockBlob.DownloadToStreamAsync(stream);
                result = stream.ToArray();
            }
            sw.Stop();
            _logger?.Verbose("BlockBlob: DownloadBytesAsync - download of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
            return result;
        }

        public string DownloadString()
        {
            return DownloadString(Encoding.UTF8);
        }

        public string DownloadString(Encoding encoding)
        {
            _logger?.Verbose("BlockBlob: DownloadString - attempting download of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            string result;
            using (MemoryStream stream = new MemoryStream())
            {
                _blockBlob.DownloadToStream(stream);
                stream.Position = 0;
                StreamReader streamReader = new StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
            }
            sw.Stop();
            _logger?.Verbose("BlockBlob: DownloadString - download of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
            return result;
        }

        public async Task<Stream> DownloadAsync()
        {
            _logger?.Verbose("BlockBlob: DownloadAsync - attempting download of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            MemoryStream stream = new MemoryStream();
            await _blockBlob.DownloadToStreamAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            sw.Stop();
            _logger?.Verbose("BlockBlob: DownloadAsync - download of {0} to stream succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
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
            _logger?.Verbose("BlockBlob: UploadBytesAsync - attempting upload of {0} bytes {1}", bytes.Length, _name);
            Stopwatch sw = Stopwatch.StartNew();
            await _blockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
            sw.Stop();
            _logger?.Verbose("BlockBlob: UploadBytesAsync - upload of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
        }

        public async Task UploadStreamAsync(Stream stream)
        {
            _logger?.Verbose("BlockBlob: UploadStreamAsync - attempting upload of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            await _blockBlob.UploadFromStreamAsync(stream);
            sw.Stop();
            _logger?.Verbose("BlockBlob: UploadStreamAsync - upload of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
        }

        public async Task UploadImageAsync(Image image, ImageFormat imageFormat)
        {
            _logger?.Verbose("BlockBlob: UploadImageAsync - attempting upload of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                ms.Position = 0;
                await _blockBlob.UploadFromStreamAsync(ms);
            }
            sw.Stop();
            _logger?.Verbose("BlockBlob: UploadImageAsync - upload of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
        }

        public async Task DeleteAsync()
        {
            _logger?.Verbose("BlockBlob: DeleteAsync - attempting delete of {0}", _name);
            Stopwatch sw = Stopwatch.StartNew();
            await _blockBlob.DeleteIfExistsAsync();
            sw.Stop();
            _logger?.Verbose("BlockBlob: DeleteAsync - delete of {0} succeeded in {1}ms", _name, sw.ElapsedMilliseconds);
        }
        
        public Uri Url => _blockBlob.Uri;
    }
}
