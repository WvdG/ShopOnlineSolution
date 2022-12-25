using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Controllers;
public class LoginController
{
    private readonly IUserRepository userRepository;

    public LoginController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Authenticate([FromBody] string username, [FromBody] string password)
    {
        return await userRepository.Authenticate(username, password);
    }
}
