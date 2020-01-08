using SistemaDeTurnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            if ((aBuscar != null) && (aBuscar.Length != 0))
            {
                /*//normalizo el string de entrada y le saco acentos
                string textoOriginal = aBuscar;
                string textoNormalizado = textoOriginal.Normalize(NormalizationForm.FormD);
                Regex reg = new Regex("[^a-zA-Z0-9 ]");
                string textoSinAcentos = reg.Replace(textoNormalizado, "");
                */
                List<Paciente> todos = db.Paciente.ToList();
                IQueryable<Paciente> qryNombre = db.Paciente;
                IQueryable<Paciente> qryApellido = db.Paciente;
                List<Paciente> listadoPacientes = new List<Paciente>();

                qryNombre = qryNombre.Where(p => p.Nombre.Contains(aBuscar));
                qryApellido = qryApellido.Where(p => p.Apellido.Contains(aBuscar));
                foreach (Paciente paciente in qryNombre)
                {
                    listadoPacientes.Add(paciente);
                }
                foreach (Paciente pac in qryApellido)
                {
                    int flag = 0;
                    foreach (Paciente p in listadoPacientes)
                    {
                        if (p.Id_Paciente == pac.Id_Paciente)
                        {
                            flag = 1;
                        }
                    }
                    if (flag == 0)
                    {
                        listadoPacientes.Add(pac);
                    }
                }

                return View(listadoPacientes);
            } else return View(this.db.Paciente.OrderBy(p => p.Apellido).ToList());
        }
        // GET: Paciente/Details/5
        public ActionResult Details(int Id)
        {
            Paciente paciente = db.Paciente.Find(Id);
            Ficha ficha = db.Ficha.FirstOrDefault(f => f.Id_Paciente == Id);
            List<Observacion> observaciones = db.Observacion.Where(o => o.Id_Paciente == Id).OrderBy(o => o.Fecha).ToList();
            List<Tratamiento> tratamientos = db.Tratamiento.Where(t => t.Id_Paciente == Id).OrderBy(t => t.Fecha).ToList();
            List<Turno> turnos = db.Turno.Where(t => t.Id_Paciente == Id).OrderBy(t => t.Fecha).ToList();
            List<Medico> medicos = db.Medico.ToList();
            this.ViewBag.Ficha = ficha;
            this.ViewBag.Observaciones = observaciones;
            this.ViewBag.Tratamientos = tratamientos;
            this.ViewBag.Turnos = turnos;
            this.ViewBag.Medicos = medicos;
            return View(paciente);
        }
        
            // GET: Paciente/Create
            public ActionResult Create()
        {
            this.ViewBag.TituloPagina = "Agregar Paciente";
            Ficha ficha = new Ficha();
            ficha.Id_Paciente = 1;
            db.Ficha.Add(ficha);
            db.SaveChanges();
            ficha = db.Ficha.FirstOrDefault(f => f.Id_Ficha == ficha.Id_Ficha);
            this.ViewBag.Ficha = ficha;
            Paciente paciente = new Paciente();
            return View("Create", paciente);
        }

        // POST: Paciente/Create
        [HttpPost]
        public ActionResult Create(Paciente paciente,int idFicha)
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
                int id = db.Paciente.FirstOrDefault(p => p.Dni == paciente.Dni).Id_Paciente;
               // traigo la ficha creada anteriormente y le asigo el id del paciente
                Ficha nuevaFicha = db.Ficha.FirstOrDefault(f => f.Id_Ficha == idFicha);
                nuevaFicha.Id_Paciente = id;
                this.db.Ficha.Attach(nuevaFicha);
                this.db.Entry(nuevaFicha).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                this.ViewBag.Ficha = nuevaFicha;
                db.SaveChanges();
                return View("Details", db.Paciente.FirstOrDefault(p => p.Id_Paciente == id));
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
            Ficha ficha = db.Ficha.FirstOrDefault(f => f.Id_Paciente == Id);
            this.ViewBag.Ficha = ficha;
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
                return View("Details",paciente.Id_Paciente);
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
        /*
        private List<Paciente> normalizarLista(IQueryable<Paciente> lista)
        {
            List<Paciente> listaNormal = new List<Paciente>();
            foreach (Paciente p in lista)
            {
                string NombreOriginal = p.Nombre;
                string ApellidoOriginal = p.Apellido;
                string NombreNormalizado = NombreOriginal.Normalize(NormalizationForm.FormD);
                string ApellidoNormalizado = ApellidoOriginal.Normalize(NormalizationForm.FormD);

                Regex reg = new Regex("[^a-zA-Z0-9 ]");
                string nombreSinAcentos = reg.Replace(NombreNormalizado, "");
                string apellidoSinAcentos = reg.Replace(ApellidoNormalizado, "");
                p.Nombre = nombreSinAcentos;
                p.Apellido = apellidoSinAcentos;
                listaNormal.Add(p);
            }

            return listaNormal;
        }*/

    }
}
