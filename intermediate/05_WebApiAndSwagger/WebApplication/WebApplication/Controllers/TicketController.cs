using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private TicketContext _context;
        public TicketController(TicketContext context)
        {
            _context = context;

            if(_context.TicketItems.Count() == 0)
            {
                _context.TicketItems.Add(new TicketItem { Concert = "Beyonce" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TicketItem> GetAll()
        {
            return _context.TicketItems.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "GetTicket")]
        public IActionResult GetById(long id)
        {
            var ticket = _context.TicketItems.FirstOrDefault(t => t.Id == id);
            if(ticket == null)
            {
                return NotFound(); // 404
            }

            return new ObjectResult(ticket); // 200
        }

        [HttpPost]
        public IActionResult Create([FromBody]TicketItem ticket)
        {
            if(ticket == null)
            {
                return BadRequest(); // 400
            }

            _context.TicketItems.Add(ticket);
            _context.SaveChanges();
            // return "/Ticket/{id}"
            return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody]TicketItem ticket)
        {
            if(ticket == null || ticket.Id != id)
            {
                return BadRequest(); // 400
            }

            var tic = _context.TicketItems.FirstOrDefault(t => t.Id == id);
            if(tic == null)
            {
                return NotFound(); //404
            }

            tic.Concert = ticket.Concert;
            tic.Available = ticket.Available;
            tic.Artist = ticket.Artist;
            _context.TicketItems.Update(tic);
            _context.SaveChanges();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var tics = _context.TicketItems.FirstOrDefault(t => t.Id == id);
            if(tics == null)
            {
                return NotFound(); // 404
            }

            _context.TicketItems.Remove(tics);
            _context.SaveChanges();

            return new NoContentResult();

        }
    }
}