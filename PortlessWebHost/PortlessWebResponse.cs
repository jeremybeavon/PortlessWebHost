using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Fiddler;

namespace PortlessWebHost
{
    [Serializable]
    public sealed class PortlessWebResponse : WebResponse
    {
        private readonly WebHeaderCollection headers;
        private readonly Uri responseUri;
        private readonly MemoryStream responseStream;

        internal PortlessWebResponse(Uri responseUri, Session session, byte[] responseBytes)
        {
            this.responseUri = responseUri;
            using (MemoryStream fullResponseStream = new MemoryStream(responseBytes))
            {
                if (!session.LoadResponseFromStream(fullResponseStream, null))
                {
                    throw new InvalidOperationException("response stream could not be parsed.");
                }

                StatusCode = session.responseCode;
                StatusDescription = session.oResponse.headers.HTTPResponseStatus;
                headers = new WebHeaderCollection();
                foreach (HTTPHeaderItem header in session.oResponse.headers)
                {
                    headers.Add(header.Name, header.Value);
                }

                responseStream = new MemoryStream(session.ResponseBody);
            }
        }

        public PortlessWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        public override long ContentLength
        {
            get
            {
                long contentLength;
                long.TryParse(headers[HttpResponseHeader.ContentLength], out contentLength);
                return contentLength;
            }

            set
            {
                headers[HttpResponseHeader.ContentLength] = value.ToString();
            }
        }

        public override string ContentType
        {
            get { return headers[HttpResponseHeader.ContentType]; }
            set { headers[HttpResponseHeader.ContentType] = value; }
        }

        public override WebHeaderCollection Headers
        {
            get { return headers; }
        }

        public override bool IsFromCache
        {
            get { return false; }
        }

        public override bool IsMutuallyAuthenticated
        {
            get { return false; }
        }

        public override Uri ResponseUri
        {
            get { return responseUri; }
        }

        public override void Close()
        {
            base.Close();
            responseStream.Dispose();
        }

        public override Stream GetResponseStream()
        {
            return responseStream;
        }
    }
}
