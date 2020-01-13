using SistemaDeTurnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeTurnos.Controllers
{
    public class TratamientoController : Controller
    {
        SystemaDeTurnosEntities db = new SystemaDeTurnosEntities();

        // GET: Tratamiento
        public ActionResult Index()
        {
            return View();
        }

        // GET: Tratamiento/Details/5
        public ActionResult Details(int Id)
        {
            return View();
        }

        // GET: Tratamiento/Create
        public ActionResult Create(int idPaciente)
        {
            Tratamiento tratamiento = new Tratamiento();
            tratamiento.Id_Paciente = idPaciente;
            this.ViewBag.Paciente = db.Paciente.Find(idPaciente);
            this.ViewBag.Medicos = db.Medico.OrderBy(m => m.Apellido).ToList();
            return View(tratamiento);
        }

        // POST: Tratamiento/Create
        [HttpPost]
        public ActionResult Create(Tratamiento tratamiento)
        {
            try
            {
                this.db.Tratamiento.Add(tratamiento);
                this.db.SaveChanges();
                this.ViewBag.Ficha = db.Ficha.FirstOrDefault(f => f.Id_Paciente == tratamiento.Id_Paciente);
                return RedirectToAction($"../Paciente/Details/{tratamiento.Id_Paciente}");
            }
            catch
            {
                return View("../Paciente/Index", this.db.Paciente.OrderBy(p => p.Apellido).ToList());
            }
        }

        // GET: Tratamiento/Edit/5
        public ActionResult Edit(int Id)
        {
            Tratamiento tratamiento = this.db.Tratamiento.Find(Id);
            this.ViewBag.Paciente = this.db.Paciente.Find(tratamiento.Id_Paciente);
            this.ViewBag.Medicos = this.db.Medico.OrderBy(m => m.Apellido).ToList();
            return View(tratamiento);
        }

        // POST: Tratamiento/Edit/5
        [HttpPost]
        public ActionResult Edit(Tratamiento tratamiento)
        {
            try
            {
                this.db.Tratamiento.Attach(tratamiento);
                this.db.Entry(tratamiento).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                Paciente paciente = db.Paciente.Find(tratamiento.Id_Paciente);
                this.ViewBag.Paciente = paciente;
                this.ViewBag.Ficha = this.db.Ficha.FirstOrDefault(f => f.Id_Paciente == tratamiento.Id_Paciente);
                return RedirectToAction($"../Paciente/Details/{paciente.Id_Paciente}");
            }
            catch
            {
                return RedirectToAction("../Paciente/Index", this.db.Paciente.OrderBy(p => p.Apellido).ToList());
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
