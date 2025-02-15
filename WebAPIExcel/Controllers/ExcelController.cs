using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIExcel.Models;

namespace WebAPIExcel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly ExcelContext _context;

        public ExcelController(ExcelContext context)
        {
            _context = context;
        }

        // GET: api/Excel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExcelItem>>> GetExcelItems()
        {
            return await _context.ExcelItems.ToListAsync();
        }

        // GET: api/Excel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExcelItem>> GetExcelItem(long id)
        {
            var excelItem = await _context.ExcelItems.FindAsync(id);

            if (excelItem == null)
            {
                return NotFound();
            }

            return excelItem;
        }

        // PUT: api/Excel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExcelItem(long id, ExcelItem excelItem)
        {
            // Verificar que el item exista
            var existingExcelItem = await _context.ExcelItems.FindAsync(id);
            if (existingExcelItem == null)
            {
                return NotFound();
            }

            // Modificacion del campo de excel
            existingExcelItem.Update(excelItem.Name);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExcelItemExists(id))
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

        // POST: api/Excel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExcelItem>> PostExcelItem(ExcelItem excelItem)
        {
            _context.ExcelItems.Add(excelItem);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetExcelItem", new { id = excelItem.Id }, excelItem);
            return CreatedAtAction("GetExcelItem", new { id = excelItem.Id }, excelItem);
        }

        // DELETE: api/Excel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExcelItem(long id)
        {
            var excelItem = await _context.ExcelItems.FindAsync(id);
            if (excelItem == null)
            {
                return NotFound();
            }

            _context.ExcelItems.Remove(excelItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExcelItemExists(long id)
        {
            return _context.ExcelItems.Any(e => e.Id == id);
        }
    }
}
