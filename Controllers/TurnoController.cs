using Newtonsoft.Json;
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
            return View("Index");
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
                turno.Fecha_Fin = turno.Fecha_Inicio.Add(new TimeSpan(1, 0, 0));
                this.db.Turno.Add(turno);
                this.db.SaveChanges();
                return RedirectToAction($"../Paciente/Details/{turno.Id_Paciente}");
            }
            catch
            {
                return RedirectToAction("/Index");
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
                turno.Fecha_Fin = turno.Fecha_Inicio.Add(new TimeSpan(1, 0, 0));
                this.db.Turno.Add(turno);
                this.db.SaveChanges();
            }
            catch
            {
            }
            return RedirectToAction("/Index");
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
                turno.Fecha_Fin = turno.Fecha_Inicio.Add(new TimeSpan(1, 0, 0));
                this.db.Turno.Attach(turno);
                this.db.Entry(turno).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                
                return RedirectToAction($"../Paciente/Details/{turno.Id_Paciente}");
            }
            catch
            {
                return RedirectToAction("/Index");
            }
        }

        // GET: Turno/Delete/5
        public ActionResult Delete(int Id)
        {
            this.db.Turno.Remove(this.db.Turno.Find(Id));
            this.db.SaveChanges();
            this.ViewBag.Medicos = this.db.Medico.ToList();
            this.ViewBag.Pacientes = this.db.Paciente.ToList();
            List<Turno> turnos = this.db.Turno.ToList();
            return RedirectToAction("/Index");
        }

        public ActionResult GetEventData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var turnos = this.db.Turno.Select(t => new {
                t.Id_Turno,
                t.Id_Medico,
                t.Id_Paciente,
                t.Fecha_Inicio,
                t.Fecha_Fin,
            }).ToList();
            return Json(turnos, JsonRequestBehavior.AllowGet);        }
        public JsonResult GetEventDetailByEventId(int Id_Turno)
        {
            db.Configuration.ProxyCreationEnabled = false;

            Turno turno = this.db.Turno.Find(Id_Turno);
            var json = JsonConvert.SerializeObject(turno);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public void UpdateEventDetails(string eventId, DateTime startDate, DateTime endDate)
        {
            using (SystemaDeTurnosEntities db = new SystemaDeTurnosEntities())
            {
                Turno eventDetail = db.Turno.Where(x => x.Id_Turno == Convert.ToInt32(eventId)).First();
                eventDetail.Fecha_Inicio = startDate;
                eventDetail.Fecha_Fin = endDate;
                db.SaveChanges();
            }
        }
        public JsonResult TraeColores()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Color> colores = this.db.Color.ToList();
            var json = JsonConvert.SerializeObject(colores);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TraeTurnosPorIdMedico(int Id_Medico)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Turno> turnos = new List<Turno>();
            IEnumerable<Turno> turnosaux = this.db.Turno.ToList().Where(t => t.Id_Medico == Id_Medico);
            foreach(Turno t in turnosaux)
            {
                turnos.Add(t);
            }
            var json = JsonConvert.SerializeObject(turnos);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
