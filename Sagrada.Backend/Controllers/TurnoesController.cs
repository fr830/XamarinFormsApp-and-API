using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sagrada.Backend.Models;
using Sagrada.Dominio;
using Microsoft.AspNet.Identity;

namespace Sagrada.Backend.Controllers
{
    [Authorize]
    public class TurnoesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Turnoes
        public async Task<ActionResult> Index()
        {
            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Include(t => t.Usuario);
            return View(await turnos.ToListAsync());
        }

        // GET: Turnoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // GET: Turnoes/Create
        public ActionResult Create()
        {
            List<Usuario> idUsuarioAux = new List<Usuario>();

            foreach (Usuario i in db.Usuarios) {
                if(User.Identity.GetUserName() == i.Email) {
                    idUsuarioAux.Add(i);
                }
            }

            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre");
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "FullName");
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre");
            ViewBag.IdUsuario = new SelectList(idUsuarioAux,"IdUsuario","Email");
            return View();
        }

        // POST: Turnoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdTurno,IdMedico,IdEspecialidad,IdUsuario,Fecha,IdObraSocial")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Turnos.Add(turno);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            List<int> idUsuarioAux = new List<int>();

            foreach (Usuario i in db.Usuarios) {
                if (User.Identity.GetUserName() == i.Email) {
                    idUsuarioAux.Add(i.IdUsuario);
                }
            }

            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", turno.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "FullName", turno.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", turno.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(idUsuarioAux);
            return View(turno);
        }

        // GET: Turnoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }

            List<int> idUsuarioAux = new List<int>();

            foreach (Usuario i in db.Usuarios) {
                if (User.Identity.GetUserName() == i.Email) {
                    idUsuarioAux.Add(i.IdUsuario);
                }
            }

            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", turno.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "FullName", turno.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", turno.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(idUsuarioAux);
            return View(turno);
        }

        // POST: Turnoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdTurno,IdMedico,IdEspecialidad,IdUsuario,Fecha,IdObraSocial")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turno).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            List<int> idUsuarioAux = new List<int>();

            foreach (Usuario i in db.Usuarios) {
                if (User.Identity.GetUserName() == i.Email) {
                    idUsuarioAux.Add(i.IdUsuario);
                }
            }

            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", turno.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "FullName", turno.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", turno.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(idUsuarioAux);
            return View(turno);
        }

        // GET: Turnoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // POST: Turnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Turno turno = await db.Turnos.FindAsync(id);
            db.Turnos.Remove(turno);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
