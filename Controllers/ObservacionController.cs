using SistemaDeTurnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeTurnos.Controllers
{
    public class ObservacionController : Controller
    {
        SystemaDeTurnosEntities db = new SystemaDeTurnosEntities();

        // GET: Observacion
        public ActionResult Index()
        {
            return View();
        }

        // GET: Observacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Observacion/Create
        public ActionResult Create(int idPaciente)
        {
            Observacion observacion = new Observacion();
            observacion.Id_Paciente = idPaciente;
            this.ViewBag.Paciente = db.Paciente.Find(idPaciente);
            this.ViewBag.Medicos = db.Medico.OrderBy(m => m.Apellido).ToList();
            return View(observacion);
        }

        // POST: Observacion/Create
        [HttpPost]
        public ActionResult Create(Observacion observacion)
        {
            try
            {
                this.db.Observacion.Add(observacion);
                this.db.SaveChanges();
                this.ViewBag.Ficha = db.Ficha.FirstOrDefault(f => f.Id_Paciente == observacion.Id_Paciente);
                return RedirectToAction($"../Paciente/Details/{observacion.Id_Paciente}");
            }
            catch
            {
                return View("../Paciente/Index",this.db.Paciente.OrderBy(p => p.Apellido).ToList());
            }
        }

        // GET: Observacion/Edit/5
        public ActionResult Edit(int Id)
        {
            Observacion observacion = this.db.Observacion.Find(Id);
            this.ViewBag.Paciente = this.db.Paciente.Find(observacion.Id_Paciente);
            this.ViewBag.Medicos = this.db.Medico.OrderBy(m => m.Apellido).ToList();
            return View(observacion);
        }

        // POST: Observacion/Edit/5
        [HttpPost]
        public ActionResult Edit(Observacion observacion)
        {
            try
            {
                this.db.Observacion.Attach(observacion);
                this.db.Entry(observacion).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                Paciente paciente = db.Paciente.Find(observacion.Id_Paciente);
                this.ViewBag.Paciente = paciente;
                this.ViewBag.Ficha = this.db.Ficha.FirstOrDefault(f => f.Id_Paciente == observacion.Id_Paciente);
                return RedirectToAction($"../Paciente/Details/{paciente.Id_Paciente}");
            }
            catch
            {
                return RedirectToAction("../Paciente/Index", this.db.Paciente.OrderBy(p => p.Apellido).ToList());
            }
        }

        // GET: Observacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Observacion/Delete/5
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
