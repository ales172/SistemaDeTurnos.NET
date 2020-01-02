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
            List<Paciente> listadoPacientes = db.Paciente.OrderBy(p => p.Apellido).ToList();
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
            return View(paciente);
        }
        /*
        public ActionResult Details(Paciente paciente)
        {
            return View();
        }*/

            // GET: Paciente/Create
            public ActionResult Create()
        {
            this.ViewBag.TituloPagina = "Agregar Paciente";
            Paciente paciente = new Paciente();
            return View("Edit", paciente);
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
                paciente.Id_Paciente = db.Paciente.Count();//Asigno el ultimo id de paciente
                db.Paciente.Add(paciente);// lo agrego y guardo los cambios
                db.SaveChanges();
                // creo una nueva ficha a la cual voy a asignarle al paciente
                Ficha nuevaFicha = new Ficha();
                nuevaFicha.Id_Paciente = paciente.Id_Paciente;
                db.Ficha.Add(nuevaFicha);
                db.SaveChanges();

                return RedirectToAction("Edit", paciente.Id_Paciente);
            }
            catch
            {
                return View();
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
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Paciente/Delete/5
        public ActionResult Delete(int Id)
        {
            Paciente paciente = this.db.Paciente.FirstOrDefault(p => p.Id_Paciente == Id);
            this.db.Paciente.Remove(paciente);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
