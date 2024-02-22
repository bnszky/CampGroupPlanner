using CampGroupPlanner.Data;
using CampGroupPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;

namespace CampGroupPlanner.Controllers
{
    public class UserController : Controller
    {
        AppDbController _db;
        public UserController(AppDbController db) {
            _db = db;
        }
        public IActionResult Index()
        {
            List<User> users = _db.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
			if(user.Email == null || !Regex.IsMatch(user.Email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
            {
                ModelState.AddModelError("Email", "Email is invalid!");
            }
			if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

		public IActionResult Edit(int? id)
		{
			if(id == null)
            {
                return NotFound();
            }

            User? user = _db.Users.SingleOrDefault(u => u.Id == id);

            if(user == null)
            {
                return NotFound();
            }

            return View(user);
		}

        [HttpPost]
        public IActionResult Edit(User user)
        {
			if (user.Email == null || !Regex.IsMatch(user.Email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
			{
				ModelState.AddModelError("Email", "Email is invalid!");
			}
			if (ModelState.IsValid)
			{
				_db.Users.Update(user);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				return View(user.Id);
			}
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			User? user = _db.Users.SingleOrDefault(u => u.Id == id);

			if (user == null)
			{
				return NotFound();
			}

            _db.Users.Remove(user);
            _db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
