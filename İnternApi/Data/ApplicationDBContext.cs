using System;
using İnternApi.Model;
using Microsoft.EntityFrameworkCore;

namespace İnternApi.Data
{
	public class ApplicationDBContext:DbContext
	{
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
		public DbSet<User> users { get; set; }
        public DbSet<Tasks> tasks { get; set; }
        public DbSet<RoleMapping> rolemaps { get; set; }
        public DbSet<Roles> roles { get; set; }
        public DbSet<Files> files { get; set; }
        public DbSet<Departments> departments { get; set; }

    }
}

