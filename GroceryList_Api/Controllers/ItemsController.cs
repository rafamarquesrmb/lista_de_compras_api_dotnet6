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
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemsController(DataContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpGet("{grocerylistid}")]
        public async Task<IActionResult> GetItems([FromRoute]Guid grocerylistid)
        {
            try
            {
                if(!await UserFromGroceryListExists(grocerylistid))
                    return Unauthorized();
                var listItems = await _context.Items.Where(x => x.GroceryListId == grocerylistid).ToListAsync();
                return Ok(listItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpGet("{grocerylistid}/{id}")]
        public async Task<IActionResult> GetItemById([FromRoute] Guid grocerylistid,[FromRoute]Guid id)
        {
            try
            {
                if (!await UserFromGroceryListExists(grocerylistid))
                    return Unauthorized();
                var item = await _context.Items.Where(x=>x.GroceryListId == grocerylistid).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{grocerylistid}/{id}")]
        public async Task<IActionResult> PutItem([FromRoute]Guid grocerylistid, [FromRoute]Guid id, [FromBody]UpdateItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (!await UserFromGroceryListExists(grocerylistid))
                    return Unauthorized();
                var item = await _context.Items.Where(x => x.GroceryListId == grocerylistid).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                    return NotFound();
                if (model.Title != null)
                    item.Title = model.Title;
                if (model.Quantity != null)
                    item.Quantity = (int)model.Quantity;
                if(model.Bought != null)
                    item.Bought = (bool)model.Bought;
                if(model.QuantityTitle != null)
                    item.QuantityTitle = model.QuantityTitle;
                _context.Items.Update(item);
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("{grocerylistid}")]
        public async Task<IActionResult> PostItem([FromRoute] Guid grocerylistid,AddItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (!await UserFromGroceryListExists(grocerylistid))
                    return Unauthorized();
                ;
                Item item = new Item
                {
                    Title = model.Title,
                    Bought = model.Bought,
                    Quantity = model.Quantity,
                    QuantityTitle = model.QuantityTitle,
                    GroceryListId = grocerylistid
                };
                var itemEntry = _context.Items.Add(item);
                await _context.SaveChangesAsync();
                item.Id = itemEntry.Entity.Id;
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{grocerylistid}/{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute]Guid grocerylistid,[FromRoute]Guid id)
        {
            try
            {
                if (await UserFromItemExists(grocerylistid) == false)
                    return Unauthorized();
                var item = await _context.Items.AsNoTracking().Where(x=> x.GroceryListId == grocerylistid).FirstOrDefaultAsync(x => x.Id == id);
                if(item == null)
                    return NotFound();
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return Ok("This item was Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private async Task<bool> UserFromGroceryListExists(Guid groceryListId)
        {
            var _exist = await _context.GroceryLists.AsNoTracking().Where(x => x.UserId == Guid.Parse(User.Identity.Name)).ToListAsync();
            if(_exist == null)
                return false;
            if (_exist.Find(x => x.Id == groceryListId) == null)
                return false;
            return true;
        }

        private async Task<bool> UserFromItemExists(Guid groceryListId)
        {
            var _groceryList = await _context.GroceryLists.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groceryListId);
            if (_groceryList == null)
                return false;
            if (_groceryList.UserId != Guid.Parse(User.Identity.Name))
                return false;
            return true;
        }
    }
}

