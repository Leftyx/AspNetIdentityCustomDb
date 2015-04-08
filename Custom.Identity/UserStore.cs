using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Identity
{
    using Microsoft.AspNet.Identity;
    using Newtonsoft.Json;

    public class UserStore : 
        IUserStore<User, int>, 
        IUserPasswordStore<User, int>, 
        IUserLockoutStore<User, int>, 
        IUserTwoFactorStore<User, int>,
        IUserRoleStore<User, int>,
        IUserClaimStore<User, int>
    {
        private readonly string FolderStorage = string.Empty;
        private readonly Biggy.Data.Json.JsonStore<User> UserDb = null;
        private readonly Biggy.Data.Json.JsonStore<Role> RoleDb = null;

        public UserStore(string folderStorage)
        {
            this.FolderStorage = folderStorage;
            this.UserDb = new Biggy.Data.Json.JsonStore<User>(this.FolderStorage, "Indentity", "Users");
            this.RoleDb = new Biggy.Data.Json.JsonStore<Role>(this.FolderStorage, "Indentity", "Roles");
        }

        #region USER STORE

        public System.Threading.Tasks.Task CreateAsync(User user)
        {
            int userId = 0;
            var users = this.UserDb.TryLoadData();
            userId = users == null ? 1 : users.Count + 1;
            user.Id = userId;

            this.UserDb.Add(user);
            return Task.FromResult(user);
        }

        public System.Threading.Tasks.Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<User> FindByIdAsync(int userId)
        {
            User user = null;
            IList<User> users = this.UserDb.TryLoadData();
            if (users == null || users.Count == 0)
            {
                return Task.FromResult(user);
            }

            user = users.Where(f => f.Id == userId).SingleOrDefault();

            return Task.FromResult(user);
        }

        public System.Threading.Tasks.Task<User> FindByNameAsync(string userName)
        {
            User user = null;
            IList<User> users = this.UserDb.TryLoadData();
            if (users == null || users.Count == 0)
            {
                return Task.FromResult(user);
            }

            user = users.Where(f => f.UserName == userName).SingleOrDefault();

            return Task.FromResult(user);
        }

        public System.Threading.Tasks.Task UpdateAsync(User user)
        {
            return Task.FromResult(this.UserDb.Update(user));
        }

        public void Dispose()
        {
            this.UserDb.FlushToDisk();
        }

        #endregion

        #region PASSWORD STORE

        public System.Threading.Tasks.Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public System.Threading.Tasks.Task<bool> HasPasswordAsync(User user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        #endregion

        #region LOCKOUT STORE

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region TWO FACTOR

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region USERS - ROLES STORE

        public Task AddToRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("role");
            }

            var roles = RoleDb.TryLoadData();
            var role = roles.Where(f => f.Name == roleName).SingleOrDefault();

            if (role == null)
            {
                throw new KeyNotFoundException("role");
            }

            if (role != null && user.Roles != null && !user.Roles.Contains(roleName, StringComparer.InvariantCultureIgnoreCase))
            {
                user.Roles.Add(roleName);
            }

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<string>>(user.Roles);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("role");
            }

            return Task.FromResult(user.Roles.Contains(roleName, StringComparer.InvariantCultureIgnoreCase));
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Roles.Remove(roleName);

            return Task.FromResult(0);
        }

        #endregion

        #region USERS - CLAIM STORE

        public Task AddClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            if (user.Claims != null && user.Claims.Where(f=>f.Value == claim.Value).Count() == 0)
            {
                user.Claims.Add(new UserClaim(claim));
            }

            return Task.FromResult(0);
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<System.Security.Claims.Claim>>(user.Claims.Select(clm => new System.Security.Claims.Claim(clm.Type, clm.Value)).ToList());
        }

        public Task RemoveClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Claims.Remove(new UserClaim(claim));

            return Task.FromResult(0);
        }

        #endregion
    }
}
