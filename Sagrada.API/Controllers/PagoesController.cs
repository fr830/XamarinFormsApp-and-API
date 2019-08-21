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
    [RoutePrefix("api/Pagoes")]
    public class PagoesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Pagoes
        public async Task<IHttpActionResult> GetPagoes() {

            var usuarios = await db.Pagos.OrderBy(p => p.IdTarjeta != null).ToListAsync();
            var responses = new List<PagoResponse>();
            foreach (var usuario in usuarios) {

                responses.Add(new PagoResponse {

                    Fecha = usuario.Fecha,
                    IdPago = usuario.IdPago,
                    IdTarjeta = usuario.IdTarjeta,
                    IdUsuario = usuario.IdUsuario,
                    Monto = usuario.Monto,
                    Tarjeta = usuario.Tarjeta,
                    Usuario = usuario.Usuario,
                    IdMedico = usuario.IdMedico,
                    Medico = usuario.Medico,

                });
            }

            return Ok(responses);
        }


        // GET: api/Pagoes/5
        [ResponseType(typeof(Pago))]
        public async Task<IHttpActionResult> GetPago(int id)
        {
            Pago pago = await db.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            return Ok(pago);
        }

        // GET: api/GetPagoByFecha/5 busca pago por fecha y usuario
        [HttpPost]
        [Route("GetPagoByFecha")]
        public async Task<IHttpActionResult> GetPagoByFecha(JObject form) {

            DateTime fecha = DateTime.Now;
            int idUsuario;
            dynamic jsonObject = form;
            try {
                fecha = jsonObject.Fecha;
                idUsuario = jsonObject.IdUsuario;
            } catch {
                return BadRequest("Missing parameter.");
            }

            var pagos = await db.Pagos.Where(t => t.Fecha == fecha && t.IdUsuario == idUsuario).FirstOrDefaultAsync();

            if (pagos == null) {
                return NotFound();
            }

            return Ok(pagos);
        }

        // PUT: api/Pagoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPago(int id, Pago pago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pago.IdPago)
            {
                return BadRequest();
            }

            db.Entry(pago).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(id))
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

        // POST: api/Pagoes
        [ResponseType(typeof(Pago))]
        public async Task<IHttpActionResult> PostPago(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pagos.Add(pago);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pago.IdPago }, pago);
        }

        // DELETE: api/Pagoes/5
        [ResponseType(typeof(Pago))]
        public async Task<IHttpActionResult> DeletePago(int id)
        {
            Pago pago = await db.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            db.Pagos.Remove(pago);
            await db.SaveChangesAsync();

            return Ok(pago);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PagoExists(int id)
        {
            return db.Pagos.Count(e => e.IdPago == id) > 0;
        }
    }
}