using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailGroupsAppv2.Models;
using EmailGroupsAppv2.Data;
using Microsoft.AspNetCore.Authorization;

namespace EmailGroupsAppv2.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class MailGroupsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public MailGroupsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/MailGroups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MailGroup>>> GetMailGroups()
    {
      return await _context.MailGroups
        .Include(x => x.Addresses)
        .OrderBy(x => x.Name)
        .ToListAsync();
    }

    // GET: api/MailGroups/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MailGroup>> GetMailGroup(int id)
    {
      var mailGroup = await _context.MailGroups.FindAsync(id);

      if (mailGroup == null)
      {
        return NotFound();
      }

      return mailGroup;
    }

    // PUT: api/MailGroups/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMailGroup(int id, MailGroup mailGroup)
    {
      if (id != mailGroup.Id)
      {
        return BadRequest();
      }

      if (await _context.MailGroups.AnyAsync(x => x.Name == mailGroup.Name))
      {
        return Conflict();
      }

      _context.Entry(mailGroup).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!MailGroupExists(id))
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

    // POST: api/MailGroups
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPost]
    public async Task<ActionResult<MailGroup>> PostMailGroup(MailGroup mailGroup)
    {
      _context.MailGroups.Add(mailGroup);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (MailGroupExists(mailGroup.Id))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetMailGroup", new { id = mailGroup.Id }, mailGroup);
    }

    // POST: api/MailGroups/5/MailAddresses
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPost("{groupId}/MailAddresses")]
    public async Task<ActionResult<MailGroup>> PostMailAddress(int groupId, MailAddress mailAddress)
    {
      if (groupId != mailAddress.GroupId)
      {
        return BadRequest();
      }

      _context.MailAddresses.Add(mailAddress);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (MailAddressExists(mailAddress.Id))
        {
          return Conflict();
        }
        else if (!MailGroupExists(groupId))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetMailAddress", new { id = mailAddress.Id }, mailAddress);
    }

    // PUT: api/MailGroups/5/MailAddresses/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPut("{groupId}/MailAddresses/{id}")]
    public async Task<IActionResult> PutMailAddress(int groupId, int id, MailAddress mailAddress)
    {
      if (id != mailAddress.Id || groupId != mailAddress.GroupId)
      {
        return BadRequest();
      }

      _context.Entry(mailAddress).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!MailGroupExists(groupId) || !MailAddressExists(id))
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

    // DELETE: api/MailGroups/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<MailGroup>> DeleteMailGroup(int id)
    {
      var mailGroup = await _context.MailGroups.FindAsync(id);
      if (mailGroup == null)
      {
        return NotFound();
      }

      _context.MailGroups.Remove(mailGroup);
      await _context.SaveChangesAsync();

      return mailGroup;
    }

    // DELETE: api/MailGroups/5/MailAddresses/5
    [HttpDelete("{groupId}/MailAddresses/{id}")]
    public async Task<ActionResult<MailAddress>> DeleteMailAddress(int groupId, int id)
    {
      var mailAddress = await _context.MailAddresses.FindAsync(id);
      if (mailAddress == null)
      {
        return NotFound();
      }
      if (mailAddress.GroupId != groupId)
      {
        return BadRequest();
      }

      _context.MailAddresses.Remove(mailAddress);
      await _context.SaveChangesAsync();

      return mailAddress;
    }

    private bool MailGroupExists(int id)
    {
      return _context.MailGroups.Any(e => e.Id == id);
    }

    private bool MailAddressExists(int id)
    {
      return _context.MailAddresses.Any(e => e.Id == id);
    }
  }
}
