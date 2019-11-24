//using LedgerLib.Entities;
using System.Data;
using System.Linq;

using LedgerLib.Entities;
using LedgerLib.Infrastructure;
using LedgerLib.Models;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LedgerLib
{
    public class LedgerContext : DbContext
    {
        private readonly string _connectionString;

        #region DbSets

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<AccountNumberEntity> AccountNumbers { get; set; }
        public DbSet<AccountTypeEntity> AccountTypes { get; set; }
        public DbSet<AllotmentEntity> Allotments { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<IdentityEntity> Identities { get; set; }
        public DbSet<PoolEntity> Pools { get; set; }
        public DbSet<SettingsEntity> SystemSettings { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        #endregion

        #region Constructors

        public LedgerContext(DbContextOptions<LedgerContext> options) : base(options)
        {
            _connectionString = CSBuilder.Build();
        }

        public LedgerContext() : base()
        {
            _connectionString = CSBuilder.Build();
        }

        #endregion

        #region Public Methods / Properties

        public SettingsEntity GetSettings { get => SystemSettings.FirstOrDefault(); }

        public void Seed()
        {
            if (GetSettings is null)
            {
                SystemSettings.Add(SettingsEntity.Default);
                SaveChanges();
            }
        }

        public DatabaseInfo DatabaseInfo()
        {
            var conn = Database.GetDbConnection();
            if (!(conn is SqlConnection connection))
            {
                return new DatabaseInfo();
            }
            var command = new SqlCommand("sp_spaceused")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };
            using var adapter = new SqlDataAdapter(command);
            var dataset = new DataSet();
            connection.Open();
            adapter.Fill(dataset);
            connection.Close();
            var ret = new DatabaseInfo(dataset);
            return ret;
        }

        public bool? DatabaseExists(string dbname)
        {
            var conn = Database.GetDbConnection();
            if (!(conn is SqlConnection connection))
            {
                return null;
            }
            using var command = new SqlCommand("Select dbid from master.dbo.sysdatabases where name = @n;")
            {
                CommandType = CommandType.Text,
                Connection = connection
            };
            command.Parameters.Add(new SqlParameter("n", dbname));
            connection.Open();
            var resultobject = command.ExecuteScalar();
            connection.Close();
            if (!(resultobject is short dbid))
            {
                return null;
            }
            return dbid != 0;
        }

        #endregion

        #region Overrides

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
            builder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AccountEntity>().HasIndex(x => x.CompanyId).IsClustered(false);
            builder.Entity<AccountEntity>().HasIndex(x => x.AccountTypeId).IsClustered(false);
            builder.Entity<AccountEntity>().HasOne(x => x.AccountType);

            builder.Entity<AccountNumberEntity>().HasIndex(x => x.AccountId).IsClustered(false);
            builder.Entity<AccountNumberEntity>().Property(x => x.StartDate).HasColumnType(Constants.Datetime2);
            builder.Entity<AccountNumberEntity>().Property(x => x.StopDate).HasColumnType(Constants.Datetime2);
            builder.Entity<AccountNumberEntity>().Property(x => x.Salt).HasColumnType(Constants.Varbinary);

            builder.Entity<AccountTypeEntity>().HasIndex(x => x.Description).IsUnique().IsClustered(false);

            builder.Entity<AllotmentEntity>().HasIndex(x => x.PoolId).IsClustered(false);
            builder.Entity<AllotmentEntity>().HasIndex(x => x.CompanyId).IsClustered(false);
            builder.Entity<AllotmentEntity>().HasOne(x => x.Company);
            builder.Entity<AllotmentEntity>().Property(x => x.Date).HasColumnType(Constants.Date);
            builder.Entity<AllotmentEntity>().Property(x => x.Amount).HasColumnType(Constants.MoneyFormat);

            builder.Entity<CompanyEntity>().HasIndex(x => x.Name).IsUnique().IsClustered(false);

            builder.Entity<IdentityEntity>().HasIndex(x => x.CompanyId).IsClustered(false);
            builder.Entity<IdentityEntity>().HasOne(x => x.Company);
            builder.Entity<IdentityEntity>().Property(x => x.UserSalt).HasColumnType(Constants.Varbinary);
            builder.Entity<IdentityEntity>().Property(x => x.PasswordSalt).HasColumnType(Constants.Varbinary);

            builder.Entity<PoolEntity>().HasIndex(x => x.Name).IsUnique().IsClustered(false);
            builder.Entity<PoolEntity>().Property(x => x.Date).HasColumnType(Constants.Date);
            builder.Entity<PoolEntity>().Property(x => x.Amount).HasColumnType(Constants.MoneyFormat);
            builder.Entity<PoolEntity>().Property(x => x.Balance).HasColumnType(Constants.MoneyFormat);

            builder.Entity<SettingsEntity>().HasKey(x => x.SystemId);
            builder.Entity<SettingsEntity>().Property(x => x.PasswordSalt).HasColumnType(Constants.Varbinary);
            builder.Entity<SettingsEntity>().Property(x => x.PasswordHash).HasColumnType(Constants.Varbinary);

            builder.Entity<TransactionEntity>().HasIndex(x => x.AccountId).IsClustered(false);
            builder.Entity<TransactionEntity>().Property(x => x.Date).HasColumnType(Constants.Date);
            builder.Entity<TransactionEntity>().Property(x => x.Balance).HasColumnType(Constants.MoneyFormat);
            builder.Entity<TransactionEntity>().Property(x => x.Payment).HasColumnType(Constants.MoneyFormat);
        }

        #endregion
    }
}
