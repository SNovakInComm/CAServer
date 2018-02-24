using System;
using Microsoft.EntityFrameworkCore;


namespace CAApi.Models
{
    public class CADBContext : DbContext
    {
        public CADBContext(DbContextOptions<CADBContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<BootEntity> Boot { get; set; }

    }
}
