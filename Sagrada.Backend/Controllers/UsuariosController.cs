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
using Sagrada.Backend.Helpers;

namespace Sagrada.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            var usuarios = db.Usuarios.Include(u => u.TipoUsuario);
            return View(await usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.IdTipoUsuario = new SelectList(db.TipoUsuarios, "IdTipoUsuario", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view) {

            if (ModelState.IsValid) {

                var pic = string.Empty;
                var folder = "~/Content/Usuarios";

                if (view.ImageFile != null) {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var user = this.ToUser(view);
                user.PathImagen = pic;
                db.Usuarios.Add(user);
                await db.SaveChangesAsync();
                UsersHelper.CreateUserASP(view.Email, "User", view.Password);
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private Usuario ToUser(UserView view) {
            return new Usuario {
                Email = view.Email,
                Nombre = view.Nombre,
                PathImagen = view.PathImagen,
                Apellido = view.Apellido,
                Telefono = view.Telefono,
                IdTipoUsuario = view.IdTipoUsuario,
                DNI = view.DNI,
                FechaDeNacimiento = view.FechaDeNacimiento,
                IdUsuario = view.IdUsuario,
                ImageArray = view.ImageArray,
                
                

            };
        }

        // GET: Usuarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTipoUsuario = new SelectList(db.TipoUsuarios, "IdTipoUsuario", "Nombre", usuario.IdTipoUsuario);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdUsuario,Nombre,Apellido,DNI,FechaDeNacimiento,Email,Telefono,PathImagen,IdTipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdTipoUsuario = new SelectList(db.TipoUsuarios, "IdTipoUsuario", "Nombre", usuario.IdTipoUsuario);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Usuario usuario = await db.Usuarios.FindAsync(id);
            db.Usuarios.Remove(usuario);
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
