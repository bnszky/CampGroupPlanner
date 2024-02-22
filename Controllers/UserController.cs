﻿using CampGroupPlanner.Data;
using CampGroupPlanner.Models;
using Microsoft.AspNetCore.Mvc;

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
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
