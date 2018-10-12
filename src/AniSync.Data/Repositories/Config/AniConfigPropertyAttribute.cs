using System;
using AniSync.Data.Models;

namespace AniSync.Data.Repositories.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class AniConfigPropertyAttribute : Attribute
    {
        public AniConfigPropertyAttribute(AniConfigurationKey key)
        {
            Key = key;
        }

        public AniConfigurationKey Key { get; }

        public object Default { get; set; }
    }
}
