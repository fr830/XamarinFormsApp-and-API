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
    public class TarjetasController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Tarjetas
        public async Task<ActionResult> Index()
        {
            var tarjetas = db.Tarjetas.Include(t => t.Banco);
            return View(await tarjetas.ToListAsync());
        }

        // GET: Tarjetas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarjeta tarjeta = await db.Tarjetas.FindAsync(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }
            return View(tarjeta);
        }

        // GET: Tarjetas/Create
        public ActionResult Create()
        {
            ViewBag.IdBanco = new SelectList(db.Bancos, "IdBanco", "Nombre");
            return View();
        }

        // POST: Tarjetas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdTarjeta,CodOperacion,IdBanco")] Tarjeta tarjeta)
        {
            if (ModelState.IsValid)
            {
                db.Tarjetas.Add(tarjeta);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdBanco = new SelectList(db.Bancos, "IdBanco", "Nombre", tarjeta.IdBanco);
            return View(tarjeta);
        }

        // GET: Tarjetas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarjeta tarjeta = await db.Tarjetas.FindAsync(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdBanco = new SelectList(db.Bancos, "IdBanco", "Nombre", tarjeta.IdBanco);
            return View(tarjeta);
        }

        // POST: Tarjetas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdTarjeta,CodOperacion,IdBanco")] Tarjeta tarjeta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarjeta).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdBanco = new SelectList(db.Bancos, "IdBanco", "Nombre", tarjeta.IdBanco);
            return View(tarjeta);
        }

        // GET: Tarjetas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarjeta tarjeta = await db.Tarjetas.FindAsync(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }
            return View(tarjeta);
        }

        // POST: Tarjetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tarjeta tarjeta = await db.Tarjetas.FindAsync(id);
            db.Tarjetas.Remove(tarjeta);
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
