using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Identity
{
    using Microsoft.AspNet.Identity;

    public class RoleStore : IRoleStore<Role, int>, IQueryableRoleStore<Role, int>
    {
        private readonly string FolderStorage = string.Empty;
        private readonly Biggy.Data.Json.JsonStore<Role> RoleDb = null;

        public RoleStore(string folderStorage)
        {
            this.FolderStorage = folderStorage;
            this.RoleDb = new Biggy.Data.Json.JsonStore<Role>(this.FolderStorage, "Indentity", "Roles");
        }

        public Task CreateAsync(Role role)
        {
            this.RoleDb.Add(role);
            return Task.FromResult(role);
        }

        public Task DeleteAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            Role role = null;
            IList<Role> roles = this.RoleDb.TryLoadData();
            if (roles == null || roles.Count == 0)
            {
                return Task.FromResult(role);
            }

            role = roles.SingleOrDefault(f => f.Id == roleId);

            return Task.FromResult(role);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            Role role = null;
            IList<Role> roles = this.RoleDb.TryLoadData();
            if (roles == null || roles.Count == 0)
            {
                return Task.FromResult(role);
            }

            role = roles.SingleOrDefault(f => f.Name == roleName);

            return Task.FromResult(role);
        }

        public Task UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.RoleDb.FlushToDisk();
        }

        public IQueryable<Role> Roles
        {
            get
            {
                return (this.RoleDb.TryLoadData().AsQueryable<Role>());
            }
        }
    }
}
