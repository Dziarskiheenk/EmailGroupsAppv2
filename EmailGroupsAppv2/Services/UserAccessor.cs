using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EmailGroupsAppv2.Services
{
  public class UserAccessor : IUserAccessor
  {
    private string userId;
    public UserAccessor()
    {
      HttpContextAccessor contextAccessor = new HttpContextAccessor();
      userId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }

    public string UserId => userId;
  }
}
