using models;

namespace usecase
{
  public class UserCreateResponse
  {
    public User User { get; set; }
    public UserCreateResponse (User user)
    {
      User = user;
    }

  }
}