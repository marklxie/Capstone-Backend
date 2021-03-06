﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_Backend.Data;
using Capstone_Backend.Models;
using System.Runtime.CompilerServices;

namespace Capstone_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase {
        private readonly Capstone_BackendContext _context;

        public RequestsController(Capstone_BackendContext context) {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest() {
            return await _context.Request.Include(u =>u.User).ToListAsync();
        }
        [HttpGet("review/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetReviewRequest(int userId) {
            return await _context.Request.Where(r => r.UserId != userId && r.Status == "REVIEW").ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id) {
            var request = await _context.Request.FindAsync(id);

            if(request == null) {
                return NotFound();
            }

            _context.Entry(request).Reference("User").Load();

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request) {
            if(id != request.Id) {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!RequestExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }
        // Put:Api/requests
        [HttpPut("review/{id}")]
        public async Task<IActionResult> ReviewRequest(int id, Request request) {

            request.Status = request.Total < 50 ? "APPROVED" : "REVIEW";
            return await PutRequest(id, request);

        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveCheapRequest(int id, Request request) {

            request.Status = "APPROVED";
            return await PutRequest(id, request);

        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectRequest(int id, Request request) {

            request.Status = "REJECTED";
            return await PutRequest(id, request);

        }


        // POST: api/Requests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Request.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id)
        {
            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Request.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool RequestExists(int id)
        {
            return _context.Request.Any(e => e.Id == id);
        }
    }
}
