using Microsoft.Data.SqlClient;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories.Contracts;
using System.Security.Cryptography;

namespace ShopOnline.Api.Repositories;
public class UserRepository : IUserRepository
{
    private readonly ShopOnlineDbContext shopOnlineDbContext;

    public UserRepository(ShopOnlineDbContext shopOnlineDbContext)
    {
        this.shopOnlineDbContext = shopOnlineDbContext;
    }

    public async Task AddUser(string username, string password)
    {
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        byte[] passwordHash = HashPassword(password, salt);

        var user = new Entities.User();
        user.UserName = username;
        user.PasswordSalt = salt;
        user.PasswordHash = passwordHash;

        await this.shopOnlineDbContext.Users.AddAsync(user);
        
        await this.shopOnlineDbContext.SaveChangesAsync();
    }

    public async Task<bool> Authenticate(string username, string password)
    {
        var user = shopOnlineDbContext.Users.SingleOrDefault(x => x.UserName == username);

        if (user == null)
            return false;

        // Hash the entered password using the retrieved salt
        var enteredPasswordHash = HashPassword(password, user.PasswordSalt);

        // Compare the entered password hash with the stored password hash
        return enteredPasswordHash.SequenceEqual(user.PasswordHash);
    }

    private byte[] HashPassword(string password, byte[] salt)
    {
        // Use a secure hashing algorithm to hash the password with the salt
        // Note: This is just an example implementation. You should use a more secure hashing algorithm in your application.
        using (var hashAlgorithm = SHA256.Create())
        {
            var combined = new byte[password.Length + salt.Length];
            Buffer.BlockCopy(password.ToCharArray(), 0, combined, 0, password.Length);
            Buffer.BlockCopy(salt, 0, combined, password.Length, salt.Length);
            return hashAlgorithm.ComputeHash(combined);
        }
    }
}
