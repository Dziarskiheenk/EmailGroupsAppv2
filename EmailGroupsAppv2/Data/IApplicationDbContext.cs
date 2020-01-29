using EmailGroupsAppv2.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmailGroupsAppv2.Data
{
  public interface IApplicationDbContext
  {
    DbSet<MailGroup> MailGroups { get; set; }
    DbSet<MailAddress> MailAddresses { get; set; }

    Task<int> SaveChangesAsync();
    void MarkMailGroupAsModified(MailGroup mailGroup);
    void MarkMailAddressAsModified(MailAddress mailAddress);
  }
}
