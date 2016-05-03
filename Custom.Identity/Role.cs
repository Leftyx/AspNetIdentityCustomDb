using System;

namespace Custom.Identity
{
    using Microsoft.AspNet.Identity;

    public class Role: IRole<int>
    {
        public Role()
        {

        }

        public Role(string name)
        {
            this.Name = name;
        }

        public Role(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
