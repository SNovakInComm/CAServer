using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAApi.Models
{
    public class User : Resource
    {
        public string Id { get; set; }

        public string FirstName { get; set;  }

        public string LastName { get; set; }

        public void Copy(UserEntity user)
        {
            this.Id = user.Id;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
        }

    }
}
