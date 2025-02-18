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
    public class GroupParentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupParentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GroupParent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupParentItem>>> GetGroupParentItems()
        {
            var groupItemsWithGroupFile = await _context.GroupParentItems
                .Select(groupParentItem => new
                {
                    groupParentItem.Id,
                    GroupItem = new GroupItemDTO
                    {
                        Id = groupParentItem.GroupItem.Id,
                        Key = groupParentItem.GroupItem.Key,
                        Concept = groupParentItem.GroupItem.Concept,
                        ExcelFileID = groupParentItem.GroupItem.ExcelFileID
                    },
                    GroupParent = new GroupItemDTO
                    {
                        Id = groupParentItem.ParentItem.Id,
                        Key = groupParentItem.ParentItem.Key,
                        Concept = groupParentItem.ParentItem.Concept,
                        ExcelFileID = groupParentItem.ParentItem.ExcelFileID
                    },
                    groupParentItem.CreateTimestamp,
                    groupParentItem.UpdateTimestamp
                })
                .ToListAsync();

            return Ok(groupItemsWithGroupFile);
            // return await _context.GroupParentItems.ToListAsync();
        }

        // GET: api/GroupParent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupParentItem>> GetGroupParentItem(long id)
        {
            var groupParentItem = await _context.GroupParentItems.FindAsync(id);
            if (groupParentItem == null)
            {
                return NotFound();
            }

            var groupItemsWithGroupFile = await _context.GroupParentItems
                .Where(gp => gp.Id == id)
                .Select(groupParentItem => new
                {
                    groupParentItem.Id,
                    GroupItem = new GroupItemDTO
                    {
                        Id = groupParentItem.GroupItem.Id,
                        Key = groupParentItem.GroupItem.Key,
                        Concept = groupParentItem.GroupItem.Concept,
                        ExcelFileID = groupParentItem.GroupItem.ExcelFileID
                    },
                    GroupParent = new GroupItemDTO
                    {
                        Id = groupParentItem.ParentItem.Id,
                        Key = groupParentItem.ParentItem.Key,
                        Concept = groupParentItem.ParentItem.Concept,
                        ExcelFileID = groupParentItem.ParentItem.ExcelFileID
                    },
                    groupParentItem.CreateTimestamp,
                    groupParentItem.UpdateTimestamp
                })
                .FirstOrDefaultAsync();

            return Ok(groupItemsWithGroupFile);
            
            // return groupParentItem;
        }
        
        // POST: api/GroupParent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GroupParentItemDTO>> PostGroupParentItem(GroupParentItemDTO groupParentItemDTO)
        {
            // Verificar si el GroupID existe
            var groupItem = await _context.GroupItems.FindAsync(groupParentItemDTO.GroupID);
            if (groupItem == null)
            {
                return BadRequest("El GroupID proporcionado no existe.");
            }

            // Verificar si el ParentID existe
            var parentItem = await _context.GroupItems.FindAsync(groupParentItemDTO.ParentID);
            if (parentItem == null)
            {
                return BadRequest("El ParentID proporcionado no existe.");
            }

            // Almacenar los datos de DTO a Item
            var groupParentItem = new GroupParentItem
            {
                ParentID = groupParentItemDTO.ParentID,
                GroupID = groupParentItemDTO.GroupID,
                GroupItem = groupItem,
                ParentItem = parentItem,
                CreateTimestamp = DateTime.Now,
                UpdateTimestamp = DateTime.Now
            };
            
            // Asignar la relación con GroupItem explícitamente
            groupParentItem.GroupItem = groupItem;
            groupParentItem.ParentItem = parentItem;
            
            _context.GroupParentItems.Add(groupParentItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroupParentItem), 
                new { id = groupParentItem.Id }, ItemToDto(groupParentItem));
        }

        // PUT: api/GroupParent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutGroupParentItem(long id, GroupParentItem groupParentItem)
        // {
        //     if (id != groupParentItem.id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(groupParentItem).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!GroupParentItemExists(id))
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

       
        // DELETE: api/GroupParent/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteGroupParentItem(long id)
        // {
        //     var groupParentItem = await _context.GroupParentItems.FindAsync(id);
        //     if (groupParentItem == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.GroupParentItems.Remove(groupParentItem);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        // private bool GroupParentItemExists(long id)
        // {
        //     return _context.GroupParentItems.Any(e => e.id == id);
        // }
        
        /**
         * * Convierte de Item a DTO
         * */
        private static GroupParentItemDTO ItemToDto(GroupParentItem groupParentItem) => new GroupParentItemDTO()
        {
            Id = groupParentItem.Id,
            ParentID = groupParentItem.ParentID,
            GroupID = groupParentItem.GroupID
        };
    }
}
