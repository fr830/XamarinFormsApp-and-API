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
    public class ObraSocialUsuariosController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: ObraSocialUsuarios
        public async Task<ActionResult> Index()
        {
            var obraSocialUsuarios = db.ObraSocialUsuarios.Include(o => o.ObraSocial).Include(o => o.Usuario);
            return View(await obraSocialUsuarios.ToListAsync());
        }

        // GET: ObraSocialUsuarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialUsuario obraSocialUsuario = await db.ObraSocialUsuarios.FindAsync(id);
            if (obraSocialUsuario == null)
            {
                return HttpNotFound();
            }
            return View(obraSocialUsuario);
        }

        // GET: ObraSocialUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: ObraSocialUsuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdObraSocial,IdUsuario,NroAfiliado")] ObraSocialUsuario obraSocialUsuario)
        {
            if (ModelState.IsValid)
            {
                db.ObraSocialUsuarios.Add(obraSocialUsuario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", obraSocialUsuario.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", obraSocialUsuario.IdUsuario);
            return View(obraSocialUsuario);
        }

        // GET: ObraSocialUsuarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialUsuario obraSocialUsuario = await db.ObraSocialUsuarios.FindAsync(id);
            if (obraSocialUsuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", obraSocialUsuario.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", obraSocialUsuario.IdUsuario);
            return View(obraSocialUsuario);
        }

        // POST: ObraSocialUsuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdObraSocial,IdUsuario,NroAfiliado")] ObraSocialUsuario obraSocialUsuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraSocialUsuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", obraSocialUsuario.IdObraSocial);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", obraSocialUsuario.IdUsuario);
            return View(obraSocialUsuario);
        }

        // GET: ObraSocialUsuarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialUsuario obraSocialUsuario = await db.ObraSocialUsuarios.FindAsync(id);
            if (obraSocialUsuario == null)
            {
                return HttpNotFound();
            }
            return View(obraSocialUsuario);
        }

        // POST: ObraSocialUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ObraSocialUsuario obraSocialUsuario = await db.ObraSocialUsuarios.FindAsync(id);
            db.ObraSocialUsuarios.Remove(obraSocialUsuario);
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
