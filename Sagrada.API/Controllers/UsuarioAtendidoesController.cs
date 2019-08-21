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
    public class UsuarioAtendidoesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/UsuarioAtendidoes
        public async Task<IHttpActionResult> GetUsuarioAtendidoes() {

            var usuarios = await db.UsuarioAtendidos.OrderBy(u => u.Fecha).ToListAsync();
            var responses = new List<UsuarioAtendidoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new UsuarioAtendidoResponse {

                    CobroMedico = usuario.CobroMedico,
                    Fecha = usuario.Fecha,
                    HistoriaClinica = usuario.HistoriaClinica,
                    IdCobroMedico = usuario.IdCobroMedico,
                    IdUsuario = usuario.IdUsuario,
                    IdUsuarioAtendido = usuario.IdUsuarioAtendido,
                    Usuario = usuario.Usuario,
                    IdObraSocial = usuario.IdObraSocial,
                    ObraSocial = usuario.ObraSocial,
                });
            }

            return Ok(responses);
        }

        // GET: api/UsuarioAtendidoes/5 busca por usuario
        [ResponseType(typeof(UsuarioAtendido))]
        public async Task<IHttpActionResult> GetUsuarioAtendido(int id)
        {
            var usuarioAtendido = await db.UsuarioAtendidos.Where(u => u.IdUsuario == id).ToListAsync();
            if (usuarioAtendido == null)
            {
                return NotFound();
            }

            var responses = new List<UsuarioAtendidoResponse>();
            foreach (var usuario in usuarioAtendido) {

                responses.Add(new UsuarioAtendidoResponse {

                    CobroMedico = usuario.CobroMedico,
                    Fecha = usuario.Fecha,
                    HistoriaClinica = usuario.HistoriaClinica,
                    IdCobroMedico = usuario.IdCobroMedico,
                    IdUsuario = usuario.IdUsuario,
                    IdUsuarioAtendido = usuario.IdUsuarioAtendido,
                    Usuario = usuario.Usuario,
                    IdObraSocial = usuario.IdObraSocial,
                    ObraSocial = usuario.ObraSocial,
                    
                });
            }

            return Ok(responses);
        }

        // PUT: api/UsuarioAtendidoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuarioAtendido(int id, UsuarioAtendido usuarioAtendido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarioAtendido.IdUsuarioAtendido)
            {
                return BadRequest();
            }

            db.Entry(usuarioAtendido).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioAtendidoExists(id))
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

        // POST: api/UsuarioAtendidoes
        [ResponseType(typeof(UsuarioAtendido))]
        public async Task<IHttpActionResult> PostUsuarioAtendido(UsuarioAtendido usuarioAtendido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UsuarioAtendidos.Add(usuarioAtendido);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuarioAtendido.IdUsuarioAtendido }, usuarioAtendido);
        }

        // DELETE: api/UsuarioAtendidoes/5
        [ResponseType(typeof(UsuarioAtendido))]
        public async Task<IHttpActionResult> DeleteUsuarioAtendido(int id)
        {
            UsuarioAtendido usuarioAtendido = await db.UsuarioAtendidos.FindAsync(id);
            if (usuarioAtendido == null)
            {
                return NotFound();
            }

            db.UsuarioAtendidos.Remove(usuarioAtendido);
            await db.SaveChangesAsync();

            return Ok(usuarioAtendido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioAtendidoExists(int id)
        {
            return db.UsuarioAtendidos.Count(e => e.IdUsuarioAtendido == id) > 0;
        }
    }
}