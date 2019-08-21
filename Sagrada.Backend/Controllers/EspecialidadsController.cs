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
    public class EspecialidadsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Especialidads
        public async Task<ActionResult> Index()
        {
            return View(await db.Especialidades.ToListAsync());
        }

        // GET: Especialidads/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = await db.Especialidades.FindAsync(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
        }

        // GET: Especialidads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Especialidads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdEspecialidad,Nombre")] Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                db.Especialidades.Add(especialidad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(especialidad);
        }

        // GET: Especialidads/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = await db.Especialidades.FindAsync(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
        }

        // POST: Especialidads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdEspecialidad,Nombre")] Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(especialidad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(especialidad);
        }

        // GET: Especialidads/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = await db.Especialidades.FindAsync(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
        }

        // POST: Especialidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Especialidad especialidad = await db.Especialidades.FindAsync(id);
            db.Especialidades.Remove(especialidad);
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
