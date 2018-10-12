using System;
using System.Reflection;
using AniSync.Data.Models;
using AniSync.Data.Repositories.Config;
using LiteDB;

namespace AniSync.Data.Repositories
{
    public class ConfigurationRepository : LiteRepository
    {
        public ConfigurationRepository(LiteDatabase database) : base(database)
        {

        }

        public T Get<T>() where T : IConfigCollection
        {
            var instance = Activator.CreateInstance<T>();

            foreach (var propertyInfo in instance.GetType().GetProperties())
            {
                var attribute = propertyInfo.GetCustomAttribute<AniConfigPropertyAttribute>();
                if (attribute != null)
                {
                    var propertyType = propertyInfo.PropertyType;
                    var propertyValue = GetOrDefault(attribute.Key);
                    if (propertyValue == null)
                    {
                        if (attribute.Default != null)
                        {
                            propertyInfo.SetValue(instance, attribute.Default);
                        }

                        continue;
                    }

                    // Deserialize.
                    if (propertyType == typeof(string))
                    {
                        propertyInfo.SetValue(instance, propertyValue);
                    }
                    else if (propertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(instance, propertyValue.Equals("y"));
                    }
                    else
                    {
                        throw new NotSupportedException($"PropertyType {propertyType.Name} is not supported in the configuration deserialization.");
                    }
                }
            }

            return instance;
        }

        public void Save<T>(T instance) where T : IConfigCollection
        {
            foreach (var propertyInfo in instance.GetType().GetProperties())
            {
                var attribute = propertyInfo.GetCustomAttribute<AniConfigPropertyAttribute>();
                if (attribute != null)
                {
                    var propertyType = propertyInfo.PropertyType;
                    var propertyValue = propertyInfo.GetValue(instance);
                    if (propertyValue == null)
                    {
                        Set(attribute.Key, null);
                        continue;
                    }

                    // Serialize.
                    if (propertyType == typeof(string))
                    {
                        Set(attribute.Key, (string) propertyValue);
                    }
                    else if (propertyType == typeof(bool))
                    {
                        Set(attribute.Key, (bool) propertyValue ? "y" : "n");
                    }
                    else
                    {
                        throw new NotSupportedException($"PropertyType {propertyType.Name} is not supported in the configuration serialization.");
                    }
                }
            }
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

        private void Set(AniConfigurationKey key, string value)
        {
            Upsert(new AniConfiguration
            {
                Id = key,
                Value = value
            });
        }

        private string GetOrDefault(AniConfigurationKey key)
        {
            return FirstOrDefault<AniConfiguration>(x => x.Id == key)?.Value;
        }
    }
}
