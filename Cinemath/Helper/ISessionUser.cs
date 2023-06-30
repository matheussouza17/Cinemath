using Cinemath.Models;

namespace Cinemath.Helper
{
    public interface ISessionUser
    {
        void CreateSessionUser(User user);
        void RemoveSession();
        User GetSessionUser();

    }
}
