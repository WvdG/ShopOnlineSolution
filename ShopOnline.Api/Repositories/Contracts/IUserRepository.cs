namespace ShopOnline.Api.Repositories.Contracts;
public interface IUserRepository
{
    Task<bool> Authenticate(string username, string password);
    Task AddUser(string username, string password);
}
