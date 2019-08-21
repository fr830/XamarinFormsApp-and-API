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
    public class ObraSocialUsuariosController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/ObraSocialUsuarios
        public async Task<IHttpActionResult> GetObraSocialUsuarios() {

            var usuarios = await db.ObraSocialUsuarios.OrderBy(o => o.IdObraSocial).ToListAsync();
            var responses = new List<ObraSocialUsuarioResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new ObraSocialUsuarioResponse {

                    IdObraSocial = usuario.IdObraSocial,
                    IdUsuario = usuario.IdUsuario,
                    NroAfiliado = usuario.NroAfiliado,
                    ObraSocial = usuario.ObraSocial,
                    Usuario = usuario.Usuario,
                    
                });
            }

            return Ok(responses);
        }

        // GET: api/ObraSocialUsuarios/5   Busca Obra social por usuario
        [ResponseType(typeof(ObraSocialUsuario))]
        public async Task<IHttpActionResult> GetObraSocialUsuario(int id)
        {
            var obraSocialUsuario = await db.ObraSocialUsuarios.Where(o => o.IdUsuario == id).ToListAsync();
            if (obraSocialUsuario == null)
            {
                return NotFound();
            }

            return Ok(obraSocialUsuario);
        }

        // PUT: api/ObraSocialUsuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutObraSocialUsuario(int id, ObraSocialUsuario obraSocialUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obraSocialUsuario.IdObraSocial)
            {
                return BadRequest();
            }

            db.Entry(obraSocialUsuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObraSocialUsuarioExists(id))
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

        // POST: api/ObraSocialUsuarios
        [ResponseType(typeof(ObraSocialUsuario))]
        public async Task<IHttpActionResult> PostObraSocialUsuario(ObraSocialUsuario obraSocialUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ObraSocialUsuarios.Add(obraSocialUsuario);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ObraSocialUsuarioExists(obraSocialUsuario.IdObraSocial))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = obraSocialUsuario.IdObraSocial }, obraSocialUsuario);
        }

        // DELETE: api/ObraSocialUsuarios/5
        [ResponseType(typeof(ObraSocialUsuario))]
        public async Task<IHttpActionResult> DeleteObraSocialUsuario(int id)
        {
            ObraSocialUsuario obraSocialUsuario = await db.ObraSocialUsuarios.FindAsync(id);
            if (obraSocialUsuario == null)
            {
                return NotFound();
            }

            db.ObraSocialUsuarios.Remove(obraSocialUsuario);
            await db.SaveChangesAsync();

            return Ok(obraSocialUsuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ObraSocialUsuarioExists(int id)
        {
            return db.ObraSocialUsuarios.Count(e => e.IdObraSocial == id) > 0;
        }
    }
}