using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Configuration;
using AccidentalFish.ApplicationSupport.Azure.KeyVault;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Azure.Tests.Unit.Configuration
{
    [TestClass]
    public class AsyncKeyVaultConfigurationTests
    {
        [TestMethod]
        public async Task GetsFromLocalConfigurationFirst()
        {
            Mock<IKeyVault> keyVault = new Mock<IKeyVault>();
            Mock<IKeyVaultConfigurationKeyEncoder> keyVaultEncoder = new Mock<IKeyVaultConfigurationKeyEncoder>();
            Mock<IAsyncConfiguration> asyncConfiguration = new Mock<IAsyncConfiguration>();

            keyVaultEncoder.Setup(x => x.Encode("mykey")).Returns("mykey");
            asyncConfiguration.Setup(x => x.GetAsync("mykey")).ReturnsAsync("localvalue");
            keyVault.Setup(x => x.GetSecretAsync("mykey")).ReturnsAsync("keyvaultvalue");

            AsyncKeyVaultConfiguration asyncKeyVaultConfiguration = new AsyncKeyVaultConfiguration(keyVault.Object, keyVaultEncoder.Object, KeyVaultConfigurationCachePolicy.Default, asyncConfiguration.Object);

            string result = await asyncKeyVaultConfiguration.GetAsync("mykey");
            Assert.AreEqual("localvalue", result);
        }

        [TestMethod]
        public async Task GetsFromKeyVaultWhenMissingLocally()
        {
            Mock<IKeyVault> keyVault = new Mock<IKeyVault>();
            Mock<IKeyVaultConfigurationKeyEncoder> keyVaultEncoder = new Mock<IKeyVaultConfigurationKeyEncoder>();
            Mock<IAsyncConfiguration> asyncConfiguration = new Mock<IAsyncConfiguration>();

            keyVaultEncoder.Setup(x => x.Encode("mykey")).Returns("mykey");
            asyncConfiguration.Setup(x => x.GetAsync("mykey")).ReturnsAsync(null);
            keyVault.Setup(x => x.GetSecretAsync("mykey")).ReturnsAsync("keyvaultvalue");

            AsyncKeyVaultConfiguration asyncKeyVaultConfiguration = new AsyncKeyVaultConfiguration(keyVault.Object, keyVaultEncoder.Object, KeyVaultConfigurationCachePolicy.Default, asyncConfiguration.Object);

            string result = await asyncKeyVaultConfiguration.GetAsync("mykey");
            Assert.AreEqual("keyvaultvalue", result);
        }

        [TestMethod]
        public async Task HandlesKeyVaultExceptionAsNullWhenMissingInKeyVault()
        {
            Mock<IKeyVault> keyVault = new Mock<IKeyVault>();
            Mock<IKeyVaultConfigurationKeyEncoder> keyVaultEncoder = new Mock<IKeyVaultConfigurationKeyEncoder>();
            Mock<IAsyncConfiguration> asyncConfiguration = new Mock<IAsyncConfiguration>();

            keyVaultEncoder.Setup(x => x.Encode("mykey")).Returns("mykey");
            asyncConfiguration.Setup(x => x.GetAsync("mykey")).ReturnsAsync(null);
            keyVault.Setup(x => x.GetSecretAsync("mykey")).Throws(new AggregateException(new KeyVaultErrorException("Not found", null) { Response = new HttpResponseMessageWrapper(new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound}, "Not found")}));

            AsyncKeyVaultConfiguration asyncKeyVaultConfiguration = new AsyncKeyVaultConfiguration(keyVault.Object, keyVaultEncoder.Object, KeyVaultConfigurationCachePolicy.Default, asyncConfiguration.Object);

            string result = await asyncKeyVaultConfiguration.GetAsync("mykey");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetsFromCache()
        {
            Mock<IKeyVault> keyVault = new Mock<IKeyVault>();
            Mock<IKeyVaultConfigurationKeyEncoder> keyVaultEncoder = new Mock<IKeyVaultConfigurationKeyEncoder>();
            Mock<IAsyncConfiguration> asyncConfiguration = new Mock<IAsyncConfiguration>();

            keyVaultEncoder.Setup(x => x.Encode("mykey")).Returns("mykey");
            asyncConfiguration.Setup(x => x.GetAsync("mykey")).ReturnsAsync("incache");
            
            AsyncKeyVaultConfiguration asyncKeyVaultConfiguration = new AsyncKeyVaultConfiguration(keyVault.Object, keyVaultEncoder.Object, KeyVaultConfigurationCachePolicy.Default, asyncConfiguration.Object);

            await asyncKeyVaultConfiguration.GetAsync("mykey");
            string result = await asyncKeyVaultConfiguration.GetAsync("mykey");
            Assert.AreEqual("incache", result);
            asyncConfiguration.Verify(x => x.GetAsync("mykey"), Times.AtMostOnce);
        }

        [TestMethod]
        public async Task IgnoresCacheBasedOnPolicy()
        {
            Mock<IKeyVault> keyVault = new Mock<IKeyVault>();
            Mock<IKeyVaultConfigurationKeyEncoder> keyVaultEncoder = new Mock<IKeyVaultConfigurationKeyEncoder>();
            Mock<IAsyncConfiguration> asyncConfiguration = new Mock<IAsyncConfiguration>();

            keyVaultEncoder.Setup(x => x.Encode("mykey")).Returns("mykey");
            asyncConfiguration.Setup(x => x.GetAsync("mykey")).ReturnsAsync("incache");

            AsyncKeyVaultConfiguration asyncKeyVaultConfiguration = new AsyncKeyVaultConfiguration(
                keyVault.Object,
                keyVaultEncoder.Object, 
                new KeyVaultConfigurationCachePolicy
                {
                    CachingEnabled = false
                },
                asyncConfiguration.Object);

            await asyncKeyVaultConfiguration.GetAsync("mykey");
            string result = await asyncKeyVaultConfiguration.GetAsync("mykey");
            Assert.AreEqual("incache", result);
            asyncConfiguration.Verify(x => x.GetAsync("mykey"), Times.Exactly(2));
        }

        [TestMethod]
        public async Task ExpiresItemsInCache()
        {
            Mock<IKeyVault> keyVault = new Mock<IKeyVault>();
            Mock<IKeyVaultConfigurationKeyEncoder> keyVaultEncoder = new Mock<IKeyVaultConfigurationKeyEncoder>();
            Mock<IAsyncConfiguration> asyncConfiguration = new Mock<IAsyncConfiguration>();

            keyVaultEncoder.Setup(x => x.Encode("mykey")).Returns("mykey");
            asyncConfiguration.Setup(x => x.GetAsync("mykey")).ReturnsAsync("incache");

            AsyncKeyVaultConfiguration asyncKeyVaultConfiguration = new AsyncKeyVaultConfiguration(
                keyVault.Object,
                keyVaultEncoder.Object,
                new KeyVaultConfigurationCachePolicy
                {
                    CachingEnabled = true,
                    ExpiresAfter = TimeSpan.FromMilliseconds(500)
                },
                asyncConfiguration.Object);

            await asyncKeyVaultConfiguration.GetAsync("mykey");
            await Task.Delay(TimeSpan.FromMilliseconds(600));
            string result = await asyncKeyVaultConfiguration.GetAsync("mykey");
            Assert.AreEqual("incache", result);
            asyncConfiguration.Verify(x => x.GetAsync("mykey"), Times.Exactly(2));
        }
    }
}
