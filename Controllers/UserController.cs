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
    }
}
