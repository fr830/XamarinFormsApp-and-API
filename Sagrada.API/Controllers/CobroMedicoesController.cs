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
    [RoutePrefix("api/CobroMedicoes")]
    public class CobroMedicoesController : ApiController
    {
        private DataContext db = new DataContext();

        
        // GET: api/CobroMedicoes
        public async Task<IHttpActionResult> GetCobroMedicoes() {

            var usuarios = await db.CobroMedicos.ToListAsync();
            var responses = new List<CobroMedicoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new CobroMedicoResponse {
                    Especialidad = usuario.Especialidad,
                    Honorarios = usuario.Honorarios,
                    IdCobroMedico = usuario.IdCobroMedico,
                    IdEspecialidad = usuario.IdEspecialidad,
                    IdMedico = usuario.IdMedico,
                    Medico = usuario.Medico,
                    UsuarioAtendidos = usuario.UsuarioAtendidos.ToList(),
                    
                });
            }

            return Ok(responses);
        }

        // GET: api/CobroMedicoes/5  Obtiene CobroMedico por especialidad
        [ResponseType(typeof(CobroMedico))]
        public async Task<IHttpActionResult> GetCobroMedico(int id) {
            var cobroMedico = await db.CobroMedicos.Where(e => e.IdEspecialidad == id).ToListAsync();
            if (cobroMedico == null) {
                return NotFound();
            }

            return Ok(cobroMedico);
        }

        // GET: api/GetHonorario/5/2  Obtiene Honorario por especialidad y medico
        [ResponseType(typeof(CobroMedico))]
        [Route("GetHonorario/{medico}/{especialidad}")]
        public async Task<IHttpActionResult> GetHonorario(int medico, int especialidad) {

            var cobroMedico = await db.CobroMedicos.Where(e => e.IdMedico == medico && e.IdEspecialidad == especialidad).FirstOrDefaultAsync();
            if (cobroMedico == null) {
                return NotFound();
            }

            return Ok(cobroMedico);
        }

        // PUT: api/CobroMedicoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCobroMedico(int id, CobroMedico cobroMedico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cobroMedico.IdCobroMedico)
            {
                return BadRequest();
            }

            db.Entry(cobroMedico).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CobroMedicoExists(id))
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

        // POST: api/CobroMedicoes
        [ResponseType(typeof(CobroMedico))]
        public async Task<IHttpActionResult> PostCobroMedico(CobroMedico cobroMedico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CobroMedicos.Add(cobroMedico);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cobroMedico.IdCobroMedico }, cobroMedico);
        }

        // DELETE: api/CobroMedicoes/5
        [ResponseType(typeof(CobroMedico))]
        public async Task<IHttpActionResult> DeleteCobroMedico(int id)
        {
            CobroMedico cobroMedico = await db.CobroMedicos.FindAsync(id);
            if (cobroMedico == null)
            {
                return NotFound();
            }

            db.CobroMedicos.Remove(cobroMedico);
            await db.SaveChangesAsync();

            return Ok(cobroMedico);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CobroMedicoExists(int id)
        {
            return db.CobroMedicos.Count(e => e.IdCobroMedico == id) > 0;
        }
    }
}