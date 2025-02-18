using Patient_Management.Domain.Entities;
using Patient_Management.Domain.Entities.Base;
using Patient_Management.Persistence.Seeds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patient_Management.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
#nullable enable
        private IDbContextTransaction? _currentTransaction;
#nullable disable
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
        .HasOne(p => p.User)
        .WithOne(u => u.Patient)
        .HasForeignKey<Patient>(p => p.UserId);

            modelBuilder.Entity<Patient>()
         .HasMany(p => p.Records)
         .WithOne(pr => pr.Patient)
         .HasForeignKey(pr => pr.PatientId);

            modelBuilder.Entity<PatientRecord>()
                .HasMany(pr => pr.Appointments)
                .WithOne(a => a.PatientRecord)
                .HasForeignKey(a => a.PatientRecordId);




            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "USER");
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(x => x.AccessFailedCount)
                    .HasColumnName("ACCESS_FAILED_COUNT");
                entity.Property(x => x.ConcurrencyStamp)
                    .HasColumnName("CONCURRENCY_STAMP");
                entity.Property(x => x.CreatedAt)
                    .HasColumnName("CREATED_AT");
                entity.Property(x => x.Email)
                    .HasColumnName("EMAIL");
                entity.Property(x => x.EmailConfirmed)
                    .HasColumnName("EMAIL_CONFIRMED");
                entity.Property(x => x.FirstName)
                    .HasColumnName("FIRST_NAME");
                entity.Property(x => x.IsActive)
                    .HasColumnName("IS_ACTIVE");
                entity.Property(x => x.IsLoggedIn)
                    .HasColumnName("IS_LOGGED_IN");
                entity.Property(x => x.LastLoginTime)
                    .HasColumnName("LAST_LOGIN_TIME");
                entity.Property(x => x.LastName)
                    .HasColumnName("LAST_NAME");
                entity.Property(x => x.LockoutEnabled)
                    .HasColumnName("LOCKOUT_ENABLED");
                entity.Property(x => x.LockoutEnd)
                    .HasColumnName("LOCKOUT_END");
                entity.Property(x => x.UserRole)
                    .HasColumnName("DEFAULT_ROLE");
                entity.Property(x => x.NormalizedEmail)
                    .HasColumnName("NORMALIZED_EMAIL");
                entity.Property(x => x.NormalizedUserName)
                    .HasColumnName("NORMALIZED_USER_NAME");
                entity.Property(x => x.PasswordHash)
                    .HasColumnName("PASSWORD_HASH");
                entity.Property(x => x.PhoneNumber)
                    .HasColumnName("PHONE_NUMBER");
                entity.Property(x => x.PhoneNumberConfirmed)
                    .HasColumnName("PHONE_NUMBER_CONFIRMED");
                entity.Property(x => x.SecurityStamp)
                    .HasColumnName("SECURITY_STAMP");
                entity.Property(x => x.TwoFactorEnabled)
                    .HasColumnName("TWO_FACTOR_ENABLED");
                entity.Property(x => x.UserName)
                    .HasColumnName("USER_NAME");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Patient_Management_ROLE");
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(x => x.ConcurrencyStamp)
                    .HasColumnName("CONCURRENCY_STAMP");
                entity.Property(x => x.Name)
                    .HasColumnName("NAME");
                entity.Property(x => x.NormalizedName)
                    .HasColumnName("NORMALIZED_NAME");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("Patient_Management_USER_ROLES");
                entity.Property(x => x.RoleId)
                    .HasColumnName("ROLE_ID");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("Patient_Management_USER_CLAIMS");
                entity.Property(x => x.ClaimType)
                    .HasColumnName("CLAIM_TYPE");
                entity.Property(x => x.ClaimValue)
                    .HasColumnName("CLAIM_VALUE");
                entity.Property(x => x.Id)
                    .HasColumnName("ID");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("Patient_Management_ROLE_CLAIMS");
                entity.Property(x => x.ClaimType)
                    .HasColumnName("CLAIM_TYPE");
                entity.Property(x => x.ClaimValue)
                    .HasColumnName("CLAIM_VALUE");
                entity.Property(x => x.Id)
                    .HasColumnName("ID");
                entity.Property(x => x.RoleId)
                    .HasColumnName("ROLE_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("Patient_Management_USER_LOGINS");
                entity.Property(x => x.LoginProvider)
                    .HasColumnName("LOGIN_PROVIDER");
                entity.Property(x => x.ProviderDisplayName)
                    .HasColumnName("PROVIDER_DISPLAY_NAME");
                entity.Property(x => x.ProviderKey)
                    .HasColumnName("PROVIDER_KEY");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("Patient_Management_USER_TOKENS");
                entity.Property(x => x.LoginProvider)
                    .HasColumnName("LOGIN_PROVIDER");
                entity.Property(x => x.Name)
                    .HasColumnName("NAME");
                entity.Property(x => x.Value)
                    .HasColumnName("VALUE");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Seed();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamp();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(
            IsolationLevel isolationLevel,
            CancellationToken cancellationToken = default
        )
        {
            _currentTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                await _currentTransaction?.CommitAsync(cancellationToken)!;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _currentTransaction?.RollbackAsync(cancellationToken)!;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public Task RetryOnExceptionAsync(Func<Task> operation)
        {
            return Database.CreateExecutionStrategy().ExecuteAsync(operation);
        }

        public Task<TResult> RetryOnExceptionAsync<TResult>(Func<Task<TResult>> operation)
        {
            return Database.CreateExecutionStrategy().ExecuteAsync(operation);
        }

        public Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await action();

                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public Task<T> ExecuteTransactionalAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await action();

                    await transaction.CommitAsync(cancellationToken);

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        private void AddTimestamp()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase && x.State == EntityState.Added);

            foreach (var entity in entities)
            {
                var now = DateTime.Now; // current datetime

                ((EntityBase)entity.Entity).CreatedAt = now;
            }
        }

        
    }
}