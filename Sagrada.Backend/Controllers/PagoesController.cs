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
    public class PagoesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Pagoes
        public async Task<ActionResult> Index()
        {
            var pagos = db.Pagos.Include(p => p.Medico).Include(p => p.Tarjeta).Include(p => p.Usuario);
            return View(await pagos.ToListAsync());
        }

        // GET: Pagoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pagos.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // GET: Pagoes/Create
        public ActionResult Create()
        {
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre");
            ViewBag.IdTarjeta = new SelectList(db.Tarjetas, "IdTarjeta", "IdTarjeta");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: Pagoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdPago,Monto,Fecha,IdUsuario,IdTarjeta,IdMedico")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Pagos.Add(pago);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", pago.IdMedico);
            ViewBag.IdTarjeta = new SelectList(db.Tarjetas, "IdTarjeta", "IdTarjeta", pago.IdTarjeta);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", pago.IdUsuario);
            return View(pago);
        }

        // GET: Pagoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pagos.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", pago.IdMedico);
            ViewBag.IdTarjeta = new SelectList(db.Tarjetas, "IdTarjeta", "IdTarjeta", pago.IdTarjeta);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", pago.IdUsuario);
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdPago,Monto,Fecha,IdUsuario,IdTarjeta,IdMedico")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdMedico = new SelectList(db.Medicos, "IdMedico", "Nombre", pago.IdMedico);
            ViewBag.IdTarjeta = new SelectList(db.Tarjetas, "IdTarjeta", "IdTarjeta", pago.IdTarjeta);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", pago.IdUsuario);
            return View(pago);
        }

        // GET: Pagoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pagos.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // POST: Pagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pago pago = await db.Pagos.FindAsync(id);
            db.Pagos.Remove(pago);
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
