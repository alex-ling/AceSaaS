using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Acesoft.Rbac;

namespace Acesoft.Web.Mvc.Controllers
{
    public class ValueController : Controller
    {
        private readonly IApplicationContext ctx;

        public ValueController(IApplicationContext ctx)
        {
            this.ctx = ctx;
        }

        // GET: Value
        public ActionResult Index()
        {
            return View();
        }

        // GET: Value/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Value/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Value/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Value/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Value/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Value/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Value/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}