using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIExcel.Models.Contexts;
using WebAPIExcel.Models.Items;

namespace WebAPIExcel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UnitController(AppDbContext context)
        {
            _context = context;
        }
        
        /**
         * Lista estatica de mediads
         */
        public static readonly List<UnitItem> unitItems = new List<UnitItem>
        { 
            new UnitItem { Id = 1, Name = "PZA", CreateTimestamp = DateTime.Now, UpdateTimestamp = DateTime.Now },
            new UnitItem { Id = 2, Name = "M2", CreateTimestamp = DateTime.Now, UpdateTimestamp = DateTime.Now },
            new UnitItem { Id = 3, Name = "M3", CreateTimestamp = DateTime.Now, UpdateTimestamp = DateTime.Now },
            new UnitItem { Id = 4, Name = "ML", CreateTimestamp = DateTime.Now, UpdateTimestamp = DateTime.Now }
        };

        // GET: api/Unit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitItem>>> GetUnitItems()
        {
            // return await _context.UnitItems.ToListAsync();
            return Ok(unitItems);
        }

        // GET: api/Unit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitItem>> GetUnitItem(long? id)
        {
            var unitItem = unitItems.FirstOrDefault(u => u.Id == id);

            if (unitItem == null)
            {
                return NotFound();
            }

            return Ok(unitItem);
        }

        // PUT: api/Unit/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutUnitItem(long? id, UnitItem unitItem)
        // {
        //     if (id != unitItem.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(unitItem).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!UnitItemExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //
        //     return NoContent();
        // }

        // POST: api/Unit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<UnitItem>> PostUnitItem(UnitItem unitItem)
        // {
        //     _context.UnitItem.Add(unitItem);
        //     await _context.SaveChangesAsync();
        //
        //     return CreatedAtAction("GetUnitItem", new { id = unitItem.Id }, unitItem);
        // }

        // DELETE: api/Unit/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteUnitItem(long? id)
        // {
        //     var unitItem = await _context.UnitItem.FindAsync(id);
        //     if (unitItem == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.UnitItem.Remove(unitItem);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        // private bool UnitItemExists(long? id)
        // {
        //     return _context.UnitItem.Any(e => e.Id == id);
        // }
    }
}
