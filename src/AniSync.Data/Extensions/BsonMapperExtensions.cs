using LiteDB;

namespace AniSync.Data.Extensions
{
    public static class BsonMapperExtensions
    {
        public static BsonMapper WithAniSyncMappings(this BsonMapper mapper)
        {
            return mapper;
        }
    }
}
