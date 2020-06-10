using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Authorisation
    {
        private readonly DataContext _context;

        public Authorisation(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The main login method for the application
        /// </summary>
        /// <param name="username">The username for the user</param>
        /// <param name="password">The password for the user</param>
        /// <returns>Returns null if checks are not passed, otherwise returns the User object</returns>
        public async Task<User> Login(string username, string password)
        {
            // Get the user from the db
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            // If the user is null (hence not found from the db), return null
            if (user == null)
                return null;

            // Check that the password hashes match, if not, return null
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // Success, return the user
            return user;
        }

        /// <summary>
        /// Hashes the provided password string and compares to the provided password hash and salt
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <param name="passwordHash">The password hash to verify against</param>
        /// <param name="passwordSalt">The password salt to verify against</param>
        /// <returns>Returns true if the passwords match, otherwise returns false</returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Use SHA512 to compute the hash, using the provided salt
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                // Compute the hash of the provided password into an array of bytes
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // Compare each element of the computedHash array and return false if any element
                // is not equal to the same element of the provided password hash
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }

            // Success, return true
            return true;
        }

        /// <summary>
        /// The main methid with which to register the user
        /// </summary>
        /// <param name="user">The user object to use</param>
        /// <param name="password">The password as a string</param>
        /// <returns>Returns the newly created user</returns>
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Creates a hashed password from a provided string
        /// </summary>
        /// <param name="password">The password string</param>
        /// <param name="passwordHash">A byte array to write the array to</param>
        /// <param name="passwordSalt">A byte array to write the salt to</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                // Use the built-in hmac key to create a random salt
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Checks if a user already exists in the database
        /// </summary>
        /// <param name="username">The username as a string to check</param>
        /// <returns>Returns true if the user exists in the database, otherwise returns false</returns>
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}