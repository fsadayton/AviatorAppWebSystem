// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHash.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The password hash.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.BL
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The password hash.
    /// </summary>
    public static class PasswordHash
    {
        /// <summary>
        /// The create a salt unique to username.
        /// </summary>
        /// <param name="userName"> The user name.  </param>
        /// <returns> The <see cref="string"/>.  </returns>
        public static string CreateSalt(string userName)
        {
            var hasher = new Rfc2898DeriveBytes(userName, Encoding.Default.GetBytes("saltingSalt"), 10000);
            return Convert.ToBase64String(hasher.GetBytes(25));
        }

        /// <summary>
        /// Hashes a password with the given a salt.
        /// </summary>
        /// <param name="salt"> The salt.  </param>
        /// <param name="password"> The password.  </param>
        /// <returns> The hashed <see cref="string"/>.  </returns>
        public static string HashPassword(string salt, string password)
        {
            var hasher = new Rfc2898DeriveBytes(password, Encoding.Default.GetBytes(salt), 10000);
            return Convert.ToBase64String(hasher.GetBytes(25));
        }
    }
}