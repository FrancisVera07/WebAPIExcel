using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIExcel.Models.Contexts;
using WebAPIExcel.Models.DTOs;
using WebAPIExcel.Models.Items;

namespace WebAPIExcel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelFileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExcelFileController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExcelFile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExcelItem>>> GetExcelItems()
        {
            return await _context.ExcelItems.ToListAsync();
        }

        // GET: api/ExcelFile/5
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

        // POST: api/ExcelFile
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExcelItemDTO>> PostExcelItem(ExcelItemDTO excelItemDTO)
        {

            // Almacenar los datos de DTO a Item
            var excelItem = new ExcelItem
            {
                Name = excelItemDTO.Name,
                CreateTimestamp = DateTime.Now,
                UpdateTimestamp = DateTime.Now
            };
            
            // Guardar los datos
            _context.ExcelItems.Add(excelItem);
            await _context.SaveChangesAsync();

            // return CreatedAtAction(nameof(GetExcelItem), new { id = excelItem.Id }, excelItem);
            return CreatedAtAction(
                nameof(GetExcelItem), new { id = excelItem.Id }, ItemToDto(excelItem)
                );
        }
        
        // PUT: api/ExcelFile/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutExcelItem(long id, ExcelItem excelItem)
        // {
        //     if (id != excelItem.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(excelItem).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ExcelItemExists(id))
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

        // DELETE: api/ExcelFile/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteExcelItem(long id)
        // {
        //     var excelItem = await _context.ExcelItems.FindAsync(id);
        //     if (excelItem == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.ExcelItems.Remove(excelItem);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        // private bool ExcelItemExists(long id)
        // {
        //     return _context.ExcelItems.Any(e => e.Id == id);
        // }

        /**
         * Convierte de Item a DTO
         */
        private static ExcelItemDTO ItemToDto(ExcelItem excelItem) => new ExcelItemDTO()
        {
            Id = excelItem.Id,
            Name = excelItem.Name
        };
    }
}
