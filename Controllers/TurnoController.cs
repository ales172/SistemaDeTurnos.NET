using SistemaDeTurnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeTurnos.Controllers
{
    public class TurnoController : Controller
    {
        SystemaDeTurnosEntities db = new SystemaDeTurnosEntities();

        // GET: Turno
        public ActionResult Index()
        {
            this.ViewBag.Pacientes = this.db.Paciente.ToList();
            this.ViewBag.Medicos = this.db.Medico.ToList();
            return View("Index",this.db.Turno.ToList());
        }

        // GET: Turno/Details/5
        public ActionResult Details(int Id)
        {
            Turno turno = this.db.Turno.Find(Id);
            this.ViewBag.Medico = this.db.Medico.Find(turno.Id_Medico);
            this.ViewBag.Paciente = this.db.Paciente.Find(turno.Id_Paciente);
            return View(turno);
        }

        public ActionResult CreateFromPaciente(int Id)
        {
            Turno turno = new Turno();
            turno.Id_Paciente = Id;
            this.ViewBag.Medicos = this.db.Medico.OrderBy(m => m.Apellido).ToList();
            this.ViewBag.Pacientes = this.db.Paciente.ToList();
            this.ViewBag.Paciente = this.db.Paciente.Find(Id);
            return View("CreateFromPaciente", turno);
        }

        [HttpPost]
        public ActionResult CreateFromPaciente(Turno turno)
        {
            this.ViewBag.Medicos = this.db.Medico.ToList();
            this.ViewBag.Pacientes = this.db.Paciente.ToList();
            try
            {
                if (turno == null)
                {
                    return Content("No se pudieron insertar los datos, faltan datos");
                }
                turno.Fin = turno.Inicio.Add(new TimeSpan(1, 0, 0));
                this.db.Turno.Add(turno);
                this.db.SaveChanges();
                return RedirectToAction($"../Paciente/Details/{turno.Id_Paciente}");
            }
            catch
            {
                return View("Index", db.Turno.ToList());
            }
        }
        // GET: Turno/Create
        public ActionResult Create()
        {
            Turno turno = new Turno();
            this.ViewBag.Medicos = this.db.Medico.OrderBy(m => m.Apellido).ToList();
            this.ViewBag.Pacientes = this.db.Paciente.OrderBy(p => p.Apellido).ToList();
            return View("Create", turno);
        }
        // POST: Turno/Create
        [HttpPost]
        public ActionResult Create(Turno turno)
        {
            this.ViewBag.Medicos = this.db.Medico.ToList();
            this.ViewBag.Pacientes = this.db.Paciente.ToList();
            try
            {
                if (turno == null)
                {
                    return Content("No se pudieron insertar los datos, faltan datos");
                }
                turno.Fin = turno.Inicio.Add(new TimeSpan(1,0,0));
                this.db.Turno.Add(turno);
                this.db.SaveChanges();
                return View("Index", db.Turno.ToList());
            }
            catch
            {
                return View("Index", db.Turno.ToList());
            }
        }

        // GET: Turno/Edit/5
        public ActionResult Edit(int Id)
        {
            this.ViewBag.Medicos = this.db.Medico.ToList();
            this.ViewBag.Pacientes = this.db.Paciente.ToList();

            return View(this.db.Turno.Find(Id));
        }

        // POST: Turno/Edit/5
        [HttpPost]
        public ActionResult Edit(Turno turno)
        {
            this.ViewBag.Medicos = this.db.Medico.ToList();
            this.ViewBag.Pacientes = this.db.Paciente.ToList();
            try
            {
                this.db.Turno.Attach(turno);
                this.db.Entry(turno).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                
                return View("Index",this.db.Turno.ToList());
            }
            catch
            {
                return View("Index",this.db.Turno.ToList());
            }
        }

        // GET: Turno/Delete/5
        public ActionResult Delete(int Id)
        {
            this.db.Turno.Remove(this.db.Turno.Find(Id));
            this.db.SaveChanges();
            this.ViewBag.Medicos = this.db.Medico.ToList();
            this.ViewBag.Pacientes = this.db.Paciente.ToList();
            return View("Index", this.db.Turno.ToList());
        }
    }
}
