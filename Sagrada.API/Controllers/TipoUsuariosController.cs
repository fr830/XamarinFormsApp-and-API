using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sagrada.API.Models;
using Sagrada.Dominio;

namespace Sagrada.API.Controllers
{
    [Authorize]
    public class TipoUsuariosController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/TipoUsuarios
        public async Task<IHttpActionResult> GetTipoUsuarios() {

            var usuarios = await db.TipoUsuarios.ToListAsync();
            var responses = new List<TipoUsuarioResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new TipoUsuarioResponse {

                    IdTipoUsuario = usuario.IdTipoUsuario,
                    Nombre = usuario.Nombre,
                    Usuarios = usuario.Usuarios.ToList(),
                    
                });
            }

            return Ok(responses);
        }

        // GET: api/TipoUsuarios/5
        [ResponseType(typeof(TipoUsuario))]
        public async Task<IHttpActionResult> GetTipoUsuario(int id)
        {
            TipoUsuario tipoUsuario = await db.TipoUsuarios.FindAsync(id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return Ok(tipoUsuario);
        }

        // PUT: api/TipoUsuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTipoUsuario(int id, TipoUsuario tipoUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoUsuario.IdTipoUsuario)
            {
                return BadRequest();
            }

            db.Entry(tipoUsuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoUsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TipoUsuarios
        [ResponseType(typeof(TipoUsuario))]
        public async Task<IHttpActionResult> PostTipoUsuario(TipoUsuario tipoUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoUsuarios.Add(tipoUsuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tipoUsuario.IdTipoUsuario }, tipoUsuario);
        }

        // DELETE: api/TipoUsuarios/5
        [ResponseType(typeof(TipoUsuario))]
        public async Task<IHttpActionResult> DeleteTipoUsuario(int id)
        {
            TipoUsuario tipoUsuario = await db.TipoUsuarios.FindAsync(id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            db.TipoUsuarios.Remove(tipoUsuario);
            await db.SaveChangesAsync();

            return Ok(tipoUsuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoUsuarioExists(int id)
        {
            return db.TipoUsuarios.Count(e => e.IdTipoUsuario == id) > 0;
        }
    }
}