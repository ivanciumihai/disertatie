﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using personal_pages;
using personal_pages.Models;

namespace Personal_Pages.Controllers
{
    public class CoursesController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Courses
        public async Task<ActionResult> Index()
        {
            var courses = _db.Courses.Include(c => c.Departament).Include(c => c.User);
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = await _db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name");
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FullName");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "CourseId,Name,Credits,DepartamentId,TeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseId = Guid.NewGuid();
                _db.Courses.Add(course);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name", course.DepartamentId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = await _db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name", course.DepartamentId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "CourseId,Name,Credits,DepartamentId,TeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(course).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name", course.DepartamentId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = await _db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _db.Courses.FindAsync(id);
            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetTeacher(Guid depId)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName("Teacher").Users.ToList();
            IQueryable<User> users = null;

            foreach (var plm in role)
            {
                 users = _db.Users.Where(x => x.RoleId == plm.RoleId && x.DepID==depId);
            }

            return new JsonResult { Data = users.Select(x => new { x.UserId, x.FirstName }).ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}