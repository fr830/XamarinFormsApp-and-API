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

namespace Sagrada.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuarioEnEsperasController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: UsuarioEnEsperas
        public async Task<ActionResult> Index()
        {
            var usuarioEnEsperas = db.UsuarioEnEsperas.Include(u => u.Especialidad).Include(u => u.Medico).Include(u => u.ObraSocial).Include(u => u.Usuario);
            return View(await usuarioEnEsperas.ToListAsync());
        }

        // GET: UsuarioEnEsperas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioEnEspera usuarioEnEspera = await db.UsuarioEnEsperas.FindAsync(id);
            if (usuarioEnEspera == null)
            {
                return HttpNotFound();
            }
            return View(usuarioEnEspera);
        }

        // GET: UsuarioEnEsperas/Create
        public ActionResult Create()
        {
            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre");
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre");
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: UsuarioEnEsperas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdUsuarioEnEspera,IdMedico,IdEspecialidad,IdUsuario,Fecha,IdObraSocial")] UsuarioEnEspera usuarioEnEspera)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioEnEsperas.Add(usuarioEnEspera);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", usuarioEnEspera.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", usuarioEnEspera.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", usuarioEnEspera.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuarioEnEspera.IdUsuario);
            return View(usuarioEnEspera);
        }

        // GET: UsuarioEnEsperas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioEnEspera usuarioEnEspera = await db.UsuarioEnEsperas.FindAsync(id);
            if (usuarioEnEspera == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", usuarioEnEspera.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", usuarioEnEspera.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", usuarioEnEspera.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuarioEnEspera.IdUsuario);
            return View(usuarioEnEspera);
        }

        // POST: UsuarioEnEsperas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdUsuarioEnEspera,IdMedico,IdEspecialidad,IdUsuario,Fecha,IdObraSocial")] UsuarioEnEspera usuarioEnEspera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioEnEspera).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", usuarioEnEspera.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", usuarioEnEspera.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", usuarioEnEspera.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuarioEnEspera.IdUsuario);
            return View(usuarioEnEspera);
        }

        // GET: UsuarioEnEsperas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioEnEspera usuarioEnEspera = await db.UsuarioEnEsperas.FindAsync(id);
            if (usuarioEnEspera == null)
            {
                return HttpNotFound();
            }
            return View(usuarioEnEspera);
        }

        // POST: UsuarioEnEsperas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UsuarioEnEspera usuarioEnEspera = await db.UsuarioEnEsperas.FindAsync(id);
            db.UsuarioEnEsperas.Remove(usuarioEnEspera);
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
