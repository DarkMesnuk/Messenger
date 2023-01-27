#region Library
using ChatWithSignal.Models.Messenger;
using ChatWithSignal.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Controllers
{
    [Authorize]
    public class MessengerController : Controller
    {
        #region Default
        private readonly IMessengerService _messengerService;

        public MessengerController(IMessengerService messengerService)
        {
            _messengerService = messengerService;
        }
        #endregion

        #region View
        [HttpGet]
        public async Task<IActionResult> OpenChat(string profileId)
        {
            await _messengerService.GetOrCreateChatAsync(User.Identity.Name, profileId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> JoinGroup(Guid messengerId)
        {
            await _messengerService.GetOrJoinGroupAsync(User.Identity.Name, messengerId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult CreateGroup() 
            => View(new CreateGroupViewModel());

        [HttpGet]
        public async Task<IActionResult> GroupSettings(Guid groupId)
        {
            var group = await _messengerService.GetOrJoinGroupAsync(User.Identity.Name, groupId);

            if (group != null)
                return View(new GroupSettingsViewModel { GroupId = group.Id, Text = group.Text, IsPublic = group.IsPublic, Name = group.Name });

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Processing
        [HttpPost]
        public async Task<IActionResult> CreateGroup(CreateGroupViewModel model)
        {
            if (ModelState.IsValid)
                await _messengerService.CreateGroupAsync(User.Identity.Name, model.Name, model.IsPublic);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> GroupSettings(GroupSettingsViewModel model)
        {
            await _messengerService.ChangeGroupSettingsAsync(User.Identity.Name, model);

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}