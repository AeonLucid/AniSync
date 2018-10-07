using System;
using Newtonsoft.Json;

namespace AniSync.Api.Plex.Responses
{
    public class PlexPinResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("trusted")]
        public bool Trusted { get; set; }

        [JsonProperty("clientIdentifier")]
        public string ClientIdentifier { get; set; }

        [JsonProperty("location")]
        public LocationData Location { get; set; }

        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("expiresAt")]
        public DateTimeOffset ExpiresAt { get; set; }

        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        public class LocationData
        {
            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("subdivisions")]
            public string Subdivisions { get; set; }

            [JsonProperty("coordinates")]
            public string Coordinates { get; set; }
        }
    }
}