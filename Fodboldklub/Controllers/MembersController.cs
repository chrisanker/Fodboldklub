using System.Text.Json;
using Fodboldklub.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fodboldklub.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult List()
        {
            List<Member>? members = getMembersFromSession();
            var membersViewModel = new MembersViewModel { Members = members };
            return View(membersViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Member inputMember)
        {
            if (ModelState.IsValid)
            {
                var members = getMembersFromSession();

                Member newMember = new Member
                {
                    FirstName = inputMember.FirstName,
                    LastName = inputMember.LastName,
                    Address = inputMember.Address,
                    Phone = inputMember.Phone,
                    Email = inputMember.Email,
                };
                members.Add(newMember);               
                HttpContext.Session.SetString("Members", JsonSerializer.Serialize(members));
            }           
            return RedirectToAction("List");
        }
        private List<Member>? getMembersFromSession()
        {
            var membersJson = HttpContext.Session.GetString("Members");
            var members = membersJson != null ? JsonSerializer.Deserialize<List<Member>>(membersJson) : new List<Member>();
            return members;
        }
    }
}

