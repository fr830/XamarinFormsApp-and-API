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
    public class ObraSocialsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: ObraSocials
        public async Task<ActionResult> Index()
        {
            return View(await db.ObraSociales.ToListAsync());
        }

        // GET: ObraSocials/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocial obraSocial = await db.ObraSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return HttpNotFound();
            }
            return View(obraSocial);
        }

        // GET: ObraSocials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ObraSocials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdObraSocial,Nombre")] ObraSocial obraSocial)
        {
            if (ModelState.IsValid)
            {
                db.ObraSociales.Add(obraSocial);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(obraSocial);
        }

        // GET: ObraSocials/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocial obraSocial = await db.ObraSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return HttpNotFound();
            }
            return View(obraSocial);
        }

        // POST: ObraSocials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdObraSocial,Nombre")] ObraSocial obraSocial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraSocial).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(obraSocial);
        }

        // GET: ObraSocials/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocial obraSocial = await db.ObraSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return HttpNotFound();
            }
            return View(obraSocial);
        }

        // POST: ObraSocials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ObraSocial obraSocial = await db.ObraSociales.FindAsync(id);
            db.ObraSociales.Remove(obraSocial);
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
