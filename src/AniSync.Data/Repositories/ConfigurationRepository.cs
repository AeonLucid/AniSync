using System;
using AniSync.Data.Models;
using LiteDB;

namespace AniSync.Data.Repositories
{
    public class ConfigurationRepository : LiteRepository
    {
        public ConfigurationRepository(LiteDatabase database) : base(database)
        {

        }

        public string GetPlexClientId()
        {
            var clientId = FirstOrDefault<AniConfiguration>(x => x.Id == AniConfigurationKey.PlexClientId);
            if (clientId == null)
            {
                clientId = new AniConfiguration
                {
                    Id = AniConfigurationKey.PlexClientId,
                    Value = Guid.NewGuid().ToString("N")
                };

                Insert(clientId);
            }

            return clientId.Value;
        }
    }
}
