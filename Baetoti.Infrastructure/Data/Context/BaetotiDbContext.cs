using Baetoti.Core.Entites;
using Microsoft.EntityFrameworkCore;

namespace Baetoti.Infrastructure.Data.Context
{
    public class BaetotiDbContext : DbContext
    {
        public BaetotiDbContext(DbContextOptions<BaetotiDbContext> options)
            : base(options)
        {
        }

        #region DbSets

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<OTP> OTPs { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(@"");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureEntities(builder);
        }

        private void ConfigureEntities(ModelBuilder builder)
        {
            //builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
