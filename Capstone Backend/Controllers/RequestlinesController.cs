using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_Backend.Data;
using Capstone_Backend.Models;
using System.Runtime.CompilerServices;
using Capstone_Backend.Controllers;

namespace Capstone_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestlinesController : ControllerBase
    {
        private readonly Capstone_BackendContext _context;

        public RequestlinesController(Capstone_BackendContext context)
        {
            _context = context;
        }

        // GET: api/Requestlines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Requestline>>> GetRequestline()
        {
            foreach(var i in _context.Requestline.ToList()) {
                await RecalculateTotal(i.RequestId, _context.Request.Find(i.RequestId));
            }
            return await _context.Requestline.ToListAsync();
        }

        // GET: api/Requestlines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Requestline>> GetRequestline(int id)
        {
            var requestline = await _context.Requestline.FindAsync(id);

            if (requestline == null)
            {
                return NotFound();
            }
            await RecalculateTotal(requestline.RequestId, _context.Request.Find(requestline.RequestId));
            return requestline;
        }

        private async Task RecalculateTotal(int id,Request request) {

            var hold = (from p in _context.Product.ToList()
                        join rl in _context.Requestline.ToList()
                        on p.Id equals rl.ProductId
                        where id == rl.RequestId
                        select new {
                            total = rl.Quantity * p.Price
                        }).Sum(t => t.total);
            request.Total = hold;
            await _context.SaveChangesAsync();
        }


        // PUT: api/Requestlines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestline(int id, Requestline requestline)
        {
            if (id != requestline.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestlineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPut("quantity/{id}")]
        public async Task<IActionResult> PositiveQuantity(int id, Requestline requestline) {
            if(requestline.Quantity < 0)
                requestline.Quantity = 0;
            else
                return NoContent();
            return await PutRequestline(id, requestline);
        }
        // POST: api/Requestlines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Requestline>> PostRequestline(Requestline requestline)
        {
            if(requestline.Quantity < 0) {
                requestline.Quantity = 0;
            }
            _context.Requestline.Add(requestline);
            await _context.SaveChangesAsync();
            await RecalculateTotal(requestline.RequestId, _context.Request.Find(requestline.RequestId));

            return CreatedAtAction("GetRequestline", new { id = requestline.Id }, requestline);
        }

        // DELETE: api/Requestlines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Requestline>> DeleteRequestline(int id)
        {
            var requestline = await _context.Requestline.FindAsync(id);
            if (requestline == null)
            {
                return NotFound();
            }
            var holdId = requestline.RequestId;
            _context.Requestline.Remove(requestline);
            await _context.SaveChangesAsync();
            await RecalculateTotal(holdId, _context.Request.Find(holdId));
            return requestline;
        }

        private bool RequestlineExists(int id)
        {
            return _context.Requestline.Any(e => e.Id == id);
        }
    }
}
