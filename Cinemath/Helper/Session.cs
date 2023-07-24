using Cinemath.Models;
using Newtonsoft.Json;

namespace Cinemath.Helper
{
    public class Session : ISessionUser
    {
        private readonly IHttpContextAccessor _httpContext;

        public Session(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public void CreateSessionUser(User user)
        {
            string value = JsonConvert.SerializeObject(user);

            _httpContext.HttpContext.Session.SetString("sessionUser", value);
        }

        public User GetSessionUser()
        {
            string sessionUser = _httpContext.HttpContext.Session.GetString("sessionUser");

            if (string.IsNullOrEmpty(sessionUser))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<User>(sessionUser);

        }

        public void RemoveSession()
        {
            _httpContext.HttpContext.Session.Remove("sessionUser");
        }
    }
}
