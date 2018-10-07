using LiteDB;

namespace AniSync.Data.Extensions
{
    public static class LiteDatabaseExtensions
    {
        public static LiteDatabase WithAniSyncMigrations(this LiteDatabase database, bool isDevelopment)
        {
            var engine = database.Engine;

            // Run migrations.
            if (engine.UserVersion == 0)
            {
                // Run stuff.
                engine.UserVersion = 1;
            }

            // Keep UserVersion as 0 when doing development.
            if (isDevelopment)
            {
                engine.UserVersion = 0;
            }

            return database;
        }
    }
}
