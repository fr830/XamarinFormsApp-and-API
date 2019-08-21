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
    public class HorariosController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Horarios
        public async Task<IHttpActionResult> GetHorarios() {

            var usuarios = await db.Horarios.ToListAsync();
            var responses = new List<HorarioResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new HorarioResponse {

                    Dia = usuario.Dia,
                    HoraFin = usuario.HoraFin,
                    HoraInicio = usuario.HoraInicio,
                    IdHorario = usuario.IdHorario,
                    IdMedico = usuario.IdMedico,
                    Medico = usuario.Medico,
                    
                });
            }

            return Ok(responses);
        }

        // GET: api/Horarios/5  Obtiene horarios por medico
        [ResponseType(typeof(Horario))]
        public async Task<IHttpActionResult> GetHorario(int id) {
            var horario = await db.Horarios.Where(h => h.IdMedico == id).ToListAsync();
            if (horario == null) {
                return NotFound();
            }

            return Ok(horario);
        }

        // PUT: api/Horarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHorario(int id, Horario horario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != horario.IdHorario)
            {
                return BadRequest();
            }

            db.Entry(horario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(id))
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

        // POST: api/Horarios
        [ResponseType(typeof(Horario))]
        public async Task<IHttpActionResult> PostHorario(Horario horario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Horarios.Add(horario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = horario.IdHorario }, horario);
        }

        // DELETE: api/Horarios/5
        [ResponseType(typeof(Horario))]
        public async Task<IHttpActionResult> DeleteHorario(int id)
        {
            Horario horario = await db.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }

            db.Horarios.Remove(horario);
            await db.SaveChangesAsync();

            return Ok(horario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HorarioExists(int id)
        {
            return db.Horarios.Count(e => e.IdHorario == id) > 0;
        }
    }
}