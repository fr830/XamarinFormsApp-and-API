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
    [RoutePrefix("api/ObraSocialMedicoes")]
    public class ObraSocialMedicoesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/ObraSocialMedicoes
        public async Task<IHttpActionResult> GetObraSocialMedicoes() {

            var usuarios = await db.ObraSocialMedicos.ToListAsync();
            var responses = new List<ObraSocialMedicoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new ObraSocialMedicoResponse {

                    IdMedico = usuario.IdMedico,
                    IdObraSocial = usuario.IdObraSocial,
                    Medico = usuario.Medico,
                    ObraSocial = usuario.ObraSocial,
                    

                });
            }

            return Ok(responses);
        }

        // GET: api/ObraSocialMedicoes/5
        [ResponseType(typeof(ObraSocialMedico))]
        public async Task<IHttpActionResult> GetObraSocialMedico(int id)
        {
            ObraSocialMedico obraSocialMedico = await db.ObraSocialMedicos.FindAsync(id);
            if (obraSocialMedico == null)
            {
                return NotFound();
            }

            return Ok(obraSocialMedico);
        }

        // GET: api/GetObraSocialByMedicoAndObraSocial/5/2  devuelve un registro si hay coincidencia
        [ResponseType(typeof(ObraSocialMedico))]
        [Route("GetObraSocialByMedicoAndObraSocial/{medico}/{obraSocial}")]
        public async Task<IHttpActionResult> GetObraSocialByMedicoAndObraSocial(int medico, int obraSocial) {

            ObraSocialMedico obraSocialMedico = await db.ObraSocialMedicos.Where(o => o.IdMedico == medico && o.IdObraSocial == obraSocial).FirstOrDefaultAsync();
            if (obraSocialMedico == null) {
                return NotFound();
            }

            return Ok(obraSocialMedico);
        }

        // GET: api/GetObraSocialByMedico/2  devuelve un registro si hay coincidencia
        
        [Route("GetObraSocialByMedico/{medico}")]
        public async Task<IHttpActionResult> GetObraSocialByMedico(int medico) {

            var usuarios = await db.ObraSocialMedicos.Where(o => o.IdMedico == medico).ToListAsync();

            if (usuarios == null) {
                return NotFound();
            }

            var responses = new List<ObraSocialMedicoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new ObraSocialMedicoResponse {

                    IdMedico = usuario.IdMedico,
                    IdObraSocial = usuario.IdObraSocial,
                    Medico = usuario.Medico,
                    ObraSocial = usuario.ObraSocial,


                });
            }

            return Ok(responses);
        }

        // PUT: api/ObraSocialMedicoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutObraSocialMedico(int id, ObraSocialMedico obraSocialMedico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obraSocialMedico.IdObraSocial)
            {
                return BadRequest();
            }

            db.Entry(obraSocialMedico).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObraSocialMedicoExists(id))
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

        // POST: api/ObraSocialMedicoes
        [ResponseType(typeof(ObraSocialMedico))]
        public async Task<IHttpActionResult> PostObraSocialMedico(ObraSocialMedico obraSocialMedico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ObraSocialMedicos.Add(obraSocialMedico);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ObraSocialMedicoExists(obraSocialMedico.IdObraSocial))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = obraSocialMedico.IdObraSocial }, obraSocialMedico);
        }

        // DELETE: api/ObraSocialMedicoes/5
        [ResponseType(typeof(ObraSocialMedico))]
        public async Task<IHttpActionResult> DeleteObraSocialMedico(int id)
        {
            ObraSocialMedico obraSocialMedico = await db.ObraSocialMedicos.FindAsync(id);
            if (obraSocialMedico == null)
            {
                return NotFound();
            }

            db.ObraSocialMedicos.Remove(obraSocialMedico);
            await db.SaveChangesAsync();

            return Ok(obraSocialMedico);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ObraSocialMedicoExists(int id)
        {
            return db.ObraSocialMedicos.Count(e => e.IdObraSocial == id) > 0;
        }
    }
}