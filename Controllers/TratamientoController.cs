using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeTurnos.Controllers
{
    public class TratamientoController : Controller
    {
        // GET: Tratamiento
        public ActionResult Index()
        {
            return View();
        }

        // GET: Tratamiento/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tratamiento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tratamiento/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tratamiento/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tratamiento/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tratamiento/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tratamiento/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
