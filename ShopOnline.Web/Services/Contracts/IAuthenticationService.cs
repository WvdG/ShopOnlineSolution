namespace ShopOnline.Web.Services.Contracts;
public interface IAuthenticationService
{
    Task<bool> Authenticate(string username, string password);
}
