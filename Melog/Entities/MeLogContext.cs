namespace Melog.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MeLogContext : DbContext
    {
        public MeLogContext()
            : base("name=MeLogContext")
        {
        }

        public virtual DbSet<Authorities> Authorities { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<ExternalAccounts> ExternalAccounts { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ArticleCategories> ArticleCategories { get; set; }
        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<RoleAuthorities> RoleAuthorities { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authorities>()
                .Property(e => e.AuthorityName)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Articles>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Articles>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.UserName)
                .IsUnicode(false);
        }
    }
}
