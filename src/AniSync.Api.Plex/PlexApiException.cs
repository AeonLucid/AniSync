using System;
using System.Runtime.Serialization;

namespace AniSync.Api.Plex
{
    public class PlexApiException : ApiException
    {
        public PlexApiException()
        {
        }

        protected PlexApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PlexApiException(string message) : base(message)
        {
        }

        public PlexApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
