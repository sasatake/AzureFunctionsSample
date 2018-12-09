using System.Threading.Tasks;
using models;
using shared;

namespace usecase
{
  public class UserCreateUseCase
  {

    public async Task<UserCreateResponse> execute (UserCreateRequest request)
    {
      var document = await DocumentDBRepository<User>.CreateItemAsync (request.User);
      return new UserCreateResponse (new User ()
      {
        Id = document.GetPropertyValue<string> ("id"), Name = document.GetPropertyValue<string> ("name"), Description = document.GetPropertyValue<string> ("description")
      });
    }
  }
}