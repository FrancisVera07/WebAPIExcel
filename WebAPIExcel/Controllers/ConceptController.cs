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
    public class ConceptController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConceptController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Concept
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConceptItem>>> GetConceptItems()
        {
            var conceptItems = await _context.ConceptItems
                .Include(ci => ci.GroupParentItem)
                .ThenInclude(gpi => gpi.GroupItem)
                .ToListAsync();
            
            var result = conceptItems.Select(item => new
            {
                item.Id,
                item.Key,
                item.Concept,
                item.Quantity,
                item.UnitPrice,
                item.Amount,
                GroupParentItem = new GroupItemDTO
                {
                    Id = item.GroupParentItem.GroupItem.Id,
                    Key = item.GroupParentItem.GroupItem.Key,
                    Concept = item.GroupParentItem.GroupItem.Concept,
                    ExcelFileID = item.GroupParentItem.GroupItem.ExcelFileID
                },
                UnitItem = UnitController.unitItems
                    .Where(u => u.Id == item.UnitID)
                    .Select(u => new
                    {
                        u.Id,
                        u.Name
                    })
                    .FirstOrDefault(),
                item.CreateTimestamp,
                item.UpdateTimestamp
            }).ToList();
         
            return Ok(result);
            // return await _context.ConceptItems.ToListAsync();
        }

        // GET: api/Concept/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConceptItem>> GetConceptItem(long id)
        {
            var conceptItem = await _context.ConceptItems.FindAsync(id);
            if (conceptItem == null)
            {
                return NotFound();
            }
            
            var result = new
            {
                conceptItem.Id,
                conceptItem.Key,
                conceptItem.Concept,
                conceptItem.Quantity,
                conceptItem.UnitPrice,
                conceptItem.Amount,
                GroupParentItem = new GroupItemDTO
                {
                    Id = conceptItem.GroupParentItem.GroupItem.Id,
                    Key = conceptItem.GroupParentItem.GroupItem.Key,
                    Concept = conceptItem.GroupParentItem.GroupItem.Concept,
                    ExcelFileID = conceptItem.GroupParentItem.GroupItem.ExcelFileID
                },
                UnitItem = UnitController.unitItems
                    .FirstOrDefault(u => u.Id == conceptItem.UnitID), // Filtrar en memoria
                conceptItem.CreateTimestamp,
                conceptItem.UpdateTimestamp
            };

            return Ok(result);
            // return conceptItem;
        }

        // POST: api/Concept
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConceptItemDTO>> PostConceptItem(ConceptItemDTO conceptItemDTO)
        {
            // Verificar si el GroupParentID existe
            var groupParentItem = await _context.GroupParentItems
                .AsNoTracking()
                .FirstOrDefaultAsync(gp => gp.Id == conceptItemDTO.GroupParentID);
            if (groupParentItem == null)
            {
                return BadRequest("El GroupParentID proporcionado no existe.");
            }
            // Verificar si el UnitID existe en lista estatica
            var unitItem = UnitController.unitItems.FirstOrDefault(u => u.Id == conceptItemDTO.UnitID);
            if (unitItem == null)
            {
                return BadRequest("El UnitID proporcionado no existe.");
            }
            
            // Almacenar los datos de DTO a Item
            var conceptItem = new ConceptItem
            {
                Key = conceptItemDTO.Key,
                Concept = conceptItemDTO.Concept,
                UnitPrice = conceptItemDTO.UnitPrice,
                Quantity = conceptItemDTO.Quantity,
                Amount = conceptItemDTO.Amount,
                GroupParentID = conceptItemDTO.GroupParentID,
                UnitID = conceptItemDTO.UnitID,
                CreateTimestamp = DateTime.Now,
                UpdateTimestamp = DateTime.Now
            };
            
            _context.ConceptItems.Add(conceptItem);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetConceptItem", new { id = conceptItem.Id }, conceptItem);
            return CreatedAtAction(nameof(GetConceptItem), 
                new { id = conceptItem.Id }, ItemToDto(conceptItem));
        }

        // PUT: api/Concept/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutConceptItem(long id, ConceptItem conceptItem)
        // {
        //     if (id != conceptItem.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(conceptItem).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ConceptItemExists(id))
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
        
        // DELETE: api/Concept/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteConceptItem(long id)
        // {
        //     var conceptItem = await _context.ConceptItems.FindAsync(id);
        //     if (conceptItem == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.ConceptItems.Remove(conceptItem);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        // private bool ConceptItemExists(long id)
        // {
        //     return _context.ConceptItems.Any(e => e.Id == id);


        /**
         * Convierte de Item a DTO
         */
        private static ConceptItemDTO ItemToDto(ConceptItem conceptItem) => new ConceptItemDTO()
        {
            Id = conceptItem.Id,
            Key = conceptItem.Key,
            Concept = conceptItem.Concept,
            Quantity = conceptItem.Quantity,
            UnitPrice = conceptItem.UnitPrice,
            Amount = conceptItem.Amount,
            GroupParentID = conceptItem.GroupParentID,
            UnitID = conceptItem.UnitID
        };
    }
}
// 