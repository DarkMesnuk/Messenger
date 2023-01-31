#region Library
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ChatWithSignal.Models.Identity;
using ChatWithSignal.Domain.Identity;
#endregion

namespace ChatWithSignal.Controllers
{
    public class IdentityController : Controller
    {
        #region Default
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _singInManager;

        public IdentityController(UserManager<Profile> userManager, SignInManager<Profile> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
        }
        #endregion

        #region View
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
            => View();
            
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Index", "Identity");
        }
        #endregion

        #region Processing
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index");

            var profile = await _userManager.FindByEmailAsync(model.Email);

            if (profile != null)
            {
                await _singInManager.SignOutAsync();
                var result = await _singInManager.PasswordSignInAsync(profile, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("", "Неправильна пошта або пароль");
            }
            else
                ModelState.AddModelError("", "Неправильна пошта або пароль");

            return View("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index");

            if (model.Password == model.RepeatPassword)
            {
                var IsExistProfile = await _userManager.FindByEmailAsync(model.Email);

                if (IsExistProfile == null)
                {
                    var profile = new Profile(model.NickName, model.Email);

                    await _userManager.CreateAsync(profile, model.Password);
                }

                return RedirectToAction("Index", "Identity");
            }
            else
                ModelState.AddModelError(nameof(RegisterViewModel.RepeatPassword), "Паролі не однакові");
            
            return View("Index");
        }
        #endregion
    }
}