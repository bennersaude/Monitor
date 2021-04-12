using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Monitor.Api.Controllers
{
    public class HomeController: Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}