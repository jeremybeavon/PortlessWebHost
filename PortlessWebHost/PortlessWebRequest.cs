using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Fiddler;
using PortlessWebHost.Internal;

namespace PortlessWebHost
{
    [Serializable]
    public sealed class PortlessWebRequest : WebRequest
    {
        private readonly Uri requestUri;
        private readonly Func<byte[], byte[]> processRequestFunc;
        private MemoryStream requestStream;
        private long contentLength;

        internal PortlessWebRequest(Uri requestUri, Func<byte[], byte[]> processRequestFunc)
        {
            this.requestUri = requestUri;
            this.processRequestFunc = processRequestFunc;
            requestStream = new MemoryStream();
            Headers = new WebHeaderCollection();
        }

        public PortlessWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public override string ConnectionGroupName { get; set; }

        public override long ContentLength
        {
            get
            {
                return contentLength;
            }

            set
            {
                contentLength = value;
                Headers[HttpRequestHeader.ContentLength] = value.ToString();
            }
        }

        public override string ContentType
        {
            get { return Headers[HttpRequestHeader.ContentType]; }
            set { Headers[HttpRequestHeader.ContentType] = value; }
        }

        public override ICredentials Credentials { get; set; }

        public override WebHeaderCollection Headers { get; set; }

        public override string Method { get; set; }

        public override IWebProxy Proxy { get; set; }

        public override Uri RequestUri
        {
            get { return requestUri; }
        }

        public override int Timeout { get; set; }

        public override void Abort()
        {
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return new CompletedAsyncResult(callback, state);
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return new CompletedAsyncResult(callback, state);
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return GetRequestStream();
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return GetResponse();
        }

        public override Stream GetRequestStream()
        {
            return requestStream;
        }

        public override WebResponse GetResponse()
        {
            return GetPortlessResponse();
        }

        public PortlessWebResponse GetPortlessResponse()
        {
            HTTPRequestHeaders headers = new HTTPRequestHeaders()
            {
                HTTPMethod = Method,
                RequestPath = requestUri.PathAndQuery
            };
            foreach (string key in Headers.Keys.Cast<string>())
            {
                headers[key] = Headers[key];
            }

            Session session = new Session(headers, requestStream.ToArray());
            using (MemoryStream fullRequestStream = new MemoryStream())
            {
                session.WriteRequestToStream(false, false, fullRequestStream);
                return new PortlessWebResponse(requestUri, session, processRequestFunc(fullRequestStream.ToArray()));
            }
        }
    }
}
