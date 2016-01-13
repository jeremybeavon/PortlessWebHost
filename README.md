# Overview
Host an ASP.Net website without any ports but requests can be made to it.

This can be used to host ASP.Net websites inside of tests.

# Example

```csharp
using System;
using System.IO;
using System.Net;
using PortlessWebHost;

// This example hosts a web site and makes a single request to it.
public static class PortlessWebHostTest
{
  public static byte[] RunSingleRequest(string virtualDirectory, string physicalPath, Uri urlToRequest)
  {
    using (WebHost host = new WebHost("/", physicalPath))
    {
      PortlessWebRequest request = host.CreateRequest(urlToRequest);
      request.Method = "GET";
      using (WebResponse response = request.GetResponse())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          byte[] responseBytes = new byte[responseStream.Length];
          responseStream.Read(bytes, 0, responseStream.Length);
          return responseBytes;
        }
      }
    }
  }
}
```
