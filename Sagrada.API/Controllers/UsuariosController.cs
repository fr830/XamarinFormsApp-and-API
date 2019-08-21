using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using Sagrada.API.Helpers;
using Sagrada.API.Models;
using Sagrada.Dominio;

namespace Sagrada.API.Controllers
{
    
    [RoutePrefix("api/Usuarios")]
    public class UsuariosController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Usuarios
        [Authorize]
        public async Task<IHttpActionResult> GetUsuarios() {

            var usuarios = await db.Usuarios.ToListAsync();
            var responses = new List<UsuarioResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new UsuarioResponse {
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    DNI = usuario.DNI,
                    Email = usuario.Email,
                    IdTipoUsuario = usuario.IdTipoUsuario,
                    IdUsuario = usuario.IdUsuario,
                    ImageArray = usuario.ImageArray,
                    ObraSocialUsuarios = usuario.ObraSocialUsuarios.ToList(),
                    Pagos = usuario.Pagos.ToList(),
                    Password = usuario.Password,
                    PathImagen = usuario.PathImagen,
                    Telefono = usuario.Telefono,
                    TipoUsuario = usuario.TipoUsuario,
                    Turnos = usuario.Turnos.ToList(),
                    UsuarioAtendidos = usuario.UsuarioAtendidos.ToList(),
                    UsuarioEnEsperas = usuario.UsuarioEnEsperas.ToList(),
                    FechaDeNacimiento = usuario.FechaDeNacimiento,
                    
                });
            }

            return Ok(responses);
        }

        // GET: api/Usuarios/5 Busca usuario por dnia
        [Authorize]
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> GetUsuarios(int id) {

            Usuario usuario = await db.Usuarios.Where(u => u.DNI == id).FirstOrDefaultAsync();
            if (usuario == null) {
                return NotFound();
            }

            return Ok(usuario);
        }

        [Authorize]
        [ResponseType(typeof(Usuario))]
        [Route("GetUsuariosById")]
        public async Task<IHttpActionResult> GetUsuariosById(int id) {

            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null) {
                return NotFound();
            }

            return Ok(usuario);
        }

        [Authorize]
        [ResponseType(typeof(Usuario))]
        [Route("GetUsuariosByDni")]
        public async Task<IHttpActionResult> GetUsuariosByDni(int dni) {

            Usuario usuario = await db.Usuarios.Where(u => u.DNI == dni).FirstOrDefaultAsync();
            if (usuario == null) {
                return NotFound();
            }

            return Ok(usuario);
        }


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(JObject form) {
            var email = string.Empty;
            var currentPassword = string.Empty;
            var newPassword = string.Empty;
            dynamic jsonObject = form;

            try {
                email = jsonObject.Email.Value;
                currentPassword = jsonObject.CurrentPassword.Value;
                newPassword = jsonObject.NewPassword.Value;
            } catch {
                return BadRequest("Incorrect call");
            }

            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            if (userASP == null) {
                return BadRequest("Incorrect call");
            }

            var response = await userManager.ChangePasswordAsync(userASP.Id, currentPassword, newPassword);
            if (!response.Succeeded) {
                return BadRequest(response.Errors.FirstOrDefault());
            }

            return Ok("ok");
        }

        [HttpPost]
        [Authorize]
        [Route("GetUserByEmail")]
        public async Task<IHttpActionResult> GetUserByEmail(JObject form) {
            var email = string.Empty;
            dynamic jsonObject = form;
            try {
                email = jsonObject.Email.Value;
            } catch {
                return BadRequest("Missing parameter.");
            }

            var user = await db.Usuarios.
                Where(u => u.Email.ToLower() == email.ToLower()).
                FirstOrDefaultAsync();
            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuario(int id, Usuario usuario) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != usuario.IdUsuario) {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!UsuarioExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Usuarios
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> PostUsuario(Usuario usuario) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (usuario.ImageArray != null && usuario.ImageArray.Length > 0) {
                var stream = new MemoryStream(usuario.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = string.Format("{0}.jpg", guid);
                var folder = "~/Content/Images";
                var fullPath = string.Format("{0}/{1}", folder, file);
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response) {
                    usuario.PathImagen = fullPath;
                }
            }

            db.Usuarios.Add(usuario);
            UsersHelper.CreateUserASP(usuario.Email, "User", usuario.Password);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuario.IdUsuario }, usuario);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> DeleteUsuario(int id)
        {
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Count(e => e.IdUsuario == id) > 0;
        }
    }
}