using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmailGroupsAppv2.Models
{
  public class MailGroup
  {
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }
    public string Description { get; set; }
    public List<MailAddress> Addresses { get; set; } = new List<MailAddress>();

    [JsonIgnore]
    public string OwnerId { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser Owner { get; set; }
  }
}
