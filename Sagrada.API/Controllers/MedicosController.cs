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
    [RoutePrefix("api/Medicos")]
    public class MedicosController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Medicos
        public async Task<IHttpActionResult> GetMedicos() {

            var usuarios = await db.Medicos.ToListAsync();
            var responses = new List<MedicoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new MedicoResponse {

                    Apellido = usuario.Apellido,
                    CobroMedicos = usuario.CobroMedicos.ToList(),
                    DNI = usuario.DNI,
                    Email = usuario.Email,
                    Horarios = usuario.Horarios.ToList(),
                    IdMedico = usuario.IdMedico,
                    Nombre = usuario.Nombre,
                    ObraSocialMedicos = usuario.ObraSocialMedicos.ToList(),
                    PathImagen = usuario.PathImagen,
                    Telefono = usuario.Telefono,
                    Turnos = usuario.Turnos.ToList(),
                    UsuarioEnEsperas = usuario.UsuarioEnEsperas.ToList(),
                    Pagos = usuario.Pagos.ToList(),
                    FechaDeNacimiento = usuario.FechaDeNacimiento,
                });
            }

            return Ok(responses);
        }

        // GET: api/Medicos/5
        [ResponseType(typeof(Medico))]
        public async Task<IHttpActionResult> GetMedico(int id)
        {
            Medico medico = await db.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }

            return Ok(medico);
        }

        // GET: api/Medicos/5 obtiene medico por dni  
        [ResponseType(typeof(Medico))]
        [Route("GetMedicoByDni/{dni}")]
        public async Task<IHttpActionResult> GetMedicoByDni(int dni) {

            Medico medico = await db.Medicos.Where(m => m.DNI == dni).FirstOrDefaultAsync();
            if (medico == null) {
                return NotFound();
            }

            return Ok(medico);
        }

        // PUT: api/Medicos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMedico(int id, Medico medico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medico.IdMedico)
            {
                return BadRequest();
            }

            db.Entry(medico).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicoExists(id))
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

        // POST: api/Medicos
        [ResponseType(typeof(Medico))]
        public async Task<IHttpActionResult> PostMedico(Medico medico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Medicos.Add(medico);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = medico.IdMedico }, medico);
        }

        // DELETE: api/Medicos/5
        [ResponseType(typeof(Medico))]
        public async Task<IHttpActionResult> DeleteMedico(int id)
        {
            Medico medico = await db.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }

            db.Medicos.Remove(medico);
            await db.SaveChangesAsync();

            return Ok(medico);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MedicoExists(int id)
        {
            return db.Medicos.Count(e => e.IdMedico == id) > 0;
        }
    }
}