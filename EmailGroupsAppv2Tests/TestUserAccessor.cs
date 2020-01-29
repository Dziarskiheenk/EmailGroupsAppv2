using EmailGroupsAppv2.Services;

namespace EmailGroupsAppv2Tests
{
  class TestUserAccessor : IUserAccessor
  {
    public TestUserAccessor(string userId)
    {
      this.UserId = userId;
    }
    public string UserId { get; }
  }
}
