using System;
using Microsoft.AspNetCore.Identity;


namespace CAApi.Models
{
    public class UserRoleEntity : IdentityRole<Guid>
    {
        public UserRoleEntity() : base() { }

        public UserRoleEntity(string name) : base(name) { }
        
    }
}
