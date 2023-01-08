using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public LoginController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    [Route(nameof(Authenticate))]
    public async Task<ActionResult<bool>> Authenticate([FromBody] LoginDto loginDto)
    {
        //await userRepository.AddUser(loginDto.Username, loginDto.Password);

        // user/pwd => allebei admin gebruiken.

        return await userRepository.Authenticate(loginDto.Username, loginDto.Password);
    }
}
