using DotnetTemplateApp.Domain.Enums;
using DotnetTemplateApp.Domain.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DotnetTemplateApp.Persistence.Contexts
{
    public partial class ApplicationDbContext : DbContext
    {
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        private static void SetUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "account");

                SetBaseModel(entity, "pk_users");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasValueGenerator<GuidValueGenerator>()
                    .HasColumnName("id");

                entity.Property(e => e.DateCreated)
                .HasColumnName("date_created");

                entity.Property(e => e.DateModified).HasColumnName("date_modified");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .HasColumnName("lastname");

                entity.Property(e => e.Middlename)
                    .HasMaxLength(255)
                    .HasColumnName("middlename");

                entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");

                entity.HasOne(d => d.UserAccount)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_user_account_id");
            });

        }
        private static void SetUserAccount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("user_accounts", "account");
                SetBaseModel(entity, "pk_user_accounts");

                entity.HasIndex(e => e.Email, "uniq_user_accounts_email")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "uniq_user_accounts_id")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "uniq_user_accounts_username")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasValueGenerator<GuidValueGenerator>()
                    .HasColumnName("id");

                entity.Property(e => e.DateCreated).HasColumnName("date_created");

                entity.Property(e => e.DateModified).HasColumnName("date_modified");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsRegistered).HasColumnName("is_registered");

                entity.Property(e => e.LastLogin).HasColumnName("last_login");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(30)
                    .HasColumnName("phone_number");

                entity.Property(e => e.PhoneNumberPrefix)
                    .HasMaxLength(5)
                    .HasColumnName("phone_number_prefix");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.Property(e => e.AccountStatus)
                    .HasColumnName("account_status")
                    .HasConversion(v => v.ToString(), v => (AccountStatus)Enum.Parse(typeof(AccountStatus), v));
            });
        }
    }
}
