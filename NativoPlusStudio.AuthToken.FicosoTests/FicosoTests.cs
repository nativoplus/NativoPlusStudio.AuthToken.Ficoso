using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.Ficoso;

namespace NativoPlusStudio.AuthToken.FicosoTests
{
    [TestClass]
    public class FicosoTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IAuthTokenProvider tokenProvider = new FicosoAuthTokenProvider(new System.Net.Http.HttpClient(), Options.Create(new Ficoso.Configurations.FicosoAuthTokenOptions()));
            Assert.IsTrue(true);
        }
    }
}
