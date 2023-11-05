using System.Text.Json;
using Fodboldklub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fodboldklub.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult List(string sortOrder)
        {
            ViewData["LastNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            List<Member>? members = getMembersFromSession();
            switch(sortOrder)
            {
                case "lastname_desc":
					members = members.OrderByDescending(m => m.LastName).ToList();
					break;
                default:
                    members = members.OrderBy(m => m.LastName).ToList();
                    break;
            }
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
                int idCounter = HttpContext.Session.GetInt32("IdCounter") ?? 0;
                idCounter++;
                Member newMember = new Member
                {
                    Id = idCounter,
                    FirstName = inputMember.FirstName,
                    LastName = inputMember.LastName,
                    Address = inputMember.Address,
                    Phone = inputMember.Phone,
                    Email = inputMember.Email,
                };
                members.Add(newMember);
                HttpContext.Session.SetInt32("IdCounter", idCounter);
                HttpContext.Session.SetString("Members", JsonSerializer.Serialize(members));
            }
            return RedirectToAction("List");
        }

        public IActionResult Edit(int id)
        {
			var memberToEdit = getMembersFromSession().FirstOrDefault(m => m.Id == id);
			if (memberToEdit == null) { return NotFound(); }
			return View(memberToEdit);
        }

        [HttpPost]
        public IActionResult EditConfirmed(int id, [Bind("Id,FirstName,LastName,Address,Phone,Email")] Member member)
        {
            var members = getMembersFromSession();
            var memberToEdit = members.FirstOrDefault(m => m.Id == id);
            
            memberToEdit.FirstName = member.FirstName;
            memberToEdit.LastName = member.LastName;
            memberToEdit.Address = member.Address;
            memberToEdit.Phone = member.Phone;
            memberToEdit.Email = member.Email;

			HttpContext.Session.SetString("Members", JsonSerializer.Serialize(members));

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

