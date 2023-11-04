using Fodboldklub.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fodboldklub.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult List()
        {
            var membersViewModel = new MembersViewModel
            {
                Members = new List<Member>()
            };

            return View(membersViewModel);
        }
    public IActionResult Add()
    {
        return View();
    }
}
}
