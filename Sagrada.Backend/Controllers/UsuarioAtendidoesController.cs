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
    public class UsuarioAtendidoesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: UsuarioAtendidoes
        public async Task<ActionResult> Index()
        {
            var usuarioAtendidos = db.UsuarioAtendidos.Include(u => u.CobroMedico).Include(u => u.ObraSocial).Include(u => u.Usuario);
            return View(await usuarioAtendidos.ToListAsync());
        }

        // GET: UsuarioAtendidoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioAtendido usuarioAtendido = await db.UsuarioAtendidos.FindAsync(id);
            if (usuarioAtendido == null)
            {
                return HttpNotFound();
            }
            return View(usuarioAtendido);
        }

        // GET: UsuarioAtendidoes/Create
        public ActionResult Create()
        {
            ViewBag.IdCobroMedico = new SelectList(db.CobroMedicos, "IdCobroMedico", "IdCobroMedico");
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: UsuarioAtendidoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdUsuarioAtendido,IdUsuario,IdCobroMedico,HistoriaClinica,Fecha,IdObraSocial")] UsuarioAtendido usuarioAtendido)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioAtendidos.Add(usuarioAtendido);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdCobroMedico = new SelectList(db.CobroMedicos, "IdCobroMedico", "IdCobroMedico", usuarioAtendido.IdCobroMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", usuarioAtendido.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuarioAtendido.IdUsuario);
            return View(usuarioAtendido);
        }

        // GET: UsuarioAtendidoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioAtendido usuarioAtendido = await db.UsuarioAtendidos.FindAsync(id);
            if (usuarioAtendido == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCobroMedico = new SelectList(db.CobroMedicos, "IdCobroMedico", "IdCobroMedico", usuarioAtendido.IdCobroMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", usuarioAtendido.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuarioAtendido.IdUsuario);
            return View(usuarioAtendido);
        }

        // POST: UsuarioAtendidoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdUsuarioAtendido,IdUsuario,IdCobroMedico,HistoriaClinica,Fecha,IdObraSocial")] UsuarioAtendido usuarioAtendido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioAtendido).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdCobroMedico = new SelectList(db.CobroMedicos, "IdCobroMedico", "IdCobroMedico", usuarioAtendido.IdCobroMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", usuarioAtendido.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", usuarioAtendido.IdUsuario);
            return View(usuarioAtendido);
        }

        // GET: UsuarioAtendidoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioAtendido usuarioAtendido = await db.UsuarioAtendidos.FindAsync(id);
            if (usuarioAtendido == null)
            {
                return HttpNotFound();
            }
            return View(usuarioAtendido);
        }

        // POST: UsuarioAtendidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UsuarioAtendido usuarioAtendido = await db.UsuarioAtendidos.FindAsync(id);
            db.UsuarioAtendidos.Remove(usuarioAtendido);
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
