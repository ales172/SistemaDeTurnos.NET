using SistemaDeTurnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeTurnos.Controllers
{
    public class MedicoController : Controller
    {
        SystemaDeTurnosEntities db = new SystemaDeTurnosEntities();

        // GET: Medico
        public ActionResult Index()
        {
            List<Medico> medicos = this.db.Medico.OrderBy(m => m.Apellido).ToList();
            return View("Index",medicos);
        }
        [HttpPost]
        public ActionResult Index(String busquedaMedico)
        {
            List<Medico> todos = db.Medico.ToList();
            IQueryable<Medico> qryNombre = db.Medico;
            IQueryable<Medico> qryApellido = db.Medico;
            List<Medico> listadoMedicos = new List<Medico>();

            if ((busquedaMedico != null) && (busquedaMedico.Length != 0))
            {
                qryNombre = qryNombre.Where(m => m.Nombre.Contains(busquedaMedico));
                qryApellido = qryApellido.Where(m => m.Apellido.Contains(busquedaMedico));
                foreach (Medico medico in qryNombre)
                {
                    listadoMedicos.Add(medico);
                }
                foreach (Medico med in qryApellido)
                {
                    int flag = 0;
                    foreach (Medico m in listadoMedicos)
                    {
                        if (m.Id_Medico == med.Id_Medico)
                        {
                            flag = 1;
                        }
                    }
                    if (flag == 0)
                    {
                        listadoMedicos.Add(med);
                    }
                }


            }
            return View(listadoMedicos);
        }


        // GET: Medico/Details/5
        public ActionResult Details(int Id)
        {
            Medico medico = db.Medico.Find(Id);
            return View(medico);
        }

        // GET: Medico/Create
        public ActionResult Create()
        {
            this.ViewBag.TituloPagina = "Agregar Medico";
            Medico medico = new Medico();
            return View("Create", medico);
        }

        // POST: Medico/Create
        [HttpPost]
        public ActionResult Create(Medico medico)
        {
            try
            {
                if (medico == null)
                {
                    return Content("No puedo insertar los datos, faltan datos");
                }

                if (medico.Nombre.Length == 0)
                {
                    return Content("No puedo insertar los datos, faltan datos");
                }
                this.db.Medico.Add(medico);// lo agrego y guardo los cambios
                this.db.SaveChanges();
                int id = this.db.Medico.FirstOrDefault(m => m.Dni == medico.Dni).Id_Medico;
                
                return RedirectToAction("Details", id);
            }
            catch
            {
                List<Medico> medicos = this.db.Medico.OrderBy(m => m.Apellido).ToList();
                return View("Index", medicos);
            }
        }

        // GET: Medico/Edit/5
        public ActionResult Edit(int Id)
        {
            this.ViewBag.TituloPagina = "Editar Medico";
            Medico aEditar = db.Medico.Find(Id);
            return View(aEditar);
        }

        // POST: Medico/Edit/5
        [HttpPost]
        public ActionResult Edit(Medico medico)
        {
            try
            {
                this.db.Medico.Attach(medico);
                this.db.Entry(medico).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                return RedirectToAction("Details", medico.Id_Medico);
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
