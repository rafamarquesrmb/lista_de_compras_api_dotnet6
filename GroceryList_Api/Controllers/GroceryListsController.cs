#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroceryList_Api.Data;
using GroceryList_Api.Models;
using Microsoft.AspNetCore.Authorization;
using GroceryList_Api.ViewModels;

namespace GroceryList_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroceryListsController : ControllerBase
    {
        private readonly DataContext _context;

        public GroceryListsController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetGroceryLists()
        {
            try
            {
                var list = await _context.GroceryLists.Where(x => x.UserId == Guid.Parse(User.Identity.Name)).ToListAsync();
                if (list == null)
                {
                    return NoContent();
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroceryList(Guid id)
        {
            try
            {
                var groceryList = await _context.GroceryLists.AsNoTracking().Where(x => x.UserId == Guid.Parse(User.Identity.Name)).FirstOrDefaultAsync(x => x.Id == id);

                if (groceryList == null)
                    return NotFound();
                return Ok(groceryList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroceryList(Guid id, UpdateGroceryListViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var groceryList = await _context.GroceryLists.AsNoTracking().Where(x => x.UserId == Guid.Parse(User.Identity.Name)).FirstOrDefaultAsync(x => x.Id == id);

                if (groceryList == null)
                    return NotFound();
                if(model.Title != null)
                    groceryList.Title = model.Title;
                if(model.Description != null)
                    groceryList.Description = model.Description;        
                _context.GroceryLists.Update(groceryList);
                await _context.SaveChangesAsync();
                return Ok(groceryList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostGroceryList(AddGroceryListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(User.Identity.Name));
                GroceryList groceryList = new GroceryList
                {
                    Title = model.Title,
                    Description = model.Description,
                    UserId = userId
                };
                var groceryListEntry = _context.GroceryLists.Add(groceryList);
                await _context.SaveChangesAsync();
                groceryList.Id = groceryListEntry.Entity.Id;
                groceryList.User = null;
                return Ok(groceryList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroceryList(Guid id)
        {
            try
            {
                var groceryList = await _context.GroceryLists.FindAsync(id);
                if (groceryList == null)
                {
                    return NotFound();
                }
                if (groceryList.UserId != Guid.Parse(User.Identity.Name))
                    return Unauthorized();
                _context.GroceryLists.Remove(groceryList);
                await _context.SaveChangesAsync();
                return Ok("This list was Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
