using models;

namespace usecase
{
  public class UserCreateRequest
  {
    public User User { get; set; }
    public UserCreateRequest (string username, string description)
    {
      User = new User ()
      {
        Name = username, Description = description,
      };
    }
  }
}