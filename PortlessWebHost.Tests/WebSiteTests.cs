using System;
using System.IO;
using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PortlessWebHost.Tests
{
    [TestClass]
    public class WebSiteTests
    {
        [TestMethod]
        public void TestWebSite()
        {
            string relativePath = @"..\..\..\PortlessWebHost.TestWebSite";
            string physicalPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            using (WebHost host = new WebHost("/", physicalPath))
            {
                FiddlerWebRequest request = host.CreateRequest(new Uri("http://localhost/Default.aspx"));
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (MemoryStream responseStream = (MemoryStream)response.GetResponseStream())
                    {
                        string responseText = Encoding.GetEncoding(1252).GetString(responseStream.ToArray());
                        responseText.Should().Contain("<h2>Web Host Test Page</h2>");
                    }
                }
            }
        }
    }
}
