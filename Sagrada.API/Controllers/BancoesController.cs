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
    public class BancoesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Bancoes
        public async Task<IHttpActionResult> GetBancoes() {

            var usuarios = await db.Bancos.ToListAsync();
            var responses = new List<BancoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new BancoResponse {

                    IdBanco = usuario.IdBanco,
                    Nombre = usuario.Nombre,
                    Tarjetas = usuario.Tarjetas,



                });
            }

            return Ok(responses);
        }

        // GET: api/Bancoes/5
        [ResponseType(typeof(Banco))]
        public async Task<IHttpActionResult> GetBanco(int id)
        {
            Banco banco = await db.Bancos.FindAsync(id);
            if (banco == null)
            {
                return NotFound();
            }

            return Ok(banco);
        }

        // PUT: api/Bancoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBanco(int id, Banco banco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != banco.IdBanco)
            {
                return BadRequest();
            }

            db.Entry(banco).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BancoExists(id))
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

        // POST: api/Bancoes
        [ResponseType(typeof(Banco))]
        public async Task<IHttpActionResult> PostBanco(Banco banco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bancos.Add(banco);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = banco.IdBanco }, banco);
        }

        // DELETE: api/Bancoes/5
        [ResponseType(typeof(Banco))]
        public async Task<IHttpActionResult> DeleteBanco(int id)
        {
            Banco banco = await db.Bancos.FindAsync(id);
            if (banco == null)
            {
                return NotFound();
            }

            db.Bancos.Remove(banco);
            await db.SaveChangesAsync();

            return Ok(banco);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BancoExists(int id)
        {
            return db.Bancos.Count(e => e.IdBanco == id) > 0;
        }
    }
}