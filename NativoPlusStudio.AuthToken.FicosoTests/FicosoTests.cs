using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.Ficoso;

namespace NativoPlusStudio.AuthToken.FicosoTests
{
    [TestClass]
    public class FicosoTests : BaseConfiguration
    {
        [TestMethod]
        public void TestMethod1()
        {
            IAuthTokenProvider tokenProvider = new FicosoAuthTokenProvider(new System.Net.Http.HttpClient(), Options.Create(new Ficoso.Configurations.FicosoAuthTokenOptions()));
            Assert.IsTrue(true);
        }
        
        //[TestMethod]
        //public void TestFicosoWorksWithToken()
        //{
        //    var client = serviceProvider.GetRequiredService<FCSHttpClient>();

        //    var token = client.GetFillingAsync("replace this with your filing id").GetAwaiter().GetResult();

        //    var ficosoResponse = token.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        //    Assert.IsTrue(ficosoResponse != null && token.IsSuccessStatusCode);
        //}
        
        //[TestMethod]
        //public void TestTokenWasFetched()
        //{
        //    var authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

        //    var token = authTokenGenerator.GetTokenAsync(protectedResource: configuration["FicosoOptions:ProtectedResourceName"]).GetAwaiter().GetResult();

        //    Assert.IsTrue(token.Token != null);
        //}
    }
}
