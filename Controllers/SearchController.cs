#region Library
using ChatWithSignal.Domain.Search;
using ChatWithSignal.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        #region Default
        private readonly IMessengerService _messengerService;
        private readonly IProfileService _profileService;

        public SearchController(IMessengerService messengerService, IProfileService profileService)
        {
            _messengerService = messengerService;
            _profileService = profileService;
        }
        #endregion

        #region View
        [HttpGet]
        public IActionResult Index() 
            => RedirectToAction("Profiles", "Search");
        
        [HttpGet]
        public async Task<IActionResult> Profiles()
        {
            var profiles = await _profileService.GetSAsync();
            var activeProfile = await _profileService.GetByEmailAsync(User.Identity.Name);

            profiles.Remove(profiles.First(x => x.Id == activeProfile.Id));

            return View(profiles);
        }

        [HttpGet]
        public async Task<IActionResult> Groups() 
            => View(await _messengerService.GetSGroupsAsync());
        #endregion
    }
}