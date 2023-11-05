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
                    Id = members.Count + 1,
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

        public IActionResult Delete(int id)
        {
            var memberToDelete = getMembersFromSession().FirstOrDefault(m => m.Id == id);
            if (memberToDelete == null) { return NotFound(); }
            return View(memberToDelete);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var members = getMembersFromSession();
            Console.WriteLine("Member Id to delete: {0}", id);
            var memberToDelete = members.FirstOrDefault(m => m.Id == id);
            members.Remove(memberToDelete);
            foreach(Member member in members)
            {
                Console.WriteLine(member.Id + " " + member.FirstName);
            }
            HttpContext.Session.SetString("Members", JsonSerializer.Serialize(members));
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

