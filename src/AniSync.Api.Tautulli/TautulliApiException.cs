using System;
using System.Runtime.Serialization;

namespace AniSync.Api.Tautulli
{
    public class TautulliApiException : ApiException
    {
        public TautulliApiException()
        {
        }

        protected TautulliApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TautulliApiException(string message) : base(message)
        {
        }

        public TautulliApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
