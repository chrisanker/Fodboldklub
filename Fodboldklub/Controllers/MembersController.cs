using System.Text.Json;
using Fodboldklub.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fodboldklub.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult List()
        {
            var membersJson = HttpContext.Session.GetString("Members");
            var members = membersJson != null ? JsonSerializer.Deserialize<List<Member>>(membersJson) : new List<Member>();

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
                var membersJson = HttpContext.Session.GetString("Members");
                var members = membersJson != null ? JsonSerializer.Deserialize<List<Member>>(membersJson) : new List<Member>();

                Member newMember = new Member
                {
                    FirstName = inputMember.FirstName,
                    LastName = inputMember.LastName,
                    Address = inputMember.Address,
                    Phone = inputMember.Phone,
                    Email = inputMember.Email,
                };
                members.Add(newMember);
                Console.WriteLine($"{newMember.FirstName} {newMember.LastName} added.");
                foreach (Member member in members)
                {
                    Console.WriteLine($"{member.FirstName} {member.LastName}");
                }
                HttpContext.Session.SetString("Members", JsonSerializer.Serialize(members));
            }           
            return RedirectToAction("List");
        }

    }
}

