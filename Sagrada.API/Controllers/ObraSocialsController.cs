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
    [RoutePrefix("api/ObraSocials")]
    public class ObraSocialsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/ObraSocials
        public async Task<IHttpActionResult> GetObraSocials() {

            var usuarios = await db.ObraSociales.ToListAsync();
            var responses = new List<ObraSocialResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new ObraSocialResponse {

                    IdObraSocial = usuario.IdObraSocial,
                    Nombre = usuario.Nombre,
                    ObraSocialUsuarios = usuario.ObraSocialUsuarios.ToList(),
                    ObraSocialMedicos = usuario.ObraSocialMedicos.ToList(),
                    Turnos = usuario.Turnos.ToList(),
                    UsuarioAtendidos = usuario.UsuarioAtendidos.ToList(),
                    UsuarioEnEsperas = usuario.UsuarioEnEsperas.ToList(),
                });
            }

            return Ok(responses);
        }

        // GET: api/ObraSocials/5
        [ResponseType(typeof(ObraSocial))]
        public async Task<IHttpActionResult> GetObraSocial(int id)
        {
            ObraSocial obraSocial = await db.ObraSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return NotFound();
            }

            return Ok(obraSocial);
        }

        // GET: api/ObraSocials/nombre
        [ResponseType(typeof(ObraSocial))]
        [Route("GetObraSocialByNombre/{nombre}")]
        public async Task<IHttpActionResult> GetObraSocialByNombre(string nombre) {

            ObraSocial obraSocial = await db.ObraSociales.Where(o => o.Nombre.ToLower() == nombre.ToLower()).FirstOrDefaultAsync();
            if (obraSocial == null) {
                return NotFound();
            }

            return Ok(obraSocial);
        }

        // PUT: api/ObraSocials/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutObraSocial(int id, ObraSocial obraSocial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obraSocial.IdObraSocial)
            {
                return BadRequest();
            }

            db.Entry(obraSocial).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObraSocialExists(id))
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

        // POST: api/ObraSocials
        [ResponseType(typeof(ObraSocial))]
        public async Task<IHttpActionResult> PostObraSocial(ObraSocial obraSocial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ObraSociales.Add(obraSocial);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = obraSocial.IdObraSocial }, obraSocial);
        }

        // DELETE: api/ObraSocials/5
        [ResponseType(typeof(ObraSocial))]
        public async Task<IHttpActionResult> DeleteObraSocial(int id)
        {
            ObraSocial obraSocial = await db.ObraSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return NotFound();
            }

            db.ObraSociales.Remove(obraSocial);
            await db.SaveChangesAsync();

            return Ok(obraSocial);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ObraSocialExists(int id)
        {
            return db.ObraSociales.Count(e => e.IdObraSocial == id) > 0;
        }
    }
}