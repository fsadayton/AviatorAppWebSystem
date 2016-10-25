// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepo.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The user repo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Collections.Generic;

namespace DataEntry_Helpers.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// The user repository
    /// </summary>
    public class UserRepo : Repository
    {
        /// <summary>
        /// Create a a new user in the database
        /// </summary>
        /// <param name="newUser"> The new user. </param>
        /// <returns> The <see cref="User"/> as it was created. </returns>
        public User CreateUser(User newUser)
        {
            try
            {
                var createdUser = this.db.Users.Add(newUser);
                this.db.SaveChanges();
                return createdUser;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a list of all active and inactive users in the database.
        /// </summary>
        /// <returns> The <see cref="List"/>. </returns>
        public List<User> GetUsers()
        {

            List<User> response;
            try
            {
                response = this.db.Users.ToList();
            }
            catch (Exception)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// Searches the database for any user found with the search criteria in 
        /// first name, last name or service provider name.
        /// </summary>
        /// <param name="searchString"> The search string. </param>
        /// <returns> The <see cref="List"/>. </returns>
        public List<User> GetUsers(string searchString)
        {
            List<User> response;
            var searchLowered = searchString.ToLower();
            try
            {
                response = this.db.Users.Where(
                    u => u.FirstName.ToLower().Contains(searchLowered)
                         || u.LastName.ToLower().Contains(searchLowered)
                         || (u.ServiceProviderID != null && u.ServiceProvider.ProviderName.Contains(searchLowered)))
                    .ToList();
            }
            catch (Exception)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// Get a user by username and password.
        /// </summary>
        /// <param name="userName"> The user name. </param>
        /// <param name="hashedPassword"> The hashed password. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public User GetUser(string userName, string hashedPassword)
        {
            User foundUser;
            try
            {
                foundUser = this.db.Users.Single(
                    u =>
                    u.IsActive
                    && u.UserCredential.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                    && u.UserCredential.PasswordHash == hashedPassword);
            }
            catch (Exception ex)
            {
                foundUser = null;
            }

            return foundUser;
        }

        /// <summary>
        /// Get a user by ID
        /// </summary>
        /// <param name="id"> The id. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public User GetUser(int id, bool checkIsActive)
        {
            User foundUser;
            try
            {
                foundUser = checkIsActive 
                    ? this.db.Users.Single(u => u.ID == id && u.IsActive) 
                    : this.db.Users.Single(u => u.ID == id);
            }
            catch (Exception ex)
            {
                foundUser = null;
            }

            return foundUser;
        }

        /// <summary>
        /// Get a user by their username
        /// </summary>
        /// <param name="userName">The user name. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public User GetUser(string userName)
        {
            User foundUser;
            try
            {
                foundUser = this.db.Users.Single(u => u.UserCredential.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                foundUser = null;
            }

            return foundUser;
        }

        /// <summary>
        /// Partial update of a user. Updates first name, last name and is active flag.
        /// </summary>
        /// <param name="userInfo"> The user info. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool UpdateUser(User userInfo)
        {
            try
            {
                var userInDb = db.Users.Single(u => u.ID == userInfo.ID);

                userInDb.IsActive = userInfo.IsActive;
                userInDb.FirstName = userInfo.FirstName;
                userInDb.LastName = userInfo.LastName;
                userInDb.ServiceProviderID = userInfo.ServiceProviderID;

                this.db.Entry(userInDb).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update the reset token of a user credential.
        /// </summary>
        /// <param name="credential"> The credential. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool UpdateUserCredentialResetToken(UserCredential credential)
        {
            try
            {
                var databaseCredential = this.db.UserCredentials.Single(c => c.ID == credential.ID);

                databaseCredential.ResetToken = credential.ResetToken;
                this.db.Entry(databaseCredential).State = EntityState.Modified;
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Change the user's password and reset their reset token.
        /// </summary>
        /// <param name="credential"> The credential. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool UpdateUserCredentialResetPassword(UserCredential credential)
        {
            try
            {
                var databaseCredential = this.db.UserCredentials.Single(c => c.ID == credential.ID);

                databaseCredential.ResetToken = credential.ResetToken;
                databaseCredential.PasswordHash = credential.PasswordHash;

                this.db.Entry(databaseCredential).State = EntityState.Modified;
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
