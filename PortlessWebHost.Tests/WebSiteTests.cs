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
                PortlessWebRequest request = host.CreateRequest(new Uri("http://localhost/Default.aspx"));
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

        [TestMethod]
        public void TestWebSite2()
        {
            string relativePath = @"..\..\..\PortlessWebHost.TestWebSite2";
            string physicalPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            using (WebHost host = new WebHost("/", physicalPath))
            {
                PortlessWebRequest request = host.CreateRequest(new Uri("http://localhost/Account/Login"));
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (MemoryStream responseStream = (MemoryStream)response.GetResponseStream())
                    {
                        string responseText = Encoding.GetEncoding(1252).GetString(responseStream.ToArray());
                        responseText.Should().Contain("Web Host Test Page 2");
                    }
                }
            }
        }

        [TestMethod]
        public void TestWebSite2WithHttps()
        {
            const string url = "https://localhost/Account/ConfirmEmail?userId=11&code=success";
            const string relativePath = @"..\..\..\PortlessWebHost.TestWebSite2";
            string physicalPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            using (WebHost host = new WebHost("/", physicalPath, Protocol.Https))
            {
                PortlessWebRequest request = host.CreateRequest(new Uri(url));
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (MemoryStream responseStream = (MemoryStream)response.GetResponseStream())
                    {
                        string responseText = Encoding.GetEncoding(1252).GetString(responseStream.ToArray());
                        responseText.Should().Contain("Thank you for confirming your email.");
                    }
                }
            }
        }

        [TestMethod]
        public void TestWebSite2Post()
        {
            const string json = "{Hometown:\"Katmandu\"}";
            string relativePath = @"..\..\..\PortlessWebHost.TestWebSite2";
            string physicalPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            using (WebHost host = new WebHost("/", physicalPath))
            {
                PortlessWebRequest request = host.CreateRequest(new Uri("http://localhost/api/Me"));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = json.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    using (TextWriter requestWriter = new StreamWriter(requestStream))
                    {
                        requestWriter.Write(json);
                    }
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (MemoryStream responseStream = (MemoryStream)response.GetResponseStream())
                    {
                        string responseText = Encoding.UTF8.GetString(responseStream.ToArray());
                        responseText.Should().Contain("Hometown = Katmandu");
                    }
                }
            }
        }

        [TestMethod]
        public void TestWebSiteAsyncRequest()
        {
            string relativePath = @"..\..\..\PortlessWebHost.TestWebSite";
            string physicalPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            using (TestAsyncWebHost host = new TestAsyncWebHost("/", physicalPath))
            {
                PortlessWebRequest request = host.CreateRequest(new Uri("http://localhost/AsyncRequest.ashx"));
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {
                    using (MemoryStream responseStream = (MemoryStream)response.GetResponseStream())
                    {
                        string responseText = Encoding.UTF8.GetString(responseStream.ToArray());
                        responseText.Should().Contain("Async test succeeded");
                    }
                }
            }
        }
    }
}
