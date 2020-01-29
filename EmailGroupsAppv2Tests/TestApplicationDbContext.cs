using EmailGroupsAppv2.Data;
using EmailGroupsAppv2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailGroupsAppv2Tests
{
  public class TestApplicationDbContext : IApplicationDbContext
  {
    public virtual DbSet<MailGroup> MailGroups { get; set; }
    public virtual DbSet<MailAddress> MailAddresses { get; set; }

    public virtual void MarkMailAddressAsModified(MailAddress mailAddress)
    {
    }

    public virtual void MarkMailGroupAsModified(MailGroup mailGroup)
    {
    }

    public virtual Task<int> SaveChangesAsync()
    {
      throw new NotImplementedException();
    }
  }
}
