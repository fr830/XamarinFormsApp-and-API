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
    public class CobroMedicoesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: CobroMedicoes
        public async Task<ActionResult> Index()
        {
            var cobroMedicos = db.CobroMedicos.Include(c => c.Especialidad).Include(c => c.Medico);
            return View(await cobroMedicos.ToListAsync());
        }

        // GET: CobroMedicoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CobroMedico cobroMedico = await db.CobroMedicos.FindAsync(id);
            if (cobroMedico == null)
            {
                return HttpNotFound();
            }
            return View(cobroMedico);
        }

        // GET: CobroMedicoes/Create
        public ActionResult Create()
        {
            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre");
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre");
            return View();
        }

        // POST: CobroMedicoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdCobroMedico,IdMedico,IdEspecialidad,Honorarios")] CobroMedico cobroMedico)
        {
            if (ModelState.IsValid)
            {
                db.CobroMedicos.Add(cobroMedico);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", cobroMedico.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", cobroMedico.IdMedico);
            return View(cobroMedico);
        }

        // GET: CobroMedicoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CobroMedico cobroMedico = await db.CobroMedicos.FindAsync(id);
            if (cobroMedico == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", cobroMedico.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", cobroMedico.IdMedico);
            return View(cobroMedico);
        }

        // POST: CobroMedicoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdCobroMedico,IdMedico,IdEspecialidad,Honorarios")] CobroMedico cobroMedico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cobroMedico).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "Nombre", cobroMedico.IdEspecialidad);
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", cobroMedico.IdMedico);
            return View(cobroMedico);
        }

        // GET: CobroMedicoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CobroMedico cobroMedico = await db.CobroMedicos.FindAsync(id);
            if (cobroMedico == null)
            {
                return HttpNotFound();
            }
            return View(cobroMedico);
        }

        // POST: CobroMedicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CobroMedico cobroMedico = await db.CobroMedicos.FindAsync(id);
            db.CobroMedicos.Remove(cobroMedico);
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
