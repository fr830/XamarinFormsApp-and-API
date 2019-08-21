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
    [RoutePrefix("api/UsuarioEnEsperas")]
    public class UsuarioEnEsperasController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/UsuarioEnEsperas
        public async Task<IHttpActionResult> GetUsuarioEnEsperas() {

            var usuarios = await db.UsuarioEnEsperas.OrderBy(u => u.Fecha).ToListAsync();
            var responses = new List<UsuarioEnEsperaResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new UsuarioEnEsperaResponse {

                    IdUsuarioEnEspera = usuario.IdUsuarioEnEspera,
                    Fecha = usuario.Fecha,
                    IdMedico = usuario.IdMedico,
                    IdUsuario = usuario.IdUsuario,
                    Medico = usuario.Medico,
                    Usuario = usuario.Usuario,
                    IdObraSocial = usuario.IdObraSocial,
                    ObraSocial = usuario.ObraSocial,
                    Especialidad = usuario.Especialidad,
                    IdEspecialidad = usuario.IdEspecialidad,                    
                });
            }

            return Ok(responses);
        }

        // GET: api/UsuarioEnEsperas/5
        [ResponseType(typeof(UsuarioEnEspera))]
        public async Task<IHttpActionResult> GetUsuarioEnEspera(int id)
        {
            UsuarioEnEspera usuarioEnEspera = await db.UsuarioEnEsperas.FindAsync(id);
            if (usuarioEnEspera == null)
            {
                return NotFound();
            }

            return Ok(usuarioEnEspera);
        }

        // GET: api/GetTurnoByFecha/5 busca Usuario en espera por fecha y medico
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

            var turno = await db.UsuarioEnEsperas.Where(t => t.Fecha == fecha && t.IdMedico == idMedico).FirstOrDefaultAsync();

            if (turno == null) {
                return NotFound();
            }

            return Ok(turno);
        }

        // PUT: api/UsuarioEnEsperas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuarioEnEspera(int id, UsuarioEnEspera usuarioEnEspera)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarioEnEspera.IdUsuarioEnEspera)
            {
                return BadRequest();
            }

            db.Entry(usuarioEnEspera).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioEnEsperaExists(id))
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

        // POST: api/UsuarioEnEsperas
        [ResponseType(typeof(UsuarioEnEspera))]
        public async Task<IHttpActionResult> PostUsuarioEnEspera(UsuarioEnEspera usuarioEnEspera)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UsuarioEnEsperas.Add(usuarioEnEspera);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuarioEnEspera.IdUsuarioEnEspera }, usuarioEnEspera);
        }

        // DELETE: api/UsuarioEnEsperas/5
        [ResponseType(typeof(UsuarioEnEspera))]
        public async Task<IHttpActionResult> DeleteUsuarioEnEspera(int id)
        {
            UsuarioEnEspera usuarioEnEspera = await db.UsuarioEnEsperas.FindAsync(id);
            if (usuarioEnEspera == null)
            {
                return NotFound();
            }

            db.UsuarioEnEsperas.Remove(usuarioEnEspera);
            await db.SaveChangesAsync();

            return Ok(usuarioEnEspera);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioEnEsperaExists(int id)
        {
            return db.UsuarioEnEsperas.Count(e => e.IdUsuarioEnEspera == id) > 0;
        }
    }
}