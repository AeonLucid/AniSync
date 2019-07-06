using AniSync.Data.Models;
using LiteDB;

namespace AniSync.Data.Repositories
{
    public class UserRepository : LiteRepository
    {
        public UserRepository(LiteDatabase database) : base(database)
        {

        }

        public void Create(AniUser user)
        {
            Insert(user);
        }

        public bool Exists(long userId)
        {
            return FirstOrDefault<AniUser>(x => x.Id == userId) != null;
        }

        public bool ContainsAdmin()
        {
            return FirstOrDefault<AniUser>(x => x.Admin) != null;
        }

        public bool IsAdmin(long userId)
        {
            var user = First<AniUser>(x => x.Id == userId);
            return user.Admin;
        }
    }
}
