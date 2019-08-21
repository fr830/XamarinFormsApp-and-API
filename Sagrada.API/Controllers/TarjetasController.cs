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
    public class TarjetasController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Tarjetas
        public async Task<IHttpActionResult> GetTarjetas() {

            var usuarios = await db.Tarjetas.ToListAsync();
            var responses = new List<TarjetaResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new TarjetaResponse {

                    Banco = usuario.Banco,
                    IdBanco = usuario.IdBanco,
                    CodOperacion = usuario.CodOperacion,
                    IdTarjeta = usuario.IdTarjeta,
                    Pagos = usuario.Pagos,


                });
            }

            return Ok(responses);
        }

        // GET: api/Tarjetas/5 obtiene tarjeta por codOperacion
        [ResponseType(typeof(Tarjeta))]
        public async Task<IHttpActionResult> GetTarjeta(int id)
        {
            Tarjeta tarjeta = await db.Tarjetas.Where(t => t.CodOperacion == id).FirstOrDefaultAsync();
            if (tarjeta == null)
            {
                return NotFound();
            }

            return Ok(tarjeta);
        }

        // PUT: api/Tarjetas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTarjeta(int id, Tarjeta tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tarjeta.IdTarjeta)
            {
                return BadRequest();
            }

            db.Entry(tarjeta).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaExists(id))
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

        // POST: api/Tarjetas
        [ResponseType(typeof(Tarjeta))]
        public async Task<IHttpActionResult> PostTarjeta(Tarjeta tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tarjetas.Add(tarjeta);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tarjeta.IdTarjeta }, tarjeta);
        }

        // DELETE: api/Tarjetas/5
        [ResponseType(typeof(Tarjeta))]
        public async Task<IHttpActionResult> DeleteTarjeta(int id)
        {
            Tarjeta tarjeta = await db.Tarjetas.FindAsync(id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            db.Tarjetas.Remove(tarjeta);
            await db.SaveChangesAsync();

            return Ok(tarjeta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarjetaExists(int id)
        {
            return db.Tarjetas.Count(e => e.IdTarjeta == id) > 0;
        }
    }
}