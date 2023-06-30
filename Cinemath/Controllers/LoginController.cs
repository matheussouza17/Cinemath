using Cinemath.Data;
using Cinemath.Helper;
using Cinemath.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinemath.Controllers
{
    public class LoginController : Controller
    {
        private readonly CinemathContext _context;

        private readonly ISessionUser _session;

        public LoginController(CinemathContext context, ISessionUser session)
        {
            _context = context;
            _session = session;
        }
        public IActionResult Index()
        {
            // se ja tiver logado redirecionar para home
            if (_session.GetSessionUser() != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public User SearchLogin(string login)
        {
            return _context.Users.FirstOrDefault(x => x.Email.ToUpper() == login.ToUpper());
        }
        public IActionResult Exit()
        {
            _session.RemoveSession();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    User user = SearchLogin(loginModel.Login);
                    if (user != null)
                    {
                        if (user.ValidPassword(loginModel.Password))
                        {
                            _session.CreateSessionUser(user);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["ErrorMessage"] = $"Invalid password!";


                    }
                    TempData["ErrorMessage"] = $"Invalid login or password!";
                }
                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["ErrorMessage"] = $"Invalid Login: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
