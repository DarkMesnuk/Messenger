#region Library
using ChatWithSignal.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Default
        private readonly IMessengerService _messengerService;

        public HomeController(IMessengerService messengerService)
        {
            _messengerService = messengerService;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messengers = await _messengerService.GetMessengersAsync(User.Identity.Name);
            return View(messengers.OrderBy(x => x.LastDateTimeActive));
        }
    }
}
