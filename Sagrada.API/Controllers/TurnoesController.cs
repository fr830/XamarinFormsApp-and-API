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
using Newtonsoft.Json.Linq;
using Sagrada.API.Models;
using Sagrada.Dominio;

namespace Sagrada.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Turnoes")]
    public class TurnoesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Turnoes
        public async Task<IHttpActionResult> GetTurnoes() {

            var usuarios = await db.Turnos.OrderBy(t => t.Fecha).ToListAsync();
            var responses = new List<TurnoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new TurnoResponse {

                    IdTurno = usuario.IdTurno,
                    Fecha = usuario.Fecha,
                    IdMedico = usuario.IdMedico,
                    Medico = usuario.Medico,
                    IdUsuario = usuario.IdUsuario,
                    Usuario = usuario.Usuario,
                    IdObraSocial = usuario.IdObraSocial,
                    ObraSocial = usuario.ObraSocial,
                    Especialidad = usuario.Especialidad,
                    IdEspecialidad = usuario.IdEspecialidad,
                });
            }

            return Ok(responses);
        }

        // GET: api/GetTurnoByUsuario/5
        [Route("GetTurnoByUsuario/{idUsuario}")]
        public async Task<IHttpActionResult> GetTurnoByUsuario(int idUsuario) {

            var usuarios = await db.Turnos.Where(t => t.IdUsuario == idUsuario).OrderBy(t => t.Fecha).ToListAsync(); 

            if (usuarios == null) {
                return NotFound();
            }

            var responses = new List<TurnoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new TurnoResponse {

                    IdTurno = usuario.IdTurno,
                    Fecha = usuario.Fecha,
                    IdMedico = usuario.IdMedico,
                    Medico = usuario.Medico,
                    IdUsuario = usuario.IdUsuario,
                    Usuario = usuario.Usuario,
                    IdObraSocial = usuario.IdObraSocial,
                    ObraSocial = usuario.ObraSocial,
                    Especialidad = usuario.Especialidad,
                    IdEspecialidad = usuario.IdEspecialidad,
                });
            }

            return Ok(responses);
        }

        // GET: api/GetTurnoByFecha/5 busca turno por fecha y medico
        [HttpPost]
        [Route("GetTurnoByFecha")]
        public async Task<IHttpActionResult> GetTurnoByFecha(JObject form) {

            DateTime fecha = DateTime.Now;
            int idMedico;
            dynamic jsonObject = form;
            try {
                fecha = jsonObject.Fecha;
                idMedico = jsonObject.IdMedico;
            } catch {
                return BadRequest("Missing parameter.");
            }

            var turno = await db.Turnos.Where(t => t.Fecha == fecha && t.IdMedico == idMedico).FirstOrDefaultAsync();

            if (turno == null) {
                return NotFound();
            }

            return Ok(turno);
        }

        // GET: api/GetTurnoByFecha/5 busca turnos nuevos
        [HttpPost]
        [Route("GetTurnosNuevos")]
        public async Task<IHttpActionResult> GetTurnosNuevos(JObject form) {

            DateTime fecha = DateTime.Now;
            int idMedico;
            dynamic jsonObject = form;
            try {
                fecha = jsonObject.Fecha;
                idMedico = jsonObject.IdMedico;
            } catch {
                return BadRequest("Missing parameter.");
            }

            var turno = await db.Turnos.Where(t => t.Fecha > fecha && t.IdMedico == idMedico).OrderBy(t => t.Fecha).ToListAsync();

            if (turno == null) {
                return NotFound();
            }

            var responses = new List<TurnoResponse>();
            foreach (var usuario in turno) {

                responses.Add(new TurnoResponse {

                    IdTurno = usuario.IdTurno,
                    Fecha = usuario.Fecha,
                    IdMedico = usuario.IdMedico,
                    Medico = usuario.Medico,
                    IdUsuario = usuario.IdUsuario,
                    Usuario = usuario.Usuario,
                    IdObraSocial = usuario.IdObraSocial,
                    ObraSocial = usuario.ObraSocial,
                    Especialidad = usuario.Especialidad,
                    IdEspecialidad = usuario.IdEspecialidad,
                    
                });
            }
            

            return Ok(responses);
        }

        // GET: api/GetTurnoByFecha/5 busca turnos viejos
        [HttpPost]
        [Route("GetTurnosViejos")]
        public async Task<IHttpActionResult> GetTurnosViejos(JObject form) {

            DateTime fecha = DateTime.Now;
            int idMedico;
            dynamic jsonObject = form;
            try {
                fecha = jsonObject.Fecha;
                idMedico = jsonObject.IdMedico;
            } catch {
                return BadRequest("Missing parameter.");
            }

            var turno = await db.Turnos.Where(t => t.Fecha <= fecha && t.IdMedico == idMedico).OrderBy(t => t.Fecha).ToListAsync();

            if (turno == null) {
                return NotFound();
            }

            var responses = new List<TurnoResponse>();
            foreach (var usuario in turno) {

                responses.Add(new TurnoResponse {

                    IdTurno = usuario.IdTurno,
                    Fecha = usuario.Fecha,
                    IdMedico = usuario.IdMedico,
                    Medico = usuario.Medico,
                    IdUsuario = usuario.IdUsuario,
                    Usuario = usuario.Usuario,
                    IdObraSocial = usuario.IdObraSocial,
                    ObraSocial = usuario.ObraSocial,
                    Especialidad = usuario.Especialidad,
                    IdEspecialidad = usuario.IdEspecialidad,

                });
            }
            

            return Ok(responses);
        }

        // PUT: api/Turnoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTurno(int id, Turno turno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != turno.IdTurno)
            {
                return BadRequest();
            }

            db.Entry(turno).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurnoExists(id))
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

        // POST: api/Turnoes
        [ResponseType(typeof(Turno))]
        public async Task<IHttpActionResult> PostTurno(Turno turno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Turnos.Add(turno);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = turno.IdTurno }, turno);
        }

        // DELETE: api/Turnoes/5
        [ResponseType(typeof(Turno))]
        public async Task<IHttpActionResult> DeleteTurno(int id)
        {
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }

            db.Turnos.Remove(turno);
            await db.SaveChangesAsync();

            return Ok(turno);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TurnoExists(int id)
        {
            return db.Turnos.Count(e => e.IdTurno == id) > 0;
        }
    }
}