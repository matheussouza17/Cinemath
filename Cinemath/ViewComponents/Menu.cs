using Cinemath.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cinemath.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessionUser = HttpContext.Session.GetString("sessionUser");
            if (string.IsNullOrEmpty(sessionUser))
            {
                return null;
            }

            User user = JsonConvert.DeserializeObject<User>(sessionUser);

            return View(user);
        }
    }
}
