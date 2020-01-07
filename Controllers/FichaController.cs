using SistemaDeTurnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeTurnos.Controllers
{
    public class FichaController : Controller
    {
        SystemaDeTurnosEntities db = new SystemaDeTurnosEntities();
        // GET: Ficha
        public ActionResult Index()
        {
            return View();
        }

        // GET: Ficha/Details/5
        public ActionResult Details(int Id)
        {
            Ficha ficha = this.db.Ficha.FirstOrDefault(f => f.Id_Ficha == Id);
            return View(ficha);
        }

        // GET: Ficha/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ficha/Create
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

        // GET: Ficha/Edit/5
        public ActionResult Edit(int Id)
        {
            Ficha ficha = this.db.Ficha.FirstOrDefault(f => f.Id_Ficha == Id);
            this.ViewBag.Paciente = this.db.Paciente.FirstOrDefault(p => p.Id_Paciente == ficha.Id_Paciente);
            return View(ficha);
        }

        // POST: Ficha/Edit/5
        [HttpPost]
        public ActionResult Edit(Ficha ficha)
        {
            try
            {
                // TODO: Add update logic here
                this.db.Ficha.Attach(ficha);
                this.db.Entry(ficha).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
       
                return RedirectToAction("Details","Paciente", ficha.Id_Paciente);
            }
            catch
            {
                return View("Index","Paciente");
            }
        }

        // GET: Ficha/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ficha/Delete/5
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
