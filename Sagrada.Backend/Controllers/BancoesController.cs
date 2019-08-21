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
    public class BancoesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Bancoes
        public async Task<ActionResult> Index()
        {
            return View(await db.Bancos.ToListAsync());
        }

        // GET: Bancoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco = await db.Bancos.FindAsync(id);
            if (banco == null)
            {
                return HttpNotFound();
            }
            return View(banco);
        }

        // GET: Bancoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bancoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdBanco,Nombre")] Banco banco)
        {
            if (ModelState.IsValid)
            {
                db.Bancos.Add(banco);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(banco);
        }

        // GET: Bancoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco = await db.Bancos.FindAsync(id);
            if (banco == null)
            {
                return HttpNotFound();
            }
            return View(banco);
        }

        // POST: Bancoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdBanco,Nombre")] Banco banco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(banco).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(banco);
        }

        // GET: Bancoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco = await db.Bancos.FindAsync(id);
            if (banco == null)
            {
                return HttpNotFound();
            }
            return View(banco);
        }

        // POST: Bancoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Banco banco = await db.Bancos.FindAsync(id);
            db.Bancos.Remove(banco);
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
