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
    public class GroupController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupItem>>> GetGroupItems()
        {
            var groupItemsWithExcelFile = await _context.GroupItems
                .Select(groupItem => new
                {
                    groupItem.Id,
                    groupItem.Key,
                    groupItem.Concept,
                    ExcelFile = new ExcelItemDTO
                    {
                        Id = groupItem.ExcelFile.Id,
                        Name = groupItem.ExcelFile.Name
                    },
                    groupItem.CreateTimestamp,
                    groupItem.UpdateTimestamp
                })
                .ToListAsync();

            return Ok(groupItemsWithExcelFile);
            // return await _context.GroupItems.ToListAsync();
        }

        // GET: api/Group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupItem>> GetGroupItem(long id)
        {
            var groupItem = await _context.GroupItems.FindAsync(id);
            if (groupItem == null)
            {
                return NotFound();
            }

            var groupItemsWithExcelFile = await _context.GroupItems
                .Where(g => g.Id == id)
                .Select(groupItem => new
                {
                    groupItem.Id,
                    groupItem.Key,
                    groupItem.Concept,
                    ExcelFile = new ExcelItemDTO
                    {
                        Id = groupItem.ExcelFile.Id,
                        Name = groupItem.ExcelFile.Name
                    },
                    groupItem.CreateTimestamp,
                    groupItem.UpdateTimestamp
                })
                .FirstOrDefaultAsync();

            return Ok(groupItemsWithExcelFile);
            
            // return groupItem;
        }

        // POST: api/Group
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GroupItemDTO>> PostGroupItem(GroupItemDTO groupItemDTO)
        {
            // Verificar si el ExcelFileID existe
            var excelItem = await _context.ExcelItems.FindAsync(groupItemDTO.ExcelFileID);
            if (excelItem == null)
            {
                return BadRequest("El ExcelFileID proporcionado no existe.");
            }
            
            // Almacenar los datos de DTO a Item
            var groupItem = new GroupItem
            {
                Key = groupItemDTO.Key,
                Concept = groupItemDTO.Concept,
                ExcelFileID = groupItemDTO.ExcelFileID,   
                ExcelFile = excelItem,
                CreateTimestamp = DateTime.Now,
                UpdateTimestamp = DateTime.Now
            };
            
            // Asignar la relación con ExcelItem explícitamente
            groupItem.ExcelFile = excelItem;
            
            _context.GroupItems.Add(groupItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroupItem), new { id = groupItem.Id }, ItemToDto(groupItem));
        }
        
        // PUT: api/Group/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutGroupItem(long id, GroupItem groupItem)
        // {
        //     if (id != groupItem.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(groupItem).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!GroupItemExists(id))
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

        // DELETE: api/Group/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteGroupItem(long id)
        // {
        //     var groupItem = await _context.GroupItems.FindAsync(id);
        //     if (groupItem == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.GroupItems.Remove(groupItem);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        // private bool GroupItemExists(long id)
        // {
        //     return _context.GroupItems.Any(e => e.Id == id);
        // }
        
        /**
        * Convierte de Item a DTO
        */
        private static GroupItemDTO ItemToDto(GroupItem grouupItem) => new GroupItemDTO()
        {
            Id = grouupItem.Id,
            Key = grouupItem.Key,
            Concept = grouupItem.Concept,
            ExcelFileID = grouupItem.ExcelFileID
        };
    }
}
