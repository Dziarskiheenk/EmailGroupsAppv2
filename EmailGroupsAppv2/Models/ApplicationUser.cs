using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailGroupsAppv2.Models
{
  public class ApplicationUser : IdentityUser
  {
    public List<MailGroup> MailGroups { get; set; }
  }
}
