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
    public class ObraSocialMedicoesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: ObraSocialMedicoes
        public async Task<ActionResult> Index()
        {
            var obraSocialMedicos = db.ObraSocialMedicos.Include(o => o.Medico).Include(o => o.ObraSocial);
            return View(await obraSocialMedicos.ToListAsync());
        }

        // GET: ObraSocialMedicoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialMedico obraSocialMedico = await db.ObraSocialMedicos.FindAsync(id);
            if (obraSocialMedico == null)
            {
                return HttpNotFound();
            }
            return View(obraSocialMedico);
        }

        // GET: ObraSocialMedicoes/Create
        public ActionResult Create()
        {
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre");
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre");
            return View();
        }

        // POST: ObraSocialMedicoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdObraSocial,IdMedico")] ObraSocialMedico obraSocialMedico)
        {
            if (ModelState.IsValid)
            {
                db.ObraSocialMedicos.Add(obraSocialMedico);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", obraSocialMedico.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", obraSocialMedico.IdObraSocial);
            return View(obraSocialMedico);
        }

        // GET: ObraSocialMedicoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialMedico obraSocialMedico = await db.ObraSocialMedicos.FindAsync(id);
            if (obraSocialMedico == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", obraSocialMedico.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", obraSocialMedico.IdObraSocial);
            return View(obraSocialMedico);
        }

        // POST: ObraSocialMedicoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdObraSocial,IdMedico")] ObraSocialMedico obraSocialMedico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraSocialMedico).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", obraSocialMedico.IdMedico);
            ViewBag.IdObraSocial = new SelectList(db.ObraSociales, "IdObraSocial", "Nombre", obraSocialMedico.IdObraSocial);
            return View(obraSocialMedico);
        }

        // GET: ObraSocialMedicoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialMedico obraSocialMedico = await db.ObraSocialMedicos.FindAsync(id);
            if (obraSocialMedico == null)
            {
                return HttpNotFound();
            }
            return View(obraSocialMedico);
        }

        // POST: ObraSocialMedicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ObraSocialMedico obraSocialMedico = await db.ObraSocialMedicos.FindAsync(id);
            db.ObraSocialMedicos.Remove(obraSocialMedico);
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
