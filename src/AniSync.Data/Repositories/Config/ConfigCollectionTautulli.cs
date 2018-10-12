using AniSync.Data.Models;

namespace AniSync.Data.Repositories.Config
{
    public class ConfigCollectionTautulli : IConfigCollection
    {
        [AniConfigProperty(AniConfigurationKey.TautulliEnabled)]
        public bool Enabled { get; set; }

        [AniConfigProperty(AniConfigurationKey.TautulliEndpoint)]
        public string Endpoint { get; set; }

        [AniConfigProperty(AniConfigurationKey.TautulliApiKey)]
        public string ApiKey { get; set; }
    }
}
