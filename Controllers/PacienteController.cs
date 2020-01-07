using SistemaDeTurnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeTurnos.Controllers
{
    public class PacienteController : Controller
    {
        SystemaDeTurnosEntities db = new SystemaDeTurnosEntities();
        // GET: Paciente
        public ActionResult Index()
        {
            List<Paciente> listadoPacientes = this.db.Paciente.OrderBy(p => p.Apellido).ToList();
            return View("Index",listadoPacientes);
        }
        [HttpPost]
        public ActionResult Index(String aBuscar)
        {
            List<Paciente> todos = db.Paciente.ToList();
            IQueryable<Paciente> qryNombre = db.Paciente;
            IQueryable<Paciente> qryApellido = db.Paciente;
            List<Paciente> listadoPacientes = new List<Paciente>();

            if((aBuscar!= null) && (aBuscar.Length != 0))
            {
                qryNombre = qryNombre.Where(p => p.Nombre.Contains(aBuscar));
                qryApellido = qryApellido.Where(p => p.Apellido.Contains(aBuscar));
                foreach (Paciente paciente in qryNombre)
                {
                    listadoPacientes.Add(paciente);
                }
                foreach (Paciente pac in qryApellido)
                {
                    int flag = 0;
                    foreach(Paciente p in listadoPacientes)
                    {
                        if(p.Id_Paciente == pac.Id_Paciente)
                        {
                            flag = 1;
                        }
                    }
                    if (flag == 0)
                    {
                    listadoPacientes.Add(pac);
                    }
                }
            

            }

            return View(listadoPacientes);
        }
        // GET: Paciente/Details/5
        public ActionResult Details(int Id)
        {
            Paciente paciente = db.Paciente.Find(Id);
            this.ViewBag.Ficha = db.Ficha.FirstOrDefault(f => f.Id_Paciente == Id);
            return View(paciente);
        }
        
            // GET: Paciente/Create
            public ActionResult Create()
        {
            this.ViewBag.TituloPagina = "Agregar Paciente";
            Paciente paciente = new Paciente();
            return View("Create", paciente);
        }

        // POST: Paciente/Create
        [HttpPost]
        public ActionResult Create(Paciente paciente)
        {
            try
            {
                if (paciente == null)
                {
                    return Content("No puedo insertar los datos, faltan datos");
                }

                if (paciente.Nombre.Length == 0)
                {
                    return Content("No puedo insertar los datos, faltan datos");
                }
                this.db.Paciente.Add(paciente);// lo agrego y guardo los cambios
                this.db.SaveChanges();
                // creo una nueva ficha a la cual voy a asignarle al paciente
                Ficha nuevaFicha = new Ficha();
                nuevaFicha.Id_Paciente = (int)this.db.Paciente.FirstOrDefault(p => p.Dni == paciente.Dni).Id_Paciente;
                db.Ficha.Add(nuevaFicha);
                db.SaveChanges();

                return RedirectToAction("Details", nuevaFicha.Id_Paciente);
            }
            catch
            {
                List<Paciente> pacientes = this.db.Paciente.OrderBy(p => p.Apellido).ToList();
                return View("Index", pacientes);
            }
        }

        // GET: Paciente/Edit/5
        public ActionResult Edit(int Id)
        {
            this.ViewBag.TituloPagina = "Editar Paciente";
            Paciente aEditar = db.Paciente.Find(Id);
            return View(aEditar);
        }

        // POST: Paciente/Edit/5
        [HttpPost]
        public ActionResult Edit(Paciente paciente)
        {
            try
            {
                this.db.Paciente.Attach(paciente);
                this.db.Entry(paciente).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                return RedirectToAction("Details",paciente.Id_Paciente);
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: Paciente/Delete/5
        public ActionResult Delete(int Id)
        {
            Paciente paciente = this.db.Paciente.FirstOrDefault(p => p.Id_Paciente == Id);
            Ficha ficha = this.db.Ficha.FirstOrDefault(f => f.Id_Paciente == paciente.Id_Paciente);
            this.db.Ficha.Remove(ficha);
            this.db.Paciente.Remove(paciente);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult verFicha(int Id)
        {
            Ficha ficha = this.db.Ficha.FirstOrDefault(f => f.Id_Ficha == Id);
            return View(ficha);
        }

    }
}
