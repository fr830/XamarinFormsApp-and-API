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
    public class HorariosController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Horarios
        public async Task<ActionResult> Index()
        {
            var horarios = db.Horarios.Include(h => h.Medico);
            return View(await horarios.ToListAsync());
        }

        // GET: Horarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = await db.Horarios.FindAsync(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }

        // GET: Horarios/Create
        public ActionResult Create()
        {
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre");
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdHorario,Dia,HoraInicio,HoraFin,IdMedico")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                db.Horarios.Add(horario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", horario.IdMedico);
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = await db.Horarios.FindAsync(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", horario.IdMedico);
            return View(horario);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdHorario,Dia,HoraInicio,HoraFin,IdMedico")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", horario.IdMedico);
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = await db.Horarios.FindAsync(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Horario horario = await db.Horarios.FindAsync(id);
            db.Horarios.Remove(horario);
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
