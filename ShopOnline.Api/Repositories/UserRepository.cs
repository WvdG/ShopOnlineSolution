using Microsoft.Data.SqlClient;
using ShopOnline.Api.Repositories.Contracts;
using System.Security.Cryptography;

namespace ShopOnline.Api.Repositories;
public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task<bool> Authenticate(string username, string password)
    {
        // Connect to the database
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Retrieve the user's password hash and salt from the database
            var command = new SqlCommand("SELECT PasswordHash, PasswordSalt FROM Users WHERE Username = @Username", connection);
            command.Parameters.AddWithValue("@Username", username);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (!reader.Read())
                {
                    // No matching user was found
                    return false;
                }

                // Retrieve the password hash and salt from the database
                var passwordHash = (byte[])reader["PasswordHash"];
                var passwordSalt = (byte[])reader["PasswordSalt"];

                // Hash the entered password using the retrieved salt
                var enteredPasswordHash = HashPassword(password, passwordSalt);

                // Compare the entered password hash with the stored password hash
                return enteredPasswordHash.SequenceEqual(passwordHash);
            }
        }
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
