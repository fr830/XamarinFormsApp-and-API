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
    public class EspecialidadsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Especialidads
        public async Task<IHttpActionResult> GetEspecialidads() {

            var usuarios = await db.Especialidades.ToListAsync();
            var responses = new List<EspecialidadResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new EspecialidadResponse {

                    CobroMedicos = usuario.CobroMedicos.ToList(),
                    IdEspecialidad = usuario.IdEspecialidad,
                    Nombre = usuario.Nombre,
                    

                });
            }

            return Ok(responses);
        }

        // GET: api/Especialidads/5
        [ResponseType(typeof(Especialidad))]
        public async Task<IHttpActionResult> GetEspecialidad(int id)
        {
            Especialidad especialidad = await db.Especialidades.FindAsync(id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return Ok(especialidad);
        }

        // PUT: api/Especialidads/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEspecialidad(int id, Especialidad especialidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != especialidad.IdEspecialidad)
            {
                return BadRequest();
            }

            db.Entry(especialidad).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecialidadExists(id))
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

        // POST: api/Especialidads
        [ResponseType(typeof(Especialidad))]
        public async Task<IHttpActionResult> PostEspecialidad(Especialidad especialidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Especialidades.Add(especialidad);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = especialidad.IdEspecialidad }, especialidad);
        }

        // DELETE: api/Especialidads/5
        [ResponseType(typeof(Especialidad))]
        public async Task<IHttpActionResult> DeleteEspecialidad(int id)
        {
            Especialidad especialidad = await db.Especialidades.FindAsync(id);
            if (especialidad == null)
            {
                return NotFound();
            }

            db.Especialidades.Remove(especialidad);
            await db.SaveChangesAsync();

            return Ok(especialidad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EspecialidadExists(int id)
        {
            return db.Especialidades.Count(e => e.IdEspecialidad == id) > 0;
        }
    }
}