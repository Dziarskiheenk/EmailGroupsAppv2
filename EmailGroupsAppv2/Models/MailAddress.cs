using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmailGroupsAppv2.Models
{
  public class MailAddress
  {
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; }
    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    public string Address { get; set; }

    public int GroupId { get; set; }
    [JsonIgnore]
    public virtual MailGroup MailGroup { get; set; }
  }
}
